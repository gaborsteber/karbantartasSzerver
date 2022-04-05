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
        public IQueryable<mainCategory> GetmainCategory()
        {
            return db.mainCategory;
        }

        // GET: api/mainCategories/5
        [ResponseType(typeof(mainCategory))]
        public IHttpActionResult GetmainCategory(int id)
        {
            mainCategory mainCategory = db.mainCategory.Find(id);
            if (mainCategory == null)
            {
                return NotFound();
            }

            return Ok(mainCategory);
        }

        // PUT: api/mainCategories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutmainCategory(int id, mainCategory mainCategory)
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

        // POST: api/mainCategories
        [ResponseType(typeof(mainCategory))]
        public IHttpActionResult PostmainCategory(mainCategory mainCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.mainCategory.Add(mainCategory);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = mainCategory.id }, mainCategory);
        }

        // DELETE: api/mainCategories/5
        [ResponseType(typeof(mainCategory))]
        public IHttpActionResult DeletemainCategory(int id)
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
    }
}