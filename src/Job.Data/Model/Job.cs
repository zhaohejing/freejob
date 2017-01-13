using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Data.Model {
    [Table("Tbl_SysUser")]
    public class Tbl_SysUser : BaseEntity {
        [Required,MaxLength(256)]
        public string WeChatToken { get; set; }
        public DateTime CreationTime { get; set; }
    }
    [Table("Tbl_UserInfo")]
    public class Tbl_UserInfo : CreateEntity, IForeignKey, IUpdateTime {
        public int Pid { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required, MaxLength(100)]

        public string Mail { get; set; }
        public int Sex { get; set; }
        public int WorkYear { get; set; }
        [Required, MaxLength(50)]

        public string Area { get; set; }
        [MaxLength(500)]

        public string Introduction { get; set; }
        [Required, MaxLength(25)]

        public string phone { get; set; }
        public int State { get; set; }


    }

    [Table("Tbl_UserWork")]

    public class Tbl_UserWork : BaseEntity, IForeignKey, ICreationTime, ISoftDelete {
        public int? CreationOne { get; set; }

        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }

        public int Pid { get; set; }
        public int WorkId { get; set; }
    }
    [Table("Tbl_UserProject")]

    public class Tbl_UserProject : CreateEntity, IForeignKey {
        public int Pid { get; set; }
        [Required, MaxLength(60)]
        public string JobName { get; set; }
        [Required, MaxLength(30)]
        public string PositionName { get; set; }
        [Required, MaxLength(300)]
        public string JobIntroduction { get; set; }
    }
    [Table("Tbl_UserLogin")]

    public class Tbl_UserLogin : CreateEntity, IForeignKey, IUpdateTime {
        [Required, MaxLength(60)]
        public string LoginName { get; set; }
        [Required, MaxLength(30)]

        public string PassWord { get; set; }

        public int Pid { get; set; }

    }
    [Table("Tbl_UserRegistion")]

    public class Tbl_UserRegistion : CreateEntity, IForeignKey, IUpdateTime {
        public int ParttimeId { get; set; }
        public int State { get; set; }
        public int Pid { get; set; }

    }
    [Table("Tbl_CompanyInfo")]

    public class Tbl_CompanyInfo : CreateEntity, IForeignKey, IUpdateTime {
        [Required, MaxLength(100)]
        public string CompanyName { get; set; }
        public int CompanyArea { get; set; }
        [Required, MaxLength(400)]
        public string CompanyAddress { get; set; }
        [Required, MaxLength(600)]
        public string CompanyIntroduction { get; set; }
        [Required, MaxLength(50)]

        public string Mail { get; set; }
        [Required, MaxLength(35)]

        public string Phone { get; set; }

        [Required, MaxLength(600)]
        public string CompanyLogo { get; set; }
        [Required, MaxLength(600)]
        public string CardImage { get; set; }
        public int State { get; set; }
        public int Pid { get; set; }

    }
    [Table("Tbl_CompanyIndustry")]

    public class Tbl_CompanyIndustry : CreateEntity, IForeignKey {
        public int Pid { get; set; }
        public int IndustryId { get; set; }
    }
    [Table("Tbl_ParttimeJob")]

    public class Tbl_ParttimeJob : BaseEntity {
        public DateTime CreationTime { get; set; }
    }
    [Table("Tbl_VerifyCode")]

    public class Tbl_VerifyCode: BaseEntity {
        public int? SysId { get; set; }
        [Required,MaxLength(20)]
        public string Mobile { get; set; }
        public int VerifyType { get; set; }
        [Required, MaxLength(20)]
        public string VerifyCode { get; set; }
        public DateTime CreationTime { get; set; }
    }


    [Table("Tbl_ParttimeInfo")]

    public class Tbl_ParttimeInfo : CreateEntity, IForeignKey, IUpdateTime {
        [Required, MaxLength(100)]
        public string ParttimeName { get; set; }
        public int ParttimeArea { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? StartTime { get; set; }
        public int? EndTime { get; set; }
        public DateTime CloseTime { get; set; }
        public decimal Pay { get; set; }
        public int PayState { get; set; }
        [Required, MaxLength(600)]
        public string ParttimeIntroduction { get; set; }
        public int FromState { get; set; }
        public int State { get; set; }
        public int NeedSum { get; set; }
        public int NeedSex { get; set; }
        public int Pid { get; set; }
    }
    [Table("Tbl_ParttimeIndustry")]

    public class Tbl_ParttimeIndustry : CreateEntity, IForeignKey {
        public int Pid { get; set; }
        public int IndustryId { get; set; }
    }
    [Table("Tbl_PublicEnum")]
    public class Tbl_PublicEnum : CreateEntity {
        [Required, MaxLength(60)]
        public string EnumName { get; set; }


    }
    [Table("Tbl_PublicSubEnum")]

    public class Tbl_PublicSubEnum : CreateEntity, IForeignKey {
        public int Pid { get; set; }
        [Required, MaxLength(60)]
        public string SubName { get; set; }
        [Required, MaxLength(255)]
        public string LevelCode { get; set; }
    }

    public abstract class BaseEntity {
        [Key]

        public int Id { get; set; }
    }

    public abstract class CreateEntity : BaseEntity, ICreationTime, ISoftDelete, IUpdateTime {
        public int? CreationOne { get; set; }
        public DateTime CreationTime { get; set; }

        public bool IsDeleted { get; set; }
        public int? UpdateOne { get; set; }

        public DateTime updateTime { get; set; }
    }
    public interface IForeignKey {
        [Required]
        int Pid { get; set; }
    }
    public interface ICreationTime {
        DateTime CreationTime { get; set; }
        int? CreationOne { get; set; }
    }

    public interface IUpdateTime {
        DateTime updateTime { get; set; }
        int? UpdateOne { get; set; }

    }
    public interface ISoftDelete {
        bool IsDeleted { get; set; }
    }
}
