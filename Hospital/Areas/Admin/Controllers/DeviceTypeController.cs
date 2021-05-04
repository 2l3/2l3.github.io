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
    [Authorize(Roles = "DeviceType")]
    public class DeviceTypeController : BaseController
    {
        private DBContext db = new DBContext();

        public ActionResult Index()
        {
            return View(db.DeviceType.Where(s => s.IsDeleted == false).OrderByDescending(s => s.Id).ToList());
        }

        public ActionResult DeleteSelected(string Selected)
        {
            var List = Selected.Split(',');
            foreach (var item in List)
            {
                if (item != "")
                {
                    var Id = Convert.ToInt32(item);
                    DeviceType DeviceType = db.DeviceType.Find(Id);
                    db.DeviceType.Remove(DeviceType);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

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
        public ActionResult Create(DeviceType DeviceType, string Save, string SaveAndContinue)
        {
            if (db.DeviceType.Any(s => s.NameAr.ToLower().Equals(DeviceType.NameAr)
             || s.NameEn.ToLower().Equals(DeviceType.NameEn)))
            {
                ViewBag.Message = LanguageID == 1 ? "نوع الجهاز  مكرر" : "Device Type is already exist";
                return View(DeviceType);
            }
            if (ModelState.IsValid)
            {
                DeviceType.IsDeleted = false;
                DeviceType.IsActive = true;
                DeviceType.CreatedDate = DateTime.Now.AddHours(10);
                DeviceType.CreatorName = User.Identity.GetUserName();
                db.DeviceType.Add(DeviceType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Message = LanguageID == 1 ? "أكمل البيانات من فضلك" : "Please complete data";
            return View(DeviceType);
        }

        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceType DeviceType = db.DeviceType.Find(id);
            if (DeviceType == null)
            {
                return HttpNotFound();
            }

            return View(DeviceType);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DeviceType DeviceType)
        {
            if (db.DeviceType.Where(s => s.Id != DeviceType.Id).Any(s => s.NameAr.ToLower().Equals(DeviceType.NameAr)
             || s.NameEn.ToLower().Equals(DeviceType.NameEn)))
            {
                ViewBag.Message = LanguageID == 1 ? "نوع الجهاز مكرر" : "Device Type is already exist";
                return View(DeviceType);
            }
            if (ModelState.IsValid)
            {
                DeviceType.IsDeleted = false;
                DeviceType.IsActive = true;
                DeviceType.ModifiedDate = DateTime.Now.AddHours(10);
                DeviceType.ModifierName = User.Identity.GetUserName();
                db.Entry(DeviceType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Message = LanguageID == 1 ? "أكمل البيانات من فضلك" : "Please complete data";
            return View(DeviceType);
        }


        // POST: Admin/Categories/Delete/5
        public ActionResult DeleteConfirmed(int id)
        {
            DeviceType DeviceType = db.DeviceType.Find(id);
            db.DeviceType.Remove(DeviceType);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}