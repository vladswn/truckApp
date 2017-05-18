using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;

namespace AppTrucking.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        //[Required(ErrorMessage = "Обов'язкове для заповнення")]
        //[Display(Name = "Назва компанії")]
        public string CompanyName { get; set; }
        //[Required(ErrorMessage = "Обов'язкове для заповнення")]
        //[Display(Name = "Ім'я")]
        public string Name { get; set; }
        [Display(Name = "Прізвище")]
        public string Surname { get; set; }
        [Display(Name = "Skype")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Довжина повинна бути від 3 до 15 символів!")]
        public string Skype { get; set; }
        [Display(Name = "Viber")]
        public string Viber { get; set; }
        //[Required(ErrorMessage = "Обов'язкове для заповнення")]
        //[Display(Name = "Телефон")]
        public string Telephone { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

    }
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        [Display(Name = "Опис")]
        public string Description { get; set; }
        public ApplicationRole(string name) : base(name) { }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Car> Cars { get; set; }
        //public DbSet<City> Cities { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<TypeOfTransport> TypeOfTransports { get; set; }
        public DbSet<MapData> MapDatas { get; set; }
        public DbSet<Order> Orders { get; set; }
        //public DbSet<Driver> Drivers { get; set; }
        static ApplicationDbContext()
        {
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}