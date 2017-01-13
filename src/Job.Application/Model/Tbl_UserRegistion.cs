namespace Job.Application
{
    using System;
    public class RegistWorkOutPut {
        public int ParttimeId { get; set; }
        public string ParttimeName { get; set; }
        public string State { get; set; }
        public decimal Pay { get; set; }
        public string PayState { get; set; }
        public string CompanyName { get; set; }
        public DateTime CloseTime { get; set; }

        public int ShowDel { get; set; }
    }

    public partial class Tbl_UserRegistion
    {
        public int Id { get; set; }

        public int ParttimeId { get; set; }

        public int State { get; set; }

        public int Pid { get; set; }

        public int? CreationOne { get; set; }

        public DateTime CreationTime { get; set; }

        public bool IsDeleted { get; set; }

        public int? UpdateOne { get; set; }

        public DateTime updateTime { get; set; }
    }
}
