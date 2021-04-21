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
    public class DeclineAccountController : ApiController
    {
        private BankingEntities1 db = new BankingEntities1();

        // GET: api/DeclineAccount
        public IQueryable<Customer> GetCustomers()
        {
            return db.Customers;
        }

        // GET: api/DeclineAccount/5
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

        // PUT: api/DeclineAccount/5
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

        // POST: api/DeclineAccount
        [ResponseType(typeof(Customer))]
        public IHttpActionResult PostCustomer(Customer customer)
        {
            Senddeclined(customer.email_id, customer.service_ref_no);
            db.decline_account(customer.service_ref_no);
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //db.Customers.Add(customer);

            //try
            //{
            //    db.SaveChanges();
            //}
            //catch (DbUpdateException)
            //{
            //    if (CustomerExists(customer.service_ref_no))
            //    {
            //        return Conflict();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return CreatedAtRoute("DefaultApi", new { id = customer.service_ref_no }, customer);
        }
        public void Senddeclined(string To, decimal? service_no)
        {

            MailMessage mail = new MailMessage("rsvzbank@gmail.com", To);
            mail.Subject = "Your account has been declined";
            mail.Body = "Your account opening request with RSVZ bank with\n Service Reference number:\t" + service_no + "\n has been denied.\n Please contact rsvz@gmail.com for further queries";
            //mail.Attachments.Add(new Attachment("C:/Users/devil/Downloads/2.png"));
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "rsvzbank@gmail.com",
                Password = "RSVZ@123"
            };
            client.EnableSsl = true;
            client.Send(mail);
        }

        // DELETE: api/DeclineAccount/5
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