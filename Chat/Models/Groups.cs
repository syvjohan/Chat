using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Chat.Models {
    public class Groups {
        [Key] public long ID { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
    }
}