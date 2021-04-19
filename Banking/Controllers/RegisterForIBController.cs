using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Banking.Models;

namespace Banking.Controllers
{
    public class RegisterForIBController : ApiController
    {
        private BankingEntities1 db = new BankingEntities1();

        // GET: api/RegisterForIB
        public IQueryable<Account_Holder> GetAccount_Holder()
        {
            return db.Account_Holder;
        }

        // GET: api/RegisterForIB/5
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

        // PUT: api/RegisterForIB/5
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

        // POST: api/RegisterForIB
        [ResponseType(typeof(Account_Holder))]
        public IHttpActionResult PostAccount_Holder(RegisterForIB registerForIB)
        {
            var validate = db.Account_Holder.Where(a => a.account_no == registerForIB.account_no && a.customer_id == registerForIB.customer_id && a.login_pass ==null).FirstOrDefault();
            var exists = db.Account_Holder.Where(a => a.account_no == registerForIB.account_no && a.customer_id == registerForIB.customer_id && a.login_pass != null).FirstOrDefault();
            var invalid = db.Account_Holder.Where(a => a.account_no == registerForIB.account_no && a.customer_id == registerForIB.customer_id).FirstOrDefault();
            if (validate!=null)
            {
                db.registerforib(registerForIB.customer_id, registerForIB.login_pass, registerForIB.trans_pass);
                return Ok("success");
            }
           if(exists!=null)
            {
                return Ok("exists");
            }
           if(invalid==null)
            {
                return Ok("invalid");
            }
            return Ok();
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

        // DELETE: api/RegisterForIB/5
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