namespace Job.Application
{
    using System;


    public partial class Tbl_UserProject
    {
        public int Id { get; set; }

        public int Pid { get; set; }

     
        public string JobName { get; set; }


        public string PositionName { get; set; }

        public string JobIntroduction { get; set; }

        public int WorkId { get; set; }

        public int? CreationOne { get; set; }

        public DateTime CreationTime { get; set; }

        public bool IsDeleted { get; set; }

        public int? UpdateOne { get; set; }

        public DateTime updateTime { get; set; }
    }
}
