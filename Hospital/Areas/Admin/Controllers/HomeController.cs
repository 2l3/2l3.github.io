using Hospital.Controllers;
using Hospital.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Hospital.Areas.Admin.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private static string Message { set; get; }
        private DBContext db = new DBContext();
        private IAuthenticationManager _authenticationManager = null;
        public ActionResult Index()
        {
            ViewBag.SystemUser = db.Users.Where(s => s.IsDeleted == false && s.Id != 1).Count();
            ViewBag.Device = db.Device.Where(s => s.IsDeleted == false).Count();

            ViewBag.DeviceRequest = db.DeviceRequest.Where(s => s.IsDeleted == false).Count();
            ViewBag.DeletedDeviceRequest = db.DeviceRequest.Where(s => s.IsDeleted == true).Count();
            ViewBag.ConfrimedRequest = db.DeviceRequest.Where(s => s.IsDeleted == false && s.ConfirmType == 7).Count();
            ViewBag.NewRequest = db.DeviceRequest.Where(s => s.IsDeleted == false && s.ConfirmType == 1).Count();
            ViewBag.ConfrimingRequest = db.DeviceRequest.Where(s => s.IsDeleted == false && s.ConfirmType == 2).Count();
            ViewBag.Confrim2 = db.DeviceRequest.Where(s => s.IsDeleted == false && s.ConfirmType == 3).Count();
            ViewBag.Confrim3 = db.DeviceRequest.Where(s => s.IsDeleted == false && s.ConfirmType == 4).Count();
            ViewBag.Confrim4 = db.DeviceRequest.Where(s => s.IsDeleted == false && s.ConfirmType == 5).Count();
            ViewBag.Confrim5 = db.DeviceRequest.Where(s => s.IsDeleted == false && s.ConfirmType == 6).Count();

            return View();
        }

        public ActionResult LogOut()
        {
            _authenticationManager = HttpContext.GetOwinContext().Authentication;
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Login", new { area = "" });
        }

        public ActionResult EditProfile()
        {
            var UserId = Convert.ToInt32(User.Identity.GetUserId());
            UserChangePassWordVM model = new UserChangePassWordVM();
            model.UserId = UserId;
            ViewBag.Message = Message;
            return View(model);
        }

        public ActionResult Notification()
        {
            return Json(db.DeviceRequest.AsNoTracking().Where(s => s.IsDeleted == false && s.ConfirmType == 1).Count(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Notification4()
        {
            return Json(db.CenterDeviceRequest.AsNoTracking().Where(s => s.IsDeleted == false && s.ConfirmType == 1).Count(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Notification2()
        {
            var CurrentDay = (int)DateTime.Now.AddHours(10).Day;
            var CurrentMonth = DateTime.Now.AddHours(10).Month;
            var Year = DateTime.Now.AddHours(10).Year;
            return Json(db.DeviceSchedule.AsNoTracking().Where(s => s.IsDeleted == false && CurrentDay >= s.DayId && CurrentMonth == s.MonthId && Year > s.LastCloseYear).Count(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Notification3()
        {
            var CurrentDay = (int)DateTime.Now.AddHours(10).Day;
            var CurrentMonth = DateTime.Now.AddHours(10).Month;
            var Year = DateTime.Now.AddHours(10).Year;
            return Json(db.CenterDeviceSchedule.AsNoTracking().Where(s => s.IsDeleted == false && CurrentDay >= s.DayId && CurrentMonth == s.MonthId && Year > s.LastCloseYear).Count(), JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> ChangePassword(UserChangePassWordVM model)
        {
            Message = "";
            var UserManager = new UserManager<ApplicationUser, int>(new UserStore<ApplicationUser, Role, int, UserLogin, UserRole, UserClaim>(new DBContext()));
            // User does not have a password so remove any validation errors caused by a missing OldPassword field
            ModelState state = ModelState["OldPassword"];
            if (state != null)
            {
                state.Errors.Clear();
            }
            if (model.NewPassword.Length < 6)
            {
                Message = LanguageID == 1 ? "الرقم السري يجب الأ يقل عن 6 خانات" : "Password Cannot be less than 6 entries";
                RedirectToAction("EditProfile");
            }
            if (!model.NewPassword.Equals(model.ConfirmPassword))
            {
                Message = LanguageID == 1 ? "الرقم السري غير مطابق للتأكيد الخاص به" : "Password not equal Confirm Password";
                RedirectToAction("EditProfile");
            }

            if (ModelState.IsValid)
            {
                IdentityResult result = await UserManager.ChangePasswordAsync(model.UserId, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    Message = LanguageID == 1 ? "تم تغيير الرقم السري بنجاح" : "Password changed successfully";
                    RedirectToAction("EditProfile");
                }
                else
                {
                    Message = LanguageID == 1 ? "الرقم السري القديم خطأ " : "Old Password Not Correct";
                    RedirectToAction("EditProfile");
                }
            }
            return RedirectToAction("EditProfile");

        }

        public ActionResult DevicesReport()
        {
            ViewBag.Centers = db.Center.AsNoTracking().Where(s => s.IsDeleted == false);
            return View();
        }

        public ActionResult PrintDevicesReport(int Type, DateTime DateFrom, DateTime DateTo)
        {
            ViewBag.Type = Type;
            var Data = new List<DeviceRequest>();
            ViewBag.DateFrom = DateFrom.ToShortDateString();
            ViewBag.DateTo = DateTo.ToShortDateString();
            DateTo = DateTo.AddHours(23).AddMinutes(59);
            if (Type == 1)
                Data = db.DeviceRequest.AsNoTracking().Include(s => s.Device.DeviceUnit).Where(s => s.IsDeleted == false && s.ConfirmType < 7 && s.CreatedDate >= DateFrom && s.CreatedDate <= DateTo).OrderByDescending(s => s.Id).ToList();
            else if (Type == 2)
                Data = db.DeviceRequest.AsNoTracking().Include(s => s.Device.DeviceUnit).Where(s => s.IsDeleted == false && s.ConfirmType == 7 && s.ConfirmedDate >= DateFrom && s.ConfirmedDate <= DateTo).OrderByDescending(s => s.Id).ToList();
            ViewBag.ReportSetting = db.ReportSetting.OrderBy(s => s.Sort);
            ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
            return View(Data.Where(s => s.DeviceId.HasValue).ToList());
        }

        public ActionResult PrintCenterDevicesReport(int Type, DateTime DateFrom, DateTime DateTo, int CenterId)
        {
            ViewBag.Center = db.Center.AsNoTracking().FirstOrDefault(s => s.Id == CenterId);
            ViewBag.Type = Type;
            var Data = new List<CenterDeviceRequest>();
            ViewBag.DateFrom = DateFrom.ToShortDateString();
            ViewBag.DateTo = DateTo.ToShortDateString();
            DateTo = DateTo.AddHours(23).AddMinutes(59);
            if (CenterId == 0)
            {
                if (Type == 1)
                    Data = db.CenterDeviceRequest.AsNoTracking().Include(s => s.Device.CenterDeviceUnit.Center).Where(s => s.IsDeleted == false && s.ConfirmType < 7 && s.CreatedDate >= DateFrom && s.CreatedDate <= DateTo).OrderByDescending(s => s.Id).ToList();
                else if (Type == 2)
                    Data = db.CenterDeviceRequest.AsNoTracking().Include(s => s.Device.CenterDeviceUnit.Center).Where(s => s.IsDeleted == false && s.ConfirmType == 7 && s.ConfirmedDate >= DateFrom && s.ConfirmedDate <= DateTo).OrderByDescending(s => s.Id).ToList();
            }
            else
            {
                if (Type == 1)
                    Data = db.CenterDeviceRequest.AsNoTracking().Include(s => s.Device.CenterDeviceUnit).Where(s => s.IsDeleted == false && s.Device.CenterDeviceUnit.CenterId == CenterId && s.ConfirmType < 7 && s.CreatedDate >= DateFrom && s.CreatedDate <= DateTo).OrderByDescending(s => s.Id).ToList();
                else if (Type == 2)
                    Data = db.CenterDeviceRequest.AsNoTracking().Include(s => s.Device.CenterDeviceUnit).Where(s => s.IsDeleted == false && s.Device.CenterDeviceUnit.CenterId == CenterId && s.ConfirmType == 7 && s.ConfirmedDate >= DateFrom && s.ConfirmedDate <= DateTo).OrderByDescending(s => s.Id).ToList();
            }

            ViewBag.ReportSetting = db.CenterReportSetting.OrderBy(s => s.Sort);
            ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
            return View(Data.Where(s => s.DeviceId.HasValue).ToList());
        }

        public ActionResult AutoPrintCenterDevicesReport(int Type, DateTime DateFrom, DateTime DateTo, int CenterId)
        {
            ViewBag.Center = db.Center.AsNoTracking().FirstOrDefault(s => s.Id == CenterId);
            ViewBag.Type = Type;
            var Data = new List<CenterDeviceRequest>();
            ViewBag.DateFrom = DateFrom.ToShortDateString();
            ViewBag.DateTo = DateTo.ToShortDateString();
            DateTo = DateTo.AddHours(23).AddMinutes(59);
            if (CenterId == 0)
            {
                if (Type == 1)
                    Data = db.CenterDeviceRequest.AsNoTracking().Include(s => s.Device.CenterDeviceUnit.Center).Where(s => s.IsDeleted == false && s.ConfirmType < 7 && s.CreatedDate >= DateFrom && s.CreatedDate <= DateTo).OrderByDescending(s => s.Id).ToList();
                else if (Type == 2)
                    Data = db.CenterDeviceRequest.AsNoTracking().Include(s => s.Device.CenterDeviceUnit.Center).Where(s => s.IsDeleted == false && s.ConfirmType == 7 && s.ConfirmedDate >= DateFrom && s.ConfirmedDate <= DateTo).OrderByDescending(s => s.Id).ToList();
            }
            else
            {
                if (Type == 1)
                    Data = db.CenterDeviceRequest.AsNoTracking().Include(s => s.Device.CenterDeviceUnit).Where(s => s.IsDeleted == false && s.Device.CenterDeviceUnit.CenterId == CenterId && s.ConfirmType < 7 && s.CreatedDate >= DateFrom && s.CreatedDate <= DateTo).OrderByDescending(s => s.Id).ToList();
                else if (Type == 2)
                    Data = db.CenterDeviceRequest.AsNoTracking().Include(s => s.Device.CenterDeviceUnit).Where(s => s.IsDeleted == false && s.Device.CenterDeviceUnit.CenterId == CenterId && s.ConfirmType == 7 && s.ConfirmedDate >= DateFrom && s.ConfirmedDate <= DateTo).OrderByDescending(s => s.Id).ToList();
            }

            ViewBag.ReportSetting = db.CenterReportSetting.OrderBy(s => s.Sort);
            ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
            return View(Data.Where(s => s.DeviceId.HasValue).ToList());
        }


        public ActionResult PrintDReport()
        {
            ViewBag.ReportSetting = db.ReportSetting.OrderBy(s => s.Sort);
            ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
            return View(db.Device.AsNoTracking().Where(s => s.IsDeleted == false).Include(s => s.DeviceUnit).ToList());
        }

        public ActionResult PrintCDReport(int CenterId)
        {
            if (CenterId == 0)
            {
                ViewBag.Center = null;
                ViewBag.ReportSetting = db.CenterReportSetting.OrderBy(s => s.Sort);
                ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
                return View(db.CenterDevice.AsNoTracking().Where(s => s.IsDeleted == false).Include(s => s.CenterDeviceUnit.Center).ToList());
            }
            else
            {
                ViewBag.Center = db.Center.AsNoTracking().FirstOrDefault(s => s.Id == CenterId);
                ViewBag.ReportSetting = db.CenterReportSetting.OrderBy(s => s.Sort);
                ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
                return View(db.CenterDevice.AsNoTracking().Where(s => s.IsDeleted == false && s.CenterDeviceUnit.CenterId == CenterId).Include(s => s.CenterDeviceUnit).ToList());
            }
        }

        public ActionResult AutoPrintCDReport(int CenterId)
        {
            if (CenterId == 0)
            {
                ViewBag.Center = null;
                ViewBag.ReportSetting = db.CenterReportSetting.OrderBy(s => s.Sort);
                ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
                return View(db.CenterDevice.AsNoTracking().Where(s => s.IsDeleted == false).Include(s => s.CenterDeviceUnit.Center).ToList());
            }
            else
            {
                ViewBag.Center = db.Center.AsNoTracking().FirstOrDefault(s => s.Id == CenterId);
                ViewBag.ReportSetting = db.CenterReportSetting.OrderBy(s => s.Sort);
                ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
                return View(db.CenterDevice.AsNoTracking().Where(s => s.IsDeleted == false && s.CenterDeviceUnit.CenterId == CenterId).Include(s => s.CenterDeviceUnit).ToList());
            }
        }

        public ActionResult AutoPrintDReport()
        {
            ViewBag.ReportSetting = db.ReportSetting.OrderBy(s => s.Sort);
            ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
            return View(db.Device.AsNoTracking().Where(s => s.IsDeleted == false).Include(s => s.DeviceUnit).ToList());
        }

        public ActionResult AutoPrintDevicesReport(int Type, DateTime DateFrom, DateTime DateTo)
        {
            ViewBag.Type = Type;
            var Data = new List<DeviceRequest>();
            ViewBag.DateFrom = DateFrom.ToShortDateString();
            ViewBag.DateTo = DateTo.ToShortDateString();
            DateTo = DateTo.AddHours(23).AddMinutes(59);
            if (Type == 1)
                Data = db.DeviceRequest.AsNoTracking().Include(s => s.Device.DeviceUnit).Where(s => s.IsDeleted == false && s.ConfirmType < 7 && s.CreatedDate >= DateFrom && s.CreatedDate <= DateTo).OrderByDescending(s => s.Id).ToList();
            else if (Type == 2)
                Data = db.DeviceRequest.AsNoTracking().Include(s => s.Device.DeviceUnit).Where(s => s.IsDeleted == false && s.ConfirmType == 7 && s.ConfirmedDate >= DateFrom && s.ConfirmedDate <= DateTo).OrderByDescending(s => s.Id).ToList();
            ViewBag.ReportSetting = db.ReportSetting.OrderBy(s => s.Sort);
            ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
            return View(Data.Where(s => s.DeviceId.HasValue).ToList());
        }

        public ActionResult DeviceUnitsReport()
        {
            ViewBag.Centers = db.Center.AsNoTracking().Where(s => s.IsDeleted == false);
            ViewBag.CenterDeviceUnits = db.CenterDeviceUnit.AsNoTracking();
            return View(db.DeviceUnit.ToList());
        }

        public ActionResult PrintDeviceUnitsReport(int Type, int DeviceUnitId)
        {
            var DeviceRequests = new List<DeviceRequest>();
            if (Type == 1)
                DeviceRequests = db.DeviceRequest.AsNoTracking().Include(s => s.Device.DeviceUnit).Where(s => s.IsDeleted == false && s.ConfirmType < 7).OrderByDescending(s => s.Id).ToList();
            else if (Type == 2)
                DeviceRequests = db.DeviceRequest.AsNoTracking().Include(s => s.Device.DeviceUnit).Where(s => s.IsDeleted == false && s.ConfirmType == 7).OrderByDescending(s => s.Id).ToList();
            else
            {
                var Data = new List<Device>();
                Data = db.Device.AsNoTracking().Include(s => s.DeviceUnit).Where(s => s.IsDeleted == false && s.DeviceUnitId == DeviceUnitId).OrderByDescending(s => s.Id).ToList();
                ViewBag.Data = Data;
                ViewBag.DeviceUnit = db.DeviceUnit.AsNoTracking().FirstOrDefault(S => S.Id == DeviceUnitId);
                ViewBag.ReportSetting = db.ReportSetting.OrderBy(s => s.Sort);
                ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
                return View("PrintDeviceUnitsReport2", db.DeviceUnit.ToList());
            }
            ViewBag.Type = Type;
            DeviceRequests = DeviceRequests.Where(s => s.DeviceId.HasValue).ToList();
            ViewBag.Data = DeviceRequests.Where(s => s.Device.DeviceUnitId == DeviceUnitId);
            ViewBag.DeviceUnit = db.DeviceUnit.AsNoTracking().FirstOrDefault(S => S.Id == DeviceUnitId);
            ViewBag.ReportSetting = db.ReportSetting.OrderBy(s => s.Sort);
            ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
            return View(db.DeviceUnit.ToList());
        }

        public ActionResult PrintCenterDeviceUnitsReport(int Type, int DeviceUnitId, int CenterId)
        {
            ViewBag.Center = db.Center.AsNoTracking().FirstOrDefault(s => s.Id == CenterId);
            var DeviceRequests = new List<CenterDeviceRequest>();
            if (Type == 1)
                DeviceRequests = db.CenterDeviceRequest.AsNoTracking().Include(s => s.Device.CenterDeviceUnit).Where(s => s.IsDeleted == false && s.ConfirmType < 7 && s.Device.CenterDeviceUnit.CenterId == CenterId).OrderByDescending(s => s.Id).ToList();
            else if (Type == 2)
                DeviceRequests = db.CenterDeviceRequest.AsNoTracking().Include(s => s.Device.CenterDeviceUnit).Where(s => s.IsDeleted == false && s.ConfirmType == 7 && s.Device.CenterDeviceUnit.CenterId == CenterId).OrderByDescending(s => s.Id).ToList();
            else
            {
                var Data = new List<CenterDevice>();
                Data = db.CenterDevice.AsNoTracking().Include(s => s.CenterDeviceUnit).Where(s => s.IsDeleted == false && s.CenterDeviceUnit.CenterId == CenterId).OrderByDescending(s => s.Id).ToList();
                if (DeviceUnitId != 0)
                    ViewBag.Data = Data.Where(s => s.CenterDeviceUnitId == DeviceUnitId);
                else
                    ViewBag.Data = Data;
                ViewBag.DeviceUnit = db.CenterDeviceUnit.AsNoTracking().FirstOrDefault(S => S.Id == DeviceUnitId);
                ViewBag.ReportSetting = db.CenterReportSetting.OrderBy(s => s.Sort);
                ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
                return View("PrintCenterDeviceUnitsReport2", db.CenterDeviceUnit.ToList());
            }
            ViewBag.Type = Type;
            DeviceRequests = DeviceRequests.Where(s => s.DeviceId.HasValue).ToList();
            if (DeviceUnitId != 0)
                ViewBag.Data = DeviceRequests.Where(s => s.Device.CenterDeviceUnitId == DeviceUnitId);
            else
                ViewBag.Data = DeviceRequests;
            ViewBag.DeviceUnit = db.CenterDeviceUnit.AsNoTracking().FirstOrDefault(S => S.Id == DeviceUnitId);
            ViewBag.ReportSetting = db.CenterReportSetting.OrderBy(s => s.Sort);
            ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
            return View(db.CenterDeviceUnit.ToList());
        }

        public ActionResult AutoPrintCenterDeviceUnitsReport(int Type, int DeviceUnitId, int CenterId)
        {
            ViewBag.Center = db.Center.AsNoTracking().FirstOrDefault(s => s.Id == CenterId);
            var DeviceRequests = new List<CenterDeviceRequest>();
            if (Type == 1)
                DeviceRequests = db.CenterDeviceRequest.AsNoTracking().Include(s => s.Device.CenterDeviceUnit).Where(s => s.IsDeleted == false && s.ConfirmType < 7 && s.Device.CenterDeviceUnit.CenterId == CenterId).OrderByDescending(s => s.Id).ToList();
            else if (Type == 2)
                DeviceRequests = db.CenterDeviceRequest.AsNoTracking().Include(s => s.Device.CenterDeviceUnit).Where(s => s.IsDeleted == false && s.ConfirmType == 7 && s.Device.CenterDeviceUnit.CenterId == CenterId).OrderByDescending(s => s.Id).ToList();
            else
            {
                var Data = new List<CenterDevice>();
                Data = db.CenterDevice.AsNoTracking().Include(s => s.CenterDeviceUnit).Where(s => s.IsDeleted == false && s.CenterDeviceUnit.CenterId == CenterId).OrderByDescending(s => s.Id).ToList();
                if (DeviceUnitId != 0)
                    ViewBag.Data = Data.Where(s => s.CenterDeviceUnitId == DeviceUnitId);
                else
                    ViewBag.Data = Data;
                ViewBag.DeviceUnit = db.CenterDeviceUnit.AsNoTracking().FirstOrDefault(S => S.Id == DeviceUnitId);
                ViewBag.ReportSetting = db.CenterReportSetting.OrderBy(s => s.Sort);
                ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
                return View("AutoPrintCenterDeviceUnitsReport2", db.CenterDeviceUnit.ToList());
            }
            ViewBag.Type = Type;
            DeviceRequests = DeviceRequests.Where(s => s.DeviceId.HasValue).ToList();
            if (DeviceUnitId != 0)
                ViewBag.Data = DeviceRequests.Where(s => s.Device.CenterDeviceUnitId == DeviceUnitId);
            else
                ViewBag.Data = DeviceRequests;
            ViewBag.DeviceUnit = db.CenterDeviceUnit.AsNoTracking().FirstOrDefault(S => S.Id == DeviceUnitId);
            ViewBag.ReportSetting = db.CenterReportSetting.OrderBy(s => s.Sort);
            ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
            return View(db.CenterDeviceUnit.ToList());
        }

        public ActionResult AutoPrintDeviceUnitsReport(int Type, int DeviceUnitId)
        {
            var DeviceRequests = new List<DeviceRequest>();
            if (Type == 1)
                DeviceRequests = db.DeviceRequest.AsNoTracking().Include(s => s.Device.DeviceUnit).Where(s => s.IsDeleted == false && s.ConfirmType < 7).OrderByDescending(s => s.Id).ToList();
            else if (Type == 2)
                DeviceRequests = db.DeviceRequest.AsNoTracking().Include(s => s.Device.DeviceUnit).Where(s => s.IsDeleted == false && s.ConfirmType == 7).OrderByDescending(s => s.Id).ToList();
            else
            {
                var Data = new List<Device>();
                Data = db.Device.AsNoTracking().Include(s => s.DeviceUnit).Where(s => s.IsDeleted == false && s.DeviceUnitId == DeviceUnitId).OrderByDescending(s => s.Id).ToList();
                ViewBag.Data = Data;
                ViewBag.DeviceUnit = db.DeviceUnit.AsNoTracking().FirstOrDefault(S => S.Id == DeviceUnitId);
                ViewBag.ReportSetting = db.ReportSetting.OrderBy(s => s.Sort);
                ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
                return View("AutoPrintDeviceUnitsReport2", db.DeviceUnit.ToList());
            }
            ViewBag.Type = Type;
            DeviceRequests = DeviceRequests.Where(s => s.DeviceId.HasValue).ToList();
            ViewBag.Data = DeviceRequests.Where(s => s.Device.DeviceUnitId == DeviceUnitId);
            ViewBag.DeviceUnit = db.DeviceUnit.AsNoTracking().FirstOrDefault(S => S.Id == DeviceUnitId);
            ViewBag.ReportSetting = db.ReportSetting.OrderBy(s => s.Sort);
            ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
            return View(db.DeviceUnit.ToList());
        }

        public ActionResult PrintHospitalReportTemplate()
        {
            return View();
        }

        public ActionResult EmptyJobOrderReport()
        {
            ViewBag.ReportSetting = db.ReportSetting.OrderBy(s => s.Sort);
            ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
            return View();
        }

        public ActionResult EmptyServiceRequest()
        {
            ViewBag.ReportSetting = db.ReportSetting.OrderBy(s => s.Sort);
            ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
            return View();
        }

        public ActionResult EmptyPPM()
        {
            ViewBag.AboveReportSetting = db.ReportSetting2.Where(s => s.Above == 1).OrderBy(s => s.Sort);
            ViewBag.BottomReportSetting = db.ReportSetting2.Where(s => s.Above == 2).OrderBy(s => s.Sort);
            ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
            return View();
        }

        public ActionResult EmptyCenterJobOrderReport()
        {
            ViewBag.ReportSetting = db.CenterReportSetting.OrderBy(s => s.Sort);
            ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
            return View();
        }

        public ActionResult EmptyCenterServiceRequest()
        {
            ViewBag.ReportSetting = db.CenterReportSetting.OrderBy(s => s.Sort);
            ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
            return View();
        }

        public ActionResult EmptyCenterPPM()
        {
            ViewBag.AboveReportSetting = db.CenterReportSetting2.Where(s => s.Above == 1).OrderBy(s => s.Sort);
            ViewBag.BottomReportSetting = db.CenterReportSetting2.Where(s => s.Above == 2).OrderBy(s => s.Sort);
            ViewBag.ReportImage = db.WebsiteData.FirstOrDefault() == null ? "" : db.WebsiteData.FirstOrDefault().ImgPath;
            return View();
        }

        public ActionResult PrintHealthCenterReportTemplate()
        {
            return View();
        }
    }
}
