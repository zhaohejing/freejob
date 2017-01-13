namespace Job.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wwwww : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_CompanyIndustry",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Pid = c.Int(nullable: false),
                        IndustryId = c.Int(nullable: false),
                        CreationOne = c.Int(),
                        CreationTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        UpdateOne = c.Int(),
                        updateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tbl_CompanyInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false, maxLength: 100),
                        CompanyArea = c.Int(nullable: false),
                        CompanyAddress = c.String(nullable: false, maxLength: 400),
                        CompanyIntroduction = c.String(nullable: false, maxLength: 600),
                        Mail = c.String(nullable: false, maxLength: 50),
                        Phone = c.String(nullable: false, maxLength: 35),
                        CompanyLogo = c.String(nullable: false, maxLength: 600),
                        CardImage = c.String(nullable: false, maxLength: 600),
                        State = c.Int(nullable: false),
                        Pid = c.Int(nullable: false),
                        CreationOne = c.Int(),
                        CreationTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        UpdateOne = c.Int(),
                        updateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tbl_ParttimeIndustry",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Pid = c.Int(nullable: false),
                        IndustryId = c.Int(nullable: false),
                        CreationOne = c.Int(),
                        CreationTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        UpdateOne = c.Int(),
                        updateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tbl_ParttimeInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParttimeName = c.String(nullable: false, maxLength: 100),
                        ParttimeArea = c.Int(nullable: false),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        StartTime = c.Int(),
                        EndTime = c.Int(),
                        CloseTime = c.DateTime(nullable: false),
                        Pay = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PayState = c.Int(nullable: false),
                        ParttimeIntroduction = c.String(nullable: false, maxLength: 600),
                        FromState = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                        NeedSum = c.Int(nullable: false),
                        NeedSex = c.Int(nullable: false),
                        Pid = c.Int(nullable: false),
                        CreationOne = c.Int(),
                        CreationTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        UpdateOne = c.Int(),
                        updateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tbl_ParttimeJob",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tbl_PublicEnum",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EnumName = c.String(nullable: false, maxLength: 60),
                        CreationOne = c.Int(),
                        CreationTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        UpdateOne = c.Int(),
                        updateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tbl_PublicSubEnum",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Pid = c.Int(nullable: false),
                        SubName = c.String(nullable: false, maxLength: 60),
                        LevelCode = c.String(nullable: false, maxLength: 255),
                        CreationOne = c.Int(),
                        CreationTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        UpdateOne = c.Int(),
                        updateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tbl_SysUser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WeChatToken = c.String(nullable: false, maxLength: 256),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tbl_UserInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Pid = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Mail = c.String(nullable: false, maxLength: 100),
                        Sex = c.Int(nullable: false),
                        WorkYear = c.Int(nullable: false),
                        Area = c.String(nullable: false, maxLength: 50),
                        Introduction = c.String(maxLength: 500),
                        phone = c.String(nullable: false, maxLength: 25),
                        State = c.Int(nullable: false),
                        CreationOne = c.Int(),
                        CreationTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        UpdateOne = c.Int(),
                        updateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tbl_UserLogin",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LoginName = c.String(nullable: false, maxLength: 60),
                        PassWord = c.String(nullable: false, maxLength: 30),
                        Pid = c.Int(nullable: false),
                        CreationOne = c.Int(),
                        CreationTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        UpdateOne = c.Int(),
                        updateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tbl_UserProject",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Pid = c.Int(nullable: false),
                        JobName = c.String(nullable: false, maxLength: 60),
                        PositionName = c.String(nullable: false, maxLength: 30),
                        JobIntroduction = c.String(nullable: false, maxLength: 300),
                        CreationOne = c.Int(),
                        CreationTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        UpdateOne = c.Int(),
                        updateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tbl_UserRegistion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParttimeId = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                        Pid = c.Int(nullable: false),
                        CreationOne = c.Int(),
                        CreationTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        UpdateOne = c.Int(),
                        updateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tbl_UserWork",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationOne = c.Int(),
                        CreationTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Pid = c.Int(nullable: false),
                        WorkId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tbl_VerifyCode",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SysId = c.Int(),
                        Mobile = c.String(nullable: false, maxLength: 20),
                        VerifyType = c.Int(nullable: false),
                        VerifyCode = c.String(nullable: false, maxLength: 20),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_VerifyCode");
            DropTable("dbo.Tbl_UserWork");
            DropTable("dbo.Tbl_UserRegistion");
            DropTable("dbo.Tbl_UserProject");
            DropTable("dbo.Tbl_UserLogin");
            DropTable("dbo.Tbl_UserInfo");
            DropTable("dbo.Tbl_SysUser");
            DropTable("dbo.Tbl_PublicSubEnum");
            DropTable("dbo.Tbl_PublicEnum");
            DropTable("dbo.Tbl_ParttimeJob");
            DropTable("dbo.Tbl_ParttimeInfo");
            DropTable("dbo.Tbl_ParttimeIndustry");
            DropTable("dbo.Tbl_CompanyInfo");
            DropTable("dbo.Tbl_CompanyIndustry");
        }
    }
}
