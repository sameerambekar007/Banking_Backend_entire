using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Controllers
{
    public class ChangeTransPassword
    {
        public decimal account_no { get; set; }
        public Nullable<decimal> trans_pass { get; set; }
        public Nullable<decimal> new_trans_pass { get; set; }

        public Nullable<decimal> reenter_trans_pass { get; set; }

    }
}