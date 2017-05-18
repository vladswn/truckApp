using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppTrucking.Models
{
    public class TypeOfTransport
    {
        public int TypeOfTransportId { get; set; }
        [Required(ErrorMessage = "Обов'язкове для заповнення")]
        [Display(Name = "Тип")]
        public string Title { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}