using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppTrucking.Models
{
    public class Service
    {
        public int ServiceId { get; set; }
        [Required(ErrorMessage = "Обов'язкове для заповнення")]
        [Display(Name = "Послуга")]
        public string Title { get; set; }
        [Display(Name = "Опис")]
        public string Description { get; set; }
        [Display(Name = "Вартість послуги")]
        public Decimal Price { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}