﻿using Blasterify.Client.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Blasterify.Client.Controllers
{
    public class ShopController : BaseController
    {
        private static readonly HttpClient client = new HttpClient();

        #region SERVICES

        public async Task<Blasterify.Models.Model.MovieModel> GetMovieAsync(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"{MvcApplication.ServicesPath}/Movie/Get?id={id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var gatMovie = JsonConvert.DeserializeObject<Blasterify.Models.Model.MovieModel>(jsonString);

                return gatMovie;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> CompleteRentAsync(string token)
        {
            var cart = GetCart();
            var json = JsonConvert.SerializeObject(new Blasterify.Models.Model.CompleteRentRequest
            {
                RentId = cart.Id,
                Token = token
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync($"{MvcApplication.ServicesPath}/Rent/CompleteRent", content);
            if (response.IsSuccessStatusCode)
            {
                Session["RentId"] = cart.Id;
                Session["Cart"] = null;

                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Blasterify.Models.Model.RentDetailModel> GetRentDetailAsync()
        {
            HttpResponseMessage response = await client.GetAsync($"{MvcApplication.ServicesPath}/Rent/GetRentDetail?rentId={(Guid)Session["RentId"]}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var gatAllMovies = JsonConvert.DeserializeObject<Blasterify.Models.Model.RentDetailModel>(jsonString);

                Session["RentDetail"] = gatAllMovies;

                return gatAllMovies;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region VIEWS

        public async Task<ActionResult> Movie(int id)
        {
            var movie = await GetMovieAsync(id);
            return View(movie ?? new Blasterify.Models.Model.MovieModel());
        }

        public ActionResult RentConfirmation()
        {
            if (Session["ClientUser"] == null)
            {
                return RedirectToAction("LogIn", "Access");
            }
            else
            {
                return View(Session["YunoCredentials"]);
            }
        }

        public ActionResult CompletedRentDetail()
        {
            if (Session["ClientUser"] == null)
            {
                return RedirectToAction("LogIn", "Access");
            }

            return View();
        }

        #endregion

        #region REQUEST

        [HttpGet]
        public async Task<JsonResult> GetRentDetailRequest()
        {
            if (Session["RentDetail"] == null)
            {
                var rentDetail = await GetRentDetailAsync();

                return Json(
                    new Result(
                        true,
                        new
                        {
                            rentDetail = rentDetail ?? new Blasterify.Models.Model.RentDetailModel()
                        },
                        "Success"
                    ),
                    JsonRequestBehavior.AllowGet
                );
            }
            else
            {
                var rentDetail = Session["RentDetail"] as Blasterify.Models.Model.RentDetailModel;

                return Json(
                    new Result(
                        true,
                        new
                        {
                            rentDetail = rentDetail ?? new Blasterify.Models.Model.RentDetailModel()
                        },
                        "Success"
                    ),
                    JsonRequestBehavior.AllowGet
                );
            }
        }

        [HttpPost]
        public async Task<JsonResult> PayNowRequest(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Json(
                   new Result(
                       false,
                       new
                       {
                           Url = "/Home/Index"
                       },
                       "Invalid Card Holder Name"
                   ),
                   JsonRequestBehavior.AllowGet
               );
            }

            if (GetCart() != null && await CompleteRentAsync(token))
            {
                return Json(
                    new Result(
                        true,
                        new
                        {
                            Url = "/Shop/CompletedRentDetail"
                        },
                        "Success"
                    ),
                    JsonRequestBehavior.AllowGet
                );
            }

            return Json(
                new Result(
                    false,
                    new
                    {
                        Url = "/Home/Index"
                    },
                    "Error"
                ),
                JsonRequestBehavior.AllowGet
            );
        }

        [HttpPost]
        public JsonResult CancelRentConfirmationRequest()
        {
            return Json(
                new Result(
                    true,
                    new
                    {
                        Url = "/Home/Index"
                    },
                    "Canceling Rent"
                ),
                JsonRequestBehavior.AllowGet
            );
        }

        #endregion
    }
}