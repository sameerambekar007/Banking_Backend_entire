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
    public class AddBeneficiariesController : ApiController
    {
        public Random random = new Random();
        private BankingEntities1 db = new BankingEntities1();

        // GET: api/AddBeneficiaries
        public IHttpActionResult GetBeneficiaries()
        {
            return Ok(db.display_Beneficiary());
        }

        // GET: api/AddBeneficiaries/5
        [ResponseType(typeof(Beneficiary))]
        public IHttpActionResult GetBeneficiary(decimal id)
        {
            Beneficiary beneficiary = db.Beneficiaries.Find(id);
            if (beneficiary == null)
            {
                return NotFound();
            }

            return Ok(beneficiary);
        }

        // PUT: api/AddBeneficiaries/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBeneficiary(decimal id, Beneficiary beneficiary)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != beneficiary.ben_id)
            {
                return BadRequest();
            }

            db.Entry(beneficiary).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeneficiaryExists(id))
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

        // POST: api/AddBeneficiaries
        [ResponseType(typeof(Beneficiary))]
        public IHttpActionResult PostBeneficiary(Beneficiary beneficiary)
        {
            var exists = db.Beneficiaries.Where(a => a.account_no == beneficiary.account_no && a.ben_account_no == beneficiary.ben_account_no).FirstOrDefault();
            if(exists!=null)
            {
                return Ok("exists");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            beneficiary.ben_id = random.Next(100000);
            db.Beneficiaries.Add(beneficiary);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (BeneficiaryExists(beneficiary.ben_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = beneficiary.ben_id }, beneficiary);
        }

        // DELETE: api/AddBeneficiaries/5
        [ResponseType(typeof(Beneficiary))]
        public IHttpActionResult DeleteBeneficiary(decimal id)
        {
            Beneficiary beneficiary = db.Beneficiaries.Find(id);
            if (beneficiary == null)
            {
                return NotFound();
            }

            db.Beneficiaries.Remove(beneficiary);
            db.SaveChanges();

            return Ok(beneficiary);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BeneficiaryExists(decimal id)
        {
            return db.Beneficiaries.Count(e => e.ben_id == id) > 0;
        }
    }
}