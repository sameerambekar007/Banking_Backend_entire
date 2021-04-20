using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Banking.Models;

namespace Banking.Controllers
{
    public class AccountStatementController : ApiController
    {
        private BankingEntities1 db = new BankingEntities1();

        // GET: api/AccountStatement
        public IHttpActionResult GetTransactions()
        {
            return Ok(db.display_Transactions1());
        }

        // GET: api/AccountStatement/5
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

        // PUT: api/AccountStatement/5
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

        // POST: api/AccountStatement
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult PostTransaction(AccountStatement accountStatement)
        {
            //return Ok(accountStatement);
            //return Ok("in controller");
            var debit = db.transaction_between_twodates2(accountStatement.trans_date, accountStatement.to_trans_date, accountStatement.account_no);
            var credit = db.transaction_between_twodatescredited(accountStatement.trans_date, accountStatement.to_trans_date, accountStatement.account_no);
            //return Ok(debit);
            //var combine = db.transaction_between_twodates3(accountStatement.trans_date, accountStatement.to_trans_date, accountStatement.account_no);
            return Ok(new { debit, credit });
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //db.Transactions.Add(transaction);

            //try
            //{
            //    db.SaveChanges();
            //}
            //catch (DbUpdateException)
            //{
            //    if (TransactionExists(transaction.ref_id))
            //    {
            //        return Conflict();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return CreatedAtRoute("DefaultApi", new { id = transaction.ref_id }, transaction);
        }

        // DELETE: api/AccountStatement/5
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