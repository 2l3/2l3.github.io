using Hospital.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Hospital.Controllers
{
    public class WebsiteController : BaseController
    {
        // GET: Website
        DBContext db = new DBContext();
        private static int DeviceRequestId = 0;
        private static int CenterDeviceRequestId = 0;

        public ActionResult SendRequest(int DeviceId, string Password)
        {
            if (db.WebsiteData.Any(s => s.Password != null && s.Password != "") && (Password == null || Password == ""))
            {
                ViewBag.Error = null;
                ViewBag.Type = "SendRequest";
                ViewBag.DeviceId = DeviceId;
                return View("Login");
            }
            else
            {
                if (db.WebsiteData.Any(s => s.Password != null && s.Password != "") == false)
                {
                    ViewBag.List = db.DeviceRequest.AsNoTracking().Where(s => s.DeviceId == DeviceId && s.IsActive == true).OrderByDescending(s => s.Id);
                    var Model = db.Device.FirstOrDefault(s => s.Id == DeviceId && s.IsDeleted == false);
                    if (Model == null) return View("DeviceDeleted");
                    ViewBag.DeviceUnits = db.DeviceUnit.ToList();
                    return View(Model);
                }
                else
                {
                    if (db.WebsiteData.Any(s => s.Password.Equals(Password)))
                    {
                        ViewBag.List = db.DeviceRequest.AsNoTracking().Where(s => s.DeviceId == DeviceId && s.IsActive == true).OrderByDescending(s => s.Id);
                        var Model = db.Device.FirstOrDefault(s => s.Id == DeviceId && s.IsDeleted == false);
                        if (Model == null) return View("DeviceDeleted");
                        ViewBag.DeviceUnits = db.DeviceUnit.ToList();
                        return View(Model);
                    }
                    else
                    {
                        ViewBag.Error = LanguageID == 1 ? "الرقم السري خطأ" : "Password not correct";
                        ViewBag.Type = "SendRequest";
                        ViewBag.DeviceId = DeviceId;
                        return View("Login");
                    }
                }
            }
        }

        public ActionResult SendCenterRequest(int DeviceId, string Password)
        {
            if (db.CenterDevice.Any(s => s.Id == DeviceId && s.CenterDeviceUnit.Center.Password != null && s.CenterDeviceUnit.Center.Password != "") && (Password == null || Password == ""))
            {
                ViewBag.Error = null;
                ViewBag.Type = "SendCenterRequest";
                ViewBag.DeviceId = DeviceId;
                return View("Login");
            }
            else
            {
                if (db.CenterDevice.Any(s => s.Id == DeviceId && s.CenterDeviceUnit.Center.Password != null && s.CenterDeviceUnit.Center.Password != "") == false)
                {
                    ViewBag.List = db.CenterDeviceRequest.AsNoTracking().Where(s => s.DeviceId == DeviceId && s.IsActive == true).OrderByDescending(s => s.Id);
                    var Model = db.CenterDevice.FirstOrDefault(s => s.Id == DeviceId && s.IsDeleted == false);
                    if (Model == null) return View("DeviceDeleted");
                    ViewBag.DeviceUnits = db.CenterDeviceUnit.ToList();
                    return View(Model);
                }
                else
                {
                    if (db.CenterDevice.Any(s => s.Id == DeviceId && s.CenterDeviceUnit.Center.Password.Equals(Password)))
                    {
                        ViewBag.List = db.CenterDeviceRequest.AsNoTracking().Where(s => s.DeviceId == DeviceId && s.IsActive == true).OrderByDescending(s => s.Id);
                        var Model = db.CenterDevice.FirstOrDefault(s => s.Id == DeviceId && s.IsDeleted == false);
                        if (Model == null) return View("DeviceDeleted");
                        ViewBag.DeviceUnits = db.CenterDeviceUnit.ToList();
                        return View(Model);
                    }
                    else
                    {
                        ViewBag.Error = LanguageID == 1 ? "الرقم السري خطأ" : "Password not correct";
                        ViewBag.Type = "SendCenterRequest";
                        ViewBag.DeviceId = DeviceId;
                        return View("Login");
                    }
                }
            }
        }

        public ActionResult SendCenterRequestNoBarCode()
        {
            ViewBag.Centers = db.Center.AsNoTracking().Where(s => s.IsDeleted == false);
            return View();
        }

        [HttpPost]
        public ActionResult SendCenterRequestNoBarCode2(int CenterId, string Password)
        {
            if (db.Center.Any(s => s.Id == CenterId && s.Password != null && s.Password != "") && (Password == null || Password == ""))
            {
                ViewBag.Error = null;
                ViewBag.Type = "SendCenterRequestNoBarCode2";
                ViewBag.DeviceId = CenterId;
                return View("Login");
            }
            else
            {
                if (db.Center.Any(s => s.Id == CenterId && s.Password != null && s.Password != "") == false)
                {
                    ViewBag.Center = db.Center.AsNoTracking().FirstOrDefault(s => s.Id == CenterId);
                    ViewBag.DeviceUnits = db.CenterDeviceUnit.AsNoTracking().Where(s => s.CenterId == CenterId).ToList();
                    return View();
                }
                else
                {
                    if (db.Center.Any(s => s.Id == CenterId && s.Password.Equals(Password)))
                    {
                        ViewBag.Center = db.Center.AsNoTracking().FirstOrDefault(s => s.Id == CenterId);
                        ViewBag.DeviceUnits = db.CenterDeviceUnit.AsNoTracking().Where(s => s.CenterId == CenterId).ToList();
                        return View();
                    }
                    else
                    {
                        ViewBag.Error = LanguageID == 1 ? "الرقم السري خطأ" : "Password not correct";
                        ViewBag.Type = "SendCenterRequestNoBarCode2";
                        ViewBag.DeviceId = CenterId;
                        return View("Login");
                    }
                }
            }
        }

        public ActionResult SendRequestNoBarCode(string Password)
        {
            if (db.WebsiteData.Any(s => s.Password != null && s.Password != "") && (Password == null || Password == ""))
            {
                ViewBag.Error = null;
                ViewBag.Type = "SendRequestNoBarCode";
                ViewBag.DeviceId = 0;
                return View("Login");
            }
            else
            {
                if (db.WebsiteData.Any(s => s.Password != null && s.Password != "") == false)
                {
                    ViewBag.DeviceUnits = db.DeviceUnit.ToList();
                    return View();
                }
                else
                {
                    if (db.WebsiteData.Any(s => s.Password.Equals(Password)))
                    {
                        ViewBag.DeviceUnits = db.DeviceUnit.ToList();
                        return View();
                    }
                    else
                    {
                        ViewBag.Error = LanguageID == 1 ? "الرقم السري خطأ" : "Password not correct";
                        ViewBag.Type = "SendRequestNoBarCode";
                        ViewBag.DeviceId = 0;
                        return View("Login");
                    }
                }
            }
        }

        [HttpPost]
        public ActionResult SendRequestNoBarCode(DeviceRequest DeviceRequest)
        {
            if (ModelState.IsValid)
            {
                DeviceRequest.WithNoBarCode = true;
                DeviceRequest.IsDeleted = false;
                DeviceRequest.IsActive = true;
                DeviceRequest.CreatedDate = DateTime.Now.AddHours(10);
                DeviceRequest.RequestType = 1;
                DeviceRequest.ConfirmType = 1;
                db.DeviceRequest.Add(DeviceRequest);
                db.SaveChanges();
                DeviceRequestId = DeviceRequest.Id;
                CenterDeviceRequestId = 0;
                return RedirectToAction("RequestSent");
            }
            return View(DeviceRequest);
        }

        [HttpPost]
        public ActionResult SendCenterRequestNoBarCode(CenterDeviceRequest DeviceRequest)
        {
            if (ModelState.IsValid)
            {
                DeviceRequest.WithNoBarCode = true;
                DeviceRequest.IsDeleted = false;
                DeviceRequest.IsActive = true;
                if (DeviceRequest.DeviceUnitId.HasValue)
                {
                    var D = db.CenterDeviceUnit.AsNoTracking().Include(s => s.Center).FirstOrDefault(s => s.Id == DeviceRequest.DeviceUnitId);
                    DeviceRequest.CenterNameAr = D.Center.NameAr;
                    DeviceRequest.CenterNameEn = D.Center.NameEn;
                }
                DeviceRequest.CreatedDate = DateTime.Now.AddHours(10);
                DeviceRequest.RequestType = 1;
                DeviceRequest.ConfirmType = 1;
                db.CenterDeviceRequest.Add(DeviceRequest);
                db.SaveChanges();
                CenterDeviceRequestId = DeviceRequest.Id;
                DeviceRequestId = 0;
                return RedirectToAction("RequestSent");
            }
            return View(DeviceRequest);
        }

        [HttpPost]
        public ActionResult SendRequest(DeviceRequest DeviceRequest)
        {
            if (ModelState.IsValid)
            {
                DeviceRequest.IsDeleted = false;
                DeviceRequest.IsActive = true;
                DeviceRequest.CreatedDate = DateTime.Now.AddHours(10);
                if (DeviceRequest.RequestType == 1 || DeviceRequest.RequestType == 3)
                {
                    DeviceRequest.DeviceUnitId = null;
                }
                else
                {
                    var To = db.DeviceUnit.AsNoTracking().FirstOrDefault(s => s.Id == DeviceRequest.DeviceUnitId);
                    DeviceRequest.ToUnitEn = To.NameEn;
                    DeviceRequest.ToUnitAr = To.NameAr;
                }
                DeviceRequest.ConfirmType = 1;
                db.DeviceRequest.Add(DeviceRequest);
                db.SaveChanges();
                DeviceRequestId = DeviceRequest.Id;
                CenterDeviceRequestId = 0;
                return RedirectToAction("RequestSent");
            }
            return View(DeviceRequest);
        }

        [HttpPost]
        public ActionResult SendCenterRequest(CenterDeviceRequest DeviceRequest)
        {
            if (ModelState.IsValid)
            {
                DeviceRequest.IsDeleted = false;
                DeviceRequest.IsActive = true;
                DeviceRequest.CreatedDate = DateTime.Now.AddHours(10);
                if (DeviceRequest.RequestType == 1 || DeviceRequest.RequestType == 3)
                {
                    DeviceRequest.DeviceUnitId = null;
                }
                else
                {
                    var To = db.CenterDeviceUnit.AsNoTracking().FirstOrDefault(s => s.Id == DeviceRequest.DeviceUnitId);
                    DeviceRequest.ToUnitEn = To.NameEn;
                    DeviceRequest.ToUnitAr = To.NameAr;
                }
                DeviceRequest.ConfirmType = 1;
                db.CenterDeviceRequest.Add(DeviceRequest);
                db.SaveChanges();
                CenterDeviceRequestId = DeviceRequest.Id;
                DeviceRequestId = 0;
                return RedirectToAction("RequestSent");
            }
            return View(DeviceRequest);
        }

        public ActionResult RequestSent()
        {
            if (CenterDeviceRequestId != 0)
            {
                ViewBag.Device = db.CenterDeviceRequest.Include(s => s.Device).FirstOrDefault(s => s.Id == CenterDeviceRequestId);
            }
            else
            {
                ViewBag.Device = db.DeviceRequest.Include(s => s.Device).FirstOrDefault(s => s.Id == DeviceRequestId);
            }
            return View();
        }
    }
}