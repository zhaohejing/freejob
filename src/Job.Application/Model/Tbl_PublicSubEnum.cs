namespace Job.Application
{
    using System;


    public partial class Tbl_PublicSubEnum
    {
        public int Id { get; set; }

        public int Pid { get; set; }

       
        public string SubName { get; set; }

    
        public string LevelCode { get; set; }

        public int? CreationOne { get; set; }

        public DateTime CreationTime { get; set; }

        public bool IsDeleted { get; set; }

        public int? UpdateOne { get; set; }

        public DateTime updateTime { get; set; }
    }
}
