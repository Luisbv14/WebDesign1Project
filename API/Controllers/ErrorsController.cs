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
using API.Models;

namespace API.Controllers
{
    [AllowAnonymous]
    public class ErrorsController : ApiController
    {
        private INTERNET_BANKING_DW1_3C2021 db = new INTERNET_BANKING_DW1_3C2021();

        // GET: api/Errors
        public IQueryable<Error> GetError()
        {
            return db.Error;
        }

        // GET: api/Errors/5
        [ResponseType(typeof(Error))]
        public IHttpActionResult GetError(int id)
        {
            Error error = db.Error.Find(id);
            if (error == null)
            {
                return NotFound();
            }

            return Ok(error);
        }

        // PUT: api/Errors/5
        [ResponseType(typeof(Error))]
        public IHttpActionResult PutError(Error error)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(error).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ErrorExists(error.Codigo))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(error);
        }

        // POST: api/Errors
        [ResponseType(typeof(Error))]
        public IHttpActionResult PostError(Error error)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Error.Add(error);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = error.Codigo }, error);
        }

        // DELETE: api/Errors/5
        [ResponseType(typeof(Error))]
        public IHttpActionResult DeleteError(int id)
        {
            Error error = db.Error.Find(id);
            if (error == null)
            {
                return NotFound();
            }

            db.Error.Remove(error);
            db.SaveChanges();

            return Ok(error);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ErrorExists(int id)
        {
            return db.Error.Count(e => e.Codigo == id) > 0;
        }
    }
}