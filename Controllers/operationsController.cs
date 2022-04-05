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
using karbantartasSzerver.Models;

namespace karbantartasSzerver.Controllers
{
    public class operationsController : ApiController
    {
        private Karbantarto01_DBEntities db = new Karbantarto01_DBEntities();

        // GET: api/operations
        public IQueryable<operations> Getoperations()
        {
            return db.operations;
        }

        // GET: api/operations/5
        [ResponseType(typeof(operations))]
        public IHttpActionResult Getoperations(int id)
        {
            operations operations = db.operations.Find(id);
            if (operations == null)
            {
                return NotFound();
            }

            return Ok(operations);
        }

        // PUT: api/operations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putoperations(int id, operations operations)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != operations.id)
            {
                return BadRequest();
            }

            db.Entry(operations).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!operationsExists(id))
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

        // POST: api/operations
        [ResponseType(typeof(operations))]
        public IHttpActionResult Postoperations(operations operations)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.operations.Add(operations);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = operations.id }, operations);
        }

        // DELETE: api/operations/5
        [ResponseType(typeof(operations))]
        public IHttpActionResult Deleteoperations(int id)
        {
            operations operations = db.operations.Find(id);
            if (operations == null)
            {
                return NotFound();
            }

            db.operations.Remove(operations);
            db.SaveChanges();

            return Ok(operations);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool operationsExists(int id)
        {
            return db.operations.Count(e => e.id == id) > 0;
        }
    }
}