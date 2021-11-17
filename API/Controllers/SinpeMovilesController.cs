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
    [Authorize]
    public class SinpeMovilesController : ApiController
    {
        private INTERNET_BANKING_DW1_3C2021 db = new INTERNET_BANKING_DW1_3C2021();

        // GET: api/SinpeMoviles
        public IQueryable<SinpeMovil> GetSinpeMovil()
        {
            return db.SinpeMovil;
        }

        // GET: api/SinpeMoviles/5
        [ResponseType(typeof(SinpeMovil))]
        public IHttpActionResult GetSinpeMovil(int id)
        {
            SinpeMovil sinpeMovil = db.SinpeMovil.Find(id);
            if (sinpeMovil == null)
            {
                return NotFound();
            }

            return Ok(sinpeMovil);
        }

        // PUT: api/SinpeMoviles/5
        [ResponseType(typeof(SinpeMovil))]
        public IHttpActionResult PutSinpeMovil(SinpeMovil sinpeMovil)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(sinpeMovil).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SinpeMovilExists(sinpeMovil.CodigoSinpe))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(sinpeMovil);
        }

        // POST: api/SinpeMoviles
        [ResponseType(typeof(SinpeMovil))]
        public IHttpActionResult PostSinpeMovil(SinpeMovil sinpeMovil)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SinpeMovil.Add(sinpeMovil);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = sinpeMovil.CodigoSinpe }, sinpeMovil);
        }

        // DELETE: api/SinpeMoviles/5
        [ResponseType(typeof(SinpeMovil))]
        public IHttpActionResult DeleteSinpeMovil(int id)
        {
            SinpeMovil sinpeMovil = db.SinpeMovil.Find(id);
            if (sinpeMovil == null)
            {
                return NotFound();
            }

            db.SinpeMovil.Remove(sinpeMovil);
            db.SaveChanges();

            return Ok(sinpeMovil);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SinpeMovilExists(int id)
        {
            return db.SinpeMovil.Count(e => e.CodigoSinpe == id) > 0;
        }
    }
}