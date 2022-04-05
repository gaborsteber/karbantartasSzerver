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
        [ResponseType(typeof(operations))]
        public IHttpActionResult Getoperations()
        {
            bool authStatus = validateUser();
            if (authStatus)
            {
                return Ok(db.operations);
            }
            else return Unauthorized();
        }

        // GET: api/operations/5
        [ResponseType(typeof(operations))]
        public IHttpActionResult Getoperations(int id)
        {
            bool authStatus = validateUser();
            if (authStatus)
            {
                operations operations = db.operations.Find(id);
                if (operations == null)
                {
                    return NotFound();
                }

                return Ok(operations);
            }
            else return Unauthorized();
        }

        // PUT: api/operations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putoperations(int id, operations operations)
        {
            bool authStatus = validateUser();
            if (authStatus)
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
            else return Unauthorized();
        }

        // POST: api/operations
        [ResponseType(typeof(operations))]
        public IHttpActionResult Postoperations(operations operations)
        {
            bool authStatus = validateUser();
            if (authStatus)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.operations.Add(operations);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = operations.id }, operations);
            }
            else return Unauthorized();
        }

        // DELETE: api/operations/5
        [ResponseType(typeof(operations))]
        public IHttpActionResult Deleteoperations(int id)
        {
            bool authStatus = validateUser();
            if (authStatus)
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
            else return Unauthorized();
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

        private bool validateUser()
        {
            bool authStatus = false;
            var re = Request;                                                       //System.Diagnostics.Debug.WriteLine(id);
            var headers = re.Headers;
            int userId = Int32.Parse(headers.GetValues("userId").First());
            System.Diagnostics.Debug.WriteLine("ez a header: " + headers.GetValues("token").First());
            IEnumerable<users> usersfromdb = db.users;
            users user = db.users.Find(userId);
            if (headers.GetValues("token").First() == (user.token))
            {
                authStatus = true;
            }
            return authStatus;
        }

    }
}