using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppTrucking.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderTime { get; set; }
        //public int CityId { get; set; }
        //public int ArrivalCityId { get; set; }
        public int CarId { get; set; }
        [Display(Name = "Підсумок")]
        public decimal Total { get; set; }
        [Display(Name = "Статус")]
        public bool Status { get; set; }
        [Display(Name = "Додаткова інформація")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string ApplicationUserId { get; set; }
        [Display(Name = "Вага")]
        public int Weight { get; set; }
        [Display(Name = "Об'єм")]
        public int Volume { get; set; }
        public bool IsSent { get; set; } = false;
        
        public virtual Car Car { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Service> Services { get; set; }
        public virtual ICollection<MapData> MapDatas { get; set; }
    }
}