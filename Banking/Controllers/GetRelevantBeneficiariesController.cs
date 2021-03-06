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
    public class GetRelevantBeneficiariesController : ApiController
    {
        private BankingEntities1 db = new BankingEntities1();

        // GET: api/GetRelevantBeneficiaries
        public IHttpActionResult GetBeneficiaries()
        {
            return Ok(db.Beneficiaries);
        }

        // GET: api/GetRelevantBeneficiaries/5
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

        // PUT: api/GetRelevantBeneficiaries/5
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

        // POST: api/GetRelevantBeneficiaries
        [ResponseType(typeof(Beneficiary))]
        public IHttpActionResult PostBeneficiary(Beneficiary beneficiary)
        {
            var beneficiarylist = db.fetch_beneficiaries(beneficiary.account_no);
            return Ok(beneficiarylist);
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //db.Beneficiaries.Add(beneficiary);

            //try
            //{
            //    db.SaveChanges();
            //}
            //catch (DbUpdateException)
            //{
            //    if (BeneficiaryExists(beneficiary.ben_id))
            //    {
            //        return Conflict();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return CreatedAtRoute("DefaultApi", new { id = beneficiary.ben_id }, beneficiary);
        }

        // DELETE: api/GetRelevantBeneficiaries/5
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