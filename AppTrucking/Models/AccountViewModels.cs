using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AppTrucking.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Електронна адреса")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запам'ятати мене?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Електронна адреса")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Некоректна електронна адреса")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Пароль має бути {0} не менше ніж {2} довжини.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Підтвердити пароль")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Паролі не співпадають.")]
        public string ConfirmPassword { get; set; }
        //[Required(ErrorMessage = "Обов'язкове для заповнення")]
        [Display(Name = "Назва компанії")]
        public string CompanyName { get; set; }
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
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Електронна адреса")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Некоректна електронна адреса")]
        public string Email { get; set; }
        [Display(Name = "Назва компанії")]
        public string CompanyName { get; set; }
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

        public IEnumerable<SelectListItem> RolesList { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
