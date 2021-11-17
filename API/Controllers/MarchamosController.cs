﻿using System;
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
    public class MarchamosController : ApiController
    {
        private INTERNET_BANKING_DW1_3C2021 db = new INTERNET_BANKING_DW1_3C2021();

        // GET: api/Marchamos
        public IQueryable<Marchamo> GetMarchamo()
        {
            return db.Marchamo;
        }

        // GET: api/Marchamos/5
        [ResponseType(typeof(Marchamo))]
        public IHttpActionResult GetMarchamo(int id)
        {
            Marchamo marchamo = db.Marchamo.Find(id);
            if (marchamo == null)
            {
                return NotFound();
            }

            return Ok(marchamo);
        }

        // PUT: api/Marchamos/5
        [ResponseType(typeof(Marchamo))]
        public IHttpActionResult PutMarchamo(Marchamo marchamo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(marchamo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MarchamoExists(marchamo.CodigoPago))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(marchamo);
        }

        // POST: api/Marchamos
        [ResponseType(typeof(Marchamo))]
        public IHttpActionResult PostMarchamo(Marchamo marchamo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Marchamo.Add(marchamo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = marchamo.CodigoPago }, marchamo);
        }

        // DELETE: api/Marchamos/5
        [ResponseType(typeof(Marchamo))]
        public IHttpActionResult DeleteMarchamo(int id)
        {
            Marchamo marchamo = db.Marchamo.Find(id);
            if (marchamo == null)
            {
                return NotFound();
            }

            db.Marchamo.Remove(marchamo);
            db.SaveChanges();

            return Ok(marchamo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MarchamoExists(int id)
        {
            return db.Marchamo.Count(e => e.CodigoPago == id) > 0;
        }
    }
}