using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Calorie.Models
{
    public class Offset
    {

        [Key]
        public int ID { get; set; }

        //[InverseProperty("Saints")]
        [ForeignKey("OffsetterID")]
        public virtual ApplicationUser Offsetter { get; set; }
        public string OffsetterID { get; set; }

        [InverseProperty("Offsets")]
        [ForeignKey("ImageID")]
        public virtual CalorieImage Image { get; set; }
        public int? ImageID { get; set; }

         [DataType(DataType.MultilineText)]
         [Display(Name = "Description of activity")]
        public string Description { get; set; }

        //  [Display(Name = "Number of calories to offset")]
        //public int OffsetCalories { get; set; }

          [ForeignKey("PledgeID")]
          public virtual Pledges.Pledge Pledge { get; set; }
          public Guid PledgeID { get; set; }

          public decimal OffsetAmount { get; set; }

        public string ThirdPartyIdentifier { get; set; }

        [Column(TypeName = "text")]
        public string JSONBlob { get; set; }

        public string BlobType { get; set; }

        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedUTC { get; set; }

    }
}