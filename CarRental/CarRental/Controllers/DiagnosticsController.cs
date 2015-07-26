using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarRental.Filters;
using CarRental.Models;
using CarRental.Repositories;
using PagedList;

namespace CarRental.Controllers
{
    [Culture]
    public class DiagnosticsController : Controller
    {
        IRepository<Order> OrderRepository;
        IRepository<Car> CarRepository;
        IRepository<Diagnostics> DiagnosticsRepository;

        public DiagnosticsController(IRepository<Order> orderRep, IRepository<Car> carRep, IRepository<Diagnostics> diagRep )
        {
            OrderRepository = orderRep;
            CarRepository = carRep;
            DiagnosticsRepository = diagRep;
        }

        // GET: Diagnostics
        [Authorize(Roles = "Admin, ServiceCenter")]
        public ActionResult Index(int page = 1)
        {
            IEnumerable<Diagnostics> diagnostic = DiagnosticsRepository.GetAll().Where(p=> p.Id!=null);
            return View(diagnostic.ToPagedList(page, 3));
        }

        //GET: Diagnostics/Create
        [Authorize(Roles = "Admin, ServiceCenter")]
        public ActionResult Create ()
        {
            return View();
        }

        //POST: Diagnostics/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, ServiceCenter")]
        public ActionResult Create(FormCollection collection)
        {
            var orderNumber = collection["OrderId_input"];
            var orderId = collection["OrderId"]; 
            var failure = collection["DetectedFailure"];
            var description = collection["Description"];
            var cost = collection["RepairCost"];

            var order = OrderRepository.Find(new Guid(orderId));
            order.Status = "Closed";
            order.StatusRu = "Закрыта";

            var car = CarRepository.Find(order.CarId);
            car.PickupDateTime = null;
            car.ReturnDateTime = null;
            car.Status = "Available";
            car.StatusRu = "Доступный";

            Diagnostics diag = new Diagnostics();

            if (cost != "")
            {              
                diag.RepairCost = Convert.ToInt32(cost);
            }
            diag.OrderId = order.Id;
            diag.DateOfCreation = DateTime.Now;
            diag.CarId = order.CarId;
            diag.Description = description;


            DiagnosticsRepository.Create(diag);
            CarRepository.Update(car);
            OrderRepository.Update(order);
            DiagnosticsRepository.Save();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin, ServiceCenter")]
        public ActionResult Detail(string id)
        {
            var diag = DiagnosticsRepository.Find(new Guid(id));

            return View(diag);
        }

        public JsonResult GetOrders()
        {
            var orders = from o in OrderRepository.GetAll()
                         join c in CarRepository.GetAll() on o.CarId equals c.Id
                         where o.Status== "Confirmed"
                         select new
                         {
                             Model = c.Model,
                             Price = c.Price,
                             Brand = c.Brand,
                             CarNumber = c.CarNumber,
                             ApplicationNumber = o.ApplicationNumber,
                             OrderId= o.Id
                         };
            return Json(orders, JsonRequestBehavior.AllowGet);

        }
    }
}