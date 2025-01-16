using Blasterify.Models.Requests;
using Blasterify.Models.Responses;
using Blasterify.Services.Data;
using Blasterify.Services.Models;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Blasterify.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientUserController : ControllerBase, IUserController
    {
        private readonly DataContext _context;

        public ClientUserController(DataContext context)
        {
            _context = context;
        }

        #region IUserController Implementation

        [AllowAnonymous]
        [HttpPost]
        [Route("SignUp")]
        public async Task<ActionResult> SignUp(SignUpRequest signUpRequest)
        {
            try
            {
                await _context!.ClientUsers!.AddAsync(new ClientUser()
                {
                    Id = Guid.NewGuid(),
                    FirstName = signUpRequest.FirstName,
                    LastName = signUpRequest.LastName,
                    Email = signUpRequest.Email,
                    PasswordHash = signUpRequest.PasswordHash
                });

                await _context.SaveChangesAsync();
                /*
                BackgroundJob.Schedule(() =>
                    Services.EmailServices.EmailAuthenticator.WelcomeStudentEmail(signUpRequest.Email!, $"{signUpRequest.FirstName} {signUpRequest.LastName}"),
                    new DateTimeOffset(DateTime.UtcNow)
                );
                */
                return Ok();
            }
            catch (Exception ex)
            {
                if (
                    ex.InnerException != null
                    && ex.InnerException.Message.Contains(IUserController.EMAIL_ALREADY_USED_EXCEPTION_MESSAGE_1)
                    && ex.InnerException.Message.Contains(IUserController.EMAIL_ALREADY_USED_EXCEPTION_MESSAGE_2)
                )
                {
                    return Conflict(new { message = "Email already used." });
                }

                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("LogIn")]
        public async Task<ActionResult> LogIn(LogInRequest logInRequest)
        {
            try
            {
                var student = await _context!.ClientUsers!.FirstOrDefaultAsync(cu => cu.Email == logInRequest.Email);

                if (student == null)
                {
                    return NotFound(new { message = "Account not found." });
                }
                else
                {
                    if (logInRequest.PasswordHash == null || logInRequest.PasswordHash.Length == 0)
                    {
                        return BadRequest(new { message = "Password is required." });
                    }

                    if (student.IsLocked && student.UnlockDate.ToUniversalTime() > DateTime.UtcNow)
                    {
                        return Unauthorized(new
                        {
                            message = "Account is locked.",
                            unlockDate = student.UnlockDate.ToUniversalTime()
                        });
                    }
                    else if (student.IsLocked && student.UnlockDate.ToUniversalTime() <= DateTime.UtcNow)
                    {
                        // Desbloqueando cuenta
                        student.IsLocked = false;
                        student.UnlockDate = DateTime.MinValue;
                        student.LogInAttempts = 0;
                    }

                    //Comparando bytes de la contraseña
                    using (SHA256 sha256Hash = SHA256.Create())
                    {
                        byte[] bytes = logInRequest.PasswordHash!;
                        for (int i = 0; i < bytes.Length; i++)
                        {
                            if (bytes[i] != student.PasswordHash![i])
                            {
                                student.LogInAttempts++;

                                // Bloqueando cuenta después de n intentos fallidos
                                if (student.LogInAttempts == IUserController.MAX_LOGIN_ATTEMPTS)
                                {
                                    student.IsLocked = true;
                                    student.UnlockDate = DateTime.UtcNow.AddMinutes(IUserController.LOG_IN_UNLOCK_MINUTES);

                                    await _context.SaveChangesAsync();

                                    return Unauthorized(new
                                    {
                                        message = "Account is locked.",
                                        unlockDate = student.UnlockDate.ToUniversalTime()
                                    });
                                }

                                await _context.SaveChangesAsync();

                                return Unauthorized(new
                                {
                                    message = "Invalid password.",
                                    remainingLogInAttempts = IUserController.MAX_LOGIN_ATTEMPTS - student.LogInAttempts
                                });
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                    //string token = ((IUserController)this).GenerateJwtToken(_configuration, student.Id!, student.GetType());
                    /*
                    if (string.IsNullOrEmpty(token))
                    {
                        return BadRequest(new { message = "Error generating token." });
                    }
                    */
                    return Ok(new LogInResponse
                    {
                        Id = student.Id,
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        Email = student.Email,
                        //Token = token,
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("SendResetPasswordEmail")]
        public async Task<ActionResult> SendResetPasswordEmail(string email)
        {
            try
            {
                var student = await _context!.ClientUsers!.FirstOrDefaultAsync(cu => cu.Email == email);

                if (student == null)
                {
                    return NotFound(new { message = "Account not found." });
                }

                student.VerificationCode = new Random().Next(IUserController.VERIFICATION_MIN_RANGE_VALUE, IUserController.VERIFICATION_MAX_RANGE_VALUE);
                student.VerificationCodeExpiration = DateTime.UtcNow.AddMinutes(IUserController.VERIFICATION_CODE_EXPIRED_MINUTES);
                student.IsVerified = false;

                await _context.SaveChangesAsync();
                /*
                BackgroundJob.Schedule(() =>
                    Services.EmailServices.EmailAuthenticator.ResetPasswordEmail(student.Email!, $"{student.FirstName} {student.LastName}", student.VerificationCode),
                    new DateTimeOffset(DateTime.UtcNow)
                );
                */
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("ResetPasswordVerification")]
        public async Task<ActionResult> ResetPasswordVerification(string email, int code)
        {
            try
            {
                if (code < IUserController.VERIFICATION_MIN_RANGE_VALUE || code > IUserController.VERIFICATION_MAX_RANGE_VALUE)
                {
                    return BadRequest(new { message = "Invalid code." });
                }

                var student = await _context!.ClientUsers!.FirstOrDefaultAsync(cu => cu.Email == email);

                if (student == null)
                {
                    return NotFound(new { message = "Account not found." });
                }
                else if (student.VerificationCode != code)
                {
                    return BadRequest(new { message = "Invalid code." });
                }
                else if (student.VerificationCodeExpiration.ToUniversalTime() < DateTime.UtcNow)
                {
                    return BadRequest(new { message = "Code expired." });
                }

                student.IsVerified = true;

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            try
            {
                if (resetPasswordRequest.NewPasswordHash == null || resetPasswordRequest.NewPasswordHash.Length == 0)
                {
                    return BadRequest(new { message = "New password is required." });
                }
                else if (resetPasswordRequest.VerificationCode < IUserController.VERIFICATION_MIN_RANGE_VALUE
                    || resetPasswordRequest.VerificationCode > IUserController.VERIFICATION_MAX_RANGE_VALUE)
                {
                    return BadRequest(new { message = "Invalid code." });
                }

                var student = await _context!.ClientUsers!.FirstOrDefaultAsync(student => student.Email == resetPasswordRequest.Email);

                if (student == null)
                {
                    return NotFound(new { message = "Account not found." });
                }
                else if (student.VerificationCode != resetPasswordRequest.VerificationCode)
                {
                    return BadRequest(new { message = "Invalid code." });
                }
                else if (student.VerificationCodeExpiration.ToUniversalTime() < DateTime.UtcNow)
                {
                    return BadRequest(new { message = "Code expired." });
                }
                else if (!student.IsVerified)
                {
                    return BadRequest(new { message = "Code not verified." });
                }

                bool isDiferent = false;

                //Comparando bytes de la contraseña actual y la nueva
                for (int i = 0; i < resetPasswordRequest.NewPasswordHash.Length; i++)
                {
                    if (student.PasswordHash![i] != resetPasswordRequest.NewPasswordHash[i])
                    {
                        isDiferent = true;
                        break;
                    }
                }

                if (!isDiferent)
                {
                    return BadRequest(new { message = "New password is the same as the current password." });
                }

                student.VerificationCode = IUserController.VERIFICATION_CODE_NULL_VALUE;
                student.VerificationCodeExpiration = DateTime.MinValue;
                student.IsVerified = false;
                student.PasswordHash = resetPasswordRequest.NewPasswordHash;

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        #endregion

        // Error - MERCHAT ORDER ID
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(Blasterify.Models.Model.ClientUserModel clientUserModel)
        {
            var createClientUser = await _context!.ClientUsers!.AddAsync(new ClientUser()
            {
                FirstName = clientUserModel.FirstName,
                LastName = clientUserModel.LastName,
                Email = clientUserModel.Email,
                PasswordHash = clientUserModel.PasswordHash,
                MerchantOrderId = string.Empty
            });

            await _context.SaveChangesAsync();

            var yunoId = await Services.YunoServices.CreateCustomer(new Blasterify.Models.Yuno.CustomerRequest()
            {
                merchant_customer_id = $"{createClientUser.Entity.Id}",
                merchant_customer_created_at = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ"),
                first_name = clientUserModel.FirstName,
                last_name = clientUserModel.LastName,
                email = clientUserModel.Email,
                created_at = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ"),
                updated_at = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ")
            });

            if (Services.YunoServices.ErrorCodes.Contains(yunoId))
            {
                return BadRequest();
            }

            createClientUser.Entity.YunoId = yunoId;
            createClientUser.Entity.MerchantOrderId = $"{createClientUser.Entity.Id}";

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<ClientUser>>> GetAll()
        {
            var clientUsers = await _context!.ClientUsers!.ToListAsync();
            return Ok(clientUsers);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var clientUser = await _context!.ClientUsers!.FindAsync(id);

            if (clientUser == null)
            {
                return NotFound();
            }

            return Ok(clientUser);
        }

        [HttpPost]
        [Route("LogIn1")]
        public async Task<ActionResult<ClientUser>> LogIn1(LogInRequest logInRequest)
        {
            try
            {
                var clientUser = await _context!.ClientUsers!.FirstOrDefaultAsync(cu => cu.Email == logInRequest.Email);

                if (clientUser == null)
                {
                    return NotFound();
                }
                else
                {
                    using (SHA256 sha256Hash = SHA256.Create())
                    {
                        byte[] bytes = logInRequest.PasswordHash!;
                        for (int i = 0; i < bytes.Length; i++)
                        {
                            if (bytes[i] != clientUser.PasswordHash![i])
                            {
                                return Unauthorized();
                            }
                        }
                    }

                    return Ok(new ClientUser
                    {
                        Id = clientUser.Id,
                        FirstName = clientUser.FirstName,
                        LastName = clientUser.LastName,
                        Email = clientUser.Email,
                        IsConnected = true,
                        LastConnectionDate = DateTime.UtcNow

                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return NotFound();
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(int id, ClientUser clientUser)
        {
            var getClientUser = await _context!.ClientUsers!.FindAsync(id);
            if (getClientUser == null)
            {
                return NotFound();
            }

            getClientUser!.FirstName = clientUser.FirstName;
            getClientUser!.LastName = clientUser.LastName;
            getClientUser!.IsConnected = clientUser.IsConnected;
            getClientUser!.Email = clientUser.Email;
            getClientUser!.PasswordHash = clientUser.PasswordHash;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        [Route("UpdateLastConnection")]
        public async Task<IActionResult> UpdateLastConnectionDate(Blasterify.Models.Request.LastUserConnection lastUserConnection)
        {
            var getClientUser = await _context!.ClientUsers!.FindAsync(lastUserConnection.Id);
            if (getClientUser == null)
            {
                return NotFound();
            }

            getClientUser!.LastConnectionDate = lastUserConnection.Date;
            getClientUser!.IsConnected = lastUserConnection.IsConnected;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var clientUser = await _context!.ClientUsers!.FindAsync(id);
            _context!.ClientUsers!.Remove(clientUser!);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}