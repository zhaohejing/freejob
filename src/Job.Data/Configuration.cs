using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Data {
    internal sealed class Configuration : DbMigrationsConfiguration<JobDbContext> {
        public Configuration() {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Job";
        }
        protected override void Seed(JobDbContext context) {
            new InitialDataBuilder().Build();
        }

    }
    public class InitialDataBuilder {
        private readonly JobDbContext _context;

        public InitialDataBuilder() {
            _context = new JobDbContext();
        }

        public void Build() {
            var list = new List<Model.Tbl_PublicEnum>();
            //性别
            var gender = _context.Tbl_PublicEnum.Add(
                 new Job.Data.Model.Tbl_PublicEnum() {
                     CreationOne = null,
                     CreationTime = DateTime.Now,
                     EnumName = "性别",
                     IsDeleted = false,
                     UpdateOne = null,
                     updateTime = DateTime.Now
                 });
            //地区
            var area = _context.Tbl_PublicEnum.Add(
                 new Job.Data.Model.Tbl_PublicEnum() {
                     CreationOne = null,
                     CreationTime = DateTime.Now,
                     EnumName = "企业所在区域",
                     IsDeleted = false,
                     UpdateOne = null,
                     updateTime = DateTime.Now
                 });
            //工作
            var pos = _context.Tbl_PublicEnum.Add(
                 new Job.Data.Model.Tbl_PublicEnum() {
                     CreationOne = null,
                     CreationTime = DateTime.Now,
                     EnumName = "擅长工作",
                     IsDeleted = false,
                     UpdateOne = null,
                     updateTime = DateTime.Now
                 });
            //行业
            var hy = _context.Tbl_PublicEnum.Add(
                 new Job.Data.Model.Tbl_PublicEnum() {
                     CreationOne = null,
                     CreationTime = DateTime.Now,
                     EnumName = "所属行业",
                     IsDeleted = false,
                     UpdateOne = null,
                     updateTime = DateTime.Now
                 });

            //结算方式
            var js = _context.Tbl_PublicEnum.Add(
                 new Job.Data.Model.Tbl_PublicEnum() {
                     CreationOne = null,
                     CreationTime = DateTime.Now,
                     EnumName = "结算方式",
                     IsDeleted = false,
                     UpdateOne = null,
                     updateTime = DateTime.Now
                 });
            //创建来源
            var rewhere = _context.Tbl_PublicEnum.Add(
                 new Job.Data.Model.Tbl_PublicEnum() {
                     CreationOne = null,
                     CreationTime = DateTime.Now,
                     EnumName = "创建来源",
                     IsDeleted = false,
                     UpdateOne = null,
                     updateTime = DateTime.Now
                 });
            //审核状态
            var state = _context.Tbl_PublicEnum.Add(
                 new Job.Data.Model.Tbl_PublicEnum() {
                     CreationOne = null,
                     CreationTime = DateTime.Now,
                     EnumName = "审核状态",
                     IsDeleted = false,
                     UpdateOne = null,
                     updateTime = DateTime.Now
                 });
            //兼职状态
            var jz = _context.Tbl_PublicEnum.Add(
                 new Job.Data.Model.Tbl_PublicEnum() {
                     CreationOne = null,
                     CreationTime = DateTime.Now,
                     EnumName = "兼职状态",
                     IsDeleted = false,
                     UpdateOne = null,
                     updateTime = DateTime.Now
                 });

            //用户报名审核状态
            var urs = _context.Tbl_PublicEnum.Add(
                 new Job.Data.Model.Tbl_PublicEnum() {
                     CreationOne = null,
                     CreationTime = DateTime.Now,
                     EnumName = "用户报名审核状态",
                     IsDeleted = false,
                     UpdateOne = null,
                     updateTime = DateTime.Now
                 });
            //工作年限
            var workyear = _context.Tbl_PublicEnum.Add(
                new Job.Data.Model.Tbl_PublicEnum() {
                    CreationOne = null,
                    CreationTime = DateTime.Now,
                    EnumName = "工作年限",
                    IsDeleted = false,
                    UpdateOne = null,
                    updateTime = DateTime.Now
                });

            _context.SaveChanges();

            _context.Tbl_PublicSubEnum.Add(new Model.Tbl_PublicSubEnum() {
                CreationOne = null,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LevelCode = "levelcode00031",
                Pid = workyear.Id,
                SubName = "1-3年",
                UpdateOne = null,
                updateTime = DateTime.Now
            });
            _context.Tbl_PublicSubEnum.Add(new Model.Tbl_PublicSubEnum() {
                CreationOne = null,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LevelCode = "levelcode00032",
                Pid = workyear.Id,
                SubName = "3-5年",
                UpdateOne = null,
                updateTime = DateTime.Now
            });
            _context.Tbl_PublicSubEnum.Add(new Model.Tbl_PublicSubEnum() {
                CreationOne = null,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LevelCode = "levelcode00033",
                Pid = workyear.Id,
                SubName = "5-10年",
                UpdateOne = null,
                updateTime = DateTime.Now
            });
            _context.Tbl_PublicSubEnum.Add(new Model.Tbl_PublicSubEnum() {
                CreationOne = null,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LevelCode = "levelcode00026",
                Pid = workyear.Id,
                SubName = "10年以上",
                UpdateOne = null,
                updateTime = DateTime.Now
            });

            _context.Tbl_PublicSubEnum.Add(new Model.Tbl_PublicSubEnum() {
                CreationOne = null,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LevelCode = "levelcode00021",
                Pid = urs.Id,
                SubName = "已通过",
                UpdateOne = null,
                updateTime = DateTime.Now
            });
            _context.Tbl_PublicSubEnum.Add(new Model.Tbl_PublicSubEnum() {
                CreationOne = null,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LevelCode = "levelcode00022",
                Pid = urs.Id,
                SubName = "未通过",
                UpdateOne = null,
                updateTime = DateTime.Now
            });
            _context.Tbl_PublicSubEnum.Add(new Model.Tbl_PublicSubEnum( ) {
                CreationOne = null,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LevelCode = "levelcode00023",
                Pid = urs.Id,
                SubName = "已驳回",
                UpdateOne = null,
                updateTime = DateTime.Now
            });
            _context.Tbl_PublicSubEnum.Add(new Model.Tbl_PublicSubEnum( ) {
                CreationOne = null,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LevelCode = "levelcode00024",
                Pid = urs.Id,
                SubName = "已报名",
                UpdateOne = null,
                updateTime = DateTime.Now
            });
            _context.Tbl_PublicSubEnum.Add(new Model.Tbl_PublicSubEnum( ) {
                CreationOne = null,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LevelCode = "levelcode00030",
                Pid = urs.Id,
                SubName = "已放弃",
                UpdateOne = null,
                updateTime = DateTime.Now
            });

            _context.Tbl_PublicSubEnum.Add(new Model.Tbl_PublicSubEnum() {
                CreationOne = null,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LevelCode = "levelcode00018",
                Pid = jz.Id,
                SubName = "已发布",
                UpdateOne = null,
                updateTime = DateTime.Now
            });
            _context.Tbl_PublicSubEnum.Add(new Model.Tbl_PublicSubEnum() {
                CreationOne = null,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LevelCode = "levelcode00019",
                Pid = jz.Id,
                SubName = "已下架",
                UpdateOne = null,
                updateTime = DateTime.Now
            });
            _context.Tbl_PublicSubEnum.Add(new Model.Tbl_PublicSubEnum() {
                CreationOne = null,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LevelCode = "levelcode00020",
                Pid = jz.Id,
                SubName = "已关闭",
                UpdateOne = null,
                updateTime = DateTime.Now
            });
            _context.Tbl_PublicSubEnum.Add(new Model.Tbl_PublicSubEnum() {
                CreationOne = null,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LevelCode = "levelcode00001",
                Pid = gender.Id,
                SubName = "男",
                UpdateOne = null,
                updateTime = DateTime.Now
            });
            _context.Tbl_PublicSubEnum.Add(new Model.Tbl_PublicSubEnum() {
                CreationOne = null,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LevelCode = "levelcode00002",
                Pid = gender.Id,
                SubName = "女",
                UpdateOne = null,
                updateTime = DateTime.Now
            });
            _context.Tbl_PublicSubEnum.Add(new Model.Tbl_PublicSubEnum( ) {
                CreationOne = null,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LevelCode = "levelcode00025",
                Pid = gender.Id,
                SubName = "男女不限",
                UpdateOne = null,
                updateTime = DateTime.Now
            });

            _context.Tbl_PublicSubEnum.Add(new Model.Tbl_PublicSubEnum() {
                CreationOne = null,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LevelCode = "levelcode00003",
                Pid = state.Id,
                SubName = "审核中",
                UpdateOne = null,
                updateTime = DateTime.Now
            });
            _context.Tbl_PublicSubEnum.Add(new Model.Tbl_PublicSubEnum() {
                CreationOne = null,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LevelCode = "levelcode00004",
                Pid = state.Id,
                SubName = "审核通过",
                UpdateOne = null,
                updateTime = DateTime.Now
            });
            _context.Tbl_PublicSubEnum.Add(new Model.Tbl_PublicSubEnum() {
                CreationOne = null,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LevelCode = "levelcode00005",
                Pid = state.Id,
                SubName = "审核失败",
                UpdateOne = null,
                updateTime = DateTime.Now
            });
           
         
          
            _context.Tbl_PublicSubEnum.Add(new Model.Tbl_PublicSubEnum() {
                CreationOne = null,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LevelCode = "levelcode00013",
                Pid = js.Id,
                SubName = "日结",
                UpdateOne = null,
                updateTime = DateTime.Now
            });
            _context.Tbl_PublicSubEnum.Add(new Model.Tbl_PublicSubEnum() {
                CreationOne = null,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LevelCode = "levelcode00014",
                Pid = js.Id,
                SubName = "周结",
                UpdateOne = null,
                updateTime = DateTime.Now
            });
            _context.Tbl_PublicSubEnum.Add(new Model.Tbl_PublicSubEnum() {
                CreationOne = null,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LevelCode = "levelcode00015",
                Pid = js.Id,
                SubName = "月结",
                UpdateOne = null,
                updateTime = DateTime.Now
            });
            _context.Tbl_PublicSubEnum.Add(new Model.Tbl_PublicSubEnum( ) {
                CreationOne = null,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LevelCode = "levelcode00028",
                Pid = js.Id,
                SubName = "一次性结",
                UpdateOne = null,
                updateTime = DateTime.Now
            });


            _context.Tbl_PublicSubEnum.Add(new Model.Tbl_PublicSubEnum() {
                CreationOne = null,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LevelCode = "levelcode00016",
                Pid = rewhere.Id,
                SubName = "企业创建",
                UpdateOne = null,
                updateTime = DateTime.Now
            });
            _context.Tbl_PublicSubEnum.Add(new Model.Tbl_PublicSubEnum() {
                CreationOne = null,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LevelCode = "levelcode00017",
                Pid = rewhere.Id,
                SubName = "个人创建",
                UpdateOne = null,
                updateTime = DateTime.Now
            });


            _context.SaveChanges();
        }

     
    }

}
