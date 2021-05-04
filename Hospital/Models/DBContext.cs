using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Hospital.Models
{
    public class DBContext : IdentityDbContext<ApplicationUser, Role, int, UserLogin, UserRole, UserClaim>
    {
        public DBContext()
            : base("name=DBContext")
        {
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
        }
        public virtual DbSet<DDevice> DDevice { set; get; }
        public virtual DbSet<DCenterDevice> DCenterDevice { set; get; }

        public virtual DbSet<WebsiteData> WebsiteData { set; get; }
        public virtual DbSet<ReportSetting2> ReportSetting2 { set; get; }
        public virtual DbSet<ReportSetting> ReportSetting { set; get; }
        public virtual DbSet<Engineer> Engineer { set; get; }
        public virtual DbSet<UserRole> UserRole { set; get; }
        public virtual DbSet<Device> Device { set; get; }
        public virtual DbSet<DeviceRequest> DeviceRequest { set; get; }
        public virtual DbSet<DeviceUnit> DeviceUnit { set; get; }
        public virtual DbSet<Center> Center { set; get; }
        public virtual DbSet<CenterDevice> CenterDevice { set; get; }
        public virtual DbSet<CenterDeviceUnit> CenterDeviceUnit { set; get; }
        public virtual DbSet<DeviceType> DeviceType { set; get; }
        public virtual DbSet<Day> Day { set; get; }
        public virtual DbSet<Month> Month { set; get; }
        public virtual DbSet<DeviceSchedule> DeviceSchedule { set; get; }
        public virtual DbSet<CenterDeviceSchedule> CenterDeviceSchedule { set; get; }
        public virtual DbSet<CenterReportSetting> CenterReportSetting { set; get; }
        public virtual DbSet<CenterDeviceRequest> CenterDeviceRequest { set; get; }

        public virtual DbSet<CenterReportSetting2> CenterReportSetting2 { set; get; }
        
    }

}