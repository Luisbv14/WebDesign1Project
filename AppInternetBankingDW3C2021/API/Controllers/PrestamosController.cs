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
    public class PrestamosController : ApiController
    {
        private INTERNET_BANKING_DW1_3C2021Entities db = new INTERNET_BANKING_DW1_3C2021Entities();

        // GET: api/Prestamos
        public IQueryable<Prestamo> GetPrestamo()
        {
            return db.Prestamo;
        }

        // GET: api/Prestamos/5
        [ResponseType(typeof(Prestamo))]
        public IHttpActionResult GetPrestamo(int id)
        {
            Prestamo prestamo = db.Prestamo.Find(id);
            if (prestamo == null)
            {
                return NotFound();
            }

            return Ok(prestamo);
        }

        // PUT: api/Prestamos/5
        [ResponseType(typeof(Prestamo))]
        public IHttpActionResult PutPrestamo(Prestamo prestamo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(prestamo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrestamoExists(prestamo.CodigoPrestamo))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(prestamo);
        }

        // POST: api/Prestamos
        [ResponseType(typeof(Prestamo))]
        public IHttpActionResult PostPrestamo(Prestamo prestamo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Prestamo.Add(prestamo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = prestamo.CodigoPrestamo }, prestamo);
        }

        // DELETE: api/Prestamos/5
        [ResponseType(typeof(Prestamo))]
        public IHttpActionResult DeletePrestamo(int id)
        {
            Prestamo prestamo = db.Prestamo.Find(id);
            if (prestamo == null)
            {
                return NotFound();
            }

            db.Prestamo.Remove(prestamo);
            db.SaveChanges();

            return Ok(prestamo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PrestamoExists(int id)
        {
            return db.Prestamo.Count(e => e.CodigoPrestamo == id) > 0;
        }
    }
}