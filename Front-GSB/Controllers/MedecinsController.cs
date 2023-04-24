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
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;

namespace Front_GSB.Controllers
{
    public class MedecinsController : Controller
    {
        //private GSB_Data_Model db = new GSB_Data_Model();

        // GET: Medecins

        public async Task<ActionResult> Index()
        {
            //l'API à consommer depuis le back
            string url = "https://localhost:44369/api/Medecins";

            using (HttpClient client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Add("token", "123456789");
                HttpResponseMessage response = await client.GetAsync(url);

                //si erreur, on propage une exception
                if (!response.IsSuccessStatusCode)
                    throw new Exception();

                // la liste des médecins
                var med = await response.Content.ReadAsAsync<IEnumerable<Medecin>>();

                return View(med);
            }
        }

        //public async Task<ActionResult> Index()
        //{
        //    var medecin = db.Medecin.Include(m => m.Departement);
        //    return View(await medecin.ToListAsync());
        //}

        // GET: Medecins/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string url = "https://localhost:44369/api/Medecins/" + id;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                //si erreur, on propage une exception
                if (!response.IsSuccessStatusCode)
                    throw new Exception();

                // la liste des médecins
                var med = await response.Content.ReadAsAsync<Medecin>();

                return View(med);
            }
        }


        //    Medecin medecin = await db.Medecin.FindAsync(id);
        //            if (medecin == null)
        //            {
        //                return HttpNotFound();
        //}
        //            return View(medecin);


        //    // GET: Medecins/Create
        [Authorize] //pour  l'IHM
        public async Task<ActionResult> Create()
        {
            string urlDepartements = "https://localhost:44369/api/Departements";
            using (HttpClient client = new HttpClient())
            {
                // pour s'authentifier sur le back-end
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + ReadToken());

                HttpResponseMessage response = await client.GetAsync(urlDepartements);

                // si erreur, on propage une exception
                if (!response.IsSuccessStatusCode)
                    throw new Exception();

                // la liste des departements lors de la creation d'un médecin
                var dep = response.Content.ReadAsAsync<IEnumerable<Departement>>().Result.ToList();

                // pour avoir la liste des départements lors de la création d'un médecin
                ViewBag.num_dep = new SelectList(dep, "num_dep", "dep_name");

                return View();
            }
        }

        //// POST: Medecins/Create
        //// Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        //// plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,prenom,nom,adresse,telephone,specialite,num_dep")] Medecin medecin)
        {
            if (ModelState.IsValid)
            {
                // on recupère les infos saisies dans l'interface et les sérialiser en json
                string json = JsonConvert.SerializeObject(medecin);

                using (HttpClient client = new HttpClient()) //utilisation client HTTP
                {
                    // pour s'authentifier sur le back-end
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + ReadToken());
                    //client.DefaultRequestHeaders.Add("token", "123456789");
                    using (var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44369/api/Medecins")) //l'objet Request
                    {
                        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                        //envoie des infos
                        var send = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

                        if (!send.IsSuccessStatusCode) //si erreur, on propage une exception
                            throw new Exception("Un problème s'est produit, veuillez contacter l'administrateur");

                        send.EnsureSuccessStatusCode();
                        //ViewBag.num_dep = new SelectList(db.Departement, "num_dep", "dep_name", medecin.num_dep);
                        return RedirectToAction("Index");
                    }
                }
                //db.Medecin.Add(medecin);
                //await db.SaveChangesAsync();
                //return RedirectToAction("Index");
            }
            //ViewBag.num_dep = new SelectList(db.Departement, "num_dep", "dep_name", medecin.num_dep);
            return View(medecin);
        }

        //// GET: Medecins/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            string urlMedecin = "https://localhost:44369/api/Medecins/" + id;
            string urldepartements = "https://localhost:44369/api/Departements";

            using (HttpClient client = new HttpClient())
            {
                // pour s'authentifier sur le back-end
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + ReadToken());

                HttpResponseMessage responseMedecin = await client.GetAsync(urlMedecin);
                HttpResponseMessage responseDepartements = await client.GetAsync(urldepartements);

                //si erreur, on propage une exception
                if (!responseMedecin.IsSuccessStatusCode || !responseDepartements.IsSuccessStatusCode)
                    throw new Exception();

                // la liste des médecins
                var medecin = await responseMedecin.Content.ReadAsAsync<Medecin>();

                // la liste des départements
                var dep = responseDepartements.Content.ReadAsAsync<IEnumerable<Departement>>().Result.ToList();

                ViewBag.num_dep = new SelectList(dep, "num_dep", "dep_name", medecin.num_dep);
                return View(medecin);
            }
        }

        //// POST: Medecins/Edit/5
        //// Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        //// plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,prenom,nom,adresse,telephone,specialite,num_dep")] Medecin medecin)
        {
            if (ModelState.IsValid)
            {
                // on recupère les infos saisies dans l'interface et les sérialiser en json
                string json = JsonConvert.SerializeObject(medecin);

                using (HttpClient client = new HttpClient()) //utilisation client HTTP
                {
                    // pour s'authentifier sur le back-end
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + ReadToken());
                    //client.DefaultRequestHeaders.Add("token", "123456789");
                    HttpContent cont = new StringContent(json, Encoding.UTF8, "application/json");
                    var send = await client.PutAsync("https://localhost:44369/api/Medecins/" + medecin.id, cont).ConfigureAwait(false);

                    if (!send.IsSuccessStatusCode) //si erreur, on propage une exception
                        throw new Exception();

                    send.EnsureSuccessStatusCode();
                    //ViewBag.num_dep = new SelectList(db.Departement, "num_dep", "dep_name", medecin.num_dep);
                    return RedirectToAction("Index");
                }
            }
            return View(medecin);
        }

        //// GET: Medecins/Delete/5  ---- Pour poser question si on est sûr de supprimer
        [Authorize]
        public async Task<ActionResult> Delete(int? id)
        {
            string url = "https://localhost:44369/api/Medecins/" + id;

            //controle indispensable, si l'id est null on propage une exception
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            using (HttpClient client = new HttpClient())
            {
                // pour s'authentifier sur le back-end
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + ReadToken());

                //client.DefaultRequestHeaders.Add("token", "123456789");
                HttpResponseMessage response = await client.GetAsync(url);

                //si erreur, on propage une exception
                if (!response.IsSuccessStatusCode)
                    throw new Exception();

                // la liste des médecins
                var med = await response.Content.ReadAsAsync<Medecin>();
                return View(med);
            }
        }

        //// POST: Medecins/Delete/5  ------ vraie suppression quand l'on clique sur delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            string url = "https://localhost:44369/api/Medecins/" + id;

            //controle indispensable
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            using (HttpClient client = new HttpClient())
            {
                // pour s'authentifier sur le back-end
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + ReadToken());
                //client.DefaultRequestHeaders.Add("token", "123456789");

                HttpResponseMessage response = await client.DeleteAsync(url);

                if (!response.IsSuccessStatusCode)
                    throw new Exception();
            }

            return RedirectToAction("Index");
        }

        private string ReadToken()
        {
            string token = string.Empty;
            try
            {
                string filename = @"C:\temp\token.txt";
                token = System.IO.File.ReadAllText(filename);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return token;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

