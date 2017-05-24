using AppTrucking.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppTrucking.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult Index()
        {
            List<OrderViewModels> orders;
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                orders = (from or in context.Orders
                          join mp in context.MapDatas on or.OrderId equals mp.OrderId
                          join us in context.Users on or.ApplicationUserId equals us.Id
                          join cr in context.Cars on or.CarId equals cr.CarId
                          join dr in context.Drivers on or.CarId equals dr.CarId
                          select new OrderViewModels()
                          {
                              #region Feilds
                              OrderId = or.OrderId,
                              BodyVolume = cr.BodyVolume,
                              CompanyName = us.CompanyName,
                              Description = or.Description,
                              Distance = mp.Distance,
                              Duration = mp.Duration,
                              E_mail = us.Email,
                              From = mp.From,
                              LiftingCapacity = cr.LiftingCapacity,
                              Name = us.Name,
                              OrderTime = or.OrderTime,
                              Prce = cr.Prce,
                              Skype = us.Skype,
                              Status = or.Status,
                              Surname = us.Surname,
                              Telephone = us.Telephone,
                              Title = cr.Title,
                              To = mp.To,
                              Tonnage = cr.Tonnage,
                              Total = or.Total,
                              Viber = us.Viber,
                              UserId = us.Id,
                              DriverName = dr.Name,
                              DriverSurName = dr.Surname,
                              DriverPhone = dr.Telephone,
                              OrderDate = or.OrderDate,
                              Services = or.Services.ToList()
                              #endregion
                          }).OrderByDescending(s=> s.OrderId).ToList();

                return View(orders);
            }
        }
        [Authorize(Roles = "User")]
        public ActionResult Order()
        {
            var carList = context.Cars.Where(s => s.IsFree == true).ToList();
            ViewBag.CarId = new SelectList(carList, "CarId", "Title");
            var free = carList.Count();
            if(free == 0)
            {
                ViewBag.Dis = free;
                ViewBag.NotFree = "Нажаль, немає вільних машин!";
            }
            ViewBag.ServiceList = context.Services.Distinct().ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Order(Order order, double distance, double time, string from, string to, string[] selectedService)
        {
            decimal serviceSum = 0;
            if (selectedService != null)
            {
                //order.Services = new List<Service>();
                foreach (var service in selectedService)
                {
                    var serviceToAdd = context.Services.Find(int.Parse(service));
                    serviceSum += serviceToAdd.Price;
                }
            }

            var getCar = context.Cars.Where(s => s.CarId == order.CarId).FirstOrDefault();
            
            var newOrder = new Order()
            {
                ApplicationUserId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(User.Identity),
                Status = false,
                Total = (getCar.Prce * Convert.ToDecimal(distance))+ serviceSum,
                Description = order.Description,
                OrderTime = DateTime.Now,
                CarId = order.CarId,
                Volume = order.Volume,
                Weight = order.Weight,
                OrderDate = order.OrderDate
            };
            getCar.IsFree = false;
            context.Entry(getCar).State = EntityState.Modified;
            if (order.Weight > getCar.Tonnage && order.Volume > getCar.BodyVolume)
            {
                throw new Exception("Вага перевищує тонаж машини!");
            }
            if (selectedService != null)
            {
                newOrder.Services = new List<Service>();
                foreach (var service in selectedService)
                {
                    var serviceToAdd = context.Services.Find(int.Parse(service));
                    newOrder.Services.Add(serviceToAdd);
                }
            }

            context.Orders.Add(newOrder);
            context.SaveChanges();

            var newMapData = new MapData()
            {
                OrderId = newOrder.OrderId,
                //DispatchTime = DateTime.Now,
                Duration = time,
                From = from,
                To = to,
                Distance = distance
            };

            context.MapDatas.Add(newMapData);
            context.SaveChanges();

            return RedirectToAction("Result");
        }
        [HttpGet]
        public ActionResult Result()
        {
            return View();
        }
        [HttpGet]
        public ActionResult About()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CarList()
        {
            var list = context.Cars.ToList();
            return View(list);
        }
        public ActionResult Services()
        {
            var list = context.Services.ToList();
            return View(list);
        }

    }
}