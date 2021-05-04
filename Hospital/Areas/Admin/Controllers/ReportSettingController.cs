using Hospital.Controllers;
using Hospital.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Hospital.Areas.Admin.Controllers
{
    public class ReportSettingController : BaseController
    {
        private DBContext db = new DBContext();
        // GET: Admin/Categories
        [Authorize(Roles = "HospitalReportSetting")]
        public ActionResult Index()
        {
            return View(db.ReportSetting.OrderBy(s => s.Sort).ToList());
        }

        [Authorize(Roles = "WebsiteData")]
        public ActionResult Setting()
        {
            var x = db.WebsiteData.FirstOrDefault();
            if (x == null)
            {
                WebsiteData WebsiteData = new WebsiteData();
                db.WebsiteData.Add(WebsiteData);
                db.SaveChanges();
                x = db.WebsiteData.FirstOrDefault();
            }
            return View(x);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveWebsiteData(WebsiteData WebsiteData, HttpPostedFileBase Img)
        {
            if (ModelState.IsValid)
            {
                if (Img != null)
                {
                    //WebImage img = new WebImage(Img.InputStream);
                    //if (img.Width > 1000)
                    //    img.Resize(1000, 1000);
                    //img.Resize(1000, 1, true, true);

                    //if (Img != null)
                    //{
                    //    var directoryPath = Server.MapPath("~/FolderName");
                    //    if (!Directory.Exists(directoryPath))
                    //    {
                    //        Directory.CreateDirectory(directoryPath);
                    //    }

                    //    var fileGuid = Guid.NewGuid();
                    //    var filename = string.Concat(fileGuid, Path.GetExtension(Img.FileName));
                    //    var savePath = Path.Combine(directoryPath, filename);
                    //    Img.SaveAs(savePath);
                    //}

                    Random R = new Random();
                    int rand = R.Next(1, 100000);
                    var x = rand + DateTime.Now.AddHours(10).ToString() + Img.FileName;
                    x = ToSafeFileName(x);
                    Img.SaveAs(Server.MapPath("~/Uploads/ReportLogo/" + x));
                    WebsiteData.ImgPath = x;
                }
                if (WebsiteData.Id == 0)
                    db.WebsiteData.Add(WebsiteData);
                else
                    db.Entry(WebsiteData).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Setting");
            }
            ViewBag.Message = LanguageID == 1 ? "أكمل البيانات من فضلك" : "Please complete data";
            return View("Setting", WebsiteData);
        }

        // GET: Admin/Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReportSetting ReportSetting, string Save, string SaveAndContinue)
        {
            if (ModelState.IsValid)
            {
                db.ReportSetting.Add(ReportSetting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Message = LanguageID == 1 ? "أكمل البيانات من فضلك" : "Please complete data";
            return View(ReportSetting);
        }

        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportSetting ReportSetting = db.ReportSetting.Find(id);
            if (ReportSetting == null)
            {
                return HttpNotFound();
            }

            return View(ReportSetting);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReportSetting ReportSetting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ReportSetting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Message = LanguageID == 1 ? "أكمل البيانات من فضلك" : "Please complete data";
            return View(ReportSetting);
        }


        // POST: Admin/Categories/Delete/5
        public ActionResult DeleteConfirmed(int id)
        {
            ReportSetting ReportSetting = db.ReportSetting.Find(id);
            db.ReportSetting.Remove(ReportSetting);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteSelected(string Selected)
        {
            var List = Selected.Split(',');
            foreach (var item in List)
            {
                if (item != "")
                {
                    var Id = Convert.ToInt32(item);
                    ReportSetting ReportSetting = db.ReportSetting.Find(Id);
                    db.ReportSetting.Remove(ReportSetting);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
    }
}