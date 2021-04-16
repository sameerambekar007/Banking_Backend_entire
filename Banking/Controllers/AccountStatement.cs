using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Banking.Controllers
{
    public class AccountStatement
    {
        public string ref_id { get; set; }
        public string mode { get; set; }
        public decimal recipient_acct { get; set; }
        public decimal account_no { get; set; }
        public decimal amount { get; set; }
        public Nullable<System.DateTime> trans_date { get; set; }
        public Nullable<System.DateTime> to_trans_date { get; set; }
        public string remarks { get; set; }
        public Nullable<decimal> trans_pass { get; set; }
        public Nullable<decimal> recipient_updated_credit { get; set; }
        public Nullable<decimal> sender_updated_debit { get; set; }

    }
}