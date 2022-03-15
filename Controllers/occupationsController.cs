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

        // GET: api/occupations
        /*public IQueryable<occupations> Getoccupations()
        {
            return db.occupations;
        }*/
        [ResponseType(typeof(occupations))]
        public IHttpActionResult Getoccupations()
        {
            var re = Request;
            var headers = re.Headers;

            //System.Diagnostics.Debug.WriteLine("kapott header id: " + headers.GetValues("userId").First());
            //System.Diagnostics.Debug.WriteLine("kapott header token: " + headers.GetValues("token").First());
            
            int userId = Int32.Parse(headers.GetValues("userId").First());
            
            IEnumerable<users> usersfromdb = db.users;
            IEnumerable<occupations> occfromdb = db.occupations;
                
            users user = db.users.Find(userId);
                bool authOK = false;

                if (headers.GetValues("token").First() == (user.token))
                {
                    authOK = true;
                    System.Diagnostics.Debug.WriteLine(authOK);
                }

                if (authOK)
                {
                    return Ok(occfromdb);
                }
                else return Unauthorized();
            
        }
            // GET: api/occupations/5
            [ResponseType(typeof(occupations))]
        public IHttpActionResult Getoccupations(int id)
        {
            occupations occupations = db.occupations.Find(id);
            if (occupations == null)
            {
                return NotFound();
            }

            return Ok(occupations);
        }

        // PUT: api/occupations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putoccupations(int id, occupations occupations)
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

        // POST: api/occupations
        [ResponseType(typeof(occupations))]
        public IHttpActionResult Postoccupations(occupations occupations)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.occupations.Add(occupations);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = occupations.id }, occupations);
        }

        // DELETE: api/occupations/5
        [ResponseType(typeof(occupations))]
        public IHttpActionResult Deleteoccupations(int id)
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
    }
}