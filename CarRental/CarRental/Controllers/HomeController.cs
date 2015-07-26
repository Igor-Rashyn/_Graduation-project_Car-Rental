using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CarRental.DataContexts;
using CarRental.Filters;
using CarRental.Models;
using CarRental.Repositories;
using CarRental.WebUI.Models;
using Ninject;

namespace CarRental.Controllers
{
    [Culture]
    public class HomeController : Controller
    {
        IRepository<Car> CarRepository;
        IListViewModel<Car> listViewModel;
        int pageSize = 2;

        public HomeController(IRepository<Car> rep, IListViewModel<Car> model)
        {
            CarRepository = rep;
            listViewModel = model;
        }

        public ActionResult Index()
        {
            //byte[] bb = System.Text.Encoding.UTF8.GetBytes("KAPUSTA");
            //Car car = new Car();
            //car.Brand = "Mercedes"; // Opel, BMW, Toyota
            //car.Model = "600"; // Mega, 700, Corola
            //car.Passengers = 5; //4,2
            //car.Location = "Wellington"; // Auckland
            //car.FuelConsumption = 12; // 7, 6, 15
            //car.Price = 45; // 32, 12,20
            //car.AirConditioning = true;
            //car.AmountOfLuggage = "2 large luggage";
            //car.AmountOfLuggageRu = "2 больших багажа";
            //car.Transmission = true;
            //car.Id = Guid.NewGuid();
            //car.DateOfChange = DateTime.Now;
            //car.DateOfCreation = DateTime.Now;
            //car.PickupDateTime = DateTime.Now; // сделать у одного
            //car.ReturnDateTime = DateTime.Now; //
            //car.Picture = bb;
            //CarRepository.Create(car);
            //CarRepository.GetAll();

            return View();
        }

        public ActionResult ChangeCulture(string lang)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath;
            // List of cultures
            List<string> cultures = new List<string>() { "ru", "en" };
            if (!cultures.Contains(lang))
            {
                lang = "en";
            }
            // To save the selected culture into cookie
            HttpCookie cookie = Request.Cookies["lang"];
            if (cookie != null)
                cookie.Value = lang;   // if cookie is already set,  to update value
            else
            {

                cookie = new HttpCookie("lang");
                cookie.HttpOnly = false;
                cookie.Value = lang;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return Redirect(returnUrl);
        }

        public ActionResult Search(string inputPickupLocation, string inputReturnLocation, string inputPickupDate, string inputReturnDate)
        {
            return RedirectToAction("Index","Cars", new { nameWhereParam = "Search", inputPickupLocation=inputPickupLocation, inputPickupDate=inputPickupDate });
        }



        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        protected override void Initialize(RequestContext requestContext)
        {
            if (requestContext.HttpContext.Request.Url!=null)
            {
                
            }

            if (requestContext.RouteData.Values["lang"]!=null && requestContext.RouteData.Values["lang"] as string!="null")
            {
                
            }

            base.Initialize(requestContext);
        }
    }
}