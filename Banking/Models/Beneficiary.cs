//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Banking.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Beneficiary
    {
        public decimal ben_id { get; set; }
        public string ben_name { get; set; }
        public decimal ben_account_no { get; set; }
        public string ben_nickname { get; set; }
        public decimal account_no { get; set; }
    
        public virtual Account_Holder Account_Holder { get; set; }
    }
}
