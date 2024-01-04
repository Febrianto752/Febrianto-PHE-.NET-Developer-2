namespace SupplyManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.accounts",
                c => new
                    {
                        guid = c.Guid(nullable: false),
                        email = c.String(nullable: false),
                        password = c.String(),
                        name = c.String(nullable: false),
                        no_telp = c.String(nullable: false),
                        role = c.String(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        modified_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.guid);
            
            CreateTable(
                "dbo.projects",
                c => new
                    {
                        guid = c.Guid(nullable: false),
                        name = c.String(nullable: false),
                        description = c.String(nullable: false, unicode: false, storeType: "text"),
                        start_date = c.DateTime(nullable: false),
                        end_date = c.DateTime(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        modified_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.guid);
            
            CreateTable(
                "dbo.project_tenders",
                c => new
                    {
                        guid = c.Guid(nullable: false),
                        vendor_guid = c.Guid(nullable: false),
                        project_guid = c.Guid(nullable: false),
                        status = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        modified_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.guid)
                .ForeignKey("dbo.projects", t => t.project_guid, cascadeDelete: true)
                .ForeignKey("dbo.vendors", t => t.vendor_guid, cascadeDelete: true)
                .Index(t => t.vendor_guid)
                .Index(t => t.project_guid);
            
            CreateTable(
                "dbo.vendors",
                c => new
                    {
                        guid = c.Guid(nullable: false),
                        business_field = c.String(),
                        type_company = c.String(),
                        profile_image = c.String(),
                        status = c.Int(nullable: false),
                        account_guid = c.Guid(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        modified_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.guid)
                .ForeignKey("dbo.accounts", t => t.account_guid, cascadeDelete: true)
                .Index(t => t.account_guid);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.project_tenders", "vendor_guid", "dbo.vendors");
            DropForeignKey("dbo.vendors", "account_guid", "dbo.accounts");
            DropForeignKey("dbo.project_tenders", "project_guid", "dbo.projects");
            DropIndex("dbo.vendors", new[] { "account_guid" });
            DropIndex("dbo.project_tenders", new[] { "project_guid" });
            DropIndex("dbo.project_tenders", new[] { "vendor_guid" });
            DropTable("dbo.vendors");
            DropTable("dbo.project_tenders");
            DropTable("dbo.projects");
            DropTable("dbo.accounts");
        }
    }
}
