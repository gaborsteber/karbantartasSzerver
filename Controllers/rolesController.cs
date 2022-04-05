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
    public class rolesController : ApiController
    {
        private Karbantarto01_DBEntities db = new Karbantarto01_DBEntities();

        // GET: api/roles
        [ResponseType(typeof(roles))]
        public IHttpActionResult Getroles()
        {
            bool authStatus = validateUser();
            if (authStatus)
            {
                return Ok(db.roles);
            }
            else return Unauthorized();
        }

        // GET: api/roles/5
        [ResponseType(typeof(roles))]
        public IHttpActionResult Getroles(int id)
        {
            bool authStatus = validateUser();
            if (authStatus)
            {
                roles roles = db.roles.Find(id);
                if (roles == null)
                {
                    return NotFound();
                }

                return Ok(roles);
            }
            else return Unauthorized();
        }

        // PUT: api/roles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putroles(int id, roles roles)
        {
            bool authStatus = validateUser();
            if (authStatus)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != roles.id)
                {
                    return BadRequest();
                }

                db.Entry(roles).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!rolesExists(id))
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

        // POST: api/roles
        [ResponseType(typeof(roles))]
        public IHttpActionResult Postroles(roles roles)
        {
            bool authStatus = validateUser();
            if (authStatus)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.roles.Add(roles);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = roles.id }, roles);
            }
            else return Unauthorized();
        }

        // DELETE: api/roles/5
        [ResponseType(typeof(roles))]
        public IHttpActionResult Deleteroles(int id)
        {
            bool authStatus = validateUser();
            if (authStatus)
            {
                roles roles = db.roles.Find(id);
                if (roles == null)
                {
                    return NotFound();
                }

                db.roles.Remove(roles);
                db.SaveChanges();

                return Ok(roles);
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

        private bool rolesExists(int id)
        {
            return db.roles.Count(e => e.id == id) > 0;
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