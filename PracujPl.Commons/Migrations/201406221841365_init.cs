namespace PracujPl.Commons.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RegionJobOffers",
                c => new
                    {
                        RegionJobOfferId = c.Int(nullable: false, identity: true),
                        LoadDateTime = c.DateTime(nullable: false),
                        JobOffers = c.Long(nullable: false),
                        RegionId_RegionId = c.Int(),
                    })
                .PrimaryKey(t => t.RegionJobOfferId)
                .ForeignKey("dbo.Regions", t => t.RegionId_RegionId)
                .Index(t => t.RegionId_RegionId);
            
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        RegionId = c.Int(nullable: false, identity: true),
                        RegionName = c.String(maxLength: 128),
                        ParentRegion_RegionId = c.Int(),
                    })
                .PrimaryKey(t => t.RegionId)
                .ForeignKey("dbo.Regions", t => t.ParentRegion_RegionId)
                .Index(t => t.ParentRegion_RegionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RegionJobOffers", "RegionId_RegionId", "dbo.Regions");
            DropForeignKey("dbo.Regions", "ParentRegion_RegionId", "dbo.Regions");
            DropIndex("dbo.Regions", new[] { "ParentRegion_RegionId" });
            DropIndex("dbo.RegionJobOffers", new[] { "RegionId_RegionId" });
            DropTable("dbo.Regions");
            DropTable("dbo.RegionJobOffers");
        }
    }
}
