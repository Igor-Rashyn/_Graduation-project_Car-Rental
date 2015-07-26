using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarRental.DataContexts;
using CarRental.Models;
using CarRental.Repositories;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Migrations;
using CarRental.Filters;

namespace CarRental.Controllers
{
    [Culture]
    public class AdministrationController : Controller
    {
        IdentityDb context = new IdentityDb();
        IRepository<Car> CarRepository;
        IRepository<Picture> PictureRepository;

        public AdministrationController(IRepository<Car> rep, IRepository<Picture> pic )
        {
            CarRepository = rep;
            PictureRepository = pic;
        }


        // GET: Administration
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {  
            return View();
        }
        //GET: /Administration/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var roles = context.Roles.ToList();
            return View(roles);
        }
        // POST: /Administration/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                context.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole()
                {
                    Name = collection["RoleName"]
                });
                context.SaveChanges();
                ViewBag.ResultMessage = "Role created successfully !";
                return View();
            }
            catch
            {
                return View();
            }
        }
        // POST: Administration/Delete
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string RoleName)
        {
            var thisRole = context.Roles.Where(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            context.Roles.Remove(thisRole);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET: Administration/Edit/id
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string roleName)
        {
            var thisRole = context.Roles.Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            return View(thisRole);
        }

        //POST: Administration/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(Microsoft.AspNet.Identity.EntityFramework.IdentityRole role)
        {
            try
            {
                context.Entry(role).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //GET: Administration/Manager
        [Authorize(Roles = "Admin")]
        public ActionResult ManageUserRoles()
        {
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            return View();
        }
        //POST: Administration/RoleAddToUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult RoleAddToUser(string UserName, string RoleName)
        {
            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            var account = new AccountController();
            account.UserManager.AddToRole(user.Id, RoleName);

            ViewBag.ResultMessage = "Role created successfully !";

            // prepopulat roles for the view dropdown
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return View("ManageUserRoles");
        }

        //POST: Administration/GetRoles
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult GetRoles(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                var account = new AccountController();

                ViewBag.RolesForThisUser = account.UserManager.GetRoles(user.Id);

                // prepopulat roles for the view dropdown
                var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.Roles = list;
            }

            return View("ManageUserRoles");
        }

        //POST: Administration/DeleteRoleForUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteRoleForUser(string UserName, string RoleName)
        {
            var account = new AccountController();
            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            if (account.UserManager.IsInRole(user.Id, RoleName))
            {
                account.UserManager.RemoveFromRole(user.Id, RoleName);
                ViewBag.ResultMessage = "Role removed from this user successfully !";
            }
            else
            {
                ViewBag.ResultMessage = "This user doesn't belong to selected role.";
            }
            // prepopulat roles for the view dropdown
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return View("ManageUserRoles");
        }

        //GET: Administration/CreateUser
        [Authorize(Roles = "Admin")]
        public ActionResult CreateUser ()
        {
            var user = new ApplicationUser();
            return View(user);
        }

        //POST: Administration/CreateUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateUser(FormCollection collection)
        {
            try
            {
                context.Users.Add(new ApplicationUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = collection["Email"],
                    UserName = collection["UserName"],
                    PasswordHash = new PasswordHasher().HashPassword(collection["Password"]),
                    EmailConfirmed = true,
                    LockoutEnabled = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                });
                context.SaveChanges();
                ViewBag.ResultMessage = "Role created successfully !";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public JsonResult GetUsers([DataSourceRequest] DataSourceRequest request)
        {

            var users = context.Users.ToList().ToDataSourceResult(request);
            return Json(users, JsonRequestBehavior.AllowGet);

        }

        //GET: Administration/ListOfCars
        public ActionResult ListOfCars()
        {
            return View();
        }

        //GET: Administration/CreationCar
        public ActionResult CreationCar()
        {
            return View();
        }

        //POST: Administration/CreationCar
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult CreationCar(Car car, HttpPostedFileBase image)
        {
            //if (ModelState.IsValid)
            //{
            try
            {
                if (image != null)
                {
                    Picture pic = new Picture();
                    pic.ImageData = new byte[image.ContentLength];
                    pic.ImageMimeType = image.ContentType;
                    image.InputStream.Read(pic.ImageData,0,image.ContentLength);
                    car.Picture = pic;
                }
                CarRepository.Create(car);
                CarRepository.Save();
                //TempData["message"] = string.Format("Изменения в игре \"{0}\" были сохранены", car.CarNumber);
                return RedirectToAction("ListOfCars");
            }
            catch
            {
                // Что-то не так со значениями данных
                return View(car);
            }
        }

        public ActionResult EditCar(string Id)
        {
            if (Id != null)
            {
                Car car = CarRepository.Find(new Guid(Id));
                return View(car);
            }
            else
            {
                return RedirectToAction("ListOfCars");
            }
        }


        //POST: Administration/CreationCar
        [HttpPost]
        public ActionResult EditCar(Car car, HttpPostedFileBase img = null)
        {
            if (img != null)
            {
                Picture pic;
                if (car.PictureId == null)
                {
                    pic = new Picture();
                }
                else
                {
                    pic = PictureRepository.Find(car.PictureId);
                }
                pic.ImageData = new byte[img.ContentLength];
                pic.ImageMimeType = img.ContentType;
                img.InputStream.Read(pic.ImageData, 0, img.ContentLength);
                car.Picture = pic;
            }
            else
            {
                Picture pic = PictureRepository.Find(car.PictureId);
                car.Picture = pic;
            }
            car.DateOfChange = DateTime.Now;
            CarRepository.Update(car);
            CarRepository.Save();
            return RedirectToAction("ListOfCars");
        }

        public JsonResult GetCars([DataSourceRequest] DataSourceRequest request)
        {
            var cars = CarRepository.GetAll()
                .Select(c=> new Car {Id=c.Id,
                                     Price =c.Price,
                                     AdditionInformation =c.AdditionInformation,
                                     AdditionInformationRu =c.AdditionInformationRu,
                                     AirConditioning = c.AirConditioning,
                                     AmountOfLuggage =c.AmountOfLuggage,
                                     AmountOfLuggageRu =c.AmountOfLuggageRu,
                                     Brand =c.Brand,
                                     CarNumber =c.CarNumber,
                                     FuelConsumption =c.FuelConsumption,
                                     Location =c.Location,
                                     Model =c.Model,
                                     Passengers=c.Passengers,
                                     PictureId=c.PictureId,
                                     Transmission=c.Transmission,
                                     PickupDateTime=c.PickupDateTime,
                                     ReturnDateTime=c.ReturnDateTime})
                .Where(c=>c.Id!=null).ToList().ToDataSourceResult(request);
            return Json(cars, JsonRequestBehavior.AllowGet);
        }


        public FileContentResult RenderImage(string id)
        {
            Car car = CarRepository.Find(new Guid(id));
            if (car != null)
            {
                return File(car.Picture.ImageData, car.Picture.ImageMimeType);
            }
            else
            {
                return null;
            }
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateCar([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Car> cars)
        {
            if (cars != null)
            {
                foreach (var car in cars)
                {
                    car.DateOfChange = DateTime.Now;
                    car.DateOfCreation = DateTime.Now;
                    ////////car.Picture = System.Text.Encoding.UTF8.GetBytes("KAPUSTA");
                    CarRepository.Update(car);
                }
            }
            CarRepository.Save();

            return Json(cars.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult DestroyCar([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Car> cars)
    {
        if (cars.Any())
        {
            foreach (var car in cars)
            {
                CarRepository.Delete(car);
            }
        }
        CarRepository.Save();

        return Json(cars.ToDataSourceResult(request, ModelState));
    }




}

}
