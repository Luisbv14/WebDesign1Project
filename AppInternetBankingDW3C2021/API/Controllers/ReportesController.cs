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
    public class ReportesController : ApiController
    {
        private INTERNET_BANKING_DW1_3C2021Entities db = new INTERNET_BANKING_DW1_3C2021Entities();

        // GET: api/Reportes
        public IQueryable<Reporte> GetReporte()
        {
            return db.Reporte;
        }

        // GET: api/Reportes/5
        [ResponseType(typeof(Reporte))]
        public IHttpActionResult GetReporte(int id)
        {
            Reporte reporte = db.Reporte.Find(id);
            if (reporte == null)
            {
                return NotFound();
            }

            return Ok(reporte);
        }

        // PUT: api/Reportes/5
        [ResponseType(typeof(Reporte))]
        public IHttpActionResult PutReporte(Reporte reporte)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(reporte).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReporteExists(reporte.CodigoReporte))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(reporte);
        }

        // POST: api/Reportes
        [ResponseType(typeof(Reporte))]
        public IHttpActionResult PostReporte(Reporte reporte)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Reporte.Add(reporte);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = reporte.CodigoReporte }, reporte);
        }

        // DELETE: api/Reportes/5
        [ResponseType(typeof(Reporte))]
        public IHttpActionResult DeleteReporte(int id)
        {
            Reporte reporte = db.Reporte.Find(id);
            if (reporte == null)
            {
                return NotFound();
            }

            db.Reporte.Remove(reporte);
            db.SaveChanges();

            return Ok(reporte);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReporteExists(int id)
        {
            return db.Reporte.Count(e => e.CodigoReporte == id) > 0;
        }
    }
}