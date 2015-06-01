using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace WebDemos.Controllers
{
    public class FileUploadController : Controller
    {
        //
        // GET: /FileUpload/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Basic()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload()
        {
            var files = Request.Files;
            
            if (files == null || files.Count.Equals(0))
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            string savePath = Server.MapPath("~/Uploads");

            if (CreateDirectory(savePath))
            {
                files[0].SaveAs(Path.Combine(savePath, Guid.NewGuid().ToString() + Path.GetExtension(files[0].FileName)));
            }

            // Any way to return thumbnail from here?

            //return new HttpStatusCodeResult(200);
            return Json(
                new 
                { 
                    id = "12345"
                });
        }

        [HttpGet]
        public ActionResult GetFile(Guid id)
        {
            string imagePath = Path.Combine(Server.MapPath("~/Uploads"), id.ToString() + ".png");

            //return File(imagePath, "image/png", "Image.png");

            // Resize the image to a thumbnail
            var x = new WebImage(imagePath);
            var thumbnailHeight = 200;

            // Keep original ratio
            double ratio = (double)x.Width / x.Height;
            //double desiredRatio = (double)width / height;
            int thumbnailWidth = Convert.ToInt32(thumbnailHeight * ratio);

            var thumbnail = new WebImage(imagePath)
                .Resize(thumbnailWidth, thumbnailHeight, false, true)
                .Crop(1, 1); // Cropping it to remove 1px border at top and left sides (bug in WebImage)
            //.Write();

            //var thumbnail = new WebImage(imagePath)
            //    .Resize(200, 1000, true, true)
            //    .Crop(1, 1); // Cropping it to remove 1px border at top and left sides (bug in WebImage)
            ////.Write();

            return File(thumbnail.GetBytes(), "image/png", "Image.png");
        }

        private bool CreateDirectory(string path)
        {
            bool result = true;

            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
