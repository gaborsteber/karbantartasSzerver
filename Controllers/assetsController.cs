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

        // GET: api/assets
       /* public IQueryable<assets> Getassets()
        {
            return db.assets;
        }*/

        [ResponseType(typeof(assets))]
        public IHttpActionResult Getassets()
        {
            var re = Request;
            var headers = re.Headers;
            int userId = Int32.Parse(headers.GetValues("userId").First());      //System.Diagnostics.Debug.WriteLine(usersfromdb.ElementAt<users>(i).id); System.Diagnostics.Debug.WriteLine(usersfromdb.ElementAt<users>(i).token);
            IEnumerable<users> usersfromdb = db.users;
            users user = db.users.Find(userId);
            bool authOK = false;

            if (headers.GetValues("token").First() == (user.token))
            {
                authOK = true;      //System.Diagnostics.Debug.WriteLine(authOK);
            }

            if (authOK)
            {
                return Ok(db.assets);
            }
            else return Unauthorized();
        }

        // GET: api/assets/5
        [ResponseType(typeof(assets))]
        public IHttpActionResult Getassets(int id)
        {
            var re = Request;
            var headers = re.Headers;
            int userId = Int32.Parse(headers.GetValues("userId").First());      //System.Diagnostics.Debug.WriteLine(usersfromdb.ElementAt<users>(i).id); System.Diagnostics.Debug.WriteLine(usersfromdb.ElementAt<users>(i).token);
            IEnumerable<users> usersfromdb = db.users;
            users user = db.users.Find(userId);
            bool authOK = false;

            if (headers.GetValues("token").First() == (user.token))
            {
                authOK = true;      //System.Diagnostics.Debug.WriteLine(authOK);
            }

            if (authOK)
            {
                assets assets = db.assets.Find(id);
                if (assets == null)
                {
                    return NotFound();
                }

                return Ok(assets);
            }
            return Unauthorized();
        }

        // PUT: api/assets/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putassets(int id, assets assets)
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

        // POST: api/assets
        [ResponseType(typeof(assets))]
        public IHttpActionResult Postassets(assets assets)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.assets.Add(assets);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = assets.id }, assets);
        }

        // DELETE: api/assets/5
        [ResponseType(typeof(assets))]
        public IHttpActionResult Deleteassets(int id)
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
    }
}