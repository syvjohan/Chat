using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Chat.Models {
    public class Users {
        [Key] public string Username { get; set; }
        public string Password { get; set; }
        public string Mail { get; set; }
    }
}