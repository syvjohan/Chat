using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Chat.Models {
    public class GroupMessages {
        [Key] public long ID { get; set; }
        public string Sender { get; set; }
        public string Message { get; set; }
        public string Timestamp { get; set; }
    }
}