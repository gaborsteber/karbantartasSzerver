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
    public class assetsController : ApiController
    {
        private Karbantarto01_DBEntities db = new Karbantarto01_DBEntities();


        [ResponseType(typeof(assets))]
        public IHttpActionResult Getassets()
        {
            bool authStatus = validateUser();
            if (authStatus)
            {
               return Ok(db.assets);
            }
            else return Unauthorized();
        }

        // GET: api/assets/5
        [ResponseType(typeof(assets))]
        public IHttpActionResult Getassets(int id)
        {
            bool authStatus = validateUser();
            if (authStatus)
            {
                assets assets = db.assets.Find(id);
                if (assets == null)
                {
                    return NotFound();
                }
                return Ok(assets);
            }
            else return Unauthorized();
        }

        // PUT: api/assets/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putassets(int id, assets assets)
        {
            bool authStatus = validateUser();
            if (authStatus)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != assets.id)
                {
                    return BadRequest();
                }

                db.Entry(assets).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!assetsExists(id))
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

        // POST: api/assets
        [ResponseType(typeof(assets))]
        public IHttpActionResult Postassets(assets assets)
        {
            bool authStatus = validateUser();
            if (authStatus)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.assets.Add(assets);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = assets.id }, assets);
            }
            else return Unauthorized();
        }

        // DELETE: api/assets/5
        [ResponseType(typeof(assets))]
        public IHttpActionResult Deleteassets(int id)
        {
            bool authStatus = validateUser();
            if (authStatus)
            {
                assets assets = db.assets.Find(id);
                if (assets == null)
                {
                    return NotFound();
                }

                db.assets.Remove(assets);
                db.SaveChanges();

                return Ok(assets);
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

        private bool assetsExists(int id)
        {
            return db.assets.Count(e => e.id == id) > 0;
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