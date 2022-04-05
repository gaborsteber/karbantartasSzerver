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
using Newtonsoft.Json.Linq;

namespace karbantartasSzerver.Controllers
{
    public class occupationsController : ApiController
    {
        private Karbantarto01_DBEntities db = new Karbantarto01_DBEntities();


        [ResponseType(typeof(occupations))]
        public IHttpActionResult Getoccupations()
        {
            bool authStatus = validateUser();
            if (authStatus)
            {
                return Ok(db.occupations);
            }
            else return Unauthorized();
            
        }
        
        // GET: api/occupations/5
        [ResponseType(typeof(occupations))]
        public IHttpActionResult Getoccupations(int id)
        {
            bool authStatus = validateUser();
            if (authStatus)
            {
                occupations occupations = db.occupations.Find(id);
                if (occupations == null)
                {
                    return NotFound();
                }

                return Ok(occupations);
            }
            else return Unauthorized();
        }

        // PUT: api/occupations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putoccupations(int id, occupations occupations)
        {
            bool authStatus = validateUser();
            if (authStatus)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != occupations.id)
                {
                    return BadRequest();
                }

                db.Entry(occupations).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!occupationsExists(id))
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

        // POST: api/occupations
        [ResponseType(typeof(occupations))]
        public IHttpActionResult Postoccupations(occupations occupations)
        {
            bool authStatus = validateUser();
            if (authStatus)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.occupations.Add(occupations);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = occupations.id }, occupations);
            }
            else return Unauthorized();
        }

        // DELETE: api/occupations/5
        [ResponseType(typeof(occupations))]
        public IHttpActionResult Deleteoccupations(int id)
        {
            bool authStatus = validateUser();
            if (authStatus)
            {
                occupations occupations = db.occupations.Find(id);
                if (occupations == null)
                {
                    return NotFound();
                }

                db.occupations.Remove(occupations);
                db.SaveChanges();

                return Ok(occupations);
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

        private bool occupationsExists(int id)
        {
            return db.occupations.Count(e => e.id == id) > 0;
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