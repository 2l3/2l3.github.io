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
    [Authorize(Roles = "Center")]
    public class CenterController : BaseController
    {
        private DBContext db = new DBContext();

        public ActionResult Index()
        {
            return View(db.Center.Where(s => s.IsDeleted == false).OrderByDescending(s => s.Id).ToList());
        }

        [Authorize(Roles = "CenterAdd")]
        // GET: Admin/Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Center Center, string Save, string SaveAndContinue)
        {
            if (db.Center.Any(s => s.NameAr.ToLower().Equals(Center.NameAr)
             || s.NameEn.ToLower().Equals(Center.NameEn)))
            {
                ViewBag.Message = LanguageID == 1 ? "أسم المركز مكرر" : "Center is already exist";
                return View(Center);
            }
            if (ModelState.IsValid)
            {
                Center.IsDeleted = false;
                Center.IsActive = true;
                Center.CreatedDate = DateTime.Now.AddHours(10);
                Center.CreatorName = User.Identity.GetUserName();
                db.Center.Add(Center);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Message = LanguageID == 1 ? "أكمل البيانات من فضلك" : "Please complete data";
            return View(Center);
        }

        [Authorize(Roles = "CenterEdit")]
        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Center Center = db.Center.Find(id);
            if (Center == null)
            {
                return HttpNotFound();
            }

            return View(Center);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Center Center)
        {
            if (db.Center.Where(s => s.Id != Center.Id).Any(s => s.NameAr.ToLower().Equals(Center.NameAr)
             || s.NameEn.ToLower().Equals(Center.NameEn)))
            {
                ViewBag.Message = LanguageID == 1 ? "أسم المركز مكرر" : "Center is already exist";
                return View(Center);
            }
            if (ModelState.IsValid)
            {
                Center.IsDeleted = false;
                Center.IsActive = true;
                Center.ModifiedDate = DateTime.Now.AddHours(10);
                Center.ModifierName = User.Identity.GetUserName();
                db.Entry(Center).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Message = LanguageID == 1 ? "أكمل البيانات من فضلك" : "Please complete data";
            return View(Center);
        }


        [Authorize(Roles = "CenterDelete")]
        // POST: Admin/Categories/Delete/5
        public ActionResult DeleteConfirmed(int id)
        {
            Center Center = db.Center.Find(id);
            db.Center.Remove(Center);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "CenterDelete")]
        public ActionResult DeleteSelected(string Selected)
        {
            var List = Selected.Split(',');
            foreach(var item in List)
            {
                if(item != "")
                {
                    var Id = Convert.ToInt32(item);
                    Center Center = db.Center.Find(Id);
                    db.Center.Remove(Center);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
    }
}