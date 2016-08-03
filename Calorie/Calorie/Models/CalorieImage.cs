using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Calorie.Models
{
    public class CalorieImage
    {

        public enum ImageType{
        DefaultUserImage=1,
        UserImage=2,
        FoodPhoto=3,
        ExercisePhoto=4,
        TeamImage=5,
        PledgePhoto=6,
        CompanyLogo=7
        }

        public CalorieImage(int? ImgID)
        {
            if (ImgID.HasValue && ImgID.Value > 0)
            {
                CalorieImageID = ImgID.Value;
            }
        }

        public CalorieImage()
        {
        }


        [Key]
        public int CalorieImageID { get; set; }

        [MaxLength]
        public byte[] ImageData { get; set; }

        [MaxLength]
        public byte[] ThumbData { get; set; }

        public ImageType Type { get; set; }
                
        //public virtual List<Models.Pledge> Sinners { get; set; }
                
        public virtual List<Models.Offset> Offsets { get; set; }

        public virtual List<Models.Pledges.Pledge> Pledges { get; set; }

             [ForeignKey("ApplicationUser_Id")]
        [InverseProperty("OwnedImages")]
        public virtual ApplicationUser User { get; set; }

        //[Column(TypeName = "NVARCHAR")]
       // [StringLength(4000)]
        public string ApplicationUser_Id { get; set; }

    }

    public class CalorieImageUpload
    {
        public String HiddenForElementID { get; set; }
        public CalorieImage.ImageType Type { get; set; }

        public int? CurrentImageID { get; set; }

        public string ThisElementID { get; set; }
    }

    public class CalorieImageGalleryVM
    {
        public List<CalorieImage> Images { get; set; }

        public bool Editable { get; set; }

    }


}