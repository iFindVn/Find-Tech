namespace FindTech.Entities.AuthenticationMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FindTechRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.FindTechUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.FindTechRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.FindTechUsers", t => t.IdentityUser_Id)
                .Index(t => t.RoleId)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.FindTechUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        DisplayName = c.String(),
                        DayOfBirth = c.DateTime(),
                        Gender = c.Int(),
                        Level = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FindTechUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FindTechUsers", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.FindTechUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.FindTechUsers", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FindTechUserRoles", "IdentityUser_Id", "dbo.FindTechUsers");
            DropForeignKey("dbo.FindTechUserLogins", "IdentityUser_Id", "dbo.FindTechUsers");
            DropForeignKey("dbo.FindTechUserClaims", "IdentityUser_Id", "dbo.FindTechUsers");
            DropForeignKey("dbo.FindTechUserRoles", "RoleId", "dbo.FindTechRoles");
            DropIndex("dbo.FindTechUserLogins", new[] { "IdentityUser_Id" });
            DropIndex("dbo.FindTechUserClaims", new[] { "IdentityUser_Id" });
            DropIndex("dbo.FindTechUserRoles", new[] { "IdentityUser_Id" });
            DropIndex("dbo.FindTechUserRoles", new[] { "RoleId" });
            DropIndex("dbo.FindTechRoles", "RoleNameIndex");
            DropTable("dbo.FindTechUserLogins");
            DropTable("dbo.FindTechUserClaims");
            DropTable("dbo.FindTechUsers");
            DropTable("dbo.FindTechUserRoles");
            DropTable("dbo.FindTechRoles");
        }
    }
}
