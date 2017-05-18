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

        //#region UserInfo
        //[Required(ErrorMessage = "Обов'язкове для заповнення")]
        //[Display(Name = "Назва компанії")]
        //public string CompanyName { get; set; }
        //[Required(ErrorMessage = "Обов'язкове для заповнення")]
        //[Display(Name = "Контактна особа")]
        //public string ContactPerson { get; set; }
        //[Required(ErrorMessage = "Обов'язкове для заповнення")]
        //[Display(Name = "Електронна адреса")]
        //[DataType(DataType.EmailAddress, ErrorMessage ="Некоректна електронна адреса")]
        //public string E_mail { get; set; }
        //[Display(Name = "Skype")]
        //[StringLength(15, MinimumLength = 3, ErrorMessage = "Довжина повинна бути від 3 до 15 символів!")]
        //public string Skype { get; set; }
        //[Display(Name = "Viber")]
        //public string Viber { get; set; }
        //[Required(ErrorMessage = "Обов'язкове для заповнення")]
        //[Display(Name = "Телефон")]
        //public string Telephone { get; set; }
        //#endregion

        //public virtual City City { get; set; }
        public virtual Car Car { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Service> Services { get; set; }
        public virtual ICollection<MapData> MapDatas { get; set; }
    }
}