namespace Calorie.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Calorie.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            

        }

        protected override void Seed(Calorie.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            
            //if (context.Images.Where(I =>I.Type ==CalorieImage.ImageType.DefaultUserImage).Count()>0){
            //    var ExisitingDefaultUserImg = new CalorieImage () {Type =CalorieImage.ImageType.DefaultUserImage  };
            //    context.Images.Attach(ExisitingDefaultUserImg);
            //    context.Images.Remove(ExisitingDefaultUserImg );

            //    var DefaultUserImgString = "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";
            //    var DefaultUserImg = new CalorieImage() { ImageData = Encoding.ASCII.GetBytes(DefaultUserImgString), Type = CalorieImage.ImageType.DefaultUserImage };
            //    context.Images.Add(DefaultUserImg);

            //   // context.SaveChanges();
            //}
            
                    


        }
    }
}
