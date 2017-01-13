using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Job.Application {
    public class MessageModel {
        public string Mobile { get; set; }
        public int? SysId { get; set; }
        public string Code { get; set; }
    }
    public class TempSerialize {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class SerializeModel : TempSerialize {
        public string LevelCode { get; set; }
    }
}