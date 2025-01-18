using Blasterify.Models.Model;
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
                var country = await _context.Countries!.FirstOrDefaultAsync(c => c.Id == signUpRequest.CountryId);

                if (country == null)
                {
                    return NotFound(new { Message = "Country not found." });
                }

                var newClientUser = (await _context!.ClientUsers!.AddAsync(new ClientUser()
                {
                    FirstName = signUpRequest.FirstName,
                    LastName = signUpRequest.LastName,
                    Email = signUpRequest.Email,
                    PasswordHash = signUpRequest.PasswordHash,
                    CountryId = signUpRequest.CountryId
                })).Entity;

                await _context.SaveChangesAsync();

                var yunoId = await Services.YunoServices.CreateCustomer(new Blasterify.Models.Yuno.CustomerRequest()
                {
                    merchant_customer_id = $"{newClientUser.Id}",
                    merchant_customer_created_at = newClientUser.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ"),
                    first_name = newClientUser.FirstName,
                    last_name = newClientUser.LastName,
                    email = newClientUser.Email,
                    created_at = newClientUser.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ"),
                    updated_at = newClientUser.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ"),
                });

                if (Services.YunoServices.ErrorCodes.Contains(yunoId))
                {
                    _context.ClientUsers!.Remove(newClientUser);

                    return BadRequest(new { message = "Error creating customer." });
                }

                newClientUser.YunoId = yunoId;
                newClientUser.MerchantOrderId = $"{newClientUser.Id}";

                await _context.SaveChangesAsync();

                /*
                BackgroundJob.Schedule(() =>
                    Services.EmailServices.EmailAuthenticator.WelcomeStudentEmail(signUpRequest.Email!, $"{signUpRequest.FirstName} {signUpRequest.LastName}"),
                    new DateTimeOffset(DateTime.UtcNow)
                );
                Message = "Cannot insert duplicate key row in object 'dbo.ClientUsers' with unique index 'IX_ClientUsers_Email'. The duplicate key value is (lmessi@blasterify.com).\r\nThe statement has been terminated."
                */
                return Ok();
            }
            catch (Exception ex)
            {
                if (
                    ex.InnerException != null
                    && ex.InnerException.Message.Contains(IUserController.EMAIL_ALREADY_USED_EXCEPTION_MESSAGE)
                )
                {
                    return Conflict(new { message = "Email already used." });
                }

                return BadRequest(new { message = ex.InnerException?.Message ?? ex.Message });
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
                    if (!student.IsVerified)
                    {
                        return Unauthorized(new { message = "Account not verified." });
                    }

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