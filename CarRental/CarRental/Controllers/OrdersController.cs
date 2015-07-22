using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CarRental.Models;
using CarRental.Repositories;
using Newtonsoft.Json;
using PagedList;

namespace CarRental.Controllers
{
    public class OrdersController : Controller
    {

        IRepository<Order> OrderRepository;
        IRepository<Car> CarRepository;

        
        public OrdersController(IRepository<Order> orderRep, IRepository<Car> carRep)
        {
            OrderRepository = orderRep;
            CarRepository = carRep;
        }


        // GET: Orders
        [Authorize(Roles = "Admin, CarFleet")]
        public ViewResult Index(int page=1)
        {
            IEnumerable<Order> orders = OrderRepository.GetAll().Where(p => p.Status == "New");

            return View(orders.ToPagedList(page, 3));
        }

        //GET: Orders/Edit/5
        [Authorize(Roles = "Admin, CarFleet")]
        public ActionResult Edit (string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }  

            Guid guid = new Guid(id);
            Order order = OrderRepository.Find(guid);
            if (order==null)
            {
                return HttpNotFound();
            }

            var cars = CarRepository.GetAll()
                .Where(p => p.Location.Contains(order.PickupLocation) && (p.ReturnDateTime<order.PickupDateTime || p.ReturnDateTime==null))
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Brand + " " + s.Model + " (№ " + s.CarNumber + ", Price: " + s.Price + ")" });

            ViewBag.CurrentCar = JsonConvert.SerializeObject(order.Car);

            ViewBag.Cars = new SelectList(cars, "Value", "Text", cars.Where(x => x.Value == order.CarId.ToString()));

            var carList = JsonConvert.SerializeObject(CarRepository.GetAll()
                .Where(p => p.Location.Contains(order.PickupLocation) && (p.ReturnDateTime < order.PickupDateTime || p.ReturnDateTime == null)).ToList());

            ViewBag.CarList = carList;

            var days = (TimeSpan)(order.ReturnDateTime - order.PickupDateTime);
            int day = Convert.ToInt32(days.Days);
            ViewBag.TotalPrice = day * order.Car.Price;
            ViewBag.Currency = "USD";

            return View(order);
        }

        // POST: Orders/Confirm/id
        // [HttpPost]
        [Authorize(Roles = "Admin, CarFleet")]
        public ActionResult Confirm (string id)
        {
            Guid guid = new Guid(id);
            var order = OrderRepository.Find(guid);
            var car = CarRepository.Find(order.CarId);

            car.PickupDateTime =order.PickupDateTime;
            car.ReturnDateTime =order.ReturnDateTime;
            car.Location =order.ReturnLocation;
            car.Status = "Rented";
            car.StatusRu = "Арендована";

            order.Status = "Confirmed";
            order.StatusRu = "Подтверждена";

            OrderRepository.Update(order);
            OrderRepository.Save();
            CarRepository.Update(car);
            CarRepository.Save();

            return RedirectToAction("Index");
        }


        // POST: Orders/Save/id
        // [HttpPost]
        [Authorize(Roles = "Admin, CarFleet")]
        [ValidateAntiForgeryToken]
        public ActionResult Save(FormCollection collection)
        {
            var iid = collection["Id"];
            var listStr = iid.Split(',').ToList();
            Guid guidOrder = new Guid(listStr[0]);
            Guid guidCar = new Guid(collection["CarId"]);
            var order = OrderRepository.Find(guidOrder);
            var car = CarRepository.Find(guidCar);

            car.PickupDateTime = Convert.ToDateTime(collection["PickupDateTime"]);
            car.ReturnDateTime = Convert.ToDateTime(collection["ReturnDateTime"]);
            car.Location = collection["ReturnLocation"];
            car.Status = "Rented";
            car.StatusRu = "Арендована";

            order.PickupDateTime = Convert.ToDateTime(car.PickupDateTime);
            order.ReturnDateTime = Convert.ToDateTime(car.ReturnDateTime);
            order.ReturnLocation = car.Location;
            order.Phone = collection["Phone"];
            order.Email = collection["Email"];
            order.FirstName = collection["FirstName"];
            order.LastName = collection["LastName"];
            order.DateOfBirthday= Convert.ToDateTime(collection["DateOfBirthday"]);
            order.Address= collection["Address"];
            order.City = collection["City"];
            order.StateOrProvince = collection["StateOrProvince"];
            order.PostcodeOrZip = collection["PostcodeOrZip"];
            order.CountryOfResidence = collection["CountryOfResidence"];
            TimeSpan days = (TimeSpan)(order.ReturnDateTime - order.PickupDateTime);
            int day = Convert.ToInt32(days.Days);
            order.TotalPrice = day * car.Price;

            order.Status = "Confirmed";
            order.StatusRu = "Подтверждена";
            order.CarId = car.Id;

            OrderRepository.Update(order);
            OrderRepository.Save();
            CarRepository.Update(car);
            CarRepository.Save();

            return RedirectToAction("Index");
        }

        //POST: Orders/Delete/id

        // [HttpPost]
        [Authorize(Roles = "Admin, CarFleet")]
        public ActionResult Delete (string id)
        {
            Guid guid = new Guid(id);
            var order = OrderRepository.Find(guid);
            var car = CarRepository.Find(order.CarId);

            car.Status = "Available";
            car.StatusRu = "Доступный";

            OrderRepository.Delete(order);
            OrderRepository.Save();
            CarRepository.Update(car);
            CarRepository.Save();

            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Admin, CarFleet")]
        public ActionResult Change(string location, DateTime date)
        {
            var test = CarRepository.GetAll().Where(p => p.Location.Contains(location) && (p.ReturnDateTime < date || p.ReturnDateTime == null));

            var carList = JsonConvert.SerializeObject(CarRepository.GetAll()
    .Where(p => p.Location.Contains(location) && (p.ReturnDateTime < date || p.ReturnDateTime == null)).ToList());

            ViewBag.CarList = carList;

            return Json(test, JsonRequestBehavior.AllowGet);
        }

    }
}