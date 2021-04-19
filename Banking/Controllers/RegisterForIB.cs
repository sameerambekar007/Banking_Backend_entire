using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Controllers
{
    public class RegisterForIB
    {
        public string customer_id { get; set; }
        public decimal account_no { get; set; }
        public string login_pass { get; set; }
        public Nullable<decimal> trans_pass { get; set; }

        public string reenter_login_pass { get; set; }
        public Nullable<decimal> reenter_trans_pass { get; set; }

    }
}