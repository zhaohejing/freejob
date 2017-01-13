namespace Job.Application
{
    using System;


    public partial class Tbl_SysUser
    {
        public int Id { get; set; }
        public string WeChatToken { get; set; }
        public int HadRegisterUser { get; set; }
        public int HadRegisterCompany { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
