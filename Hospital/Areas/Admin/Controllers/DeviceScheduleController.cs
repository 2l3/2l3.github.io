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
    [Authorize(Roles = "HospitalSchedule")]
    public class DeviceScheduleController : BaseController
    {
        // GET: Admin/DeviceSchedule
        private DBContext db = new DBContext();

        public ActionResult Index()
        {
            return View(db.Device.AsNoTracking().Where(s => s.IsDeleted == false && s.IsActive == true && s.DeviceSchedules.Count() > 0).Include(s => s.DeviceSchedules).Include(s => s.DeviceUnit).OrderByDescending(s => s.Id).ToList());
        }

        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.DayId = new SelectList(db.Day, "Id", "Name");
            ViewBag.MonthId = new SelectList(db.Month, "Id", LanguageID == 1 ? "NameAr" : "NameEn");
            ViewBag.DeviceName = LanguageID == 1 ? db.Device.Find(id).NameAr : db.Device.Find(id).NameEn;
            return View(db.DeviceSchedule.Where(s => s.DeviceId == id).ToList());
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.DayId = new SelectList(db.Day, "Id", "Name");
            ViewBag.MonthId = new SelectList(db.Month, "Id", LanguageID == 1 ? "NameAr" : "NameEn");
            ViewBag.DeviceName = LanguageID == 1 ? db.Device.Find(id).NameAr : db.Device.Find(id).NameEn;
            return View(db.DeviceSchedule.Where(s => s.DeviceId == id).ToList());
        }

        public ActionResult DeleteConfirmed(int id)
        {
            var DeviceSchedule = db.DeviceSchedule.Where(s => s.DeviceId == id);
            db.DeviceSchedule.RemoveRange(DeviceSchedule);
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
                    var DeviceSchedule = db.DeviceSchedule.Where(s => s.DeviceId == Id);
                    db.DeviceSchedule.RemoveRange(DeviceSchedule);
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

                    DeviceSchedule Model = db.DeviceSchedule.Find(Id);
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