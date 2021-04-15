using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using Banking.Models;

namespace Banking.Controllers
{
    public class Account_HolderInsertController : ApiController
    {
        public Random random = new Random();
        private BankingEntities1 db = new BankingEntities1();

        // GET: api/Account_HolderInsert
        public IHttpActionResult GetAccount_Holder()
        {
            return Ok(db.display_Acccount_Holder1());
        }

        // GET: api/Account_HolderInsert/5
        [ResponseType(typeof(Account_Holder))]
        public IHttpActionResult GetAccount_Holder(decimal id)
        {
            Account_Holder account_Holder = db.Account_Holder.Find(id);
            if (account_Holder == null)
            {
                return NotFound();
            }

            return Ok(account_Holder);
        }

        // PUT: api/Account_HolderInsert/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAccount_Holder(decimal id, Account_Holder account_Holder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != account_Holder.account_no)
            {
                return BadRequest();
            }

            db.Entry(account_Holder).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Account_HolderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Account_HolderInsert
        [ResponseType(typeof(Account_Holder))]
        public IHttpActionResult PostAccount_Holder(Account_Holder account_Holder)
        {
           var email1= db.send_email(account_Holder.service_ref_no).FirstOrDefault(); 
           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var builder = new StringBuilder();
            while (builder.Length < 16)
            {
                builder.Append(random.Next(10).ToString());
            }
            var c= Convert.ToString(builder);
            var b = Decimal.Parse(c);
            // var result = db.Admins.Where(a => a.admin_id == admin.admin_id && a.admin_password == admin.admin_password).FirstOrDefault();
            account_Holder.customer_id = account_Holder.customer_name +"@" + random.Next(10000,99999).ToString();
            account_Holder.account_no = b;
            var optnetbanking = db.Customers.Where(a => a.service_ref_no == account_Holder.service_ref_no && a.opt_netbanking=="true").FirstOrDefault();
            if(optnetbanking!=null)
            { 
            account_Holder.login_pass = "RSVZ" + "@" + random.Next(10000).ToString();
            account_Holder.trans_pass = random.Next(10000,99999);
            
            }
            account_Holder.balance = 5000;
            account_Holder.account_status = "open";
            if (optnetbanking != null)
            {
                SendMail(email1, account_Holder.customer_id, account_Holder.account_no, account_Holder.login_pass, account_Holder.trans_pass);
            }
            else { 
                SendMail1(email1, account_Holder.customer_id, account_Holder.account_no);
            }
            db.Account_Holder.Add(account_Holder);
           
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (Account_HolderExists(account_Holder.account_no))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }


            return Ok(email1);
        }
        public void SendMail(string To,string custid,decimal accountno,string loginpass,decimal? transpass)
        {
          
            MailMessage mail = new MailMessage("rsvzbank@gmail.com",To);
            mail.Subject ="Trail";
            mail.Body = "Customer Id:\t" + custid +
                "\nAccount Number:\t" + accountno + "\nLogin Passwword:\t" + loginpass + "\nTransaction Password:\t" + transpass;
            //Attachment attachment = new Attachment(@"");
            //mail.Attachments.Add(attachment);
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "rsvzbank@gmail.com",
                Password = "RSVZ@123"
            };
            client.EnableSsl = true;
            client.Send(mail);
        }
        public void SendMail1(string To, string custid, decimal accountno)
        {

            MailMessage mail = new MailMessage("rsvzbank@gmail.com", To);
            mail.Subject = "Trail";
            mail.Body = "Customer Id:\t" + custid + "\n Account Number:\t" + accountno;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "rsvzbank@gmail.com",
                Password = "RSVZ@123"
            };
            client.EnableSsl = true;
            client.Send(mail);
        }
        //[HttpPost]
        //public IHttpActionResult Get([FromBody] Customer customer)
        //{
        //    var result = db.Customers.Where(a => a.service_ref_no == customer.service_ref_no && a.first_name == customer.first_name && a.last_name == customer.last_name).FirstOrDefault();
        //    if (result != null)
        //    {
        //        return Ok("Found");
        //    }
        //    else
        //    {
        //        return Ok("NOT FOUND");
        //    }
        //}

        // DELETE: api/Account_HolderInsert/5
        [ResponseType(typeof(Account_Holder))]
        public IHttpActionResult DeleteAccount_Holder(decimal id)
        {
            Account_Holder account_Holder = db.Account_Holder.Find(id);
            if (account_Holder == null)
            {
                return NotFound();
            }

            db.Account_Holder.Remove(account_Holder);
            db.SaveChanges();

            return Ok(account_Holder);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Account_HolderExists(decimal id)
        {
            return db.Account_Holder.Count(e => e.account_no == id) > 0;
        }
    }
}