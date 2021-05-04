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
    [Authorize(Roles = "HealthCenterSchedule")]
    public class CenterDeviceScheduleController : BaseController
    {
        // GET: Admin/CenterDeviceSchedule
        private DBContext db = new DBContext();

        public ActionResult Index()
        {
            return View(db.CenterDevice.AsNoTracking().Where(s => s.IsDeleted == false && s.IsActive == true && s.DeviceSchedules.Count() > 0).Include(s => s.DeviceSchedules).Include(s => s.CenterDeviceUnit.Center).OrderByDescending(s => s.Id).ToList());
        }

        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.DayId = new SelectList(db.Day, "Id", "Name");
            ViewBag.MonthId = new SelectList(db.Month, "Id", LanguageID == 1 ? "NameAr" : "NameEn");
            ViewBag.CenterDeviceName = LanguageID == 1 ? db.CenterDevice.Find(id).NameAr : db.CenterDevice.Find(id).NameEn;
            return View(db.CenterDeviceSchedule.Where(s => s.CenterDeviceId == id).ToList());
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.DayId = new SelectList(db.Day, "Id", "Name");
            ViewBag.MonthId = new SelectList(db.Month, "Id", LanguageID == 1 ? "NameAr" : "NameEn");
            ViewBag.CenterDeviceName = LanguageID == 1 ? db.CenterDevice.Find(id).NameAr : db.CenterDevice.Find(id).NameEn;
            return View(db.CenterDeviceSchedule.Where(s => s.CenterDeviceId == id).ToList());
        }

        public ActionResult DeleteConfirmed(int id)
        {
            var CenterDeviceSchedule = db.CenterDeviceSchedule.Where(s => s.CenterDeviceId == id);
            db.CenterDeviceSchedule.RemoveRange(CenterDeviceSchedule);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteSelected(string Selected)
        {
            var List = Selected.Split(',');
            foreach (var item in List)
            {
                if (item != "" && item != "on")
                {
                    var Id = Convert.ToInt32(item);
                    var CenterDeviceSchedule = db.CenterDeviceSchedule.Where(s => s.CenterDeviceId == Id);
                    db.CenterDeviceSchedule.RemoveRange(CenterDeviceSchedule);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult SaveEditDeviceSchedual(string Dates)
        {
            var SplitDates = Dates.Split(',');

            for (var i = 0; i < SplitDates.Count(); i++)
            {
                if (SplitDates[i] != "")
                {
                    var Id = Convert.ToInt32(SplitDates[i].Split(':')[0]);
                    var Day = Convert.ToInt32(SplitDates[i].Split(':')[1]);
                    var Month = Convert.ToInt32(SplitDates[i].Split(':')[2]);

                    CenterDeviceSchedule Model = db.CenterDeviceSchedule.Find(Id);
                    Model.DayId = Day;
                    Model.MonthId = Month;
                    Model.ModifiedDate = DateTime.Now.AddHours(10);
                    Model.ModifierName = User.Identity.GetUserName();
                    db.Entry(Model).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
    }
}