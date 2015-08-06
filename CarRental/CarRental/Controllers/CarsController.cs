using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using CarRental.Filters;
using CarRental.Models;
using CarRental.Repositories;
using CarRental.WebUI.Models;

namespace CarRental.Controllers
{
    [Culture]
    public class CarsController : Controller
    {

        IRepository<Car> CarRepository;
        IListViewModel<Car> listViewModel;
        IRepository<Order> OrderRepository;
        int pageSize = 2;

        public CarsController(IRepository<Car> rep, IListViewModel<Car> model, IRepository<Order> orderRep)
        {
            CarRepository = rep;
            listViewModel = model;
            OrderRepository = orderRep;
        }
       
        // GET: Cars/Index
       public ActionResult Index(string inputPickupLocation, string inputReturnLocation, string inputPickupDate, string inputReturnDate, string sortOrder, bool? checkboxDifferentLocation, string currentFilter, string searchString, int page = 1)
        {
            IEnumerable<Car> sortCars;

            if (checkboxDifferentLocation==false || checkboxDifferentLocation==null)
            {
                inputReturnLocation = inputPickupLocation;
            }


            Session["CurrentSort"] = sortOrder;
            Session["PriceSortParm"]=sortOrder=="Price_Up" ? "Price_Down" : "Price_Up";
            Session["SizeSortParm"] = sortOrder == "Size_Up" ? "Size_Down" : "Size_Up";
            Session["ConsumptionSortParm"] = sortOrder == "Consumption_Up" ? "Consumption_Down" : "Consumption_Up";

            if (inputPickupDate==null && inputPickupLocation==null)
            {
                searchString = currentFilter;
            }

            Session["CurrentFilter"] = searchString;

#region  Search    
            if (!String.IsNullOrEmpty(searchString))
            {
                if (!String.IsNullOrEmpty(inputPickupDate) && !String.IsNullOrEmpty(inputPickupLocation))
                {
                   var datePickup = ConvertDate(inputPickupDate);
                    Session["PickupDate"] = datePickup;
                    Session["PickupDateOriginal"] = Convert.ToString(inputPickupDate);
                    Session["PickupLocation"] = inputPickupLocation;
                    Session["ReturnLocation"] = inputReturnLocation;
                    Session["DifferentLocation"] = checkboxDifferentLocation;
                }
                else if (!String.IsNullOrEmpty(inputPickupLocation))
                {
                    Session["PickupLocation"] = inputPickupLocation;
                    Session["PickupDate"] = null;
                }
                else if (!String.IsNullOrEmpty(inputPickupDate))
                {
                    Session["PickupLocation"] = null;
                    var datePickup = ConvertDate(inputPickupDate);
                    Session["PickupDate"] = datePickup;
                }

                if (!String.IsNullOrEmpty(inputReturnDate))
                {
                   var datereturn = ConvertDate(inputReturnDate);
                    Session["ReturnDate"] = datereturn;
                    Session["ReturnDateOriginal"] = Convert.ToString(inputReturnDate);
                }


                if (!String.IsNullOrEmpty(Session["PickupLocation"] as string) && !String.IsNullOrEmpty(Session["PickupDate"]as string))
                {
                    sortCars = CarRepository.GetAll().Where(s => s.Location.Contains(Session["PickupLocation"]as string)
                      && (s.ReturnDateTime < Convert.ToDateTime(Session["PickupDate"]) || s.ReturnDateTime ==null));
                }
                else if (!String.IsNullOrEmpty(Session["PickupDate"] as string))
                {
                    sortCars = CarRepository.GetAll().Where(s => s.ReturnDateTime < Convert.ToDateTime(Session["PickupDate"])
                                               || s.Status == "Available");
                }
                else
                {
                    sortCars = CarRepository.GetAll().Where(s => s.Location.Contains(Session["PickupLocation"] as string)
                                      && s.Status == "Available");
                }
            }
            else
            {
                sortCars = CarRepository.GetAll().Where(p => p.Status == "Available");
            }
#endregion

#region Sort conditions
            switch (sortOrder)
            {
                case "Price_Up":
                    sortCars = sortCars.OrderBy(p => p.Price);
                    break;
                case "Price_Down":
                    sortCars = sortCars.OrderByDescending(p => p.Price);
                    break;
                case "Size_Up":
                    sortCars = sortCars.OrderBy(p => p.Passengers);
                    break;
                case "Size_Down":
                    sortCars = sortCars.OrderByDescending(p => p.Passengers);
                    break;
                case "Consumption_Up":
                    sortCars = sortCars.OrderBy(p => p.FuelConsumption);
                    break;
                case "Consumption_Down":
                    sortCars = sortCars.OrderByDescending(p => p.FuelConsumption);
                    break;
                default:
                    sortCars = sortCars.OrderBy(car => car.Id);
                    break;
            }
#endregion         

            listViewModel.Model = sortCars
                                .Skip((page - 1) * pageSize)
                                .Take(2);
            listViewModel.PagingInfo = new PageInfo
                                        {
                                            CurrentPage = page,
                                            ItemsPerPage = pageSize,
                                            TotalItems = sortCars.Count()
                                        };
            return Request.IsAjaxRequest()?(ActionResult)PartialView("SortView", listViewModel) : View(listViewModel);
        }

    // GET: Cars/Order
       public ActionResult Order(FormCollection collection)
        {
            Car car=null;
            Guid newGuid = Guid.Empty;
            try {
                newGuid = Guid.Parse(collection["item.Id"]);
                car = CarRepository.Find(newGuid);
            }
            catch
            {

            }

            DateTime dtPickup = DateTime.Now; // НУЖНО ЗАМЕНИТЬ БД УСТАНОВИТЬ ЧТОБЫ ДАТА МОГЛА БЫТЬ NULL!!!!!!!!!!!!
            DateTime dtReturn = DateTime.Now;

            if (!String.IsNullOrEmpty(Session["PickupDateOriginal"] as string))
            {
                string pickupDate = ConvertDateForOrder((Session["PickupDateOriginal"] as string).Replace("/", "."));
                dtPickup = Convert.ToDateTime(pickupDate);
            }

            if (!String.IsNullOrEmpty(Session["ReturnDateOriginal"] as string))
            {
                string returnDate = ConvertDateForOrder((Session["ReturnDateOriginal"] as string).Replace("/", "."));
                dtReturn = Convert.ToDateTime(returnDate);
            }

            Order order = new Order()
            {
                Car = car,
                CarId = newGuid,
                PickupDateTime = dtPickup,
                PickupLocation = Session["PickupLocation"] as string, 
                ReturnLocation = Session["DifferentLocation"] as bool?==true? Session["ReturnLocation"] as string : Session["PickupLocation"] as string, 
                ReturnDateTime = dtReturn
        };

            if (!String.IsNullOrEmpty((Session["PickupDateOriginal"] as string)) & !String.IsNullOrEmpty((Session["ReturnDateOriginal"] as string)))
            {
                TimeSpan days = (TimeSpan)(order.ReturnDateTime - order.PickupDateTime);
                int day = Convert.ToInt32(days.Days);
                ViewBag.TotalPrice = day * order.Car.Price;
                ViewBag.Currency = "USD";
            }

            return View(order);
        }


        // POST: Cars/Order/Create
       [HttpPost]
       [ValidateAntiForgeryToken]
        public ActionResult Create (Order order)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Car car = CarRepository.Find(order.CarId);
                    car.Status = "Booked";
                    car.StatusRu = "Забронировано";

                    order.DateOfChange = DateTime.Now;
                    order.TotalPrice = ((TimeSpan)(order.ReturnDateTime - order.PickupDateTime)).Days * car.Price;
                    order.CarId = car.Id;

                    //SendMail(Convert.ToString(order.ApplicationNumber), order.Email, order.FirstName+" "+order.LastName);

                    OrderRepository.Create(order);
                    OrderRepository.Save();
                    CarRepository.Update(car);

                    return RedirectToAction("Index");
                }
            }
            catch(RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(order);
        }


        private void SendMail(string number, string mail, string name)
        {
            MailMessage message = new MailMessage();
            message.To.Add(new MailAddress(mail));
            message.From = new MailAddress("nimbik@gmail.com");
            message.Subject = "Car renteal. Your order number: " + number;
            message.Body = @"Hello  " + name + "\n"
                        + "This is the number of your order. Please use this to get the car. " + "\n"
                        + "Number: " + number;
            message.IsBodyHtml = false;

            SmtpClient client = new SmtpClient();
            try
            {
                var credential = new NetworkCredential {
                    UserName = "test@gmail.com",
                    Password = "test"
                };
                client.Credentials = credential;
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.Send(message);
                Email email = new Email {
                    Body = message.Body,
                    From= "nimbik@gmail.com",
                    To =mail,
                    Subject = message.Subject
                };
            }

            catch
            {
            }
        }

       
        private string ConvertDate (string line)
        {
            var str = line.Substring(0, 10);
            var listStr = str.Split('/').ToList();
            return   listStr[2] + "/" + listStr[0] + "/" + listStr[1];
        }


        private string ConvertDateForOrder(string line)
        {
            var str = line.Substring(0, 10);
            var listStr = str.Split('.').ToList();
            return listStr[1] + "." + listStr[0] + "." + listStr[2];
        }

        private DateTime ConverDateForDatabase (DateTime date)
        {// YYYY-MM-DD HH:
            var str = Convert.ToString(date);
            str = str.Substring(0, 10); // 2015-06-30 12:12:59.530
            var listStr = str.Split('.').ToList();
            return Convert.ToDateTime(listStr[2] + "-" + listStr[1] + "-" + listStr[0]);
        }
    
    }
}