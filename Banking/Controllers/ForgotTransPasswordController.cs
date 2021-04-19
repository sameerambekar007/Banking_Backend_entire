﻿using System;
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
    public class ForgotTransPasswordController : ApiController
    {
        public Random random = new Random();
        private BankingEntities1 db = new BankingEntities1();

        // GET: api/ForgotTransPassword
        public IQueryable<Account_Holder> GetAccount_Holder()
        {
            return db.Account_Holder;
        }

        // GET: api/ForgotTransPassword/5
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

        // PUT: api/ForgotTransPassword/5
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

        // POST: api/ForgotTransPassword
        [ResponseType(typeof(Account_Holder))]
        public IHttpActionResult PostAccount_Holder(ForgotTranspassword forgotTransPassword)
        {
            if (forgotTransPassword.email_id == db.fetchemail(forgotTransPassword.account_no).FirstOrDefault())
            {
                int onetimepassword = random.Next(10000, 99999);
                db.insertotp(onetimepassword);
                SendOtpforTranspasssword(forgotTransPassword.email_id, onetimepassword);
                return Ok("otp sent");
            }
            else
            {
                return Ok("not matched");
            }
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //db.Account_Holder.Add(account_Holder);

            //try
            //{
            //    db.SaveChanges();
            //}
            //catch (DbUpdateException)
            //{
            //    if (Account_HolderExists(account_Holder.account_no))
            //    {
            //        return Conflict();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            // return CreatedAtRoute("DefaultApi", new { id = account_Holder.account_no }, account_Holder);
        }
        [Route("Api/sendTranspassword")]
        [ResponseType(typeof(Account_Holder))]
        public IHttpActionResult PostAccount_Holder1(ForgotTranspassword forgotTransPassword)
        {
            decimal? a = db.fetchtotp().FirstOrDefault();
            if (forgotTransPassword.otp == a)
            {
                SendTranspassword(forgotTransPassword.email_id, db.fetchtranspassword(forgotTransPassword.account_no).FirstOrDefault());
                return Ok("success");
            }
            else
            {
                return Ok("fail");
            }
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //db.Account_Holder.Add(account_Holder);

            //try
            //{
            //    db.SaveChanges();
            //}
            //catch (DbUpdateException)
            //{
            //    if (Account_HolderExists(account_Holder.account_no))
            //    {
            //        return Conflict();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return CreatedAtRoute("DefaultApi", new { id = account_Holder.account_no }, account_Holder);
        }
        public void SendOtpforTranspasssword(string To, decimal otp)
        {

            MailMessage mail = new MailMessage("rsvzbank@gmail.com", To);
            mail.Subject = "OTP for verification";
            mail.Body = "Otp:\t" + otp;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "rsvzbank@gmail.com",
                Password = "RSVZ@123"
            };
            client.EnableSsl = true;
            client.Send(mail);
        }
        public void SendTranspassword(string To, decimal? transpassword)
        {

            MailMessage mail = new MailMessage("rsvzbank@gmail.com", To);
            mail.Subject = "OTP for verification";
            mail.Body = "Transaction Password:\t" + transpassword;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "rsvzbank@gmail.com",
                Password = "RSVZ@123"
            };
            client.EnableSsl = true;
            client.Send(mail);
        }


        // DELETE: api/ForgotTransPassword/5
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