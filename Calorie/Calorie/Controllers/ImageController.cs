using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using Calorie.BusinessLogic;
using Calorie.Models;
using Microsoft.AspNet.Identity;

namespace Calorie.Controllers
{
    public class ImageController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUser CurrentUser()
        {
            var id = User.Identity.GetUserId();
            return db.Users.FirstOrDefault(u => u.Id == id);
        }


        [Authorize]
        public ActionResult DeleteImage(string _ImageID)
        {
            var ImageID = GenericLogic.GetInt(_ImageID);
            var ThisUserID = "";
            if (CurrentUser() != null)
            {
                ThisUserID = CurrentUser().Id;
            }

            var Img = db.Images.FirstOrDefault(i => i.CalorieImageID == ImageID);
            if (Img != null && Img.ApplicationUser_Id==ThisUserID)
            {
                //we can delete it.
                db.Images.Remove(Img);
                db.SaveChanges();

                Response.StatusCode = (int)HttpStatusCode.OK;
                return Content("OK", MediaTypeNames.Text.Plain);
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Content("Couldn't find image or not permitted", MediaTypeNames.Text.Plain);
        }


     
        public ActionResult GetImage(string ImageID, bool Thumb=false)
        {
                      
            int ImageIDInt = 0;
            int.TryParse(ImageID, out ImageIDInt);

            if (ImageIDInt > 0)
            {
                ApplicationDbContext db = new ApplicationDbContext();
                var Img = db.Images.FirstOrDefault(I => I.CalorieImageID == ImageIDInt);
                if (Img != null)
                {
                    if (Thumb && Img.ThumbData!=null)
                    {
                        return File(Img.ThumbData.ToArray(), MediaTypeNames.Image.Jpeg );
                    }
                    return File(Img.ImageData.ToArray(), MediaTypeNames.Image.Jpeg);
                }
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Content("Nope", MediaTypeNames.Text.Plain);
            

        }
                
        
        [HttpPost]
        public ActionResult SaveImage()
        {

            if (Request.Files.Count > 0)
            {
                try
                {

                    ApplicationDbContext db = new ApplicationDbContext();


                    int CurrentImageID ;
                    int.TryParse(Request.Form["CurrentImageID"], out CurrentImageID );             

                    HttpPostedFileBase file = Request.Files[0];
                    MemoryStream inputStream= new MemoryStream();
                    file?.InputStream.CopyTo(inputStream);
                    
                    var NewImg = ImageLogic.ProcessImage(inputStream);
                    NewImg.Type = CalorieImage.ImageType.UserImage;
                    NewImg.ApplicationUser_Id = CurrentUser()?.Id;
                                        
                    db.Images.Add(NewImg);
                    db.SaveChanges();

                    return Content(NewImg.CalorieImageID.ToString(), "text/xml");

                }
                catch(Exception ex) {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Content(ex.ToString(), MediaTypeNames.Text.Plain );
                }

            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Content("No File Found", MediaTypeNames.Text.Plain);
                        
        }


       [HttpPost]
        public ActionResult SaveImageFromURL(string url)
        {

            try
            {
                var ID = ImageLogic.GetAndSaveImageFromURL(url, CalorieImage.ImageType.UserImage);
                return Content(ID.ToString(), "text/xml");
            }
            catch
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("", MediaTypeNames.Text.Plain);
            }            

        }


        [HttpPost]
        public ActionResult SaveImageFromURLWithDelete(string url,string idToDelete)
        {

            try
            {
                var idToDeleteInt = GenericLogic.GetInt(idToDelete);
                if (idToDeleteInt > 0)
                {
                    try
                    {
                        var CurrentImage = db.Images.FirstOrDefault(I => I.CalorieImageID == idToDeleteInt);
                        if (CurrentImage != null)
                        {
                            db.Images.Remove(CurrentImage);
                            db.SaveChanges();                           
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }


                var ID = ImageLogic.GetAndSaveImageFromURL(url, CalorieImage.ImageType.UserImage);
                return Content(ID.ToString(), "text/xml");
            }
            catch
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("", MediaTypeNames.Text.Plain);
            }

        }

    }
}
