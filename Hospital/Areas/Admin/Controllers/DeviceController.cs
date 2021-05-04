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
    [Authorize]
    public class DeviceController : BaseController
    {
        private DBContext db = new DBContext();
        // GET: Admin/Device
        [Authorize(Roles = "HospitalDevice")]
        public ActionResult Index()
        {
            return View(db.Device.AsNoTracking().Where(s => s.IsDeleted == false).Include(s => s.DeviceSchedules).Include(s => s.DeviceUnit).OrderByDescending(s => s.Id).ToList());
        }

        // GET: Admin/Categories/Create
        public ActionResult Create()
        {
            ViewBag.DeviceUnitId = new SelectList(db.DeviceUnit, "Id", LanguageID == 1 ? "NameAr" : "NameEn");
            ViewBag.DeviceTypeId = new SelectList(db.DeviceType, "Id", LanguageID == 1 ? "NameAr" : "NameEn");
            return View();
        }

        public ActionResult PrintBarCodeMulti(string Ids)
        {
            List<Device> Devices = new List<Device>();
            var arr = Ids.Split(',');
            foreach(var item in arr)
            {
                if (item == "") continue;
                int Id = Convert.ToInt32(item);
                Devices.Add(db.Device.FirstOrDefault(s => s.Id == Id));
            }
            return View(Devices);
        }

        public ActionResult DevicePPM()
        {
            ViewBag.Centers = db.Center.AsNoTracking().Where(s => s.IsDeleted == false);
            ViewBag.CenterDeviceUnits = db.CenterDeviceUnit.AsNoTracking();
            ViewBag.Engineers = db.Engineer.Where(s => s.IsDeleted == false).ToList();
            ViewBag.DeviceTypes = db.DeviceType.AsNoTracking().Where(s => s.IsDeleted == false);
            var UserId = Convert.ToInt32(User.Identity.GetUserId());
            var Setting = db.Users.AsNoTracking().FirstOrDefault(s => s.Id == UserId);
            if (Setting.EngineerId.HasValue)
                ViewBag.SelectedEngineerId = Setting.EngineerId;
            else
                ViewBag.SelectedEngineerId = 0;

            return View(db.DeviceUnit.AsNoTracking());
        }

        public ActionResult PMMReportPrint(string DateFrom,string DateTo,int DeviceUnitId,int? EngineerId,int DeviceCategoryId,int DeviceTypeId,int ScheduleTypeId,int Daman)
        {
            ViewBag.DateFrom = DateFrom;
            ViewBag.DateTo = DateTo;
            ViewBag.Engineer = EngineerId != 0 ? db.Engineer.FirstOrDefault(s => s.Id == EngineerId).NameAr: "....................................";
            var EngineerSignImg = EngineerId != 0 ? db.Engineer.FirstOrDefault(s => s.Id == EngineerId).SignImg : null;
            if (EngineerSignImg == null || EngineerSignImg == "")
                ViewBag.EngineerSignImg = "";
            else
                ViewBag.EngineerSignImg = EngineerSignImg;
            var Models = new List<Device>();

            if (ScheduleTypeId == 1)
                Models = DeviceUnitId == 0 ? db.Device.AsNoTracking().Where(s => s.IsDeleted == false && s.DeviceSchedules.Count() > 0).Include(s => s.DeviceUnit).ToList()
                    : db.Device.AsNoTracking().Where(s => s.IsDeleted == false && s.DeviceUnitId == DeviceUnitId && s.DeviceSchedules.Count() > 0).Include(s => s.DeviceUnit).ToList();
            else if (ScheduleTypeId == 2)
                Models = DeviceUnitId == 0 ? db.Device.AsNoTracking().Where(s => s.IsDeleted == false && s.DeviceSchedules.Count() == 0).Include(s => s.DeviceUnit).ToList()
                    : db.Device.AsNoTracking().Where(s => s.IsDeleted == false && s.DeviceUnitId == DeviceUnitId && s.DeviceSchedules.Count() == 0).Include(s => s.DeviceUnit).ToList();
            else
                Models = DeviceUnitId == 0 ? db.Device.AsNoTracking().Where(s => s.IsDeleted == false).Include(s => s.DeviceUnit).ToList()
                    : db.Device.AsNoTracking().Where(s => s.IsDeleted == false && s.DeviceUnitId == DeviceUnitId).Include(s => s.DeviceUnit).ToList();
            if (Daman == 1)
                Models = Models.Where(s => s.DamanExpireDate.HasValue && s.DamanExpireDate.Value.Date.Ticks >= DateTime.Now.AddHours(10).Date.Ticks).ToList();
            else if (Daman == 2)
                Models = Models.Where(s => s.DamanExpireDate.HasValue == false || s.DamanExpireDate.Value.Date.Ticks < DateTime.Now.AddHours(10).Date.Ticks).ToList();
            if (DeviceTypeId != 0)
                Models = Models.Where(s => s.DeviceTypeId == DeviceTypeId).ToList();
            if (DeviceCategoryId != 0)
                Models = Models.Where(s => s.DeviceCategoryId == DeviceCategoryId).ToList();
            ViewBag.DeviceUnit = db.DeviceUnit.FirstOrDefault(s => s.Id == DeviceUnitId);
            ViewBag.AboveReportSetting = db.ReportSetting2.Where(s => s.Above == 1).OrderBy(s => s.Sort);
            ViewBag.BottomReportSetting = db.ReportSetting2.Where(s => s.Above == 2).OrderBy(s => s.Sort);
            ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
            return View("PMMReportPrint", Models);
        }

        public ActionResult PMMReportPrintMulti(string DateFrom, string DateTo, string SelectedDevice,int? EngineerId)
        {
            var Arr = SelectedDevice.Split(',');

            ViewBag.DateFrom = DateFrom;
            ViewBag.DateTo = DateTo;
            ViewBag.Engineer = EngineerId != 0 ? db.Engineer.FirstOrDefault(s => s.Id == EngineerId).NameAr : "....................................";
            var EngineerSignImg = EngineerId != 0 ? db.Engineer.FirstOrDefault(s => s.Id == EngineerId).SignImg : null;
            if (EngineerSignImg == null || EngineerSignImg == "")
                ViewBag.EngineerSignImg = "";
            else
                ViewBag.EngineerSignImg = EngineerSignImg;
            ViewBag.AboveReportSetting = db.ReportSetting2.Where(s => s.Above == 1).OrderBy(s => s.Sort);
            ViewBag.BottomReportSetting = db.ReportSetting2.Where(s => s.Above == 2).OrderBy(s => s.Sort);
            ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;

            var Models = new List<Device>();
            foreach(var i in Arr)
            {
                if (i == "") continue;
                var Id = Convert.ToInt32(i);
                var S = db.DeviceSchedule.Find(Id);
                if (S.LastPrintPPMYear != DateTime.Now.AddHours(10).Year)
                {
                    S.LastPrintPPMYear = DateTime.Now.AddHours(10).Year;
                    db.Entry(S).State = EntityState.Modified;
                    db.SaveChanges();

                    var Device = db.Device.Find(S.DeviceId);
                    Device.LastModificationDate = DateTime.Now.AddHours(10);
                    db.Entry(Device).State = EntityState.Modified;
                    db.SaveChanges();
                }

                Models.Add(db.Device.AsNoTracking().Include(v => v.DeviceUnit).FirstOrDefault(s=> s.Id == S.DeviceId));
            }

            return View("PMMReportPrintMulti", Models);
        }

        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device Device = db.Device.Find(id);
            if (Device == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeviceTypeId = new SelectList(db.DeviceType, "Id", LanguageID == 1 ? "NameAr" : "NameEn", Device.DeviceTypeId);
            ViewBag.DeviceUnitId = new SelectList(db.DeviceUnit, "Id", LanguageID == 1 ? "NameAr" : "NameEn", Device.DeviceUnitId);
            return View(Device);
        }

        public ActionResult GetBarCode(int Id)
        {
            var Device = db.Device.Find(Id);
            ViewBag.Device = Device;
            return PartialView("_BarCode");
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

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Device Device, string Save, string SaveAndContinue, HttpPostedFileBase Image)
        {

            if ((!Device.ComputerNumber.ToLower().Equals("na")) && db.Device.Any(s => s.IsDeleted == false && s.ComputerNumber.ToLower().Equals(Device.ComputerNumber)))
            {
                ViewBag.Message = LanguageID == 1 ? "رقم الكمبيوتر مكرر" : "Computer number is already exist";
                ViewBag.DeviceTypeId = new SelectList(db.DeviceType, "Id", LanguageID == 1 ? "NameAr" : "NameEn", Device.DeviceTypeId);
                ViewBag.DeviceUnitId = new SelectList(db.DeviceUnit, "Id", LanguageID == 1 ? "NameAr" : "NameEn", Device.DeviceUnitId);
                return View(Device);
            }

            if ((!Device.ComputerNumber.ToLower().Equals("na")) && db.DDevice.Any(s => s.ComputerNumber.ToLower().Equals(Device.ComputerNumber)))
            {
                ViewBag.Message = LanguageID == 1 ? "رقم الكمبيوتر مكرر" : "Computer number is already exist";
                ViewBag.DeviceTypeId = new SelectList(db.DeviceType, "Id", LanguageID == 1 ? "NameAr" : "NameEn", Device.DeviceTypeId);
                ViewBag.DeviceUnitId = new SelectList(db.DeviceUnit, "Id", LanguageID == 1 ? "NameAr" : "NameEn", Device.DeviceUnitId);
                return View(Device);
            }

            if ((!Device.Serial.ToLower().Equals("na")) && db.Device.Any(s => s.IsDeleted == false && s.Serial.ToLower().Equals(Device.Serial)))
            {
                ViewBag.Message = LanguageID == 1 ? "رقم السيريال مكرر" : "Serial number is already exist";
                ViewBag.DeviceTypeId = new SelectList(db.DeviceType, "Id", LanguageID == 1 ? "NameAr" : "NameEn", Device.DeviceTypeId);
                ViewBag.DeviceUnitId = new SelectList(db.DeviceUnit, "Id", LanguageID == 1 ? "NameAr" : "NameEn", Device.DeviceUnitId);
                return View(Device);
            }

            if ((!Device.Serial.ToLower().Equals("na")) && db.DDevice.Any(s => s.Serial.ToLower().Equals(Device.Serial)))
            {
                ViewBag.Message = LanguageID == 1 ? "رقم السيريال مكرر" : "Serial number is already exist";
                ViewBag.DeviceTypeId = new SelectList(db.DeviceType, "Id", LanguageID == 1 ? "NameAr" : "NameEn", Device.DeviceTypeId);
                ViewBag.DeviceUnitId = new SelectList(db.DeviceUnit, "Id", LanguageID == 1 ? "NameAr" : "NameEn", Device.DeviceUnitId);
                return View(Device);
            }

            if (ModelState.IsValid)
            {
                Device.IsDeleted = false;
                Device.IsActive = true;
                Device.CreatedDate = DateTime.Now.AddHours(10);
                Device.CreatorName = User.Identity.GetUserName();
                if (Image != null)
                {
                    Random R = new Random();
                    int rand = R.Next(1, 100000);
                    var x = rand + DateTime.Now.AddHours(10).ToString() + Image.FileName;
                    x = ToSafeFileName(x);
                    Image.SaveAs(Server.MapPath("~/Content/Uploads/Device/" + x));
                    Device.ImgPath = x;
                }
                db.Device.Add(Device);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            ViewBag.Message = LanguageID == 1 ? "أكمل البيانات من فضلك" : "Please complete data";
            ViewBag.DeviceTypeId = new SelectList(db.DeviceType, "Id", LanguageID == 1 ? "NameAr" : "NameEn", Device.DeviceTypeId);
            ViewBag.DeviceUnitId = new SelectList(db.DeviceUnit, "Id", LanguageID == 1 ? "NameAr" : "NameEn", Device.DeviceUnitId);
            return View(Device);
        }

        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device Device = db.Device.Find(id);
            if (Device == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeviceTypeId = new SelectList(db.DeviceType, "Id", LanguageID == 1 ? "NameAr" : "NameEn", Device.DeviceTypeId);
            ViewBag.DeviceUnitId = new SelectList(db.DeviceUnit, "Id", LanguageID == 1 ? "NameAr" : "NameEn", Device.DeviceUnitId);
            return View(Device);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Device Device, HttpPostedFileBase Image)
        {
            if ((!Device.ComputerNumber.ToLower().Equals("na")))
            {
                var List = db.Device.AsNoTracking().Where(s => (s.Id > Device.Id || s.Id < Device.Id) && s.IsDeleted == false && s.ComputerNumber.Equals(Device.ComputerNumber));
                if (List.Count() > 0)
                {
                    ViewBag.Message = LanguageID == 1 ? "رقم الكمبيوتر مكرر" : "Computer number is already exist";
                    ViewBag.DeviceTypeId = new SelectList(db.DeviceType, "Id", LanguageID == 1 ? "NameAr" : "NameEn", Device.DeviceTypeId);
                    ViewBag.DeviceUnitId = new SelectList(db.DeviceUnit, "Id", LanguageID == 1 ? "NameAr" : "NameEn", Device.DeviceUnitId);
                    return View(Device);
                }
            }

            if ((!Device.Serial.ToLower().Equals("na")))
            {
                var List = db.Device.AsNoTracking().Where(s => (s.Id > Device.Id || s.Id < Device.Id) && s.IsDeleted == false && s.Serial.Equals(Device.Serial));
                if (List.Count() > 0)
                {
                    ViewBag.Message = LanguageID == 1 ? "رقم السيريال مكرر" : "Serial number is already exist";
                    ViewBag.DeviceTypeId = new SelectList(db.DeviceType, "Id", LanguageID == 1 ? "NameAr" : "NameEn", Device.DeviceTypeId);
                    ViewBag.DeviceUnitId = new SelectList(db.DeviceUnit, "Id", LanguageID == 1 ? "NameAr" : "NameEn", Device.DeviceUnitId);
                    return View(Device);
                }
            }

            if ((!Device.ComputerNumber.ToLower().Equals("na")))
            {
                var List = db.DDevice.AsNoTracking().Where(s => s.ComputerNumber.Equals(Device.ComputerNumber));
                if (List.Count() > 0)
                {
                    ViewBag.Message = LanguageID == 1 ? "رقم الكمبيوتر مكرر" : "Computer number is already exist";
                    ViewBag.DeviceTypeId = new SelectList(db.DeviceType, "Id", LanguageID == 1 ? "NameAr" : "NameEn", Device.DeviceTypeId);
                    ViewBag.DeviceUnitId = new SelectList(db.DeviceUnit, "Id", LanguageID == 1 ? "NameAr" : "NameEn", Device.DeviceUnitId);
                    return View(Device);
                }
            }

            if ((!Device.Serial.ToLower().Equals("na")))
            {
                var List = db.DDevice.AsNoTracking().Where(s => s.Serial.Equals(Device.Serial));
                if (List.Count() > 0)
                {
                    ViewBag.Message = LanguageID == 1 ? "رقم السيريال مكرر" : "Serial number is already exist";
                    ViewBag.DeviceTypeId = new SelectList(db.DeviceType, "Id", LanguageID == 1 ? "NameAr" : "NameEn", Device.DeviceTypeId);
                    ViewBag.DeviceUnitId = new SelectList(db.DeviceUnit, "Id", LanguageID == 1 ? "NameAr" : "NameEn", Device.DeviceUnitId);
                    return View(Device);
                }
            }

            if (ModelState.IsValid)
            {
                Device.ModifiedDate = DateTime.Now.AddHours(10);
                Device.ModifierName = User.Identity.GetUserName();
                if (Image != null)
                {
                    Random R = new Random();
                    int rand = R.Next(1, 100000);
                    var x = rand + DateTime.Now.AddHours(10).ToString() + Image.FileName;
                    x = ToSafeFileName(x);
                    Image.SaveAs(Server.MapPath("~/Content/Uploads/Device/" + x));
                    Device.ImgPath = x;
                }
                db.Entry(Device).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Message = LanguageID == 1 ? "أكمل البيانات من فضلك" : "Please complete data";
            ViewBag.DeviceTypeId = new SelectList(db.DeviceType, "Id", LanguageID == 1 ? "NameAr" : "NameEn", Device.DeviceTypeId);
            ViewBag.DeviceUnitId = new SelectList(db.DeviceUnit, "Id", LanguageID == 1 ? "NameAr" : "NameEn", Device.DeviceUnitId);
            return View(Device);
        }


        // POST: Admin/Categories/Delete/5
        public ActionResult DeleteConfirmed(int id)
        {
            db.DeviceSchedule.RemoveRange(db.DeviceSchedule.Where(s => s.DeviceId == id));
            db.DeviceRequest.RemoveRange(db.DeviceRequest.Where(s => s.DeviceId == id));
            Device Device = db.Device.Find(id);
            db.Device.Remove(Device);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult PPM(int DeviceId,string DateFrom, string DateTo)
        {
            var Device = db.Device.Find(DeviceId);
            ViewBag.DeviceName = Device.NameEn;
            ViewBag.DateFrom = DateFrom != "" ? DateFrom:".......";
            ViewBag.DateTo = DateTo != "" ? DateTo : ".......";
            ViewBag.PPMText = db.WebsiteData.FirstOrDefault() == null ? "................" : db.WebsiteData.FirstOrDefault().PPMStickerText;
            return View();
        }

        public ActionResult ArchiveDevice(string Selected)
        {
            var List = Selected.Split(',');
            foreach (var item in List)
            {
                if (item != "" && item != "on")
                {
                    var Id = Convert.ToInt32(item);
                    Device Device = db.Device.Find(Id);

                    var DDevice = new DDevice();
                    DDevice.Com = Device.Com;
                    DDevice.CompanyName = Device.CompanyName;
                    DDevice.CompanyPhone = Device.CompanyPhone;
                    DDevice.CompanyEmail = Device.CompanyEmail;
                    DDevice.ComputerNumber = Device.ComputerNumber;
                    DDevice.CreatorName = User.Identity.GetUserName();
                    DDevice.CreatedDate = DateTime.Now.AddHours(10);
                    DDevice.DamanExpireDate = Device.DamanExpireDate;
                    DDevice.Desc = Device.Desc;
                    DDevice.DeviceCategoryId = Device.DeviceCategoryId;
                    DDevice.DeviceTypeId = Device.DeviceTypeId;
                    DDevice.DeviceUnitId = Device.DeviceUnitId;
                    DDevice.ImgPath = Device.ImgPath;
                    DDevice.LastModificationDate = Device.LastModificationDate;
                    DDevice.Model = Device.Model;
                    DDevice.NameAr = Device.NameAr;
                    DDevice.NameEn = Device.NameEn;
                    DDevice.Serial = Device.Serial;
                    DDevice.ShowDaman = Device.ShowDaman;
                    DDevice.ShowLastModification = Device.ShowLastModification;
                    DDevice.IsActive = true;
                    DDevice.IsDeleted = false;
                    db.DDevice.Add(DDevice);
                    db.SaveChanges();
                }
            }
            return DeleteSelected(Selected);
        }

        public ActionResult DeleteSelected(string Selected)
        {
            var List = Selected.Split(',');
            foreach (var item in List)
            {
                if (item != "" && item != "on")
                {
                    var Id = Convert.ToInt32(item);
                    db.DeviceSchedule.RemoveRange(db.DeviceSchedule.Where(s => s.DeviceId == Id));
                    db.DeviceRequest.RemoveRange(db.DeviceRequest.Where(s => s.DeviceId == Id));
                    Device Device = db.Device.Find(Id);
                    db.Device.Remove(Device);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult ShowLastModification(string Selected)
        {
            var List = Selected.Split(',');
            foreach (var item in List)
            {
                if (item != "" && item != "on")
                {
                    var Id = Convert.ToInt32(item);
                    Device Device = db.Device.Find(Id);
                    Device.ShowLastModification = true;
                    db.Entry(Device).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult HideLastModification(string Selected)
        {
            var List = Selected.Split(',');
            foreach (var item in List)
            {
                if (item != "" && item != "on")
                {
                    var Id = Convert.ToInt32(item);
                    Device Device = db.Device.Find(Id);
                    Device.ShowLastModification = false;
                    db.Entry(Device).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult DeviceSchedual(string Selected,int ScheduleCount)
        {
            ViewBag.DayId = new SelectList(db.Day, "Id", "Name");
            ViewBag.MonthId = new SelectList(db.Month, "Id", LanguageID == 1 ? "NameAr" : "NameEn");
            ViewBag.Selected = Selected;
            ViewBag.ScheduleCount = ScheduleCount;

            var Devices = Selected.Split(',');
            foreach(var item in Devices)
            {
                if (item != "")
                {
                    var DeviceId = Convert.ToInt32(item);
                    var DeviceSchedule = db.DeviceSchedule.Where(s => s.DeviceId == DeviceId);
                    db.DeviceSchedule.RemoveRange(DeviceSchedule);
                    db.SaveChanges();
                }
            }

            return View();
        }

        public ActionResult SaveDeviceSchedual(string Selected, int ScheduleCount,string Dates)
        {
            var SplitDates = Dates.Split(',');
            var Devices = Selected.Split(',');

            for (var i=0;i< ScheduleCount;i++)
            {
                var Day = Convert.ToInt32(SplitDates[i].Split(':')[0]);
                var Month = Convert.ToInt32(SplitDates[i].Split(':')[1]);

                foreach(var item in Devices)
                {
                    if(item != "")
                    {
                        var DeviceID = Convert.ToInt32(item);
                        DeviceSchedule Model = new DeviceSchedule();
                        Model.DayId = Day;
                        Model.MonthId = Month;
                        Model.LastCloseYear = Model.LastPrintPPMYear = DateTime.Now.AddHours(10).Year - 1;
                        Model.DeviceId = DeviceID;
                        Model.IsActive = true;
                        Model.IsDeleted = false;
                        Model.CreatedDate = DateTime.Now.AddHours(10);
                        Model.CreatorName = User.Identity.GetUserName();
                        db.DeviceSchedule.Add(Model);
                        db.SaveChanges();
                    }
                }
            }

            return RedirectToAction("Index");
        }
    }
}