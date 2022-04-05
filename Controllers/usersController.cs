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
    public class usersController : ApiController
    {
        private Karbantarto01_DBEntities db = new Karbantarto01_DBEntities();

        // GET: api/users
        [ResponseType(typeof(users))]
        public IHttpActionResult Getusers()
        {
            try
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
                return Ok(usersfromdb);
            }
            else return Unauthorized();
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
        }
  
        // GET: api/users/5
        [ResponseType(typeof(users))]
        public IHttpActionResult Getusers(int id)
        {
            try
            {  
                var re = Request;                                                       //System.Diagnostics.Debug.WriteLine(id);
                var headers = re.Headers;
                int userId = Int32.Parse(headers.GetValues("userId").First());             
           
            IEnumerable<users> usersfromdb = db.users;
            users user = db.users.Find(userId);
            if (headers.GetValues("token").First() == (user.token))
            {
                users users = db.users.Find(id);
                if (users == null)
                {
                    return NotFound();
                }
                return Ok(users);
            }
            else return Unauthorized();
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
}
        public JObject Getusers(string luname, string lpass)
        {
            IEnumerable<users> usersfromdb = db.users;
            JObject user = new JObject();
            for (int i = 0; i < usersfromdb.Count(); i++)
            {
                if (usersfromdb.ElementAt<users>(i).username == luname && usersfromdb.ElementAt<users>(i).password == lpass)
                {
                    var allChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    var random = new Random();
                    var resultToken = new string(
                    
                    Enumerable.Repeat(allChar, 24)
                       .Select(token => token[random.Next(token.Length)]).ToArray());
                    usersfromdb.ElementAt<users>(i).token = resultToken;        //System.Diagnostics.Debug.WriteLine(usersfromdb.ElementAt<users>(i).token);        //System.Diagnostics.Debug.WriteLine(usersfromdb.ElementAt<users>(i).id);

                    user.Add("id", usersfromdb.ElementAt<users>(i).id);
                    user.Add("username", usersfromdb.ElementAt<users>(i).username);
                    user.Add("roleId", usersfromdb.ElementAt<users>(i).roleId);
                    user.Add("fullName", usersfromdb.ElementAt<users>(i).fullName);
                    user.Add("occupationId", usersfromdb.ElementAt<users>(i).occupationId);
                    user.Add("token", usersfromdb.ElementAt<users>(i).token);
                    user.Add("password", usersfromdb.ElementAt<users>(i).password);
                                        
                    TokenToDb(usersfromdb.ElementAt<users>(i).id, usersfromdb.ElementAt<users>(i));
                }
               // else user.Add("token", "false");
            }
            return user;        //JSON megy vissza a válaszban, a fenti user felépítésben.
        }
        
       // PUT: api/users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putusers(int id, users users)
        {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != users.id)
                {
                    return BadRequest();
                }
                                
                db.Entry(users).State = EntityState.Modified;
                
                /*var re = Request;                                                       //System.Diagnostics.Debug.WriteLine(id);
                var headers = re.Headers;
                int userId = Int32.Parse(headers.GetValues("userId").First());
                bool authOk = false;
                if (headers.GetValues("token").First() == (db.users.Find(userId).token))
                {
                    authOk = true;
                    System.Diagnostics.Debug.WriteLine("azonositas: " + authOk);
                }
                //else return Unauthorized();
                if (authOk)
                {*/
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!usersExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return StatusCode(HttpStatusCode.NoContent);
            /*}
            else return Unauthorized();*/
        }
            
           

        public IHttpActionResult TokenToDb(int id, users users)
        {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != users.id)
                {
                    return BadRequest();
                }

                db.Entry(users).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!usersExists(id))
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

        // POST: api/users
        [ResponseType(typeof(users))]
        public IHttpActionResult Postusers(users users)
        {
            try
            {
            var re = Request;                                                       //System.Diagnostics.Debug.WriteLine(id);
            var headers = re.Headers;
            int userId = Int32.Parse(headers.GetValues("userId").First());
            bool authOk = false;
            if (headers.GetValues("token").First() == (db.users.Find(userId).token))
            {
                authOk = true;
                System.Diagnostics.Debug.WriteLine("azonositas: " + authOk);
            }
            if (authOk)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.users.Add(users);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = users.id }, users);
            }
            else return Unauthorized();
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
        }

        // DELETE: api/users/5
        [ResponseType(typeof(users))]
        public IHttpActionResult Deleteusers(int id)
        {
            /*try
            {
                var re = Request;                                                       //System.Diagnostics.Debug.WriteLine(id);
                var headers = re.Headers;
                int userId = Int32.Parse(headers.GetValues("userId").First());
                bool authOk = false;
                if (headers.GetValues("token").First() == (db.users.Find(userId).token))
                {
                    authOk = true;
                    System.Diagnostics.Debug.WriteLine("azonositas: " + authOk);
                }
                if (authOk)
                {*/
                    //System.Diagnostics.Debug.WriteLine("Az auth ok");
                    users users = db.users.Find(id);
                    
                    if (users == null)
                    {
                        return NotFound();
                    }

                    db.users.Remove(users);
                    //System.Diagnostics.Debug.WriteLine("törlés");
                    db.SaveChanges();

                    return Ok(users);
                /*}
                else return Unauthorized();
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }*/
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool usersExists(int id)
        {
            return db.users.Count(e => e.id == id) > 0;
        }

       /* // PUT: api/users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putusers3(int id, users users)
        {
            var re = Request;                                                       //System.Diagnostics.Debug.WriteLine(id);
            var headers = re.Headers;
            int userId = Int32.Parse(headers.GetValues("userId").First());
            IEnumerable<users> usersfromdb = db.users;
            users user = db.users.Find(userId);
            bool logOut = Boolean.Parse(headers.GetValues("logout").First());

            if (headers.GetValues("token").First() == (user.token) && !logOut)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != users.id)
                {
                    return BadRequest();
                }

                db.Entry(users).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!usersExists(id))
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

            if (headers.GetValues("token").First() == (user.token) && logOut)
            {
                user.token = null;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != users.id)
                {
                    return BadRequest();
                }

                db.Entry(users).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!usersExists(id))
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
        }*/
    }
}