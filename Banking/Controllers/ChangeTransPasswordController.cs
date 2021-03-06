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
    public class ChangeTransPasswordController : ApiController
    {
        private BankingEntities1 db = new BankingEntities1();

        // GET: api/ChangeTransPassword
        public IQueryable<Account_Holder> GetAccount_Holder()
        {
            return db.Account_Holder;
        }

        // GET: api/ChangeTransPassword/5
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

        // PUT: api/ChangeTransPassword/5
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

        // POST: api/ChangeTransPassword
        [ResponseType(typeof(Account_Holder))]
        public IHttpActionResult PostAccount_Holder(ChangeTransPassword changeTransPassword)
        {
            var result = db.Account_Holder.Where(a => a.account_no == changeTransPassword.account_no && a.trans_pass == changeTransPassword.trans_pass).FirstOrDefault();

            if (result != null)
            {
                db.change_trans_password(changeTransPassword.account_no, changeTransPassword.new_trans_pass);
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

        // DELETE: api/ChangeTransPassword/5
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