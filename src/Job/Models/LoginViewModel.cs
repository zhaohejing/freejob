using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Job.Models {
    public class LoginViewModel {

        [Required]
        public string OpenId { get; set; }
    }
    public class RegistWork {
        public int UserId { get; set; }
        public int WorkId { get; set; }
    }
    public class RegistUserState {
        public int UserId { get; set; }
        public int WorkId { get; set; }
        //public int state { get; set; }
    }
}