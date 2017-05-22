using System;
using System.Collections.Generic;

namespace AppTrucking.Models
{
    public class OrderViewModels
    {

        public int OrderId { get; set; }
        public DateTime OrderTime { get; set; }
        public decimal Total { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
        public int Weight { get; set; }
        public int Volume { get; set; }
        public bool IsSent { get; set; }

        public string Title { get; set; }
        public string LiftingCapacity { get; set; }
        public int BodyVolume { get; set; }
        public double Tonnage { get; set; }
        public Decimal Prce { get; set; }
        public string CarNumber { get; set; }

        public string From { get; set; }
        public string To { get; set; }
        public double Distance { get; set; }
        public double Duration { get; set; }

        public string CompanyName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FullName { get { return String.Format($"{Name} {Surname}"); } }
        public string E_mail { get; set; }
        public string Skype { get; set; }
        public string Viber { get; set; }
        public string Telephone { get; set; }
        public string UserId { get; set; }


        public string DriverSurName { get; set; }
        public string DriverName { get; set; }
        public string DriverFullName { get { return String.Format($"{DriverName} {DriverSurName}"); } }
        public string DriverPhone { get; set; }

        public List<Service> Services { get; set; }
    }
}