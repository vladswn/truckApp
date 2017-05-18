using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppTrucking.Models
{
    public class Driver
    {
        public int DriverId { get; set; }
        [Required(ErrorMessage = "Обов'язкове для заповнення")]
        [Display(Name = "Ім'я")]
        public string Name { get; set; }
        [Display(Name = "Прізвище")]
        public string Surname { get; set; }
        [Display(Name = "Skype")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Довжина повинна бути від 3 до 15 символів!")]
        public string Skype { get; set; }
        [Display(Name = "Viber")]
        public string Viber { get; set; }
        [Required(ErrorMessage = "Обов'язкове для заповнення")]
        [Display(Name = "Телефон")]
        public string Telephone { get; set; }
        public int CarId { get; set; }

        public virtual Car Car { get; set; }
    }
}