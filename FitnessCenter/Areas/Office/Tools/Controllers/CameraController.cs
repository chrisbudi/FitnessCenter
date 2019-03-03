using System;
using System.Web.Helpers;
using System.Web.Mvc;
using ViewModel.Camera;

namespace FitnessCenter.Areas.Tools.Controllers
{
    [RequireHttps]

    public class CameraController : Controller
    {
        private const int AvatarWidth = 200; // ToDo - Change the size of the stored avatar image
        private const int AvatarHeight = 200; // ToDo - Change the size of the stored avatar image

        public ActionResult Camera(string camera, string cameraSource)
        {
            return PartialView("Camera/CameraPage", new ViewModelCamera()
            {
                CameraObject = camera,
                CameraSource = cameraSource
            });
        }

        // GET: General/Core
        [HttpPost]
        public JsonResult CaptureImage(string id, string t, string l, string h, string w, string imageSource)
        {
            try
            {
                string source = "";
                if (imageSource != null)
                {
                    byte[] bytes = Convert.FromBase64String(imageSource.Split(',')[1]);

                    var img = new WebImage(bytes);

                    // Calculate dimesnions
                    int top = Convert.ToInt32(t.Replace("-", "").Replace("px", ""));
                    int left = Convert.ToInt32(l.Replace("-", "").Replace("px", ""));
                    int height = Convert.ToInt32(h.Replace("-", "").Replace("px", ""));
                    int width = Convert.ToInt32(w.Replace("-", "").Replace("px", ""));

                    // Get image and resize it, ... 

                    img.Resize(width, height);
                    // ... crop the part the user selected, ...


                    img.Crop(top, left, img.Height - top - AvatarHeight, img.Width - left - AvatarWidth);
                    // ... delete the temporary file,...
                    //System.IO.File.Delete(fn);
                    // ... and save the new one.

                    byte[] bt = img.GetBytes();
                    source = Convert.ToBase64String(img.GetBytes());
                    string sourceImage = $"data:image/jpeg;base64,{source}";
                }
                return Json(new { success = true, src = $"data:image/jpeg;base64,{source}" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = ex.ToString() });
            }
        }
    }
}