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
    public class subCategoriesController : ApiController
    {
        private Karbantarto01_DBEntities db = new Karbantarto01_DBEntities();

        // GET: api/subCategories
        public IQueryable<subCategory> GetsubCategory()
        {
            return db.subCategory;
        }

        // GET: api/subCategories/5
        [ResponseType(typeof(subCategory))]
        public IHttpActionResult GetsubCategory(int id)
        {
            subCategory subCategory = db.subCategory.Find(id);
            if (subCategory == null)
            {
                return NotFound();
            }

            return Ok(subCategory);
        }

        // PUT: api/subCategories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutsubCategory(int id, subCategory subCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != subCategory.id)
            {
                return BadRequest();
            }

            db.Entry(subCategory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!subCategoryExists(id))
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

        // POST: api/subCategories
        [ResponseType(typeof(subCategory))]
        public IHttpActionResult PostsubCategory(subCategory subCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.subCategory.Add(subCategory);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = subCategory.id }, subCategory);
        }

        // DELETE: api/subCategories/5
        [ResponseType(typeof(subCategory))]
        public IHttpActionResult DeletesubCategory(int id)
        {
            subCategory subCategory = db.subCategory.Find(id);
            if (subCategory == null)
            {
                return NotFound();
            }

            db.subCategory.Remove(subCategory);
            db.SaveChanges();

            return Ok(subCategory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool subCategoryExists(int id)
        {
            return db.subCategory.Count(e => e.id == id) > 0;
        }
    }
}