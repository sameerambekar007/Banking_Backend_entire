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
    public class AdminsLoginController : ApiController
    {
        private BankingEntities1 db = new BankingEntities1();

        [HttpPost]
        public IHttpActionResult Get([FromBody] Admin admin)
        {
            var result = db.Admins.Where(a => a.admin_id == admin.admin_id && a.admin_password == admin.admin_password).FirstOrDefault();
            if (result != null)
            {
                return Ok("Found");
            }
            else
            {
                return Ok("NOT FOUND");
            }
        }
        // GET: api/AdminsLogin
        public IQueryable<Admin> GetAdmins()
        {
            return db.Admins;
        }

        // GET: api/AdminsLogin/5
        [ResponseType(typeof(Admin))]
        public IHttpActionResult GetAdmin(string id)
        {
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return NotFound();
            }

            return Ok(admin);
        }

        // PUT: api/AdminsLogin/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAdmin(string id, Admin admin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != admin.admin_id)
            {
                return BadRequest();
            }

            db.Entry(admin).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminExists(id))
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

        // POST: api/AdminsLogin
        //[ResponseType(typeof(Admin))]
        //public IHttpActionResult PostAdmin(Admin admin)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Admins.Add(admin);

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (AdminExists(admin.admin_id))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtRoute("DefaultApi", new { id = admin.admin_id }, admin);
        //}

        // DELETE: api/AdminsLogin/5
        [ResponseType(typeof(Admin))]
        public IHttpActionResult DeleteAdmin(string id)
        {
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return NotFound();
            }

            db.Admins.Remove(admin);
            db.SaveChanges();

            return Ok(admin);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AdminExists(string id)
        {
            return db.Admins.Count(e => e.admin_id == id) > 0;
        }
    }
}