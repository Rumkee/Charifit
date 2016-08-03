using Calorie.Models;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using System.Drawing;
using System.IO;
using System.Net;

namespace Calorie.BusinessLogic
{
    public class ImageLogic
    {

        public static CalorieImage ProcessImage(MemoryStream Input)
        {

            var NewImg = new CalorieImage();
       
            ISupportedImageFormat format = new PngFormat { Quality = 99 };
           
          
            //image
            using (MemoryStream maxStream = new MemoryStream())
            {
                var RL = new ImageProcessor.Imaging.ResizeLayer(new Size(500, 500),
                    ImageProcessor.Imaging.ResizeMode.Pad) {Upscale = false};

                using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                {
                    imageFactory.Load(Input).Resize(RL)
                                            .Format(format)
                                            .BackgroundColor(Color.Transparent)
                                            .Save(maxStream);
                }

                NewImg.ImageData = maxStream.ToArray();
                
            }

            //thumbnail
            using (MemoryStream thumbStream = new MemoryStream())
            {
                var RL = new ImageProcessor.Imaging.ResizeLayer(new Size(100, 100),
                    ImageProcessor.Imaging.ResizeMode.Pad) {Upscale = false};

                using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                {
                    imageFactory.Load(Input).Resize(RL)
                                            .Format(format)
                                            .BackgroundColor(Color.Transparent)
                                            .Save(thumbStream);
                }

                NewImg.ThumbData= thumbStream.ToArray();

            }

            return NewImg;

        }

        public static int GetAndSaveImageFromURL(string URL, Calorie.Models.CalorieImage.ImageType _Type)
        {

            HttpWebRequest req = (System.Net.HttpWebRequest)HttpWebRequest.Create(URL);
            req.AllowWriteStreamBuffering = true;
            req.Timeout = 20000;

            WebResponse resp = req.GetResponse();

            MemoryStream memStream;
            using (Stream response = resp.GetResponseStream())
            {
                memStream = new MemoryStream();

                byte[] buffer = new byte[1024];
                int byteCount;
                do
                {
                    byteCount = response.Read(buffer, 0, buffer.Length);
                    memStream.Write(buffer, 0, byteCount);
                } while (byteCount > 0);
            }

            resp.Close();
                        
            var NewImg = ProcessImage(memStream);
            NewImg.Type = _Type;

            ApplicationDbContext db = new ApplicationDbContext();
            db.Images.Add(NewImg);
            db.SaveChanges();

            return NewImg.CalorieImageID;


        }
    }
}