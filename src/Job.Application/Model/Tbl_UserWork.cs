namespace Job.Application
{
    using System;


    public partial class Tbl_UserWork
    {
        public int Id { get; set; }

        public int? CreationOne { get; set; }

        public DateTime CreationTime { get; set; }

        public bool IsDeleted { get; set; }

        public int Pid { get; set; }

        public int WorkId { get; set; }
    }
}
