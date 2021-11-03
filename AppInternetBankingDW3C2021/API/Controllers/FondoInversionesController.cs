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
    public class FondoInversionesController : ApiController
    {
        private INTERNET_BANKING_DW1_3C2021Entities db = new INTERNET_BANKING_DW1_3C2021Entities();

        // GET: api/FondoInversiones
        public IQueryable<FondoInversion> GetFondoInversion()
        {
            return db.FondoInversion;
        }

        // GET: api/FondoInversiones/5
        [ResponseType(typeof(FondoInversion))]
        public IHttpActionResult GetFondoInversion(int id)
        {
            FondoInversion fondoInversion = db.FondoInversion.Find(id);
            if (fondoInversion == null)
            {
                return NotFound();
            }

            return Ok(fondoInversion);
        }

        // PUT: api/FondoInversiones/5
        [ResponseType(typeof(FondoInversion))]
        public IHttpActionResult PutFondoInversion(FondoInversion fondoInversion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(fondoInversion).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FondoInversionExists(fondoInversion.CodigoInversion))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(fondoInversion);
        }

        // POST: api/FondoInversiones
        [ResponseType(typeof(FondoInversion))]
        public IHttpActionResult PostFondoInversion(FondoInversion fondoInversion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FondoInversion.Add(fondoInversion);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = fondoInversion.CodigoInversion }, fondoInversion);
        }

        // DELETE: api/FondoInversiones/5
        [ResponseType(typeof(FondoInversion))]
        public IHttpActionResult DeleteFondoInversion(int id)
        {
            FondoInversion fondoInversion = db.FondoInversion.Find(id);
            if (fondoInversion == null)
            {
                return NotFound();
            }

            db.FondoInversion.Remove(fondoInversion);
            db.SaveChanges();

            return Ok(fondoInversion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FondoInversionExists(int id)
        {
            return db.FondoInversion.Count(e => e.CodigoInversion == id) > 0;
        }
    }
}