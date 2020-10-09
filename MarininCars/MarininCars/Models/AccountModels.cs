using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarininCars.Models
{
    public class LoginM
    {
        public string Login {get;set;}
        public string Password { get; set; }
    }
    public class RegisterM
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public int IdRole { get; set; }
        public string Email { get; set; }
    }
}