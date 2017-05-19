using AppTrucking.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AppTrucking.Controllers
{
    public class AdminController : Controller
    {
        public AdminController() { }
        public AdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }
        ApplicationDbContext context = new ApplicationDbContext();

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> UsersList()
        {
            var list = await UserManager.Users.ToListAsync();
            
            return View(list);
        }

        public async Task<ActionResult> UserDetails(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var user = await UserManager.FindByIdAsync(id);
            ViewBag.RoleList = await UserManager.GetRolesAsync(user.Id);
            return View(user);
        }

        // GET: /Users/Create
        public async Task<ActionResult> Create()
        {
            //Get the list of Roles
            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            return View();
        }

        //
        // POST: /Users/Create
        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel userViewModel, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = userViewModel.Email,
                    Email = userViewModel.Email,
                    // Add the Address Info:
                    Name = userViewModel.Name,
                    Surname = userViewModel.Surname,
                    Skype = userViewModel.Skype,
                    Viber = userViewModel.Viber,
                    Telephone = userViewModel.Telephone,
                    CompanyName = userViewModel.CompanyName,
                };



                // Then create:
                var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First());
                            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                            return View();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First());
                    ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
                    return View();

                }
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
            return View();
        }


        // GET: /Users/Edit/1
        public async Task<ActionResult> EditUser(string id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userRoles = await UserManager.GetRolesAsync(user.Id);

            EditUserViewModel nd = new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                CompanyName = user.CompanyName,
                Skype = user.Skype,
                Surname = user.Surname,
                Telephone = user.Telephone,
                Viber = user.Viber,
                RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                })
            };

            return View(nd);
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUser(EditUserViewModel editUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                user.UserName = editUser.Email;
                user.Email = editUser.Email;
                user.Name = editUser.Name;
                user.Surname = editUser.Surname;
                user.Email = editUser.Email;
                user.CompanyName = editUser.CompanyName;
                user.Skype = editUser.Skype;
                user.Telephone = editUser.Telephone;
                user.Viber = editUser.Viber;


                var userRoles = await UserManager.GetRolesAsync(user.Id);

                selectedRole = selectedRole ?? new string[] { };

                var result = await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }

        public async Task<ActionResult> DeleteUser(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var user = await UserManager.FindByIdAsync(id);
            return View(user);
        }

        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteUserConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return HttpNotFound();
                }
                var user = await UserManager.FindByIdAsync(id);

                var delete = await UserManager.DeleteAsync(user);

                if (!delete.Succeeded)
                {
                    ModelState.AddModelError("", delete.Errors.First());
                    return View();
                }
                return RedirectToAction("UsersList");
            }
            return View();
        }

        public ActionResult DriverList()
        {
            var list = context.Drivers.ToList();
            return View(list);
        }
        [HttpGet]
        public ActionResult AddDriver()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddDriver(Driver driver)
        {
            context.Drivers.Add(driver);
            context.SaveChanges();
            return RedirectToAction("DriverList");
        }
        [HttpGet]
        public ActionResult EditDriver(int id)
        {
            var driver = context.Drivers.Find(id);
            return View(driver);
        }
        [HttpPost]
        public ActionResult EditDriver(Driver driver)
        {
            context.Entry(driver).State = EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("DriverList");
        }
        public ActionResult DeleteDriver(int id)
        {
            var driver = context.Drivers.Find(id);
            return View(driver);
        }

        //[HttpPost, ActionName("DeleteDriver")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteDriverConfirmed(int id)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var driver = context.Drivers.Find(id);
        //        if (id == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        var user = await UserManager.FindByIdAsync(id);

        //        var delete = await UserManager.DeleteAsync(user);

        //        if (!delete.Succeeded)
        //        {
        //            ModelState.AddModelError("", delete.Errors.First());
        //            return View();
        //        }
        //        return RedirectToAction("UsersList");
        //    }
        //    return View();
        //}

    }
}