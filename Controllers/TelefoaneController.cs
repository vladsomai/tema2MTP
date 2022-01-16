using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tema2MTP.Models;

namespace Tema2MTP.Controllers
{

    public class TelefoaneController : Controller
    {
        private masterEntities db = new masterEntities();

        //action pentru functia de cautare
        /*
         * In momentul in care utilizatorul apasa pe butonul cauta se va face un http post request catre aceasta functie
         * parametrul "modelTelefon" este stringul introdus in campul de input
         */
        [HttpPost]
        public ActionResult Index(string modelTelefon)
        {
            var listaTelefoane = db.Telefoanes.ToList();

            //verificam daca utilizatorul a apasat pe butonul cauta iar acesta nu este completat
            //in acest caz returnam toate telefoanele
            if (modelTelefon.Length == 0)
            {
                return View(listaTelefoane);
            }
            else
            {
                //vom crea o lista noua
                List<Telefoane> telefonCautat = new List<Telefoane>();

                //facem o cautare liniara in lista returnata de baza de date mai sus
                foreach (var telefon in listaTelefoane)
                {
                    //in cazul in care unul din telefoanele din baza de date contine textul introdus de utilizator in campul
                    //cauta, vom adauga acel telefon in noua lista cu telefoanele gasita
                    //pentru a avea o cautare corecta, transformam ambele string-uri in lowercase
                    if (telefon.Model.ToLower().Contains(modelTelefon.ToLower()))
                        telefonCautat.Add(telefon);
                }

                //returnam lista cu telefoanele care contin textul introdus de utilizator
                return View(telefonCautat);
            }
        }

        //http get request - returneaza toate telefoanele
        [HttpGet]
        public ActionResult Index()
        {
            return View( db.Telefoanes.ToList());
        }

        //functie care v-a scadea stocul disponibil in momentul in care utilizatorul apasa pe butonul cumpara
        public ActionResult Cumpara(int? id)
        {

            //Cautam telefonul pe care s-a apasat butonul cumpara
            Telefoane telefonCumparat = db.Telefoanes.Find(id);

            //Scadem stocul
            telefonCumparat.Stoc = telefonCumparat.Stoc - 1;

            //Salvam noul telefon cu stocul modificat in baza de date
            db.Entry(telefonCumparat).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Telefoane/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Telefoane telefoane = db.Telefoanes.Find(id);
            if (telefoane == null)
            {
                return HttpNotFound();
            }
            return View(telefoane);
        }


        // GET: Telefoane/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Telefoane/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "ID_Telefon,Model,An_Fabricatie,Specificatii,Pret,Stoc")] Telefoane telefoane)
        {
            if (ModelState.IsValid)
            {
                db.Telefoanes.Add(telefoane);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(telefoane);
        }

        // GET: Telefoane/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Telefoane telefoane = db.Telefoanes.Find(id);
            if (telefoane == null)
            {
                return HttpNotFound();
            }
            return View(telefoane);
        }

        // POST: Telefoane/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ID_Telefon,Model,An_Fabricatie,Specificatii,Pret,Stoc")] Telefoane telefoane)
        {
            if (ModelState.IsValid)
            {
                db.Entry(telefoane).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(telefoane);
        }

        // GET: Telefoane/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Telefoane telefoane = db.Telefoanes.Find(id);
            if (telefoane == null)
            {
                return HttpNotFound();
            }
            return View(telefoane);
        }

        // POST: Telefoane/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Telefoane telefoane = db.Telefoanes.Find(id);
            db.Telefoanes.Remove(telefoane);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
