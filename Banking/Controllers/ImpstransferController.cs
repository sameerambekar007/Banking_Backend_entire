﻿using System;
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
    public class ImpstransferController : ApiController
    {
        public Random random = new Random();
        private BankingEntities1 db = new BankingEntities1();

        // GET: api/Impstransfer
        public IHttpActionResult GetTransactions()
        {
            return Ok(db.display_Transactions());
        }

        // GET: api/Impstransfer/5
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

        // PUT: api/Impstransfer/5
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

        // POST: api/Impstransfer
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult PostTransaction(Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            transaction.ref_id = "RefIMPS" + "@" + random.Next(10000,99999).ToString();
            transaction.mode = "imps";
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

            return CreatedAtRoute("DefaultApi", new { id = transaction.ref_id }, transaction);
        }

        // DELETE: api/Impstransfer/5
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