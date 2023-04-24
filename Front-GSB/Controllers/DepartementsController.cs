using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model;
using System.Net.Http;

namespace Front_GSB.Controllers
{
    public class DepartementsController : Controller
    {
        // private GSB_Data_Model db = new GSB_Data_Model();

        // GET: Departements
        public async Task<ActionResult> Index()
        {
            //l'API à consommer depuis le back
            string url = "https://localhost:44369/api/Departements";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("token", "123456789");
                HttpResponseMessage response = await client.GetAsync(url);

                //si erreur, on propage une exception
                if (!response.IsSuccessStatusCode)
                    throw new Exception();

                // la liste des villes
                var departs = await response.Content.ReadAsAsync<IEnumerable<Departement>>();

                return View(departs);
            }
        }

        // GET: Departements/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string url = "https://localhost:44369/api/Departements/"+id;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                //si erreur, on propage une exception
                if (!response.IsSuccessStatusCode)
                    throw new Exception();

                // la liste des villes
                var departs = await response.Content.ReadAsAsync<Departement>();

                return View(departs);
            }

            //Departement departement = await db.Departement.FindAsync(id);
            //if (departement == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(departement);
        }

        //// GET: Departements/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Departements/Create
        //// Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        //// plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "num_dep,dep_name,region_name")] Departement departement)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Departement.Add(departement);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    return View(departement);
        //}

        //// GET: Departements/Edit/5
        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Departement departement = await db.Departement.FindAsync(id);
        //    if (departement == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(departement);
        //}

        //// POST: Departements/Edit/5
        //// Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        //// plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "num_dep,dep_name,region_name")] Departement departement)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(departement).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(departement);
        //}

        //// GET: Departements/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Departement departement = await db.Departement.FindAsync(id);
        //    if (departement == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(departement);
        //}

        //// POST: Departements/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    Departement departement = await db.Departement.FindAsync(id);
        //    db.Departement.Remove(departement);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
