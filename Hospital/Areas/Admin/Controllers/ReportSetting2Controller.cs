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
    [Authorize(Roles = "HospitalPPMReportSetting")]
    public class ReportSetting2Controller : BaseController
    {
        private DBContext db = new DBContext();
        // GET: Admin/Categories
        public ActionResult Index()
        {
            return View(db.ReportSetting2.OrderBy(s => s.Sort).ToList());
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
        public ActionResult Create(ReportSetting2 ReportSetting2, string Save, string SaveAndContinue)
        {
            if (ModelState.IsValid)
            {
                db.ReportSetting2.Add(ReportSetting2);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Message = LanguageID == 1 ? "أكمل البيانات من فضلك" : "Please complete data";
            return View(ReportSetting2);
        }

        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportSetting2 ReportSetting2 = db.ReportSetting2.Find(id);
            if (ReportSetting2 == null)
            {
                return HttpNotFound();
            }

            return View(ReportSetting2);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReportSetting2 ReportSetting2)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ReportSetting2).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Message = LanguageID == 1 ? "أكمل البيانات من فضلك" : "Please complete data";
            return View(ReportSetting2);
        }


        // POST: Admin/Categories/Delete/5
        public ActionResult DeleteConfirmed(int id)
        {
            ReportSetting2 ReportSetting2 = db.ReportSetting2.Find(id);
            db.ReportSetting2.Remove(ReportSetting2);
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
                    ReportSetting2 ReportSetting2 = db.ReportSetting2.Find(Id);
                    db.ReportSetting2.Remove(ReportSetting2);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
    }
}