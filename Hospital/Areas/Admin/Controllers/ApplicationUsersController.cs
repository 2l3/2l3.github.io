using Hospital.Controllers;
using Hospital.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Hospital.Areas.Admin.Controllers
{
    [Authorize(Roles = "ApplicationUsers")]
    public class ApplicationUsersController : BaseController
    {
        private DBContext db = new DBContext();
        private IAuthenticationManager _authenticationManager = null;

        // GET: Admin/ApplicationUsers
        public ActionResult Index()
        {
            return View(db.Users.Where(s => s.IsDeleted == false && s.Id != 1).Include(s => s.Engineer).OrderByDescending(s => s.Id).ToList());
        }

        [Authorize(Roles = "ApplicationUsersAdd")]
        // GET: Admin/ApplicationUsers/Create
        public ActionResult Create()
        {
            ViewBag.EngineerId = new SelectList(db.Engineer.Where(s => s.IsDeleted == false), "Id", LanguageID == 1 ? "NameAr" : "NameEn");
            return View();
        }

        // POST: Admin/ApplicationUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateUserViewModel CreateUser, string Save, string SaveAndContinue)
        {
            if (CreateUser.Password.Length < 6)
            {
                ViewBag.Message = LanguageID == 1 ? "الرقم السري يجب الأ يقل عن 6 احرف" : "Password Cannot be less than 6 character";
                ViewBag.EngineerId = new SelectList(db.Engineer.Where(s => s.IsDeleted == false), "Id", LanguageID == 1 ? "NameAr" : "NameEn", CreateUser.EngineerId);
                return View(CreateUser);
            }
            if (db.Users.Any(s => s.Email.ToLower().Equals(CreateUser.Email.ToLower())))
            {
                ViewBag.Message = LanguageID == 1 ? "البريد الألكتروني مكرر" : "Email already exist";
                ViewBag.EngineerId = new SelectList(db.Engineer.Where(s => s.IsDeleted == false), "Id", LanguageID == 1 ? "NameAr" : "NameEn", CreateUser.EngineerId);
                return View(CreateUser);
            }
            if (db.Users.Any(s => s.UserName.ToLower().Equals(CreateUser.UserName.ToLower())))
            {
                ViewBag.Message = LanguageID == 1 ? " أسم المستخدم مكرر" : "Username already exist";
                ViewBag.EngineerId = new SelectList(db.Engineer.Where(s => s.IsDeleted == false), "Id", LanguageID == 1 ? "NameAr" : "NameEn", CreateUser.EngineerId);
                return View(CreateUser);
            }
            IdentityManager _identityManager = new IdentityManager();
            var user = new ApplicationUser();
            user.UserName = CreateUser.UserName;
            user.Email = CreateUser.Email;
            user.PhoneNumber = CreateUser.PhoneNumber;
            user.Name = CreateUser.Name;
            user.EngineerId = CreateUser.EngineerId;
            user.IsActive = true;
            user.IsDeleted = false;
            user.CreatedDate = DateTime.Now.AddHours(10);
            user.ModifiedDate = DateTime.Now.AddHours(10);
            // Creating The User----------------------------------------------------
            var status = _identityManager.CreateUser(user, CreateUser.Password);
            if (status.Succeeded)
            {
                db.SaveChanges();
                return RedirectToAction("Index");
                //------------------------------------------------------------------------
            }
            ViewBag.EngineerId = new SelectList(db.Engineer.Where(s => s.IsDeleted == false), "Id", LanguageID == 1 ? "NameAr" : "NameEn", CreateUser.EngineerId);
            ViewBag.Message = LanguageID == 1 ? "أكمل البيانات من فضلك" : "Please complete data";
            return View(CreateUser);
        }


        [Authorize(Roles = "ApplicationUsersEdit")]
        // GET: Admin/ApplicationUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            CreateUserViewModel CreateUser = new CreateUserViewModel();
            CreateUser.Email = applicationUser.Email;
            CreateUser.UserName = applicationUser.UserName;
            CreateUser.PhoneNumber = applicationUser.PhoneNumber;
            CreateUser.Password = "";
            CreateUser.EngineerId = applicationUser.EngineerId;
            CreateUser.Name = applicationUser.Name;
            ViewBag.EngineerId = new SelectList(db.Engineer.Where(s => s.IsDeleted == false), "Id", LanguageID == 1 ? "NameAr" : "NameEn", CreateUser.EngineerId);
            return View(CreateUser);
        }

        [Authorize(Roles = "ApplicationUsersRoles")]
        public ActionResult Roles(int Id)
        {
            ViewBag.User = db.Users.Find(Id);
            ViewBag.UserRoles = db.UserRole.AsNoTracking().Where(s => s.UserId == Id);
            return View(db.Roles.AsNoTracking().Where(s => s.Id != 1));
        }

        public ActionResult EditUser(CreateUserViewModel CreateUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = db.Users.Find(CreateUser.Id);
                applicationUser.ModifiedBy = Convert.ToInt32(User.Identity.GetUserId());
                applicationUser.ModifierName = User.Identity.GetUserName();
                applicationUser.UserName = CreateUser.UserName;
                applicationUser.PhoneNumber = CreateUser.PhoneNumber;
                applicationUser.Email = CreateUser.Email;
                applicationUser.EngineerId = CreateUser.EngineerId;
                applicationUser.Name = CreateUser.Name;
                db.Entry(applicationUser).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.EngineerId = new SelectList(db.Engineer.Where(s => s.IsDeleted == false), "Id", LanguageID == 1 ? "NameAr" : "NameEn", CreateUser.EngineerId);
            return View("Edit", CreateUser);
        }

        // POST: Admin/ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreateUserViewModel CreateUser)
        {
            if (ModelState.IsValid)
            {
                var Id = CreateUser.Id;
                if (CreateUser.Password.Length < 6)
                {
                    ViewBag.Message = LanguageID == 1 ? "الرقم السري يجب الأ يقل عن 6 خانات" : "Password Cannot be less than 6 entries";
                    ViewBag.EngineerId = new SelectList(db.Engineer.Where(s => s.IsDeleted == false), "Id", LanguageID == 1 ? "NameAr" : "NameEn", CreateUser.EngineerId);
                    return View(CreateUser);
                }
                if (db.Users.Any(s => s.Email.ToLower().Equals(CreateUser.Email.ToLower()) && s.Id != Id))
                {
                    ViewBag.Message = LanguageID == 1 ? "البريد الألكتروني مكرر" : "Email already exist";
                    ViewBag.EngineerId = new SelectList(db.Engineer.Where(s => s.IsDeleted == false), "Id", LanguageID == 1 ? "NameAr" : "NameEn", CreateUser.EngineerId);
                    return View(CreateUser);
                }
                if (db.Users.Any(s => s.UserName.ToLower().Equals(CreateUser.UserName.ToLower()) && s.Id != Id))
                {
                    ViewBag.Message = LanguageID == 1 ? " أسم المستخدم مكرر" : "Username already exist";
                    ViewBag.EngineerId = new SelectList(db.Engineer.Where(s => s.IsDeleted == false), "Id", LanguageID == 1 ? "NameAr" : "NameEn", CreateUser.EngineerId);
                    return View(CreateUser);
                }

                if (CreateUser.Password.Contains('*'))
                    return EditUser(CreateUser);

                CreateUser.Id = 0;
                IdentityManager _identityManager = new IdentityManager();
                var user = new ApplicationUser();
                user.UserName = CreateUser.UserName;
                user.Email = CreateUser.Email;
                user.PhoneNumber = CreateUser.PhoneNumber;
                user.Name = CreateUser.Name;
                user.IsActive = true;
                user.IsDeleted = false;
                user.CreatedDate = DateTime.Now.AddHours(10);
                user.EngineerId = CreateUser.EngineerId;
                user.ModifiedDate = DateTime.Now.AddHours(10);

                DeleteConfirmed(Id);

                // Creating The User----------------------------------------------------
                var status = _identityManager.CreateUser(user, CreateUser.Password);
                if (status.Succeeded)
                {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                    //------------------------------------------------------------------------
                }
            }
            ViewBag.Message = LanguageID == 1 ? "أكمل البيانات من فضلك" : "Please complete data";
            ViewBag.EngineerId = new SelectList(db.Engineer.Where(s => s.IsDeleted == false), "Id", LanguageID == 1 ? "NameAr" : "NameEn", CreateUser.EngineerId);
            return View(CreateUser);
        }

        public async Task<IdentityResult> ChangePassword(int UserId, string NewPassword)
        {
            var UserManager = new UserManager<ApplicationUser, int>(new UserStore<ApplicationUser, Role, int, UserLogin, UserRole, UserClaim>(new DBContext()));
            // User does not have a password so remove any validation errors caused by a missing OldPassword field
            ModelState state = ModelState["OldPassword"];
            if (state != null)
            {
                state.Errors.Clear();
            }

            if (ModelState.IsValid)
            {
                await UserManager.RemovePasswordAsync(UserId);
                IdentityResult result = await UserManager.AddPasswordAsync(UserId, NewPassword);
            }
            return await Task.FromResult<IdentityResult>(IdentityResult.Success);
        }


        [Authorize(Roles = "ApplicationUsersDelete")]
        // POST: Admin/ApplicationUsers/Delete/5
        public ActionResult DeleteConfirmed(int id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser != null)
            {
                db.Users.Remove(applicationUser);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult SaveRoles(int UserId, string Selected)
        {
            var AllRoles = db.UserRole.Where(s => s.UserId == UserId);
            db.UserRole.RemoveRange(AllRoles);
            db.SaveChanges();

            var List = Selected.Split(',');
            foreach (var item in List)
            {
                if (item != "")
                {
                    var RoleId = Convert.ToInt32(item);

                    var UserRole = new UserRole();
                    UserRole.UserId = UserId;
                    UserRole.RoleId = RoleId;

                    db.UserRole.Add(UserRole);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }
    }
}
