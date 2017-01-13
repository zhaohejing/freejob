namespace Job.Application
{
    using System;


    public partial class Tbl_UserLogin
    {
        public int Id { get; set; }

    
        public string LoginName { get; set; }

        public string PassWord { get; set; }

        public int Pid { get; set; }

        public int? CreationOne { get; set; }

        public DateTime CreationTime { get; set; }

        public bool IsDeleted { get; set; }

        public int? UpdateOne { get; set; }

        public DateTime updateTime { get; set; }
    }
}
