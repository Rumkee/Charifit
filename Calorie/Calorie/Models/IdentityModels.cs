using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;
using System.Web.Mvc;
using Calorie.Models.Charities;

namespace Calorie.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public enum ApplicationUserStatus
        {
            Newb,
            Something,
            Star,
            etc
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;

                    }

        [ForeignKey("ProfilePictureID")]
        public virtual CalorieImage ProfilePicture { get; set; }
        public int? ProfilePictureID { get; set; }

        public BusinessLogic.CurrencyLogic.CurrencyEnum Currency { get; set; }

        //public virtual List<Models.Pledge> Sinners { get; set; }
                
        public virtual List<Models.Offset> Offsetters { get; set; }

        [Display(Name = "Emails From Third Parties")]
        public bool RecieveThirdPartyEmails { get; set; }

        [Display(Name = "Emails From Us")]
        public bool RecieveFirstPartyEmails { get; set; }

        [ForeignKey("TeamID")]
        public virtual Teams.Team Team{ get; set; }
        public int? TeamID { get; set; }

        public bool IsTeamAdmin{ get; set; }

        public bool IsSuperAdmin { get; set; }

        public bool IsExercisor { get; set; }

        public bool IsSponsor { get; set; }

        public string JustGivingUsername { get; set; }
        
        public virtual List<Models.BadgeAward> BadgeAwards { get; set; }

        public ApplicationUserStatus Status { get; set; }
    
        public virtual List<Calorie.Models.Pledges.PledgeContributors> Contributions { get; set; }

        public virtual List<Models.CalorieImage> OwnedImages { get; set; }

        public virtual List<Models.Teams.TeamJoinRequests> TeamJoinRequests { get; set; }

        public virtual List<Trackers.Tracker> Trackers { get; set; }

        public virtual List<Models.Message> Messages{ get; set; }

        public bool IsCompany { get; set; }

        [AllowHtml]
        public string CompanyDescription { get; set; }
        public string CompanyURL { get; set; }

        public virtual List<Pledges.PreferredActivity> PreferredActivities { get; set; }

        public virtual List<PreferredCharity> PreferredCharities { get; set; }

        public virtual List<Models.UserAlert> Alerts { get; set; }

        public virtual List<Pledges.PledgeBookmark> Bookmarks { get; set; }


    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("local", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Calorie.Models.CalorieImage> Images { get; set; }

        public System.Data.Entity.DbSet<Calorie.Models.Pledges.Pledge > Pledges { get; set; }
                
        public IQueryable<Pledges.Pledge> OpenPledges { get {
                return Pledges.Where(p => !p.Closed && p.ExpiryDate>DateTime.UtcNow);
            } }

        public System.Data.Entity.DbSet<Calorie.Models.Offset> Offsets { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public System.Data.Entity.DbSet<Calorie.Models.Social.Like> Likes { get; set; }

        public System.Data.Entity.DbSet<Calorie.Models.Teams.Team> Teams { get; set; }
        public System.Data.Entity.DbSet<Calorie.Models.Teams.TeamJoinRequests> TeamJoinRequests { get; set; }


        public System.Data.Entity.DbSet<Calorie.Models.Pledges.PledgeContributors> PledgeContributors { get; set; }

        public System.Data.Entity.DbSet<Calorie.Models.Trackers.Tracker> Trackers { get; set; }

        public System.Data.Entity.DbSet<Calorie.Models.Message > Messages{ get; set; }

        public System.Data.Entity.DbSet<Calorie.Models.Pledges.PledgeActivity> PledgeActivities { get; set; }

        public System.Data.Entity.DbSet<Calorie.Models.UserAlert> Alerts { get; set; }

        public System.Data.Entity.DbSet<Calorie.Models.Charities.Charity> Charities { get; set; }




        //it keeps adding application users here...remove as needed
        // public System.Data.Entity.DbSet<Calorie.Models.ApplicationUser> ApplicationUsers { get; set; }              


    }

    
}