using Hospital.Controllers;
using Hospital.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Hospital.Areas.Admin.Controllers
{
    [Authorize(Roles = "Engineer")]
    public class EngineerController : BaseController
    {
        private DBContext db = new DBContext();
        // GET: Admin/Categories
        public ActionResult Index()
        {
            return View(db.Engineer.Where(s => s.IsDeleted == false).OrderByDescending(s => s.Id).ToList());
        }

        // GET: Admin/Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult DeleteSelected(string Selected)
        {
            var List = Selected.Split(',');
            foreach (var item in List)
            {
                if (item != "")
                {
                    var Id = Convert.ToInt32(item);
                    Engineer Engineer = db.Engineer.Find(Id);
                    db.Engineer.Remove(Engineer);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Engineer Engineer, string Save, string SaveAndContinue, HttpPostedFileBase Img)
        {
            if (db.Engineer.Any(s => s.NameAr.ToLower().Equals(Engineer.NameAr)
             || s.NameEn.ToLower().Equals(Engineer.NameEn)))
            {
                ViewBag.Message = LanguageID == 1 ? "أسم المهندس مكرر" : "Engineer is already exist";
                return View(Engineer);
            }
            if (ModelState.IsValid)
            {
                Engineer.IsDeleted = false;
                Engineer.IsActive = true;
                Engineer.CreatedDate = DateTime.Now.AddHours(10);
                Engineer.CreatorName = User.Identity.GetUserName();
                if (Img != null)
                {
                    Random R = new Random();
                    int rand = R.Next(1, 100000);
                    var x = rand + DateTime.Now.AddHours(10).ToString() + Img.FileName;
                    x = ToSafeFileName(x);
                    Img.SaveAs(Server.MapPath("~/Content/Uploads/Engineer/" + x));
                    Engineer.SignImg = x;
                }
                db.Engineer.Add(Engineer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Message = LanguageID == 1 ? "أكمل البيانات من فضلك" : "Please complete data";
            return View(Engineer);
        }

        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Engineer Engineer = db.Engineer.Find(id);
            if (Engineer == null)
            {
                return HttpNotFound();
            }

            return View(Engineer);
        }

        private string ToSafeFileName(string s)
        {
            return s
                .Replace("\\", "")
                .Replace("/", "")
                .Replace("\"", "")
                .Replace("*", "")
                .Replace(":", "")
                .Replace("?", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace(" ", "")
                .Replace("|", "");
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Engineer Engineer, HttpPostedFileBase Img)
        {
            if (db.Engineer.Where(s => s.Id != Engineer.Id).Any(s => s.NameAr.ToLower().Equals(Engineer.NameAr)
             || s.NameEn.ToLower().Equals(Engineer.NameEn)))
            {
                ViewBag.Message = LanguageID == 1 ? "أسم المهندس مكرر" : "Engineer is already exist";
                return View(Engineer);
            }
            if (ModelState.IsValid)
            {
                Engineer.IsDeleted = false;
                Engineer.IsActive = true;
                Engineer.ModifiedDate = DateTime.Now.AddHours(10);
                Engineer.ModifierName = User.Identity.GetUserName();
                if (Img != null)
                {
                    Random R = new Random();
                    int rand = R.Next(1, 100000);
                    var x = rand + DateTime.Now.AddHours(10).ToString() + Img.FileName;
                    x = ToSafeFileName(x);
                    Img.SaveAs(Server.MapPath("~/Content/Uploads/Engineer/" + x));
                    Engineer.SignImg = x;
                }
                db.Entry(Engineer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Message = LanguageID == 1 ? "أكمل البيانات من فضلك" : "Please complete data";
            return View(Engineer);
        }


        // POST: Admin/Categories/Delete/5
        public ActionResult DeleteConfirmed(int id)
        {
            Engineer Engineer = db.Engineer.Find(id);
            db.Engineer.Remove(Engineer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}