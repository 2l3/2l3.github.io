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
    public class CenterDeviceController : BaseController
    {
        private DBContext db = new DBContext();
        // GET: Admin/CenterDevice
        [Authorize(Roles = "HealthCenterDevice")]
        public ActionResult Index()
        {
            return View(db.CenterDevice.AsNoTracking().Where(s => s.IsDeleted == false).Include(s => s.DeviceSchedules).Include(s => s.CenterDeviceUnit.Center).OrderByDescending(s => s.Id).ToList());
        }

        [Authorize(Roles = "HealthCenterDeviceAdd")]
        // GET: Admin/Categories/Create
        public ActionResult Create()
        {
            ViewBag.CenterDeviceUnit = db.CenterDeviceUnit.ToList();
            ViewBag.Centers = db.Center.Where(s => s.IsDeleted == false && s.IsActive == true).ToList();
            ViewBag.DeviceTypeId = new SelectList(db.DeviceType, "Id", LanguageID == 1 ? "NameAr" : "NameEn");
            return View();
        }

        public ActionResult PrintBarCodeMulti(string Ids)
        {
            List<CenterDevice> CenterDevices = new List<CenterDevice>();
            var arr = Ids.Split(',');
            foreach(var item in arr)
            {
                if (item == "") continue;
                int Id = Convert.ToInt32(item);
                CenterDevices.Add(db.CenterDevice.FirstOrDefault(s => s.Id == Id));
            }
            return View(CenterDevices);
        }

        public ActionResult CenterDevicePPM()
        {
            ViewBag.Engineers = db.Engineer.ToList();
            return View(db.CenterDeviceUnit.AsNoTracking());
        }

        public ActionResult PMMReportPrint(string DateFrom,string DateTo,int CenterDeviceUnitId,int? EngineerId,int DeviceTypeId,int DeviceCategoryId,int CenterId,int ScheduleTypeId,int Daman)
        {
            ViewBag.DateFrom = DateFrom;
            ViewBag.DateTo = DateTo;
            ViewBag.Engineer = EngineerId != 0 ? db.Engineer.FirstOrDefault(s => s.Id == EngineerId).NameAr: "....................................";
            var EngineerSignImg = EngineerId != 0 ? db.Engineer.FirstOrDefault(s => s.Id == EngineerId).SignImg : null;
            if (EngineerSignImg == null || EngineerSignImg == "")
                ViewBag.EngineerSignImg = "";
            else
                ViewBag.EngineerSignImg = EngineerSignImg;
            var Models = new List<CenterDevice>();

            if (ScheduleTypeId == 1)
                Models = db.CenterDevice.AsNoTracking().Where(s => s.IsDeleted == false && s.CenterDeviceUnit.CenterId == CenterId && s.DeviceSchedules.Count() > 0).Include(s => s.CenterDeviceUnit).ToList();
            else if (ScheduleTypeId == 2)
                Models = db.CenterDevice.AsNoTracking().Where(s => s.IsDeleted == false && s.CenterDeviceUnit.CenterId == CenterId && s.DeviceSchedules.Count() == 0).Include(s => s.CenterDeviceUnit).ToList();
            else
                Models = db.CenterDevice.AsNoTracking().Where(s => s.IsDeleted == false && s.CenterDeviceUnit.CenterId == CenterId).Include(s => s.CenterDeviceUnit).ToList();

            if (CenterDeviceUnitId != 0)
                Models = Models.Where(s => s.CenterDeviceUnitId == CenterDeviceUnitId).ToList();
            if (Daman == 1)
                Models = Models.Where(s => s.DamanExpireDate.HasValue && s.DamanExpireDate.Value.Date.Ticks >= DateTime.Now.AddHours(10).Date.Ticks).ToList();
            else if (Daman == 2)
                Models = Models.Where(s => s.DamanExpireDate.HasValue == false || s.DamanExpireDate.Value.Date.Ticks < DateTime.Now.AddHours(10).Date.Ticks).ToList();
            if (DeviceTypeId != 0)
                Models = Models.Where(s => s.DeviceTypeId == DeviceTypeId).ToList();
            if (DeviceCategoryId != 0)
                Models = Models.Where(s => s.DeviceCategoryId == DeviceCategoryId).ToList();
            ViewBag.Center = db.Center.FirstOrDefault(s => s.Id == CenterId);
            ViewBag.AboveReportSetting = db.CenterReportSetting2.Where(s => s.Above == 1).OrderBy(s => s.Sort);
            ViewBag.BottomReportSetting = db.CenterReportSetting2.Where(s => s.Above == 2).OrderBy(s => s.Sort);
            ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
            return View("PMMReportPrint", Models);
        }

        public ActionResult PMMReportPrintMulti(string DateFrom, string DateTo, string SelectedDevice, int? EngineerId)
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
            ViewBag.AboveReportSetting = db.CenterReportSetting2.Where(s => s.Above == 1).OrderBy(s => s.Sort);
            ViewBag.BottomReportSetting = db.CenterReportSetting2.Where(s => s.Above == 2).OrderBy(s => s.Sort);
            ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;

            var Models = new List<CenterDevice>();
            foreach (var i in Arr)
            {
                if (i == "") continue;
                var Id = Convert.ToInt32(i);
                var S = db.CenterDeviceSchedule.Find(Id);
                if (S.LastPrintPPMYear != DateTime.Now.AddHours(10).Year)
                {
                    S.LastPrintPPMYear = DateTime.Now.AddHours(10).Year;
                    db.Entry(S).State = EntityState.Modified;
                    db.SaveChanges();

                    var Device = db.CenterDevice.Find(S.CenterDeviceId);
                    Device.LastModificationDate = DateTime.Now.AddHours(10);
                    db.Entry(Device).State = EntityState.Modified;
                    db.SaveChanges();
                }

                Models.Add(db.CenterDevice.AsNoTracking().Include(v => v.CenterDeviceUnit.Center).FirstOrDefault(s => s.Id == S.CenterDeviceId));
            }

            return View("PMMReportPrintMulti", Models);
        }

        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CenterDevice CenterDevice = db.CenterDevice.Include(s => s.CenterDeviceUnit).FirstOrDefault(s => s.Id == id);
            if (CenterDevice == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeviceTypeId = new SelectList(db.DeviceType, "Id", LanguageID == 1 ? "NameAr" : "NameEn", CenterDevice.DeviceTypeId);
            ViewBag.CenterDeviceUnit = db.CenterDeviceUnit.ToList();
            ViewBag.Centers = db.Center.Where(s => s.IsDeleted == false && s.IsActive == true).ToList();
            return View(CenterDevice);
        }

        public ActionResult GetBarCode(int Id)
        {
            var CenterDevice = db.CenterDevice.Find(Id);
            ViewBag.CenterDevice = CenterDevice;
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
        public ActionResult Create(CenterDevice CenterDevice, string Save, string SaveAndContinue, HttpPostedFileBase Image,int Center)
        {

            if ((!CenterDevice.ComputerNumber.ToLower().Equals("na")) && db.CenterDevice.Any(s => s.CenterDeviceUnit.CenterId == Center && s.IsDeleted == false && s.ComputerNumber.ToLower().Equals(CenterDevice.ComputerNumber)))
            {
                ViewBag.Message = LanguageID == 1 ? "رقم الكمبيوتر مكرر" : "Computer number is already exist";
                ViewBag.DeviceTypeId = new SelectList(db.DeviceType, "Id", LanguageID == 1 ? "NameAr" : "NameEn", CenterDevice.DeviceTypeId);
                ViewBag.CenterDeviceUnit = db.CenterDeviceUnit.ToList();
                ViewBag.Centers = db.Center.Where(s => s.IsDeleted == false && s.IsActive == true).ToList();
                return View(CenterDevice);
            }

            if ((!CenterDevice.ComputerNumber.ToLower().Equals("na")) && db.DCenterDevice.Any(s => s.CenterDeviceUnit.CenterId == Center && s.ComputerNumber.ToLower().Equals(CenterDevice.ComputerNumber)))
            {
                ViewBag.Message = LanguageID == 1 ? "رقم الكمبيوتر مكرر" : "Computer number is already exist";
                ViewBag.DeviceTypeId = new SelectList(db.DeviceType, "Id", LanguageID == 1 ? "NameAr" : "NameEn", CenterDevice.DeviceTypeId);
                ViewBag.CenterDeviceUnit = db.CenterDeviceUnit.ToList();
                ViewBag.Centers = db.Center.Where(s => s.IsDeleted == false && s.IsActive == true).ToList();
                return View(CenterDevice);
            }

            if ((!CenterDevice.Serial.ToLower().Equals("na")) && db.CenterDevice.Any(s => s.CenterDeviceUnit.CenterId == Center && s.IsDeleted == false && s.Serial.ToLower().Equals(CenterDevice.Serial)))
            {
                ViewBag.Message = LanguageID == 1 ? "رقم السيريال مكرر" : "Serial number is already exist";
                ViewBag.DeviceTypeId = new SelectList(db.DeviceType, "Id", LanguageID == 1 ? "NameAr" : "NameEn", CenterDevice.DeviceTypeId);
                ViewBag.CenterDeviceUnit = db.CenterDeviceUnit.ToList();
                ViewBag.Centers = db.Center.Where(s => s.IsDeleted == false && s.IsActive == true).ToList();
                return View(CenterDevice);
            }

            if ((!CenterDevice.Serial.ToLower().Equals("na")) && db.DCenterDevice.Any(s => s.CenterDeviceUnit.CenterId == Center && s.Serial.ToLower().Equals(CenterDevice.Serial)))
            {
                ViewBag.Message = LanguageID == 1 ? "رقم السيريال مكرر" : "Serial number is already exist";
                ViewBag.DeviceTypeId = new SelectList(db.DeviceType, "Id", LanguageID == 1 ? "NameAr" : "NameEn", CenterDevice.DeviceTypeId);
                ViewBag.CenterDeviceUnit = db.CenterDeviceUnit.ToList();
                ViewBag.Centers = db.Center.Where(s => s.IsDeleted == false && s.IsActive == true).ToList();
                return View(CenterDevice);
            }

            if (ModelState.IsValid)
            {
                CenterDevice.IsDeleted = false;
                CenterDevice.IsActive = true;
                CenterDevice.CreatedDate = DateTime.Now.AddHours(10);
                CenterDevice.CreatorName = User.Identity.GetUserName();
                if (Image != null)
                {
                    Random R = new Random();
                    int rand = R.Next(1, 100000);
                    var x = rand + DateTime.Now.AddHours(10).ToString() + Image.FileName;
                    x = ToSafeFileName(x);
                    Image.SaveAs(Server.MapPath("~/Content/Uploads/CenterDevice/" + x));
                    CenterDevice.ImgPath = x;
                }
                db.CenterDevice.Add(CenterDevice);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            ViewBag.Message = LanguageID == 1 ? "أكمل البيانات من فضلك" : "Please complete data";
            ViewBag.DeviceTypeId = new SelectList(db.DeviceType, "Id", LanguageID == 1 ? "NameAr" : "NameEn", CenterDevice.DeviceTypeId);
            ViewBag.CenterDeviceUnit = db.CenterDeviceUnit.ToList();
            ViewBag.Centers = db.Center.Where(s => s.IsDeleted == false && s.IsActive == true).ToList();
            return View(CenterDevice);
        }

        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CenterDevice CenterDevice = db.CenterDevice.Include(s => s.CenterDeviceUnit).FirstOrDefault(s => s.Id == id);
            ViewBag.CenterId = CenterDevice.CenterDeviceUnit.CenterId;
            if (CenterDevice == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeviceTypeId = new SelectList(db.DeviceType, "Id", LanguageID == 1 ? "NameAr" : "NameEn", CenterDevice.DeviceTypeId);
            ViewBag.CenterDeviceUnit = db.CenterDeviceUnit.ToList();
            ViewBag.Centers = db.Center.Where(s => s.IsDeleted == false && s.IsActive == true).ToList();
            return View(CenterDevice);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CenterDevice CenterDevice, HttpPostedFileBase Image,int Center)
        {
            if ((!CenterDevice.ComputerNumber.ToLower().Equals("na")))
            {
                var List = db.CenterDevice.AsNoTracking().Where(s => (s.Id > CenterDevice.Id || s.Id < CenterDevice.Id) && s.CenterDeviceUnit.CenterId == Center && s.IsDeleted == false && s.ComputerNumber.Equals(CenterDevice.ComputerNumber));
                if (List.Count() > 0)
                {
                    ViewBag.Message = LanguageID == 1 ? "رقم الكمبيوتر مكرر" : "Computer number is already exist";
                    ViewBag.DeviceTypeId = new SelectList(db.DeviceType, "Id", LanguageID == 1 ? "NameAr" : "NameEn", CenterDevice.DeviceTypeId);
                    ViewBag.CenterDeviceUnit = db.CenterDeviceUnit.ToList();
                    ViewBag.Centers = db.Center.Where(s => s.IsDeleted == false && s.IsActive == true).ToList();
                    ViewBag.CenterId = Center;
                    return View(CenterDevice);
                }
            }

            if ((!CenterDevice.Serial.ToLower().Equals("na")))
            {
                var List = db.CenterDevice.AsNoTracking().Where(s => (s.Id > CenterDevice.Id || s.Id < CenterDevice.Id) && s.CenterDeviceUnit.CenterId == Center && s.IsDeleted == false && s.Serial.Equals(CenterDevice.Serial));
                if (List.Count() > 0)
                {
                    ViewBag.Message = LanguageID == 1 ? "رقم السيريال مكرر" : "Serial number is already exist";
                    ViewBag.DeviceTypeId = new SelectList(db.DeviceType, "Id", LanguageID == 1 ? "NameAr" : "NameEn", CenterDevice.DeviceTypeId);
                    ViewBag.CenterDeviceUnit = db.CenterDeviceUnit.ToList();
                    ViewBag.Centers = db.Center.Where(s => s.IsDeleted == false && s.IsActive == true).ToList();
                    ViewBag.CenterId = Center;
                    return View(CenterDevice);
                }
            }

            if ((!CenterDevice.ComputerNumber.ToLower().Equals("na")))
            {
                var List = db.DCenterDevice.AsNoTracking().Where(s => s.CenterDeviceUnit.CenterId == Center && s.ComputerNumber.Equals(CenterDevice.ComputerNumber));
                if (List.Count() > 0)
                {
                    ViewBag.Message = LanguageID == 1 ? "رقم الكمبيوتر مكرر" : "Computer number is already exist";
                    ViewBag.DeviceTypeId = new SelectList(db.DeviceType, "Id", LanguageID == 1 ? "NameAr" : "NameEn", CenterDevice.DeviceTypeId);
                    ViewBag.CenterDeviceUnit = db.CenterDeviceUnit.ToList();
                    ViewBag.Centers = db.Center.Where(s => s.IsDeleted == false && s.IsActive == true).ToList();
                    ViewBag.CenterId = Center;
                    return View(CenterDevice);
                }
            }

            if ((!CenterDevice.Serial.ToLower().Equals("na")))
            {
                var List = db.DCenterDevice.AsNoTracking().Where(s => s.CenterDeviceUnit.CenterId == Center && s.Serial.Equals(CenterDevice.Serial));
                if (List.Count() > 0)
                {
                    ViewBag.Message = LanguageID == 1 ? "رقم السيريال مكرر" : "Serial number is already exist";
                    ViewBag.DeviceTypeId = new SelectList(db.DeviceType, "Id", LanguageID == 1 ? "NameAr" : "NameEn", CenterDevice.DeviceTypeId);
                    ViewBag.CenterDeviceUnit = db.CenterDeviceUnit.ToList();
                    ViewBag.Centers = db.Center.Where(s => s.IsDeleted == false && s.IsActive == true).ToList();
                    ViewBag.CenterId = Center;
                    return View(CenterDevice);
                }
            }

            if (ModelState.IsValid)
            {
                CenterDevice.ModifiedDate = DateTime.Now.AddHours(10);
                CenterDevice.ModifierName = User.Identity.GetUserName();
                if (Image != null)
                {
                    Random R = new Random();
                    int rand = R.Next(1, 100000);
                    var x = rand + DateTime.Now.AddHours(10).ToString() + Image.FileName;
                    x = ToSafeFileName(x);
                    Image.SaveAs(Server.MapPath("~/Content/Uploads/CenterDevice/" + x));
                    CenterDevice.ImgPath = x;
                }
                db.Entry(CenterDevice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Message = LanguageID == 1 ? "أكمل البيانات من فضلك" : "Please complete data";
            ViewBag.DeviceTypeId = new SelectList(db.DeviceType, "Id", LanguageID == 1 ? "NameAr" : "NameEn", CenterDevice.DeviceTypeId);
            ViewBag.CenterDeviceUnit = db.CenterDeviceUnit.ToList();
            ViewBag.Centers = db.Center.Where(s => s.IsDeleted == false && s.IsActive == true).ToList();
            ViewBag.CenterId = Center;
            return View(CenterDevice);
        }


        // POST: Admin/Categories/Delete/5
        public ActionResult DeleteConfirmed(int id)
        {
            db.CenterDeviceSchedule.RemoveRange(db.CenterDeviceSchedule.Where(s => s.CenterDeviceId == id));
            db.CenterDeviceRequest.RemoveRange(db.CenterDeviceRequest.Where(s => s.DeviceId == id));
            CenterDevice CenterDevice = db.CenterDevice.Find(id);
            db.CenterDevice.Remove(CenterDevice);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult PPM(int CenterDeviceId,string DateFrom, string DateTo)
        {
            var CenterDevice = db.CenterDevice.Find(CenterDeviceId);
            ViewBag.CenterDeviceName = CenterDevice.NameEn;
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
                    CenterDevice Device = db.CenterDevice.Find(Id);

                    var DDevice = new DCenterDevice();
                    DDevice.Com = Device.Com;
                    DDevice.CompanyName = Device.CompanyName;
                    DDevice.CompanyEmail = Device.CompanyEmail;
                    DDevice.CompanyPhone = Device.CompanyPhone;
                    DDevice.ComputerNumber = Device.ComputerNumber;
                    DDevice.CreatorName = User.Identity.GetUserName();
                    DDevice.CreatedDate = DateTime.Now.AddHours(10);
                    DDevice.DamanExpireDate = Device.DamanExpireDate;
                    DDevice.Desc = Device.Desc;
                    DDevice.DeviceCategoryId = Device.DeviceCategoryId;
                    DDevice.DeviceTypeId = Device.DeviceTypeId;
                    DDevice.CenterDeviceUnitId = Device.CenterDeviceUnitId;
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
                    db.DCenterDevice.Add(DDevice);
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
                    db.CenterDeviceSchedule.RemoveRange(db.CenterDeviceSchedule.Where(s => s.CenterDeviceId == Id));
                    db.CenterDeviceRequest.RemoveRange(db.CenterDeviceRequest.Where(s => s.DeviceId == Id));                   
                    CenterDevice CenterDevice = db.CenterDevice.Find(Id);
                    db.CenterDevice.Remove(CenterDevice);
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
                    CenterDevice CenterDevice = db.CenterDevice.Find(Id);
                    CenterDevice.ShowLastModification = true;
                    db.Entry(CenterDevice).State = EntityState.Modified;
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
                    CenterDevice CenterDevice = db.CenterDevice.Find(Id);
                    CenterDevice.ShowLastModification = false;
                    db.Entry(CenterDevice).State = EntityState.Modified;
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

            var CenterDevices = Selected.Split(',');
            foreach(var item in CenterDevices)
            {
                if (item != "")
                {
                    var CenterDeviceId = Convert.ToInt32(item);
                    var CenterDeviceSchedule = db.CenterDeviceSchedule.Where(s => s.CenterDeviceId == CenterDeviceId);
                    db.CenterDeviceSchedule.RemoveRange(CenterDeviceSchedule);
                    db.SaveChanges();
                }
            }

            return View();
        }

        public ActionResult SaveDeviceSchedual(string Selected, int ScheduleCount,string Dates)
        {
            var SplitDates = Dates.Split(',');
            var CenterDevices = Selected.Split(',');

            for (var i=0;i< ScheduleCount;i++)
            {
                var Day = Convert.ToInt32(SplitDates[i].Split(':')[0]);
                var Month = Convert.ToInt32(SplitDates[i].Split(':')[1]);

                foreach(var item in CenterDevices)
                {
                    if(item != "")
                    {
                        CenterDeviceSchedule Model = new CenterDeviceSchedule();
                        Model.DayId = Day;
                        Model.MonthId = Month;
                        Model.LastCloseYear = Model.LastPrintPPMYear = DateTime.Now.AddHours(10).Year - 1;
                        Model.CenterDeviceId = Convert.ToInt32(item);
                        Model.IsActive = true;
                        Model.IsDeleted = false;
                        Model.CreatedDate = DateTime.Now.AddHours(10);
                        Model.CreatorName = User.Identity.GetUserName();
                        db.CenterDeviceSchedule.Add(Model);
                        db.SaveChanges();
                    }
                }
            }

            return RedirectToAction("Index");
        }
    }
}