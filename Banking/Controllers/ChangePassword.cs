using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Controllers
{
    public class ChangePassword
    {
        public decimal account_no { get; set; }
        public string login_pass { get; set; }

        public string new_pass { get; set; }
        public string reenter_new_pass { get; set; }

    }
}