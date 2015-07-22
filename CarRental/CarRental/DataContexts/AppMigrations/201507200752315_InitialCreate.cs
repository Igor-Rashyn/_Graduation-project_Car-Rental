namespace CarRental.DataContexts.AppMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DateOfCreation = c.DateTime(),
                        DateOfChange = c.DateTime(),
                        CreatorId = c.String(maxLength: 128),
                        Status = c.String(),
                        StatusRu = c.String(),
                        PickupDateTime = c.DateTime(),
                        ReturnDateTime = c.DateTime(),
                        Location = c.String(),
                        Passengers = c.Int(nullable: false),
                        AmountOfLuggage = c.String(maxLength: 50),
                        AmountOfLuggageRu = c.String(maxLength: 50),
                        Transmission = c.Boolean(nullable: false),
                        FuelConsumption = c.Int(nullable: false),
                        Brand = c.String(nullable: false, maxLength: 50),
                        Model = c.String(nullable: false, maxLength: 50),
                        Price = c.Int(nullable: false),
                        AdditionInformation = c.String(maxLength: 500),
                        AdditionInformationRu = c.String(maxLength: 500),
                        AirConditioning = c.Boolean(nullable: false),
                        CarNumber = c.Int(nullable: false, identity: true),
                        PictureId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pictures", t => t.PictureId)
                .Index(t => t.PictureId);
            
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ImageData = c.Binary(),
                        ImageMimeType = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Diagnostics",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DateOfCreation = c.DateTime(),
                        CreatorId = c.String(maxLength: 128),
                        CarId = c.Guid(),
                        OrderId = c.Guid(nullable: false),
                        DetectedFailure = c.Boolean(nullable: false),
                        RepairCost = c.Int(nullable: false),
                        Description = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cars", t => t.CarId)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.CarId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        DateOfCreation = c.DateTime(),
                        DateOfChange = c.DateTime(),
                        Status = c.String(),
                        StatusRu = c.String(),
                        CreatorId = c.String(maxLength: 128),
                        ApplicationNumber = c.Int(nullable: false, identity: true),
                        PickupLocation = c.String(nullable: false),
                        PickupDateTime = c.DateTime(nullable: false),
                        ReturnLocation = c.String(nullable: false),
                        ReturnDateTime = c.DateTime(nullable: false),
                        CarId = c.Guid(),
                        TotalPrice = c.Int(nullable: false),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 50),
                        Phone = c.String(nullable: false, maxLength: 20),
                        DateOfBirthday = c.DateTime(nullable: false),
                        Address = c.String(),
                        City = c.String(),
                        StateOrProvince = c.String(),
                        PostcodeOrZip = c.String(),
                        CountryOfResidence = c.String(),
                        LicenseNo = c.String(),
                        CountryOfIssue = c.String(),
                        ExpiryDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cars", t => t.CarId)
                .Index(t => t.CarId);
            
            CreateTable(
                "dbo.Emails",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Subject = c.String(),
                        From = c.String(),
                        To = c.String(),
                        Body = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Diagnostics", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "CarId", "dbo.Cars");
            DropForeignKey("dbo.Diagnostics", "CarId", "dbo.Cars");
            DropForeignKey("dbo.Cars", "PictureId", "dbo.Pictures");
            DropIndex("dbo.Orders", new[] { "CarId" });
            DropIndex("dbo.Diagnostics", new[] { "OrderId" });
            DropIndex("dbo.Diagnostics", new[] { "CarId" });
            DropIndex("dbo.Cars", new[] { "PictureId" });
            DropTable("dbo.Emails");
            DropTable("dbo.Orders");
            DropTable("dbo.Diagnostics");
            DropTable("dbo.Pictures");
            DropTable("dbo.Cars");
        }
    }
}
