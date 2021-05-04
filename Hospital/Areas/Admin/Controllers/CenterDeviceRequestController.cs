using Hospital.Controllers;
using Hospital.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Hospital.Areas.Admin.Controllers
{
    [Authorize]
    public class CenterDeviceRequestController : BaseController
    {
        private DBContext db = new DBContext();
        // GET: Admin/DeviceRequest
        [Authorize(Roles = "HealthCenterRequest")]
        public ActionResult Index()
        {
            ViewBag.DeviceUnits = db.CenterDeviceUnit.ToList();
            return View();
        }

        [Authorize(Roles = "HealthCenterRequestDelete")]
        public ActionResult DIndex()
        {
            ViewBag.DeviceUnits = db.CenterDeviceUnit.ToList();
            return View();
        }

        public ActionResult GetDeviceRequests(string Name, int? DeviceUnitId, int ConfirmType, int? RequestType, DateTime? Date, string ComputerNumber, string Serial)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string SearchFilter = Request["search[value]"];

            var objs = Name == null ?
                db.CenterDeviceRequest.AsNoTracking().Where(s => s.IsDeleted == false).Include(s => s.Device.CenterDeviceUnit.Center).OrderByDescending(s => s.Id).ToList()
                : db.CenterDeviceRequest.AsNoTracking().Where(s => s.IsDeleted == false && (s.Device.NameAr.Contains(Name) || s.Device.NameEn.Contains(Name) || s.DeviceName.Contains(Name))).Include(s => s.Device.CenterDeviceUnit.Center).OrderByDescending(s => s.Id).ToList();

            if (Date != null)
                objs = objs.Where(s => s.CreatedDate.Value.Date == Date.Value || s.CreatedDate.Value.Date == Date.Value.AddDays(1)).ToList();
            if (DeviceUnitId != null && DeviceUnitId != 0)
                objs = objs.Where(s => s.WithNoBarCode != true && s.Device.CenterDeviceUnitId == DeviceUnitId).ToList();
            if (ComputerNumber != null && ComputerNumber != "")
                objs = objs.Where(s => s.Device.ComputerNumber == ComputerNumber || s.ComputerNumber == ComputerNumber).ToList();
            if (Serial != null && Serial != "")
                objs = objs.Where(s => s.Device.Serial == Serial).ToList();
            if (RequestType != 0 && RequestType != null)
                objs = objs.Where(s => s.RequestType == RequestType).ToList();
            if (ConfirmType != 0)
                objs = objs.Where(s => s.ConfirmType == ConfirmType).ToList();


            return Json(new { data = objs.Skip(start).Take(length), draw = Request["draw"], recordsTotal = objs.Count(), recordsFiltered = objs.Count() }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetDeletedDeviceRequests(string Name, int? DeviceUnitId, int? RequestType, DateTime? Date)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string SearchFilter = Request["search[value]"];

            var objs = Name == null ?
                db.CenterDeviceRequest.AsNoTracking().Where(s => s.IsDeleted == true).Include(s => s.Device.CenterDeviceUnit.Center).OrderByDescending(s => s.Id).ToList()
                : db.CenterDeviceRequest.AsNoTracking().Where(s => s.IsDeleted == true && (s.Device.NameAr.Contains(Name) || s.Device.NameEn.Contains(Name) || s.DeviceName.Contains(Name))).Include(s => s.Device.CenterDeviceUnit.Center).OrderByDescending(s => s.Id).ToList();

            if (Date != null)
                objs = objs.Where(s => s.CreatedDate.Value.Date == Date.Value || s.CreatedDate.Value.Date == Date.Value.AddDays(1)).ToList();
            if (DeviceUnitId != null && DeviceUnitId != 0)
                objs = objs.Where(s => (s.WithNoBarCode != true && s.Device.CenterDeviceUnitId == DeviceUnitId) || (s.WithNoBarCode == true && s.DeviceUnitId == DeviceUnitId)).ToList();
            if (RequestType != 0 && RequestType != null)
                objs = objs.Where(s => s.RequestType == RequestType).ToList();


            return Json(new { data = objs.Skip(start).Take(length), draw = Request["draw"], recordsTotal = objs.Count(), recordsFiltered = objs.Count() }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Edit(int Id)
        {
            var Model = db.CenterDeviceRequest.Include(s => s.Device.CenterDeviceUnit.Center).FirstOrDefault(s => s.Id == Id);
            ViewBag.DeviceUnit = Model.DeviceUnitId.HasValue ?
                db.CenterDeviceUnit.FirstOrDefault(s => s.Id == Model.DeviceUnitId)
                : db.CenterDeviceUnit.FirstOrDefault(s => s.Id == Model.Device.CenterDeviceUnitId);

            ViewBag.Engineers = db.Engineer.Where(s => s.IsDeleted == false).ToList();
            var UserId = Convert.ToInt32(User.Identity.GetUserId());
            var Setting = db.Users.AsNoTracking().FirstOrDefault(s => s.Id == UserId);
            if (Setting.EngineerId.HasValue)
                ViewBag.SelectedEngineerId = Setting.EngineerId;
            else
                ViewBag.SelectedEngineerId = 0;

            return View(Model);
        }

        [HttpPost]
        public ActionResult Edit(CenterDeviceRequest DeviceRequest)
        {
            var Model = db.CenterDeviceRequest.Find(DeviceRequest.Id);
            if (Model == null)
            {
                return HttpNotFound();
            }
            Model.IsActive = DeviceRequest.IsActive;
            if (Model.RequestType != 2)
            {
                Model.ConfirmType = DeviceRequest.ConfirmType;
                if (Model.ConfirmType >= 7)
                    Model.ConfirmedDate = DateTime.Now.AddHours(10);
            }
            Model.ReportDate = DeviceRequest.ReportDate;
            Model.Notes = DeviceRequest.Notes;
            Model.EngineerId = DeviceRequest.EngineerId == 0 ? null : DeviceRequest.EngineerId;
            Model.ModifiedDate = DateTime.Now.AddHours(10);
            Model.ModifierName = User.Identity.GetUserName();
            db.Entry(Model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DEdit(CenterDeviceRequest DeviceRequest)
        {
            var Model = db.CenterDeviceRequest.Find(DeviceRequest.Id);
            if (Model == null)
            {
                return HttpNotFound();
            }
            Model.IsActive = DeviceRequest.IsActive;
            Model.ModifiedDate = DateTime.Now.AddHours(10);
            Model.ModifierName = User.Identity.GetUserName();
            db.Entry(Model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DIndex");
        }

        public ActionResult Detail(int Id)
        {
            var Model = db.CenterDeviceRequest.Include(s => s.Device.CenterDeviceUnit.Center).FirstOrDefault(s => s.Id == Id);
            ViewBag.DeviceUnit = Model.DeviceUnitId.HasValue ?
                db.CenterDeviceUnit.FirstOrDefault(s => s.Id == Model.DeviceUnitId)
                : db.CenterDeviceUnit.FirstOrDefault(s => s.Id == Model.Device.CenterDeviceUnitId);
            return View(Model);
        }

        public ActionResult DDetail(int Id)
        {
            var Model = db.CenterDeviceRequest.Include(s => s.Device.CenterDeviceUnit.Center).FirstOrDefault(s => s.Id == Id);
            ViewBag.DeviceUnit = Model.DeviceUnitId.HasValue ?
                db.CenterDeviceUnit.FirstOrDefault(s => s.Id == Model.DeviceUnitId)
                : db.CenterDeviceUnit.FirstOrDefault(s => s.Id == Model.Device.CenterDeviceUnitId);
            return View(Model);
        }



        public ActionResult Print(int Id)
        {
            var Model = db.CenterDeviceRequest.Include(s => s.Device.CenterDeviceUnit.Center).FirstOrDefault(s => s.Id == Id);
            ViewBag.DeviceUnit = Model.DeviceUnitId.HasValue ?
                db.CenterDeviceUnit.FirstOrDefault(s => s.Id == Model.DeviceUnitId)
                : db.CenterDeviceUnit.FirstOrDefault(s => s.Id == Model.Device.CenterDeviceUnitId);

            ViewBag.ReportSetting = db.CenterReportSetting.OrderBy(s => s.Sort);
            ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;

            ViewBag.Engineer = Model.EngineerId != null ? db.Engineer.FirstOrDefault(s => s.Id == Model.EngineerId).NameAr : null;
            var EngineerSignImg = Model.EngineerId != null ? db.Engineer.FirstOrDefault(s => s.Id == Model.EngineerId).SignImg : null;
            if (EngineerSignImg == null || EngineerSignImg == "")
                ViewBag.EngineerSignImg = null;
            else
                ViewBag.EngineerSignImg = EngineerSignImg;

            return View(Model);
        }

        public ActionResult Print2(int Id)
        {
            var Model = db.CenterDeviceRequest.Include(s => s.Device.CenterDeviceUnit.Center).FirstOrDefault(s => s.Id == Id);
            ViewBag.DeviceUnit = Model.DeviceUnitId.HasValue ?
                db.CenterDeviceUnit.FirstOrDefault(s => s.Id == Model.DeviceUnitId)
                : db.CenterDeviceUnit.FirstOrDefault(s => s.Id == Model.Device.CenterDeviceUnitId);

            ViewBag.Center = db.Center.AsNoTracking().FirstOrDefault(s => s.Id == Model.Device.CenterDeviceUnit.CenterId);
            ViewBag.ReportSetting = db.CenterReportSetting.OrderBy(s => s.Sort);
            ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
            return View(Model);
        }

        public ActionResult DeleteConfirmed(int id, string DeleteReason)
        {
            CenterDeviceRequest DeviceRequest = db.CenterDeviceRequest.Find(id);
            DeviceRequest.IsDeleted = true;
            DeviceRequest.DeletedDate = DateTime.Now.AddHours(10);
            DeviceRequest.DeleteReason = DeleteReason;
            DeviceRequest.DeleterName = User.Identity.GetUserName();
            db.Entry(DeviceRequest).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            CenterDeviceRequest DeviceRequest = db.CenterDeviceRequest.Find(id);
            db.CenterDeviceRequest.Remove(DeviceRequest);
            db.SaveChanges();
            return RedirectToAction("DIndex");
        }

        public ActionResult DeleteSelected(string Selected)
        {
            var List = Selected.Split(',');
            foreach (var item in List)
            {
                if (item != "")
                {
                    var Id = Convert.ToInt32(item);
                    CenterDeviceRequest DeviceRequest = db.CenterDeviceRequest.Find(Id);
                    db.CenterDeviceRequest.Remove(DeviceRequest);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("DIndex");
        }

        public ActionResult DeleteConfirmedSelected(string Selected)
        {
            var List = Selected.Split(',');
            foreach (var item in List)
            {
                if (item != "")
                {
                    var Id = Convert.ToInt32(item);
                    CenterDeviceRequest DeviceRequest = db.CenterDeviceRequest.Find(Id);
                    DeviceRequest.IsDeleted = true;
                    DeviceRequest.DeletedDate = DateTime.Now.AddHours(10);
                    DeviceRequest.DeleteReason = "---";
                    DeviceRequest.DeleterName = User.Identity.GetUserName();
                    db.Entry(DeviceRequest).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Confirm(int id)
        {
            CenterDeviceRequest DeviceRequest = db.CenterDeviceRequest.Find(id);
            var Device = db.CenterDevice.Find(DeviceRequest.DeviceId);
            if (DeviceRequest.DeviceUnitId.HasValue)
            {
                Device.CenterDeviceUnitId = DeviceRequest.DeviceUnitId.Value;
                db.Entry(Device).State = EntityState.Modified;
                db.SaveChanges();
            }
            DeviceRequest.ConfirmType = 8;
            db.Entry(DeviceRequest).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}