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
    public class SolicitudTarjetasController : ApiController
    {
        private INTERNET_BANKING_DW1_3C2021Entities db = new INTERNET_BANKING_DW1_3C2021Entities();

        // GET: api/SolicitudTarjetas
        public IQueryable<SolicitudTarjeta> GetSolicitudTarjeta()
        {
            return db.SolicitudTarjeta;
        }

        // GET: api/SolicitudTarjetas/5
        [ResponseType(typeof(SolicitudTarjeta))]
        public IHttpActionResult GetSolicitudTarjeta(int id)
        {
            SolicitudTarjeta solicitudTarjeta = db.SolicitudTarjeta.Find(id);
            if (solicitudTarjeta == null)
            {
                return NotFound();
            }

            return Ok(solicitudTarjeta);
        }

        // PUT: api/SolicitudTarjetas/5
        [ResponseType(typeof(SolicitudTarjeta))]
        public IHttpActionResult PutSolicitudTarjeta(SolicitudTarjeta solicitudTarjeta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(solicitudTarjeta).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolicitudTarjetaExists(solicitudTarjeta.CodigoSolicitud))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(solicitudTarjeta);
        }

        // POST: api/SolicitudTarjetas
        [ResponseType(typeof(SolicitudTarjeta))]
        public IHttpActionResult PostSolicitudTarjeta(SolicitudTarjeta solicitudTarjeta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SolicitudTarjeta.Add(solicitudTarjeta);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = solicitudTarjeta.CodigoSolicitud }, solicitudTarjeta);
        }

        // DELETE: api/SolicitudTarjetas/5
        [ResponseType(typeof(SolicitudTarjeta))]
        public IHttpActionResult DeleteSolicitudTarjeta(int id)
        {
            SolicitudTarjeta solicitudTarjeta = db.SolicitudTarjeta.Find(id);
            if (solicitudTarjeta == null)
            {
                return NotFound();
            }

            db.SolicitudTarjeta.Remove(solicitudTarjeta);
            db.SaveChanges();

            return Ok(solicitudTarjeta);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SolicitudTarjetaExists(int id)
        {
            return db.SolicitudTarjeta.Count(e => e.CodigoSolicitud == id) > 0;
        }
    }
}