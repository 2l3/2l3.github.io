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
    [Authorize(Roles = "HealthCenterReportSetting")]
    public class CenterReportSettingController : BaseController
    {
        private DBContext db = new DBContext();
        // GET: Admin/Categories
        public ActionResult Index()
        {
            return View(db.CenterReportSetting.OrderBy(s => s.Sort).ToList());
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
                .Replace("|", "");
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
        public ActionResult Create(CenterReportSetting CenterReportSetting, string Save, string SaveAndContinue)
        {
            if (ModelState.IsValid)
            {
                db.CenterReportSetting.Add(CenterReportSetting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Message = LanguageID == 1 ? "أكمل البيانات من فضلك" : "Please complete data";
            return View(CenterReportSetting);
        }

        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CenterReportSetting CenterReportSetting = db.CenterReportSetting.Find(id);
            if (CenterReportSetting == null)
            {
                return HttpNotFound();
            }

            return View(CenterReportSetting);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CenterReportSetting CenterReportSetting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(CenterReportSetting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Message = LanguageID == 1 ? "أكمل البيانات من فضلك" : "Please complete data";
            return View(CenterReportSetting);
        }


        // POST: Admin/Categories/Delete/5
        public ActionResult DeleteConfirmed(int id)
        {
            CenterReportSetting CenterReportSetting = db.CenterReportSetting.Find(id);
            db.CenterReportSetting.Remove(CenterReportSetting);
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
                    CenterReportSetting CenterReportSetting = db.CenterReportSetting.Find(Id);
                    db.CenterReportSetting.Remove(CenterReportSetting);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
    }
}