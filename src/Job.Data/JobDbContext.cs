using Job.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Data {
   public class JobDbContext :DbContext{
        public JobDbContext() : base("Default") {

        }
        public JobDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString) {

        }
        public JobDbContext(DbConnection connection)
            : base(connection, true) {

        }
        
        public IDbSet<Tbl_ParttimeIndustry> Tbl_ParttimeIndustry { get; set; }
        public IDbSet<Tbl_VerifyCode> Tbl_VerifyCode { get; set; }
        public IDbSet<Tbl_SysUser> Tbl_SysUser { get; set; }
        public IDbSet<Tbl_UserInfo> Tbl_UserInfo { get; set; }
        public IDbSet<Tbl_UserWork> Tbl_UserWork { get; set; }
        public IDbSet<Tbl_UserProject> Tbl_UserProject { get; set; }
        public IDbSet<Tbl_UserLogin> Tbl_UserLogin { get; set; }
        public IDbSet<Tbl_UserRegistion> Tbl_UserRegistion { get; set; }
        public IDbSet<Tbl_CompanyInfo> Tbl_CompanyInfo { get; set; }
        public IDbSet<Tbl_CompanyIndustry> Tbl_CompanyIndustry { get; set; }
        public IDbSet<Tbl_ParttimeJob> Tbl_ParttimeJob { get; set; }
        public IDbSet<Tbl_PublicEnum> Tbl_PublicEnum { get; set; }
        public IDbSet<Tbl_PublicSubEnum> Tbl_PublicSubEnum { get; set; }
        public IDbSet<Tbl_ParttimeInfo> Tbl_ParttimeInfo { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
