using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Controllers
{
    public class ForgotTranspassword
    {
        public decimal account_no { get; set; }
        public string login_pass { get; set; }

        public string email_id { get; set; }
        public Nullable<decimal> otp { get; set; }

    }
}