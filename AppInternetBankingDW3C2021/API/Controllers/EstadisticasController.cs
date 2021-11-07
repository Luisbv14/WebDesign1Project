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
    public class EstadisticasController : ApiController
    {
        private INTERNET_BANKING_DW1_3C2021 db = new INTERNET_BANKING_DW1_3C2021();

        // GET: api/Estadisticas
        public IQueryable<Estadistica> GetEstadistica()
        {
            return db.Estadistica;
        }

        // GET: api/Estadisticas/5
        [ResponseType(typeof(Estadistica))]
        public IHttpActionResult GetEstadistica(int id)
        {
            Estadistica estadistica = db.Estadistica.Find(id);
            if (estadistica == null)
            {
                return NotFound();
            }

            return Ok(estadistica);
        }

        // PUT: api/Estadisticas/5
        [ResponseType(typeof(Estadistica))]
        public IHttpActionResult PutEstadistica(Estadistica estadistica)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(estadistica).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadisticaExists(estadistica.Codigo))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(estadistica);
        }

        // POST: api/Estadisticas
        [ResponseType(typeof(Estadistica))]
        public IHttpActionResult PostEstadistica(Estadistica estadistica)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Estadistica.Add(estadistica);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = estadistica.Codigo }, estadistica);
        }

        // DELETE: api/Estadisticas/5
        [ResponseType(typeof(Estadistica))]
        public IHttpActionResult DeleteEstadistica(int id)
        {
            Estadistica estadistica = db.Estadistica.Find(id);
            if (estadistica == null)
            {
                return NotFound();
            }

            db.Estadistica.Remove(estadistica);
            db.SaveChanges();

            return Ok(estadistica);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EstadisticaExists(int id)
        {
            return db.Estadistica.Count(e => e.Codigo == id) > 0;
        }
    }
}