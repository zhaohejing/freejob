namespace Job.Application {
    using System;
    using System.Collections.Generic;




    public class BaseFilter {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    public class WorkFilter:BaseFilter {
        public int CompanyId { get; set; }
        public int Cate { get; set; }
        public int Area { get; set; }
    }
    public class BaseWorkOutput {
        public DateTime CreationTime { get; set; }
        public int Id { get; set; }
        public string WorkName { get; set; }
        public string CompanyName { get; set; }
        public string temp { get; set; }

        public string WorkArea { get; set; }
        public List<string> WorkCate { get; set; }
        public string State { get; set; }
        public decimal Pay { get; set; }

        public string PayState { get; set; }
    }
    public class RegistWorkOutput {
        public DateTime CreationTime { get; set; }
        public int Id { get; set; }
        public string WorkName { get; set; }
        public string CompanyName { get; set; }

        public string WorkArea { get; set; }
        public List<string> WorkCate { get; set; }
        public string State { get; set; }
        public decimal Pay { get; set; }

        public string PayState { get; set; }
        public int IsHaveNew { get; set; }
    }
    public class CompanyOutPut {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Area { get; set; }
        public string CompanyLogo { get; set; }
        public List<string> Industry { get; set; }
    }
    public class CompanyTemp {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Area { get; set; }
        public string CompanyLogo { get; set; }
        public string Industry { get; set; }
    }
    public class WorkDetail {
        public WorkOutput WorkOutput { get; set; }
        public CompanyOutPut CompanyOutPut { get; set; }
    }
    public class WorkOutput: BaseWorkOutput {
        public int NeedSum { get; set; }
        public int JoinCount { get; set; }
        public string NeedSex { get; set; }
        public DateTime StartDate { get; set; }
        public string ParttimeIntroduction { get; set; }
        public DateTime EndDate { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public DateTime CloseTime { get; set; }
        public int CreationOne { get; set; }

        //是否可以报名(0 可以报名  1 和  2（已通过） 都不能 )
        public int CanRegist { get; set; }

    }
    public class WorkTemp {
        public int Id { get; set; }
        public string WorkName { get; set; }
    }

    public class WorkInfo {
        public string WorkName { get; set; }
        public int WorkArea { get; set; }
        public List<int> WorkCates { get; set; }
        public DateTime? StartDate { get; set; }
        public string WorkIntroduction { get; set; }
        public DateTime? EndDate { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public int NeedSum { get; set; }
        public DateTime? CloseTime { get; set; }

        public int NeedSex { get; set; }
        public decimal Pay { get; set; }

        public int PayState { get; set; }
    }
    public partial class Tbl_ParttimeInfo
    {
        public int Id { get; set; }

   
        public string ParttimeName { get; set; }

        public int ParttimeState { get; set; }

        public int ParttimeArea { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime CloseTime { get; set; }

        public decimal Pay { get; set; }

        public int PayState { get; set; }

      
        public string ParttimeIntroduction { get; set; }

        public int FromState { get; set; }

        public int State { get; set; }

        public int NeedSum { get; set; }

        public int NeedSex { get; set; }

        public int Pid { get; set; }

        public int? CreationOne { get; set; }

        public DateTime CreationTime { get; set; }

        public bool IsDeleted { get; set; }

        public int? UpdateOne { get; set; }

        public DateTime updateTime { get; set; }
    }
}
