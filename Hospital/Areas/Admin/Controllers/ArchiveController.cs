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
    public class ArchiveController : BaseController
    {
        private DBContext db = new DBContext();

        // GET: Admin/Archive
        [Authorize(Roles = "HospitalArchive")]
        public ActionResult Hospital()
        {
            return View(db.DDevice.AsNoTracking().Where(s => s.IsDeleted == false).Include(s => s.DeviceUnit).OrderByDescending(s => s.Id).ToList());
        }

        [Authorize(Roles = "HealthCenterArchive")]
        public ActionResult Center()
        {
            return View(db.DCenterDevice.AsNoTracking().Where(s => s.IsDeleted == false).Include(s => s.CenterDeviceUnit.Center).OrderByDescending(s => s.Id).ToList());
        }

        [Authorize(Roles = "HealthCenterArchive")]
        public ActionResult CenterDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DCenterDevice CenterDevice = db.DCenterDevice.Include(s => s.CenterDeviceUnit).FirstOrDefault(s => s.Id == id);
            if (CenterDevice == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeviceTypeId = new SelectList(db.DeviceType, "Id", LanguageID == 1 ? "NameAr" : "NameEn", CenterDevice.DeviceTypeId);
            ViewBag.CenterDeviceUnit = db.CenterDeviceUnit.ToList();
            ViewBag.Centers = db.Center.Where(s => s.IsDeleted == false && s.IsActive == true).ToList();
            return View(CenterDevice);
        }

        [Authorize(Roles = "HospitalArchive")]
        public ActionResult HospitalDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DDevice Device = db.DDevice.Find(id);
            if (Device == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeviceTypeId = new SelectList(db.DeviceType, "Id", LanguageID == 1 ? "NameAr" : "NameEn", Device.DeviceTypeId);
            ViewBag.DeviceUnitId = new SelectList(db.DeviceUnit, "Id", LanguageID == 1 ? "NameAr" : "NameEn", Device.DeviceUnitId);
            return View(Device);
        }

        [Authorize(Roles = "HealthCenterArchiveDelete")]
        public ActionResult CenterDeleteConfirmed(int id)
        {
            DCenterDevice CenterDevice = db.DCenterDevice.Find(id);
            db.DCenterDevice.Remove(CenterDevice);
            db.SaveChanges();
            return RedirectToAction("Center");
        }

        [Authorize(Roles = "HealthCenterArchiveDelete")]
        public ActionResult CenterDeleteSelected(string Selected)
        {
            var List = Selected.Split(',');
            foreach (var item in List)
            {
                if (item != "" && item != "on")
                {
                    var Id = Convert.ToInt32(item);
                    DCenterDevice CenterDevice = db.DCenterDevice.Find(Id);
                    db.DCenterDevice.Remove(CenterDevice);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Center");
        }

        [Authorize(Roles = "HospitalArchiveDelete")]
        public ActionResult HospitalDeleteConfirmed(int id)
        {
            DDevice Device = db.DDevice.Find(id);
            db.DDevice.Remove(Device);
            db.SaveChanges();
            return RedirectToAction("Hospital");
        }

        [Authorize(Roles = "HospitalArchiveDelete")]
        public ActionResult HospitalDeleteSelected(string Selected)
        {
            var List = Selected.Split(',');
            foreach (var item in List)
            {
                if (item != "" && item != "on")
                {
                    var Id = Convert.ToInt32(item);
                    DDevice Device = db.DDevice.Find(Id);
                    db.DDevice.Remove(Device);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Hospital");
        }

        [Authorize(Roles = "HospitalArchive")]
        public ActionResult HospitalPrint(string Ids)
        {
            var Data = new List<DDevice>();
            var List = Ids.Split(',');
            foreach (var item in List)
            {
                if (item != "" && item != "on")
                {
                    var Id = Convert.ToInt32(item);
                    DDevice Device = db.DDevice.AsNoTracking().Include(s => s.DeviceUnit).FirstOrDefault(s => s.Id == Id);
                    Data.Add(Device);
                }
            }
            ViewBag.ReportSetting = db.ReportSetting.OrderBy(s => s.Sort);
            ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
            return View(Data);
        }

        [Authorize(Roles = "HealthCenterArchive")]
        public ActionResult CenterPrint(string Ids)
        {
            var Data = new List<DCenterDevice>();
            var List = Ids.Split(',');
            foreach (var item in List)
            {
                if (item != "" && item != "on")
                {
                    var Id = Convert.ToInt32(item);
                    DCenterDevice Device = db.DCenterDevice.AsNoTracking().Include(s => s.CenterDeviceUnit.Center).FirstOrDefault(s => s.Id == Id);
                    Data.Add(Device);
                }
            }
            ViewBag.ReportSetting = db.CenterReportSetting.OrderBy(s => s.Sort);
            ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
            return View(Data);
        }

        [Authorize(Roles = "HospitalArchiveCancel")]
        public ActionResult CancelHospitalArchive(string Selected)
        {
            var List = Selected.Split(',');
            foreach (var item in List)
            {
                if (item != "" && item != "on")
                {
                    var Id = Convert.ToInt32(item);
                    DDevice DDevice = db.DDevice.Find(Id);

                    var Device = new Device();
                    Device.Com = DDevice.Com;
                    Device.CompanyName = DDevice.CompanyName;
                    Device.ComputerNumber = DDevice.ComputerNumber;
                    Device.CreatorName = User.Identity.GetUserName();
                    Device.CreatedDate = DateTime.Now.AddHours(10);
                    Device.DamanExpireDate = DDevice.DamanExpireDate;
                    Device.Desc = DDevice.Desc;
                    Device.DeviceCategoryId = DDevice.DeviceCategoryId;
                    Device.DeviceTypeId = DDevice.DeviceTypeId;
                    Device.DeviceUnitId = DDevice.DeviceUnitId;
                    Device.ImgPath = DDevice.ImgPath;
                    Device.LastModificationDate = DDevice.LastModificationDate;
                    Device.Model = DDevice.Model;
                    Device.NameAr = DDevice.NameAr;
                    Device.NameEn = DDevice.NameEn;
                    Device.Serial = DDevice.Serial;
                    Device.ShowDaman = DDevice.ShowDaman;
                    Device.ShowLastModification = DDevice.ShowLastModification;
                    Device.IsActive = true;
                    Device.IsDeleted = false;
                    db.Device.Add(Device);
                    db.SaveChanges();
                }
            }
            return HospitalDeleteSelected(Selected);
        }

        [Authorize(Roles = "HealthCenterArchiveCancel")]
        public ActionResult CancelCenterArchive(string Selected)
        {
            var List = Selected.Split(',');
            foreach (var item in List)
            {
                if (item != "" && item != "on")
                {
                    var Id = Convert.ToInt32(item);
                    DCenterDevice DDevice = db.DCenterDevice.Find(Id);

                    var Device = new CenterDevice();
                    Device.Com = DDevice.Com;
                    Device.CompanyName = DDevice.CompanyName;
                    Device.ComputerNumber = DDevice.ComputerNumber;
                    Device.CreatorName = User.Identity.GetUserName();
                    Device.CreatedDate = DateTime.Now.AddHours(10);
                    Device.DamanExpireDate = DDevice.DamanExpireDate;
                    Device.Desc = DDevice.Desc;
                    Device.DeviceCategoryId = DDevice.DeviceCategoryId;
                    Device.DeviceTypeId = DDevice.DeviceTypeId;
                    Device.CenterDeviceUnitId = DDevice.CenterDeviceUnitId;
                    Device.ImgPath = DDevice.ImgPath;
                    Device.LastModificationDate = DDevice.LastModificationDate;
                    Device.Model = DDevice.Model;
                    Device.NameAr = DDevice.NameAr;
                    Device.NameEn = DDevice.NameEn;
                    Device.Serial = DDevice.Serial;
                    Device.ShowDaman = DDevice.ShowDaman;
                    Device.ShowLastModification = DDevice.ShowLastModification;
                    Device.IsActive = true;
                    Device.IsDeleted = false;

                    db.CenterDevice.Add(Device);
                    db.SaveChanges();
                }
            }
            return CenterDeleteSelected(Selected);
        }

    }
}