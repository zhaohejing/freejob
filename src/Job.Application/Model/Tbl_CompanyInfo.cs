namespace Job.Application {
    using System;
    using System.Collections.Generic;

    public class CompanyWorkUsers {
        public CompanyWork CompanyWork { get; set; }
        public List<WorkUsers> WorkUsers { get; set; }
    }
    public class CompanyWork {
        public int Id { get; set; }
        public string ParttimeName { get; set; }
        public decimal Pay { get; set; }
        public string PayState { get; set; }
        public string State { get; set; }
        public List<string> Industrys { get; set; }
    }
    public class WorkUsers {
        public string State { get; set; }
        public int ParttimeId { get; set; }
        public int Pid { get; set; }
        public string Name { get; set; }
        public string WorkYear { get; set; }
        public string Sex { get; set; }
        public string Introduction { get; set; }
    }
    public class CompanyInfo {
        public string CompanyLogo { get; set; }
        public string CompanyName { get; set; }
        /// <summary>
        /// 公司所属行业 多个
        /// </summary>
        public List<int> CompanyIndustrys { get; set; }

        public string CompanyArea { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyIntroduction { get; set; }
        public string CardImage { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Code { get; set; }
    }

    public partial class Tbl_CompanyInfo
    {
        public int Id { get; set; }

      
        public string CompanyName { get; set; }

        public int CompanyArea { get; set; }

      
        public string CompanyAddress { get; set; }

      
        public string CompanyIntroduction { get; set; }

     
        public string Mail { get; set; }

     
        public string Phone { get; set; }

      
        public string CompanyLogo { get; set; }

    
        public string CardImage { get; set; }

        public int State { get; set; }

        public int Pid { get; set; }

        public int? CreationOne { get; set; }

        public DateTime CreationTime { get; set; }

        public bool IsDeleted { get; set; }

        public int? UpdateOne { get; set; }

        public DateTime updateTime { get; set; }
    }
}
