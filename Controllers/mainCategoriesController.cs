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
    public class mainCategoriesController : ApiController
    {
        private Karbantarto01_DBEntities db = new Karbantarto01_DBEntities();

        // GET: api/mainCategories
        [ResponseType(typeof(mainCategory))]
        public IHttpActionResult GetmainCategory()
        {
            bool authStatus = validateUser();
            if (authStatus)
            {
                return Ok(db.mainCategory);
            }
            else return Unauthorized();
        }

        // GET: api/mainCategories/5
        [ResponseType(typeof(mainCategory))]
        public IHttpActionResult GetmainCategory(int id)
        {
            bool authStatus = validateUser();
            if (authStatus)
            {
                mainCategory mainCategory = db.mainCategory.Find(id);
                if (mainCategory == null)
                {
                    return NotFound();
                }

                return Ok(mainCategory);
            }
            else return Unauthorized();
        }

        // PUT: api/mainCategories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutmainCategory(int id, mainCategory mainCategory)
        {
            bool authStatus = validateUser();
            if (authStatus)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != mainCategory.id)
                {
                    return BadRequest();
                }

                db.Entry(mainCategory).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!mainCategoryExists(id))
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

        // POST: api/mainCategories
        [ResponseType(typeof(mainCategory))]
        public IHttpActionResult PostmainCategory(mainCategory mainCategory)
        {
            bool authStatus = validateUser();
            if (authStatus)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.mainCategory.Add(mainCategory);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = mainCategory.id }, mainCategory);
            }
            else return Unauthorized();
        }

        // DELETE: api/mainCategories/5
        [ResponseType(typeof(mainCategory))]
        public IHttpActionResult DeletemainCategory(int id)
        {
            bool authStatus = validateUser();
            if (authStatus)
            {
                mainCategory mainCategory = db.mainCategory.Find(id);
                if (mainCategory == null)
                {
                    return NotFound();
                }

                db.mainCategory.Remove(mainCategory);
                db.SaveChanges();

                return Ok(mainCategory);
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

        private bool mainCategoryExists(int id)
        {
            return db.mainCategory.Count(e => e.id == id) > 0;
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