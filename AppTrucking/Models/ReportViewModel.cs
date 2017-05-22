using System;

namespace AppTrucking.Models
{
    public class ReportViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FullName
        {
            get
            {
                return string.Format($"{Name} {Surname}");
            }
        }
        public string CarName { get; set; }
        public Decimal Sum { get; set; }
        public Decimal Total { get; set; } 
    }
}