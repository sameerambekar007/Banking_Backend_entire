﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class BankingEntities1 : DbContext
    {
        public BankingEntities1()
            : base("name=BankingEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account_Holder> Account_Holder { get; set; }
        public virtual DbSet<Beneficiary> Beneficiaries { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
    
        public virtual ObjectResult<display_customers_Result> display_customers()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<display_customers_Result>("display_customers");
        }
    
        public virtual ObjectResult<display_customer_requests_Result> display_customer_requests()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<display_customer_requests_Result>("display_customer_requests");
        }
    
        public virtual ObjectResult<display_Acccount_Holder_Result> display_Acccount_Holder()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<display_Acccount_Holder_Result>("display_Acccount_Holder");
        }
    
        public virtual ObjectResult<display_customer_requests1_Result> display_customer_requests1()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<display_customer_requests1_Result>("display_customer_requests1");
        }
    }
}
