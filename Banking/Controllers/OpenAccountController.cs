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
    public class OpenAccountController : ApiController
    {
        public Random random = new Random();
        private BankingEntities1 db = new BankingEntities1();

        // GET: api/OpenAccount
        public IHttpActionResult GetCustomers()
        {
            var products = db.display_customers().ToList();
            return Ok(products);
        }

        // GET: api/OpenAccount/5
        [ResponseType(typeof(Customer))]
        public IHttpActionResult GetCustomer(decimal id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // PUT: api/OpenAccount/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCustomer(decimal id, Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.service_ref_no)
            {
                return BadRequest();
            }

            db.Entry(customer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // POST: api/OpenAccount
        [ResponseType(typeof(Customer))]
        public IHttpActionResult PostCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            customer.acct_status = "pending";
            customer.service_ref_no = random.Next(100000000);
           
            db.Customers.Add(customer);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CustomerExists(customer.service_ref_no))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = customer.service_ref_no }, customer);
        }

        // DELETE: api/OpenAccount/5
        [ResponseType(typeof(Customer))]
        public IHttpActionResult DeleteCustomer(decimal id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            db.SaveChanges();

            return Ok(customer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(decimal id)
        {
            return db.Customers.Count(e => e.service_ref_no == id) > 0;
        }
    }
}