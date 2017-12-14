namespace Homework05.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
               "dbo.Devices",
               c => new
               {
                   Id = c.Int(nullable: false, identity: true),
                   UserId = c.String(maxLength: 128),
                   DeviceId = c.String(),
               })
               .PrimaryKey(t => t.Id)
               .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true);

            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionText = c.String(),
                        QuestionType = c.Int(nullable: false),
                        Options = c.String(),
                        Minimum = c.Double(nullable: false),
                        Maximum = c.Double(nullable: false),
                        StepSize = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.StudyGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudyGroupName = c.String(),
                        StudyGroupCreatedTime = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SurveyResponses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SurveyId = c.Int(nullable: false),
                        QuestionId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        ResponseText = c.String(),
                        ResponseReceivedTime = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.X_Survey_Group", t => t.SurveyId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.SurveyId)
                .Index(t => t.QuestionId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Surveys",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SurveyName = c.String(),
                        SurveyType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
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
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
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
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
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
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.X_Coordinator_Group",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CoordinatorId = c.String(maxLength: 128),
                        StudyGroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CoordinatorId)
                .ForeignKey("dbo.StudyGroups", t => t.StudyGroupId, cascadeDelete: true)
                .Index(t => t.CoordinatorId)
                .Index(t => t.StudyGroupId);
            
            CreateTable(
                "dbo.X_Survey_Group",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SurveyId = c.Int(nullable: false),
                        StudyGroupId = c.Int(nullable: false),
                        SurveyCreatedTime = c.String(),
                        FrequencyOfNotifications = c.Int(nullable: false),
                        Time1 = c.String(),
                        Time2 = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StudyGroups", t => t.StudyGroupId, cascadeDelete: true)
                .ForeignKey("dbo.Surveys", t => t.SurveyId, cascadeDelete: true)
                .Index(t => t.SurveyId)
                .Index(t => t.StudyGroupId);
            
            CreateTable(
                "dbo.X_Survey_Question",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SurveyId = c.Int(nullable: false),
                        QuestionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.Surveys", t => t.SurveyId, cascadeDelete: true)
                .Index(t => t.SurveyId)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.X_User_Group",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        StudyGroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StudyGroups", t => t.StudyGroupId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.StudyGroupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.X_User_Group", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.X_User_Group", "StudyGroupId", "dbo.StudyGroups");
            DropForeignKey("dbo.X_Survey_Question", "SurveyId", "dbo.Surveys");
            DropForeignKey("dbo.X_Survey_Question", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.X_Survey_Group", "SurveyId", "dbo.Surveys");
            DropForeignKey("dbo.X_Survey_Group", "StudyGroupId", "dbo.StudyGroups");
            DropForeignKey("dbo.X_Coordinator_Group", "StudyGroupId", "dbo.StudyGroups");
            DropForeignKey("dbo.X_Coordinator_Group", "CoordinatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SurveyResponses", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SurveyResponses", "SurveyId", "dbo.Surveys");
            DropForeignKey("dbo.SurveyResponses", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.X_User_Group", new[] { "StudyGroupId" });
            DropIndex("dbo.X_User_Group", new[] { "UserId" });
            DropIndex("dbo.X_Survey_Question", new[] { "QuestionId" });
            DropIndex("dbo.X_Survey_Question", new[] { "SurveyId" });
            DropIndex("dbo.X_Survey_Group", new[] { "StudyGroupId" });
            DropIndex("dbo.X_Survey_Group", new[] { "SurveyId" });
            DropIndex("dbo.X_Coordinator_Group", new[] { "StudyGroupId" });
            DropIndex("dbo.X_Coordinator_Group", new[] { "CoordinatorId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.SurveyResponses", new[] { "UserId" });
            DropIndex("dbo.SurveyResponses", new[] { "QuestionId" });
            DropIndex("dbo.SurveyResponses", new[] { "SurveyId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.X_User_Group");
            DropTable("dbo.X_Survey_Question");
            DropTable("dbo.X_Survey_Group");
            DropTable("dbo.X_Coordinator_Group");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Surveys");
            DropTable("dbo.SurveyResponses");
            DropTable("dbo.StudyGroups");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Questions");
            DropTable("dbo.Devices");
        }
    }
}
