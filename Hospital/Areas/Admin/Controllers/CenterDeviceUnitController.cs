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
    [Authorize(Roles = "HealthCenterCategory")]
    public class CenterDeviceUnitController : BaseController
    {
        private DBContext db = new DBContext();
        // GET: Admin/Categories
        public ActionResult Index()
        {
            return View(db.CenterDeviceUnit.Include(s => s.Center).OrderByDescending(s => s.Id).ToList());
        }

        // GET: Admin/Categories/Create
        public ActionResult Create()
        {
            ViewBag.CenterId = LanguageID == 1 ? new SelectList(db.Center.Where(s => s.IsDeleted == false && s.IsActive == true), "Id", "NameAr")
                : new SelectList(db.Center.Where(s => s.IsDeleted == false && s.IsActive == true), "Id", "NameEn");
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CenterDeviceUnit CenterDeviceUnit, string Save, string SaveAndContinue)
        {
            if (db.CenterDeviceUnit.Any(s => s.CenterId == CenterDeviceUnit.CenterId && (s.NameAr.ToLower().Equals(CenterDeviceUnit.NameAr)
             || s.NameEn.ToLower().Equals(CenterDeviceUnit.NameEn))))
            {
                ViewBag.CenterId = LanguageID == 1 ? new SelectList(db.Center.Where(s => s.IsDeleted == false && s.IsActive == true), "Id", "NameAr", CenterDeviceUnit.CenterId)
            : new SelectList(db.Center.Where(s => s.IsDeleted == false && s.IsActive == true), "Id", "NameEn", CenterDeviceUnit.CenterId);

                ViewBag.Message = LanguageID == 1 ? "أسم القسم مكرر" : "Department is already exist";
                return View(CenterDeviceUnit);
            }
            if (ModelState.IsValid)
            {
                db.CenterDeviceUnit.Add(CenterDeviceUnit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CenterId = LanguageID == 1 ? new SelectList(db.Center.Where(s => s.IsDeleted == false && s.IsActive == true), "Id", "NameAr", CenterDeviceUnit.CenterId)
               : new SelectList(db.Center.Where(s => s.IsDeleted == false && s.IsActive == true), "Id", "NameEn", CenterDeviceUnit.CenterId);
            ViewBag.Message = LanguageID == 1 ? "أكمل البيانات من فضلك" : "Please complete data";
            return View(CenterDeviceUnit);
        }

        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CenterDeviceUnit CenterDeviceUnit = db.CenterDeviceUnit.Find(id);
            if (CenterDeviceUnit == null)
            {
                return HttpNotFound();
            }
            ViewBag.CenterId = LanguageID == 1 ? new SelectList(db.Center.Where(s => s.IsDeleted == false && s.IsActive == true), "Id", "NameAr", CenterDeviceUnit.CenterId)
              : new SelectList(db.Center.Where(s => s.IsDeleted == false && s.IsActive == true), "Id", "NameEn", CenterDeviceUnit.CenterId);
            return View(CenterDeviceUnit);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CenterDeviceUnit CenterDeviceUnit)
        {
            var List = db.CenterDeviceUnit.AsNoTracking().Where(s => s.CenterId == CenterDeviceUnit.CenterId && (s.Id > CenterDeviceUnit.Id || s.Id < CenterDeviceUnit.Id) && (s.NameAr.Equals(CenterDeviceUnit.NameAr)
             || s.NameEn.Equals(CenterDeviceUnit.NameEn)));
            if (List.Count() > 0)
            {
                ViewBag.CenterId = LanguageID == 1 ? new SelectList(db.Center.Where(s => s.IsDeleted == false && s.IsActive == true), "Id", "NameAr", CenterDeviceUnit.CenterId)
            : new SelectList(db.Center.Where(s => s.IsDeleted == false && s.IsActive == true), "Id", "NameEn", CenterDeviceUnit.CenterId);

                ViewBag.Message = LanguageID == 1 ? "أسم القسم مكرر" : "Department is already exist";
                return View(CenterDeviceUnit);
            }
            if (ModelState.IsValid)
            {
                db.Entry(CenterDeviceUnit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CenterId = LanguageID == 1 ? new SelectList(db.Center.Where(s => s.IsDeleted == false && s.IsActive == true), "Id", "NameAr", CenterDeviceUnit.CenterId)
              : new SelectList(db.Center.Where(s => s.IsDeleted == false && s.IsActive == true), "Id", "NameEn", CenterDeviceUnit.CenterId);
            ViewBag.Message = LanguageID == 1 ? "أكمل البيانات من فضلك" : "Please complete data";
            return View(CenterDeviceUnit);
        }


        // POST: Admin/Categories/Delete/5
        public ActionResult DeleteConfirmed(int id)
        {
            CenterDeviceUnit CenterDeviceUnit = db.CenterDeviceUnit.Find(id);
            db.CenterDeviceUnit.Remove(CenterDeviceUnit);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteSelected(string Selected)
        {
            var List = Selected.Split(',');
            foreach (var item in List)
            {
                if (item != "")
                {
                    var Id = Convert.ToInt32(item);
                    CenterDeviceUnit CenterDeviceUnit = db.CenterDeviceUnit.Find(Id);
                    db.CenterDeviceUnit.Remove(CenterDeviceUnit);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
    }
}