using AppTrucking.Models;
using System;
using System.Collections.Generic;
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
            return View();
        }

        public ActionResult Order()
        {
            ViewBag.CarId = new SelectList(context.Cars, "CarId", "Title");
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
                CarId = order.CarId
            };
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

            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}