using Hospital.Controllers;
using Hospital.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hospital.Areas.Admin.Controllers
{
    [Authorize(Roles = "CenterDeviceScheduleNotification")]
    public class CenterDeviceScheduleNotificationController : BaseController
    {
        private DBContext db = new DBContext();

        // GET: Admin/DeviceScheduleNotification
        public ActionResult Index()
        {
            ViewBag.Engineers = db.Engineer.Where(s => s.IsDeleted == false).ToList();
            var UserId = Convert.ToInt32(User.Identity.GetUserId());
            var Setting = db.Users.AsNoTracking().FirstOrDefault(s => s.Id == UserId);
            if (Setting.EngineerId.HasValue)
                ViewBag.SelectedEngineerId = Setting.EngineerId;
            else
                ViewBag.SelectedEngineerId = 0;
            ViewBag.CurrentYear = DateTime.Now.AddHours(10).Year;
            var CurrentDay = (int)DateTime.Now.AddHours(10).Day;
            var CurrentMonth = DateTime.Now.AddHours(10).Month;
            var Year = DateTime.Now.AddHours(10).Year;
            return View(db.CenterDeviceSchedule.AsNoTracking().Where(s => s.IsDeleted == false && CurrentDay >= s.DayId && CurrentMonth == s.MonthId && Year > s.LastCloseYear).Include(s => s.CenterDevice).Include(s => s.CenterDevice.CenterDeviceUnit).OrderByDescending(s => s.CenterDevice.Id));
        }

        public ActionResult CloseNotification(string Ids)
        {
            var arr = Ids.Split(',');
            foreach(var i in arr)
            {
                if (i == "") continue;
                var Id = Convert.ToInt32(i);
                var S = db.CenterDeviceSchedule.Find(Id);
                S.LastCloseYear = DateTime.Now.AddHours(10).Year;
                db.Entry(S).State = EntityState.Modified;
                db.SaveChanges();

                var D = db.CenterDevice.Find(S.CenterDeviceId);
                if(D.DeviceCategoryId == 1 || D.DeviceCategoryId == 2)
                {
                    D.LastModificationDate = DateTime.Now.AddHours(10);
                    db.Entry(D).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
    }
}