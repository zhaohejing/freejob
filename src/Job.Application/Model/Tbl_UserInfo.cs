namespace Job.Application
{
    using System;
    using System.Collections.Generic;

    public partial class Tbl_UserInfo
    {
        public int Id { get; set; }

        public int Pid { get; set; }

     
        public string Name { get; set; }

      
        public string Mail { get; set; }

        public int Sex { get; set; }

        public int WorkYear { get; set; }

     
        public string Area { get; set; }

      
        public string Introduction { get; set; }

     
        public string phone { get; set; }

        public int State { get; set; }

        public int? CreationOne { get; set; }

        public DateTime CreationTime { get; set; }

        public bool IsDeleted { get; set; }

        public int? UpdateOne { get; set; }

        public DateTime updateTime { get; set; }
    }

    public class BaseUser {
        public string Name { get; set; }
        public IEnumerable<string> Works { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public string WorkYear { get; set; }
        public string Introduction { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public string State { get; set; }

    }


    public class UserInfo :BaseUser{
        public int Sex { get; set; }
        public int Area { get; set; }
        public string Code { get; set; }
    }

    public class Project {
        public string ProjectName { get; set; }
        public string positionName { get; set; }
        public string ProjectIntroduction { get; set; }
    }
}
