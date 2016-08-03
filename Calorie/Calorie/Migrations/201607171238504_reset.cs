namespace Calorie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reset : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserAlerts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(maxLength: 128),
                        Type = c.Int(nullable: false),
                        Data = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ProfilePictureID = c.Int(),
                        Currency = c.Int(nullable: false),
                        RecieveThirdPartyEmails = c.Boolean(nullable: false),
                        RecieveFirstPartyEmails = c.Boolean(nullable: false),
                        TeamID = c.Int(),
                        IsTeamAdmin = c.Boolean(nullable: false),
                        IsSuperAdmin = c.Boolean(nullable: false),
                        IsExercisor = c.Boolean(nullable: false),
                        IsSponsor = c.Boolean(nullable: false),
                        JustGivingUsername = c.String(),
                        Status = c.Int(nullable: false),
                        IsCompany = c.Boolean(nullable: false),
                        CompanyDescription = c.String(),
                        CompanyURL = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.TeamID)
                .ForeignKey("dbo.CalorieImages", t => t.ProfilePictureID)
                .Index(t => t.ProfilePictureID)
                .Index(t => t.TeamID)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.BadgeAwards",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BadgeID = c.Int(nullable: false),
                        UserID = c.String(maxLength: 128),
                        Awarded = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.PledgeBookmarks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PledgeID = c.Guid(nullable: false),
                        UserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Pledges", t => t.PledgeID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.PledgeID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Pledges",
                c => new
                    {
                        PledgeID = c.Guid(nullable: false, identity: true, defaultValueSql: "NewID()"),
                        Closed = c.Boolean(nullable: false),
                        Activity_Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Activity_Units = c.Int(nullable: false),
                        Story = c.String(),
                        OpenToOtherContributors = c.Boolean(nullable: false),
                        JustGivingCharityID = c.String(),
                        CharityImageURL = c.String(),
                        CharityName = c.String(),
                        ExpiryDate = c.DateTime(nullable: false),
                        CreatedUTC = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                        CharityID = c.Int(),
                    })
                .PrimaryKey(t => t.PledgeID)
                .ForeignKey("dbo.Charities", t => t.CharityID)
                .Index(t => t.CharityID);
            
            CreateTable(
                "dbo.PledgeActivities",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Pledge_PledgeID = c.Guid(nullable: false),
                        Activity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Pledges", t => t.Pledge_PledgeID)
                .Index(t => t.Pledge_PledgeID);
            
            CreateTable(
                "dbo.Charities",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        JustGivingCharityID = c.String(),
                        JustGivingCharityBlob = c.String(),
                        JustGivingCharityImageURL = c.String(),
                        Description = c.String(),
                        CharityURL = c.String(),
                        JustGivingRegistrationNumber = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PledgeContributors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Currency = c.Int(nullable: false),
                        IsOriginator = c.Boolean(nullable: false),
                        SinnerID = c.String(nullable: false, maxLength: 128),
                        PledgeID = c.Guid(nullable: false),
                        Comment = c.String(),
                        AmountAnonymous = c.Boolean(nullable: false),
                        UserAnonymous = c.Boolean(nullable: false),
                        ThirdPartyRef = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Pledges", t => t.PledgeID)
                .ForeignKey("dbo.AspNetUsers", t => t.SinnerID)
                .Index(t => t.SinnerID)
                .Index(t => t.PledgeID);
            
            CreateTable(
                "dbo.CalorieImages",
                c => new
                    {
                        CalorieImageID = c.Int(nullable: false, identity: true),
                        ImageData = c.Binary(),
                        ThumbData = c.Binary(),
                        Type = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CalorieImageID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Offsets",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OffsetterID = c.String(maxLength: 128),
                        ImageID = c.Int(),
                        Description = c.String(),
                        PledgeID = c.Guid(nullable: false),
                        OffsetAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ThirdPartyIdentifier = c.String(),
                        JSONBlob = c.String(unicode: false, storeType: "text"),
                        BlobType = c.String(),
                        CreatedUTC = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CalorieImages", t => t.ImageID)
                .ForeignKey("dbo.AspNetUsers", t => t.OffsetterID)
                .ForeignKey("dbo.Pledges", t => t.PledgeID)
                .Index(t => t.OffsetterID)
                .Index(t => t.ImageID)
                .Index(t => t.PledgeID);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        ImageID = c.Int(nullable: false),
                        Availability = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CalorieImages", t => t.ImageID)
                .Index(t => t.ImageID);
            
            CreateTable(
                "dbo.TeamJoinRequests",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(maxLength: 128),
                        TeamID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Teams", t => t.TeamID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.TeamID);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        Level = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        MessageBody = c.String(),
                        UserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.PreferredActivities",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(maxLength: 128),
                        Activity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.PreferredCharities",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(maxLength: 128),
                        CharityID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Charities", t => t.CharityID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.CharityID);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Trackers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        AuthToken = c.String(),
                        AccessToken = c.String(),
                        RefreshToken = c.String(),
                        AccessTokenExpiry = c.DateTime(),
                        ThirdPartyUserID = c.String(),
                        UserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LinkType = c.String(),
                        LinkID = c.String(),
                        UserID = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.CalorieImagePledges",
                c => new
                    {
                        CalorieImage_CalorieImageID = c.Int(nullable: false),
                        Pledge_PledgeID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.CalorieImage_CalorieImageID, t.Pledge_PledgeID })
                .ForeignKey("dbo.CalorieImages", t => t.CalorieImage_CalorieImageID, cascadeDelete: true)
                .ForeignKey("dbo.Pledges", t => t.Pledge_PledgeID, cascadeDelete: true)
                .Index(t => t.CalorieImage_CalorieImageID)
                .Index(t => t.Pledge_PledgeID);
            
            CreateTable(
                "dbo.TeamPledges",
                c => new
                    {
                        Team_ID = c.Int(nullable: false),
                        Pledge_PledgeID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Team_ID, t.Pledge_PledgeID })
                .ForeignKey("dbo.Teams", t => t.Team_ID, cascadeDelete: true)
                .ForeignKey("dbo.Pledges", t => t.Pledge_PledgeID, cascadeDelete: true)
                .Index(t => t.Team_ID)
                .Index(t => t.Pledge_PledgeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Trackers", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "ProfilePictureID", "dbo.CalorieImages");
            DropForeignKey("dbo.PreferredCharities", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.PreferredCharities", "CharityID", "dbo.Charities");
            DropForeignKey("dbo.PreferredActivities", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.PledgeBookmarks", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.TeamPledges", "Pledge_PledgeID", "dbo.Pledges");
            DropForeignKey("dbo.TeamPledges", "Team_ID", "dbo.Teams");
            DropForeignKey("dbo.AspNetUsers", "TeamID", "dbo.Teams");
            DropForeignKey("dbo.TeamJoinRequests", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.TeamJoinRequests", "TeamID", "dbo.Teams");
            DropForeignKey("dbo.Teams", "ImageID", "dbo.CalorieImages");
            DropForeignKey("dbo.CalorieImages", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CalorieImagePledges", "Pledge_PledgeID", "dbo.Pledges");
            DropForeignKey("dbo.CalorieImagePledges", "CalorieImage_CalorieImageID", "dbo.CalorieImages");
            DropForeignKey("dbo.Offsets", "PledgeID", "dbo.Pledges");
            DropForeignKey("dbo.Offsets", "OffsetterID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Offsets", "ImageID", "dbo.CalorieImages");
            DropForeignKey("dbo.PledgeContributors", "SinnerID", "dbo.AspNetUsers");
            DropForeignKey("dbo.PledgeContributors", "PledgeID", "dbo.Pledges");
            DropForeignKey("dbo.Pledges", "CharityID", "dbo.Charities");
            DropForeignKey("dbo.PledgeBookmarks", "PledgeID", "dbo.Pledges");
            DropForeignKey("dbo.PledgeActivities", "Pledge_PledgeID", "dbo.Pledges");
            DropForeignKey("dbo.BadgeAwards", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserAlerts", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.TeamPledges", new[] { "Pledge_PledgeID" });
            DropIndex("dbo.TeamPledges", new[] { "Team_ID" });
            DropIndex("dbo.CalorieImagePledges", new[] { "Pledge_PledgeID" });
            DropIndex("dbo.CalorieImagePledges", new[] { "CalorieImage_CalorieImageID" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Trackers", new[] { "UserID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.PreferredCharities", new[] { "CharityID" });
            DropIndex("dbo.PreferredCharities", new[] { "UserID" });
            DropIndex("dbo.PreferredActivities", new[] { "UserID" });
            DropIndex("dbo.Messages", new[] { "UserID" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.TeamJoinRequests", new[] { "TeamID" });
            DropIndex("dbo.TeamJoinRequests", new[] { "UserID" });
            DropIndex("dbo.Teams", new[] { "ImageID" });
            DropIndex("dbo.Offsets", new[] { "PledgeID" });
            DropIndex("dbo.Offsets", new[] { "ImageID" });
            DropIndex("dbo.Offsets", new[] { "OffsetterID" });
            DropIndex("dbo.CalorieImages", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.PledgeContributors", new[] { "PledgeID" });
            DropIndex("dbo.PledgeContributors", new[] { "SinnerID" });
            DropIndex("dbo.PledgeActivities", new[] { "Pledge_PledgeID" });
            DropIndex("dbo.Pledges", new[] { "CharityID" });
            DropIndex("dbo.PledgeBookmarks", new[] { "UserID" });
            DropIndex("dbo.PledgeBookmarks", new[] { "PledgeID" });
            DropIndex("dbo.BadgeAwards", new[] { "UserID" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "TeamID" });
            DropIndex("dbo.AspNetUsers", new[] { "ProfilePictureID" });
            DropIndex("dbo.UserAlerts", new[] { "UserID" });
            DropTable("dbo.TeamPledges");
            DropTable("dbo.CalorieImagePledges");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Likes");
            DropTable("dbo.Trackers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.PreferredCharities");
            DropTable("dbo.PreferredActivities");
            DropTable("dbo.Messages");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.TeamJoinRequests");
            DropTable("dbo.Teams");
            DropTable("dbo.Offsets");
            DropTable("dbo.CalorieImages");
            DropTable("dbo.PledgeContributors");
            DropTable("dbo.Charities");
            DropTable("dbo.PledgeActivities");
            DropTable("dbo.Pledges");
            DropTable("dbo.PledgeBookmarks");
            DropTable("dbo.BadgeAwards");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.UserAlerts");
        }
    }
}
