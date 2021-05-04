using Hospital.Controllers;
using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Hospital.Areas.Admin.Controllers
{
    [Authorize(Roles = "HospitalCategory")]
    public class DeviceUnitController : BaseController
    {
        private DBContext db = new DBContext();
        // GET: Admin/Categories
        public ActionResult Index()
        {
            return View(db.DeviceUnit.OrderByDescending(s => s.Id).ToList());
        }

        public ActionResult DeleteSelected(string Selected)
        {
            var List = Selected.Split(',');
            foreach (var item in List)
            {
                if (item != "")
                {
                    var Id = Convert.ToInt32(item);
                    DeviceUnit DeviceUnit = db.DeviceUnit.Find(Id);
                    db.DeviceUnit.Remove(DeviceUnit);
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
        public ActionResult Create(DeviceUnit DeviceUnit, string Save, string SaveAndContinue)
        {
            if (db.DeviceUnit.Any(s => s.NameAr.ToLower().Equals(DeviceUnit.NameAr)
             || s.NameEn.ToLower().Equals(DeviceUnit.NameEn)))
            {
                ViewBag.Message = LanguageID == 1 ? "أسم القسم مكرر" : "Department is already exist";
                return View(DeviceUnit);
            }
            if (ModelState.IsValid)
            {
                db.DeviceUnit.Add(DeviceUnit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Message = LanguageID == 1 ? "أكمل البيانات من فضلك" : "Please complete data";
            return View(DeviceUnit);
        }

        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceUnit DeviceUnit = db.DeviceUnit.Find(id);
            if (DeviceUnit == null)
            {
                return HttpNotFound();
            }

            return View(DeviceUnit);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DeviceUnit DeviceUnit)
        {
            var List = db.DeviceUnit.AsNoTracking().Where(s => (s.Id > DeviceUnit.Id || s.Id < DeviceUnit.Id) && (s.NameAr.Equals(DeviceUnit.NameAr)
             || s.NameEn.Equals(DeviceUnit.NameEn)));
            if (List.Count() > 0)
            {
                ViewBag.Message = LanguageID == 1 ? "أسم القسم مكرر" : "Department is already exist";
                return View(DeviceUnit);
            }
            if (ModelState.IsValid)
            {
                db.Entry(DeviceUnit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Message = LanguageID == 1 ? "أكمل البيانات من فضلك" : "Please complete data";
            return View(DeviceUnit);
        }


        // POST: Admin/Categories/Delete/5
        public ActionResult DeleteConfirmed(int id)
        {
            DeviceUnit DeviceUnit = db.DeviceUnit.Find(id);
            db.DeviceUnit.Remove(DeviceUnit);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}