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

        public ActionResult Search(string inputPickupLocation, string inputReturnLocation, string inputPickupDate, string inputReturnDate, bool? checkboxDifferentLocation)
        {
            return RedirectToAction("Index","Cars", new { searchString = "Search", inputPickupLocation=inputPickupLocation, inputReturnLocation= inputReturnLocation, inputPickupDate =inputPickupDate, inputReturnDate= inputReturnDate, checkboxDifferentLocation = checkboxDifferentLocation });
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