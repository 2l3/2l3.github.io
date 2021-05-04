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
    public class DeviceRequestController : BaseController
    {
        private DBContext db = new DBContext();
        // GET: Admin/DeviceRequest
        [Authorize(Roles = "HospitalRequest")]
        public ActionResult Index()
        {
            ViewBag.DeviceUnits = db.DeviceUnit.ToList();
            return View();
        }

        [Authorize(Roles = "HospitalCanceledRequest")]
        public ActionResult DIndex()
        {
            ViewBag.DeviceUnits = db.DeviceUnit.ToList();
            return View();
        }

        public ActionResult GetDeviceRequests(string Name, int? DeviceUnitId, int ConfirmType, int? RequestType, DateTime? Date, string ComputerNumber, string Serial)
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string SearchFilter = Request["search[value]"];

            var objs = Name == null ?
                db.DeviceRequest.AsNoTracking().Where(s => s.IsDeleted == false).Include(s => s.Device).OrderByDescending(s => s.Id).ToList()
                : db.DeviceRequest.AsNoTracking().Where(s => s.IsDeleted == false && (s.Device.NameAr.Contains(Name) || s.Device.NameEn.Contains(Name) || s.DeviceName.Contains(Name))).Include(s => s.Device).OrderByDescending(s => s.Id).ToList();

            if (Date != null)
                //objs = objs.Where(s => s.CreatedDate.Value.Date.Ticks == Date.Value.Date.AddHours(10).Ticks).ToList();
                objs = objs.Where(s => s.CreatedDate.Value.Date == Date.Value || s.CreatedDate.Value.Date == Date.Value.AddDays(1)).ToList();
            if (DeviceUnitId != null && DeviceUnitId != 0)
                objs = objs.Where(s => (s.WithNoBarCode != true && s.Device.DeviceUnitId == DeviceUnitId) || (s.WithNoBarCode == true && s.DeviceUnitId == DeviceUnitId)).ToList();
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
                db.DeviceRequest.AsNoTracking().Where(s => s.IsDeleted == true).Include(s => s.Device).OrderByDescending(s => s.Id).ToList()
                : db.DeviceRequest.AsNoTracking().Where(s => s.IsDeleted == true && (s.Device.NameAr.Contains(Name) || s.Device.NameEn.Contains(Name) || s.DeviceName.Contains(Name))).Include(s => s.Device).OrderByDescending(s => s.Id).ToList();

            if (Date != null)
                objs = objs.Where(s => s.CreatedDate.Value.Date == Date.Value || s.CreatedDate.Value.Date == Date.Value.AddDays(1)).ToList();
            if (DeviceUnitId != null && DeviceUnitId != 0)
                objs = objs.Where(s => s.WithNoBarCode != true && s.Device.DeviceUnitId == DeviceUnitId).ToList();
            if (RequestType != 0 && RequestType != null)
                objs = objs.Where(s => s.RequestType == RequestType).ToList();


            return Json(new { data = objs.Skip(start).Take(length), draw = Request["draw"], recordsTotal = objs.Count(), recordsFiltered = objs.Count() }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Edit(int Id)
        {
            var Model = db.DeviceRequest.Include(s => s.Device.DeviceUnit).FirstOrDefault(s => s.Id == Id);
            ViewBag.DeviceUnit = Model.DeviceUnitId.HasValue ?
                db.DeviceUnit.FirstOrDefault(s => s.Id == Model.DeviceUnitId)
                : db.DeviceUnit.FirstOrDefault(s => s.Id == Model.Device.DeviceUnitId);

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
        public ActionResult Edit(DeviceRequest DeviceRequest)
        {
            var Model = db.DeviceRequest.Find(DeviceRequest.Id);
            if (Model == null)
            {
                return HttpNotFound();
            }
            //var ConfirmType = Model.ConfirmType;
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
            //if (Model.RequestType == 1 && Model.ConfirmType == 7 && Model.ConfirmType != ConfirmType)
            //    return Edit(Model.Id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DEdit(DeviceRequest DeviceRequest)
        {
            var Model = db.DeviceRequest.Find(DeviceRequest.Id);
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
            var Model = db.DeviceRequest.Include(s => s.Device.DeviceUnit).FirstOrDefault(s => s.Id == Id);
            ViewBag.DeviceUnit = Model.DeviceUnitId.HasValue ?
                db.DeviceUnit.FirstOrDefault(s => s.Id == Model.DeviceUnitId)
                : db.DeviceUnit.FirstOrDefault(s => s.Id == Model.Device.DeviceUnitId);
            return View(Model);
        }

        public ActionResult DDetail(int Id)
        {
            var Model = db.DeviceRequest.Include(s => s.Device.DeviceUnit).FirstOrDefault(s => s.Id == Id);
            ViewBag.DeviceUnit = Model.DeviceUnitId.HasValue ?
                db.DeviceUnit.FirstOrDefault(s => s.Id == Model.DeviceUnitId)
                : db.DeviceUnit.FirstOrDefault(s => s.Id == Model.Device.DeviceUnitId);
            return View(Model);
        }



        public ActionResult Print(int Id)
        {
            var Model = db.DeviceRequest.Include(s => s.Device.DeviceUnit).FirstOrDefault(s => s.Id == Id);
            ViewBag.DeviceUnit = Model.DeviceUnitId.HasValue ?
                db.DeviceUnit.FirstOrDefault(s => s.Id == Model.DeviceUnitId)
                : db.DeviceUnit.FirstOrDefault(s => s.Id == Model.Device.DeviceUnitId);

            ViewBag.ReportSetting = db.ReportSetting.OrderBy(s => s.Sort);
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
            var Model = db.DeviceRequest.Include(s => s.Device.DeviceUnit).FirstOrDefault(s => s.Id == Id);
            ViewBag.DeviceUnit = Model.DeviceUnitId.HasValue ?
                db.DeviceUnit.FirstOrDefault(s => s.Id == Model.DeviceUnitId)
                : db.DeviceUnit.FirstOrDefault(s => s.Id == Model.Device.DeviceUnitId);

            ViewBag.ReportSetting = db.ReportSetting.OrderBy(s => s.Sort);
            ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
            return View(Model);
        }

        public ActionResult DeleteConfirmed(int id, string DeleteReason)
        {
            DeviceRequest DeviceRequest = db.DeviceRequest.Find(id);
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
            DeviceRequest DeviceRequest = db.DeviceRequest.Find(id);
            db.DeviceRequest.Remove(DeviceRequest);
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
                    DeviceRequest DeviceRequest = db.DeviceRequest.Find(Id);
                    db.DeviceRequest.Remove(DeviceRequest);
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
                    DeviceRequest DeviceRequest = db.DeviceRequest.Find(Id);
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
            DeviceRequest DeviceRequest = db.DeviceRequest.Find(id);
            var Device = db.Device.Find(DeviceRequest.DeviceId);
            if (DeviceRequest.DeviceUnitId.HasValue)
            {
                Device.DeviceUnitId = DeviceRequest.DeviceUnitId.Value;
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