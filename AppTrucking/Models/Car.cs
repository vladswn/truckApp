using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppTrucking.Models
{
    public class Car
    {
        public int CarId { get; set; }
        [Required(ErrorMessage = "Обов'язкове для заповнення")]
        [Display(Name = "Модель машини")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Обов'язкове для заповнення")]
        [Display(Name = "Вантажопідйомність")]
        public string LiftingCapacity { get; set; }
        [Required(ErrorMessage = "Обов'язкове для заповнення")]
        [Display(Name = "Обсяг кузова")]
        [Range(minimum:1,maximum:200, ErrorMessage ="Мінімальний обсяг кузова - 1 м.куб. ")]
        public int BodyVolume { get; set; }
        [Required(ErrorMessage = "Обов'язкове для заповнення")]
        [Display(Name = "Тонаж")]
        [Range(minimum: 0.1, maximum: Double.MaxValue, ErrorMessage = "Мінімальний обсяг кузова - 100 кг ")]
        public double Tonnage { get; set; }
        [Display(Name = "Вартість")]
        public Decimal Prce { get; set; }
        [Display(Name = "Тип транспорту")]
        public int TypeOfTransportId { get; set; }
        public Boolean IsFree { get; set; } = true;
        public string Number { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual TypeOfTransport TypeOfTransport { get; set; }
        public virtual ICollection<Driver> Drivers { get; set; }
    }
}