using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Model;
using ORM_GSB;

namespace backend_GSB.Controllers
{
    public class MedecinsController : ApiController
    {
        private GSB_Data_Model db = new GSB_Data_Model();

        // GET: api/Medecins
        public IQueryable<Medecin> GetMedecin()
        {
            return db.Medecin;
        }

        // GET: api/Medecins/5
        [ResponseType(typeof(Medecin))]
        public async Task<IHttpActionResult> GetMedecin(int id)
        {
            Medecin medecin = await db.Medecin.FindAsync(id);
            if (medecin == null)
            {
                return NotFound();
            }

            return Ok(medecin);
        }

        // PUT: api/Medecins/5
        [ResponseType(typeof(void))]
        //[AuthentificationFilter]
        [Authorize]
        public async Task<IHttpActionResult> PutMedecin(int id, Medecin medecin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != medecin.id)
            {
                return BadRequest();
            }

            db.Entry(medecin).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedecinExists(id))
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

        // POST: api/Medecins
        [ResponseType(typeof(Medecin))]
        //[AuthentificationFilter]
        [Authorize]
        public async Task<IHttpActionResult> PostMedecin(Medecin medecin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Medecin.Add(medecin);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = medecin.id }, medecin);
        }

        // DELETE: api/Medecins/5
        [ResponseType(typeof(Medecin))]
        //[AuthentificationFilter]
        [Authorize]
        public async Task<IHttpActionResult> DeleteMedecin(int id)
        {
            Medecin medecin = await db.Medecin.FindAsync(id);
            if (medecin == null)
            {
                return NotFound();
            }

            db.Medecin.Remove(medecin);
            await db.SaveChangesAsync();

            return Ok(medecin);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MedecinExists(int id)
        {
            return db.Medecin.Count(e => e.id == id) > 0;
        }
    }
}