using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppTrucking.Models
{
    public class City
    {
        public int CityId { get; set; }
        [Required(ErrorMessage ="Обов'язкове для заповнення")]
        [Display(Name ="Мітсо")]
        public string Title { get; set; }

       // public virtual ICollection<Order> Orders { get; set; }
    }
}