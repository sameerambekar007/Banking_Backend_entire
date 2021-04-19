using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Http.Description;
using Banking.Models;

namespace Banking.Controllers
{
    public class RtgstransferController : ApiController
    {
        public Random random = new Random();
        private BankingEntities1 db = new BankingEntities1();

        // GET: api/Rtgstransfer
        public IHttpActionResult GetTransactions()
        {
            return Ok(db.display_Transactions1());
        }

        // GET: api/Rtgstransfer/5
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult GetTransaction(string id)
        {
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        // PUT: api/Rtgstransfer/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTransaction(string id, Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transaction.ref_id)
            {
                return BadRequest();
            }

            db.Entry(transaction).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
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

        // POST: api/Rtgstransfer
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult PostTransaction(Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var validpassword = db.Account_Holder.Where(a => a.trans_pass == transaction.trans_pass && a.account_no == transaction.account_no).FirstOrDefault();
            var insufficientamt = db.Account_Holder.Where(a => a.balance >= transaction.amount && a.account_no == transaction.account_no).FirstOrDefault();
            if (validpassword != null && insufficientamt != null)
            {
                //transaction.ref_id = "RefRTGS" + "@" + random.Next(10000, 99999).ToString();
                transaction.mode = "rtgs";
                DateTime myDateTime = DateTime.Now;
                transaction.trans_date = myDateTime;
                db.debit_account(transaction.account_no, transaction.amount);
                db.credit_account(transaction.recipient_acct, transaction.amount);
                transaction.sender_updated_debit = db.updated_balance(transaction.account_no).FirstOrDefault();
                transaction.recipient_updated_credit = db.updated_balance(transaction.recipient_acct).FirstOrDefault();
                db.Transactions.Add(transaction);
                try
                {
                    db.SaveChanges();
                }

                catch (DbUpdateException)
                {
                    if (TransactionExists(transaction.ref_id))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }

                Sendrtgssuccessdebited(db.fetchemail(transaction.account_no).FirstOrDefault(), transaction.ref_id, transaction.amount, transaction.mode, transaction.recipient_acct, transaction.trans_date, transaction.remarks);
                Sendrtgssuccesscredited(db.fetchemail(transaction.recipient_acct).FirstOrDefault(), transaction.ref_id, transaction.amount, transaction.mode, transaction.account_no, transaction.trans_date, transaction.remarks);

                return Ok("success");
            }

            if (validpassword == null)
            {
                return Ok("invalidpass");
            }
            if (insufficientamt == null)
            {
                return Ok("notenoughbalance");
            }
            return CreatedAtRoute("DefaultApi", new { id = transaction.ref_id }, transaction);
        }
        public void Sendrtgssuccessdebited(string To, string ref_id, decimal? amount, string mode, decimal? account_no, DateTime? tdate, string remarks)
        {

            MailMessage mail = new MailMessage("rsvzbank@gmail.com", To);
            mail.Subject = "Transaction Successful!" + "Your account has been debited by\t" + amount;
            mail.Body = "Transaction Reference id\t" + ref_id + "\n Amount\t" + amount + "\nMode of transaction\t" + mode + "\n Recepient Account\t" + account_no + "\nTransaction time\t" + tdate + "\n Remarks\t" + remarks;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "rsvzbank@gmail.com",
                Password = "RSVZ@123"
            };
            client.EnableSsl = true;
            client.Send(mail);
        }
        public void Sendrtgssuccesscredited(string To, string ref_id, decimal? amount, string mode, decimal? account_no, DateTime? tdate, string remarks)
        {

            MailMessage mail = new MailMessage("rsvzbank@gmail.com", To);
            mail.Subject = "Transaction Successful!" + "Your account has been credited by\t" + amount;
            mail.Body = "Transaction Reference id\t" + ref_id + "\n Amount\t" + amount + "Mode of transaction\t" + mode + "\n Sender Account number\t" + account_no + "Transaction time\t" + tdate + "\n Remarks\t" + remarks;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "rsvzbank@gmail.com",
                Password = "RSVZ@123"
            };
            client.EnableSsl = true;
            client.Send(mail);
        }

        // DELETE: api/Rtgstransfer/5
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult DeleteTransaction(string id)
        {
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }

            db.Transactions.Remove(transaction);
            db.SaveChanges();

            return Ok(transaction);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransactionExists(string id)
        {
            return db.Transactions.Count(e => e.ref_id == id) > 0;
        }
    }
}