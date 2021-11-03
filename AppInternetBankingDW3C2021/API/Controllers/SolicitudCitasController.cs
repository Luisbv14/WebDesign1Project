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
    public class SolicitudCitasController : ApiController
    {
        private INTERNET_BANKING_DW1_3C2021Entities db = new INTERNET_BANKING_DW1_3C2021Entities();

        // GET: api/SolicitudCitas
        public IQueryable<SolicitudCita> GetSolicitudCita()
        {
            return db.SolicitudCita;
        }

        // GET: api/SolicitudCitas/5
        [ResponseType(typeof(SolicitudCita))]
        public IHttpActionResult GetSolicitudCita(int id)
        {
            SolicitudCita solicitudCita = db.SolicitudCita.Find(id);
            if (solicitudCita == null)
            {
                return NotFound();
            }

            return Ok(solicitudCita);
        }

        // PUT: api/SolicitudCitas/5
        [ResponseType(typeof(SolicitudCita))]
        public IHttpActionResult PutSolicitudCita(SolicitudCita solicitudCita)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(solicitudCita).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolicitudCitaExists(solicitudCita.CodigoCita))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(solicitudCita);
        }

        // POST: api/SolicitudCitas
        [ResponseType(typeof(SolicitudCita))]
        public IHttpActionResult PostSolicitudCita(SolicitudCita solicitudCita)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SolicitudCita.Add(solicitudCita);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = solicitudCita.CodigoCita }, solicitudCita);
        }

        // DELETE: api/SolicitudCitas/5
        [ResponseType(typeof(SolicitudCita))]
        public IHttpActionResult DeleteSolicitudCita(int id)
        {
            SolicitudCita solicitudCita = db.SolicitudCita.Find(id);
            if (solicitudCita == null)
            {
                return NotFound();
            }

            db.SolicitudCita.Remove(solicitudCita);
            db.SaveChanges();

            return Ok(solicitudCita);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SolicitudCitaExists(int id)
        {
            return db.SolicitudCita.Count(e => e.CodigoCita == id) > 0;
        }
    }
}