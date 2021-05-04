using Hospital.Controllers;
using Hospital.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Hospital.Areas.Admin.Controllers
{
    [Authorize(Roles = "Dashboard")]
    public class DashboardController : BaseController
    {
        private DBContext db = new DBContext();
        // GET: Admin/Dashboard
        public ActionResult Index(int? SearchType, int? CenterId)
        {
            if (CenterId.HasValue)
                ViewBag.CenterId = CenterId;
            else
                ViewBag.CenterId = 0;

            if (SearchType.HasValue)
                ViewBag.SearchType = SearchType;
            else
                ViewBag.SearchType = 0;

            ViewBag.Centers = db.Center.AsNoTracking().Where(s => s.IsDeleted == false);

            if (CenterId.HasValue && CenterId != 0)
            {
                var Center = db.Center.Find(CenterId);
                var Requests = db.CenterDeviceRequest.AsNoTracking().Where(s => s.Device.CenterDeviceUnit.CenterId == CenterId && s.IsDeleted == false).Include(s => s.Device);
                var AllRequests = Requests.Where(s => s.RequestType == 1).Include(s => s.Device).ToList();
                var Archived = db.DCenterDevice.AsNoTracking().Where(s => s.CenterDeviceUnit.CenterId == CenterId).Include(s => s.CenterDeviceUnit).ToList();

                ViewBag.MovementRequestCount = Requests.Where(s => s.CreatedDate.Value.Year == DateTime.Now.Year && s.RequestType == 2).Count();
                ViewBag.AddPPMRequestCount = Requests.Where(s => s.CreatedDate.Value.Year == DateTime.Now.Year && s.RequestType == 3).Count();

                if (SearchType == 6)
                {
                    ViewBag.AllRequests = ViewBag.DamageRequestCount = AllRequests.Where(s => s.CreatedDate.Value.Year == DateTime.Now.Year).Count();
                    ViewBag.DoneRequests = AllRequests.Where(s => s.CreatedDate.Value.Year == DateTime.Now.Year && s.ConfirmType == 7).Count();
                    ViewBag.NotDoneRequests = AllRequests.Where(s => s.CreatedDate.Value.Year == DateTime.Now.Year && s.ConfirmType != 7).Count();
                    ViewBag.ArchivedCount = Archived.Where(s => s.CreatedDate.Value.Year == DateTime.Now.Year).Count();
                    ViewBag.ArchivedList = Archived.Where(s => s.CreatedDate.Value.Year == DateTime.Now.Year).GroupBy(s => s.CenterDeviceUnitId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });

                    var DeviceTypes = AllRequests.Where(s => s.CreatedDate.Value.Year == DateTime.Now.Year).GroupBy(s => s.Device.DeviceTypeId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceType.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceType.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });
                    ViewBag.DeviceTypes = DeviceTypes;

                    var DeviceCategory = AllRequests.Where(s => s.CreatedDate.Value.Year == DateTime.Now.Year).GroupBy(s => s.Device.DeviceCategoryId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = s.Key == 1 ? "A_فئة" : (s.Key == 2 ? "B_فئة" : "C_فئة"),
                            NameEn = s.Key == 1 ? "Category A" : (s.Key == 2 ? "Category B" : "Category C"),
                            Id = s.Count()
                        });
                    ViewBag.DeviceCategory = DeviceCategory;

                    var DeviceUnits = AllRequests.Where(s => s.CreatedDate.Value.Year == DateTime.Now.Year).GroupBy(s => s.Device.CenterDeviceUnitId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.CenterDeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.CenterDeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });
                    ViewBag.DeviceUnits = DeviceUnits;
                }
                else if (SearchType == 5)
                {
                    ViewBag.AllRequests = ViewBag.DamageRequestCount = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-6).Date.Ticks).Count();
                    ViewBag.DoneRequests = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-6).Date.Ticks && s.ConfirmType == 7).Count();
                    ViewBag.NotDoneRequests = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-6).Date.Ticks && s.ConfirmType != 7).Count();
                    ViewBag.ArchivedCount = Archived.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-6).Date.Ticks).Count();
                    ViewBag.ArchivedList = Archived.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-6).Date.Ticks).GroupBy(s => s.CenterDeviceUnitId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });

                    var DeviceTypes = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-6).Date.Ticks).GroupBy(s => s.Device.DeviceTypeId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceType.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceType.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });
                    ViewBag.DeviceTypes = DeviceTypes;

                    var DeviceCategory = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-6).Date.Ticks).GroupBy(s => s.Device.DeviceCategoryId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = s.Key == 1 ? "A_فئة" : (s.Key == 2 ? "B_فئة" : "C_فئة"),
                            NameEn = s.Key == 1 ? "Category A" : (s.Key == 2 ? "Category B" : "Category C"),
                            Id = s.Count()
                        });
                    ViewBag.DeviceCategory = DeviceCategory;

                    var DeviceUnits = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-6).Date.Ticks).GroupBy(s => s.Device.CenterDeviceUnitId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.CenterDeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.CenterDeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });
                    ViewBag.DeviceUnits = DeviceUnits;
                }
                else if (SearchType == 4)
                {
                    ViewBag.AllRequests = ViewBag.DamageRequestCount = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-3).Date.Ticks).Count();
                    ViewBag.DoneRequests = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-3).Date.Ticks && s.ConfirmType == 7).Count();
                    ViewBag.NotDoneRequests = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-3).Date.Ticks && s.ConfirmType != 7).Count();
                    ViewBag.ArchivedCount = Archived.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-3).Date.Ticks).Count();
                    ViewBag.ArchivedList = Archived.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-3).Date.Ticks).GroupBy(s => s.CenterDeviceUnitId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });

                    var DeviceTypes = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-3).Date.Ticks).GroupBy(s => s.Device.DeviceTypeId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceType.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceType.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });
                    ViewBag.DeviceTypes = DeviceTypes;

                    var DeviceCategory = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-3).Date.Ticks).GroupBy(s => s.Device.DeviceCategoryId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = s.Key == 1 ? "A_فئة" : (s.Key == 2 ? "B_فئة" : "C_فئة"),
                            NameEn = s.Key == 1 ? "Category A" : (s.Key == 2 ? "Category B" : "Category C"),
                            Id = s.Count()
                        });
                    ViewBag.DeviceCategory = DeviceCategory;

                    var DeviceUnits = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-3).Date.Ticks).GroupBy(s => s.Device.CenterDeviceUnitId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.CenterDeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.CenterDeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });
                    ViewBag.DeviceUnits = DeviceUnits;
                }
                else if (SearchType == 3)
                {
                    ViewBag.AllRequests = ViewBag.DamageRequestCount = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-1).Date.Ticks).Count();
                    ViewBag.DoneRequests = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-1).Date.Ticks && s.ConfirmType == 7).Count();
                    ViewBag.NotDoneRequests = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-1).Date.Ticks && s.ConfirmType != 7).Count();
                    ViewBag.ArchivedCount = Archived.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-1).Date.Ticks).Count();
                    ViewBag.ArchivedList = Archived.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-1).Date.Ticks).GroupBy(s => s.CenterDeviceUnitId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });

                    var DeviceTypes = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-1).Date.Ticks).GroupBy(s => s.Device.DeviceTypeId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceType.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceType.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });
                    ViewBag.DeviceTypes = DeviceTypes;

                    var DeviceCategory = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-1).Date.Ticks).GroupBy(s => s.Device.DeviceCategoryId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = s.Key == 1 ? "A_فئة" : (s.Key == 2 ? "B_فئة" : "C_فئة"),
                            NameEn = s.Key == 1 ? "Category A" : (s.Key == 2 ? "Category B" : "Category C"),
                            Id = s.Count()
                        });
                    ViewBag.DeviceCategory = DeviceCategory;

                    var DeviceUnits = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-1).Date.Ticks).GroupBy(s => s.Device.CenterDeviceUnitId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.CenterDeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.CenterDeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });
                    ViewBag.DeviceUnits = DeviceUnits;
                }
                else if (SearchType == 2)
                {
                    ViewBag.AllRequests = ViewBag.DamageRequestCount = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddDays(-7).Date.Ticks).Count();
                    ViewBag.DoneRequests = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddDays(-7).Date.Ticks && s.ConfirmType == 7).Count();
                    ViewBag.NotDoneRequests = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddDays(-7).Date.Ticks && s.ConfirmType != 7).Count();
                    ViewBag.ArchivedCount = Archived.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddDays(-7).Date.Ticks).Count();
                    ViewBag.ArchivedList = Archived.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddDays(-7).Date.Ticks).GroupBy(s => s.CenterDeviceUnitId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });

                    var DeviceTypes = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddDays(-7).Date.Ticks).GroupBy(s => s.Device.DeviceTypeId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceType.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceType.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });
                    ViewBag.DeviceTypes = DeviceTypes;

                    var DeviceCategory = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddDays(-7).Date.Ticks).GroupBy(s => s.Device.DeviceCategoryId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = s.Key == 1 ? "A_فئة" : (s.Key == 2 ? "B_فئة" : "C_فئة"),
                            NameEn = s.Key == 1 ? "Category A" : (s.Key == 2 ? "Category B" : "Category C"),
                            Id = s.Count()
                        });
                    ViewBag.DeviceCategory = DeviceCategory;

                    var DeviceUnits = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddDays(-7).Date.Ticks).GroupBy(s => s.Device.CenterDeviceUnitId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.CenterDeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.CenterDeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });
                    ViewBag.DeviceUnits = DeviceUnits;
                }
                else
                {
                    ViewBag.AllRequests = ViewBag.DamageRequestCount = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks == DateTime.Now.Date.Ticks).Count();
                    ViewBag.DoneRequests = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks == DateTime.Now.Date.Ticks && s.ConfirmType == 7).Count();
                    ViewBag.NotDoneRequests = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks == DateTime.Now.Date.Ticks && s.ConfirmType != 7).Count();
                    ViewBag.ArchivedCount = Archived.Where(s => s.CreatedDate.Value.Date.Ticks == DateTime.Now.Date.Ticks).Count();
                    ViewBag.ArchivedList = Archived.Where(s => s.CreatedDate.Value.Date.Ticks == DateTime.Now.Date.Ticks).GroupBy(s => s.CenterDeviceUnitId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });

                    var DeviceTypes = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks == DateTime.Now.Date.Ticks).GroupBy(s => s.Device.DeviceTypeId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceType.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceType.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });
                    ViewBag.DeviceTypes = DeviceTypes;

                    var DeviceCategory = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks == DateTime.Now.Date.Ticks).GroupBy(s => s.Device.DeviceCategoryId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = s.Key == 1 ? "A_فئة" : (s.Key == 2 ? "B_فئة" : "C_فئة"),
                            NameEn = s.Key == 1 ? "Category A" : (s.Key == 2 ? "Category B" : "Category C"),
                            Id = s.Count()
                        });
                    ViewBag.DeviceCategory = DeviceCategory;

                    var DeviceUnits = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks == DateTime.Now.Date.Ticks).GroupBy(s => s.Device.CenterDeviceUnitId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.CenterDeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.CenterDeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });
                    ViewBag.DeviceUnits = DeviceUnits;
                }
            }
            else
            {
                var Requests = db.DeviceRequest.AsNoTracking().Where(s => s.IsDeleted == false && s.DeviceId.HasValue).Include(s => s.Device);
                var AllRequests = Requests.Where(s => s.RequestType == 1).Include(s => s.Device).ToList();
                ViewBag.MovementRequestCount = Requests.Where(s => s.CreatedDate.Value.Year == DateTime.Now.Year && s.RequestType == 2).Count();
                ViewBag.AddPPMRequestCount = Requests.Where(s => s.CreatedDate.Value.Year == DateTime.Now.Year && s.RequestType == 3).Count();
                var Archived = db.DDevice.AsNoTracking().Include(s => s.DeviceUnit).ToList();

                if (SearchType == 6)
                {
                    ViewBag.AllRequests = ViewBag.DamageRequestCount = AllRequests.Where(s => s.CreatedDate.Value.Year == DateTime.Now.Year).Count();
                    ViewBag.DoneRequests = AllRequests.Where(s => s.CreatedDate.Value.Year == DateTime.Now.Year && s.ConfirmType == 7).Count();
                    ViewBag.NotDoneRequests = AllRequests.Where(s => s.CreatedDate.Value.Year == DateTime.Now.Year && s.ConfirmType != 7).Count();
                    ViewBag.ArchivedCount = Archived.Where(s => s.CreatedDate.Value.Year == DateTime.Now.Year).Count();
                    ViewBag.ArchivedList = Archived.Where(s => s.CreatedDate.Value.Year == DateTime.Now.Year).GroupBy(s => s.DeviceUnitId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });

                    var DeviceTypes = AllRequests.Where(s => s.CreatedDate.Value.Year == DateTime.Now.Year).GroupBy(s => s.Device.DeviceTypeId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceType.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceType.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });
                    ViewBag.DeviceTypes = DeviceTypes;

                    var DeviceCategory = AllRequests.Where(s => s.CreatedDate.Value.Year == DateTime.Now.Year).GroupBy(s => s.Device.DeviceCategoryId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = s.Key == 1 ? "A_فئة" : (s.Key == 2 ? "B_فئة" : "C_فئة"),
                            NameEn = s.Key == 1 ? "Category A" : (s.Key == 2 ? "Category B" : "Category C"),
                            Id = s.Count()
                        });
                    ViewBag.DeviceCategory = DeviceCategory;

                    var DeviceUnits = AllRequests.Where(s => s.CreatedDate.Value.Year == DateTime.Now.Year).GroupBy(s => s.Device.DeviceUnitId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });
                    ViewBag.DeviceUnits = DeviceUnits;

                }
                else if (SearchType == 5)
                {
                    ViewBag.AllRequests = ViewBag.DamageRequestCount = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-6).Date.Ticks).Count();
                    ViewBag.DoneRequests = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-6).Date.Ticks && s.ConfirmType == 7).Count();
                    ViewBag.NotDoneRequests = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-6).Date.Ticks && s.ConfirmType != 7).Count();
                    ViewBag.ArchivedCount = Archived.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-6).Date.Ticks).Count();
                    ViewBag.ArchivedList = Archived.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-6).Date.Ticks).GroupBy(s => s.DeviceUnitId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });

                    var DeviceTypes = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-6).Date.Ticks).GroupBy(s => s.Device.DeviceTypeId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceType.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceType.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });
                    ViewBag.DeviceTypes = DeviceTypes;

                    var DeviceCategory = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-6).Date.Ticks).GroupBy(s => s.Device.DeviceCategoryId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = s.Key == 1 ? "A_فئة" : (s.Key == 2 ? "B_فئة" : "C_فئة"),
                            NameEn = s.Key == 1 ? "Category A" : (s.Key == 2 ? "Category B" : "Category C"),
                            Id = s.Count()
                        });
                    ViewBag.DeviceCategory = DeviceCategory;

                    var DeviceUnits = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-6).Date.Ticks).GroupBy(s => s.Device.DeviceUnitId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });
                    ViewBag.DeviceUnits = DeviceUnits;
                }
                else if (SearchType == 4)
                {
                    ViewBag.AllRequests = ViewBag.DamageRequestCount = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-3).Date.Ticks).Count();
                    ViewBag.DoneRequests = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-3).Date.Ticks && s.ConfirmType == 7).Count();
                    ViewBag.NotDoneRequests = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-3).Date.Ticks && s.ConfirmType != 7).Count();
                    ViewBag.ArchivedCount = Archived.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-3).Date.Ticks).Count();
                    ViewBag.ArchivedList = Archived.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-3).Date.Ticks).GroupBy(s => s.DeviceUnitId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        }); ;

                    var DeviceTypes = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-3).Date.Ticks).GroupBy(s => s.Device.DeviceTypeId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceType.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceType.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });
                    ViewBag.DeviceTypes = DeviceTypes;

                    var DeviceCategory = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-3).Date.Ticks).GroupBy(s => s.Device.DeviceCategoryId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = s.Key == 1 ? "A_فئة" : (s.Key == 2 ? "B_فئة" : "C_فئة"),
                            NameEn = s.Key == 1 ? "Category A" : (s.Key == 2 ? "Category B" : "Category C"),
                            Id = s.Count()
                        });
                    ViewBag.DeviceCategory = DeviceCategory;

                    var DeviceUnits = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-3).Date.Ticks).GroupBy(s => s.Device.DeviceUnitId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });
                    ViewBag.DeviceUnits = DeviceUnits;
                }
                else if (SearchType == 3)
                {
                    ViewBag.AllRequests = ViewBag.DamageRequestCount = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-1).Date.Ticks).Count();
                    ViewBag.DoneRequests = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-1).Date.Ticks && s.ConfirmType == 7).Count();
                    ViewBag.NotDoneRequests = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-1).Date.Ticks && s.ConfirmType != 7).Count();
                    ViewBag.ArchivedCount = Archived.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-1).Date.Ticks).Count();
                    ViewBag.ArchivedList = Archived.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-1).Date.Ticks).GroupBy(s => s.DeviceUnitId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        }); ;

                    var DeviceTypes = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-1).Date.Ticks).GroupBy(s => s.Device.DeviceTypeId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceType.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceType.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });
                    ViewBag.DeviceTypes = DeviceTypes;

                    var DeviceCategory = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-1).Date.Ticks).GroupBy(s => s.Device.DeviceCategoryId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = s.Key == 1 ? "A_فئة" : (s.Key == 2 ? "B_فئة" : "C_فئة"),
                            NameEn = s.Key == 1 ? "Category A" : (s.Key == 2 ? "Category B" : "Category C"),
                            Id = s.Count()
                        });
                    ViewBag.DeviceCategory = DeviceCategory;

                    var DeviceUnits = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddMonths(-1).Date.Ticks).GroupBy(s => s.Device.DeviceUnitId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });
                    ViewBag.DeviceUnits = DeviceUnits;
                }
                else if (SearchType == 2)
                {
                    ViewBag.AllRequests = ViewBag.DamageRequestCount = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddDays(-7).Date.Ticks).Count();
                    ViewBag.DoneRequests = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddDays(-7).Date.Ticks && s.ConfirmType == 7).Count();
                    ViewBag.NotDoneRequests = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddDays(-7).Date.Ticks && s.ConfirmType != 7).Count();
                    ViewBag.ArchivedCount = Archived.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddDays(-7).Date.Ticks).Count();
                    ViewBag.ArchivedList = Archived.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddDays(-7).Date.Ticks).GroupBy(s => s.DeviceUnitId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        }); ;

                    var DeviceTypes = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddDays(-7).Date.Ticks).GroupBy(s => s.Device.DeviceTypeId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceType.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceType.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });
                    ViewBag.DeviceTypes = DeviceTypes;

                    var DeviceCategory = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddDays(-7).Date.Ticks).GroupBy(s => s.Device.DeviceCategoryId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = s.Key == 1 ? "A_فئة" : (s.Key == 2 ? "B_فئة" : "C_فئة"),
                            NameEn = s.Key == 1 ? "Category A" : (s.Key == 2 ? "Category B" : "Category C"),
                            Id = s.Count()
                        });
                    ViewBag.DeviceCategory = DeviceCategory;

                    var DeviceUnits = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.AddDays(-7).Date.Ticks).GroupBy(s => s.Device.DeviceUnitId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });
                    ViewBag.DeviceUnits = DeviceUnits;
                }
                else
                {
                    ViewBag.AllRequests = ViewBag.DamageRequestCount = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks == DateTime.Now.Date.Ticks).Count();
                    ViewBag.DoneRequests = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks == DateTime.Now.Date.Ticks && s.ConfirmType == 7).Count();
                    ViewBag.NotDoneRequests = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks == DateTime.Now.Date.Ticks && s.ConfirmType != 7).Count();
                    ViewBag.ArchivedCount = Archived.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.Date.Ticks).Count();
                    ViewBag.ArchivedList = Archived.Where(s => s.CreatedDate.Value.Date.Ticks >= DateTime.Now.Date.Ticks).GroupBy(s => s.DeviceUnitId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });

                    var DeviceTypes = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks == DateTime.Now.Date.Ticks).GroupBy(s => s.Device.DeviceTypeId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceType.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceType.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });
                    ViewBag.DeviceTypes = DeviceTypes;

                    var DeviceCategory = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks == DateTime.Now.Date.Ticks).GroupBy(s => s.Device.DeviceCategoryId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = s.Key == 1 ? "A_فئة" : (s.Key == 2 ? "B_فئة" : "C_فئة"),
                            NameEn = s.Key == 1 ? "Category A" : (s.Key == 2 ? "Category B" : "Category C"),
                            Id = s.Count()
                        });
                    ViewBag.DeviceCategory = DeviceCategory;

                    var DeviceUnits = AllRequests.Where(s => s.CreatedDate.Value.Date.Ticks == DateTime.Now.Date.Ticks).GroupBy(s => s.Device.DeviceUnitId)
                        .Select(s => new DeviceType()
                        {
                            NameAr = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameAr,
                            NameEn = db.DeviceUnit.AsNoTracking().FirstOrDefault(c => c.Id == s.Key).NameEn,
                            Id = s.Count()
                        });
                    ViewBag.DeviceUnits = DeviceUnits;
                }
            }

            return View();
        }
    }
}