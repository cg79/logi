
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Logistica.Helpers;

namespace Logistica.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ContentResult UploadFiles(string directory = null)
        {
            var r = new List<UploadFilesResult>();

            try
            {
                foreach (string file in Request.Files)
                {
                    HttpPostedFileWrapper hpf = Request.Files[file] as HttpPostedFileWrapper;
                    if (hpf.ContentLength == 0)
                        continue;
                    Image thumb = hpf.GetThumbnail(150);
                    Guid imgGuid = Guid.Parse(file);

                    string imgUrl = string.Concat(imgGuid.ToString(), ".png");

                    //string savedFileName = Path.Combine(Server.MapPath("~/App_Data"), Path.GetFileName(hpf.FileName));
                    //hpf.SaveAs(savedFileName);
                    directory = (directory == null) ? "Avatar/" : string.Concat(directory, "/");

                    string filePath = string.Concat("~/Images/", directory, imgUrl);
                    var fn = Server.MapPath(filePath);
                    thumb.Save(fn, System.Drawing.Imaging.ImageFormat.Png);

                    r.Add(new UploadFilesResult()
                    {
                        Name = imgUrl,
                        Length = hpf.ContentLength,
                        Type = hpf.ContentType
                    });
                }
                //return Content("{\"name\":\"" + r[0].Name + "\",\"type\":\"" + r[0].Type + "\",\"size\":\"" + string.Format("{0} bytes", r[0].Length) + "\"}", "application/json");
                return Content(r.ToJSON(), "application/json");
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
