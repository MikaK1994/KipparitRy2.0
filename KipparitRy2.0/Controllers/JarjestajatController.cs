using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KipparitRy2._0.Models;
using PagedList;

namespace KipparitRy2._0.Controllers
{
    public class JarjestajatController : Controller
    {
        private KipparitRyEntitiesX db = new KipparitRyEntitiesX();

        // GET: Jarjestajat
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? pagesize)
        {
            if (Session["Kayttajanimi"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.LoggedStatus = "In";
                ViewBag.LoginError = 0; //ei virhettä sisäänkirjautuessa

                ViewBag.CurrentSort = sortOrder;
                //if-lause vb.pnsp jälkeen = Jos ensimmäinen lause on tosi ? toinen lause toteutuu : jos epätosi, niin tämä kolmas lause toteutuu
                ViewBag.CustomerNameSortParm = String.IsNullOrEmpty(sortOrder) ? "customername_desc" : "";
                ViewBag.NameSortParm = sortOrder == "Name" ? "Name_desc" : "Name";


                //Hakufiltterin laitto muistiin
                if (searchString != null) //tarkistetaan onko käyttäjän antama arvo (esim. kirjain a tai sana villa) eri suuruinen kuin null
                {
                    page = 1; //jos a-kirjainta etsitään, niin vie sivulle 1 kaikki tuotteet, jossa a-kirjain
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.currentFilter = searchString;

                var jarjestajat = from p in db.Jarjestajat
                                  select p;

                if (!String.IsNullOrEmpty(searchString)) //Jos hakufiltteri on käytössä, niin käytetään sitä ja sen lisäksi lajitellaan tulokset
                {
                    switch (sortOrder)
                    {
                        case "customername_desc":
                            jarjestajat = jarjestajat.Where(p => p.Nimi.Contains(searchString)).OrderByDescending(p => p.Nimi);
                            break;
                        case "Name":
                            jarjestajat = jarjestajat.Where(p => p.Nimi.Contains(searchString)).OrderBy(p => p.Nimi);
                            break;
                        case "Name_desc":
                            jarjestajat = jarjestajat.Where(p => p.Nimi.Contains(searchString)).OrderByDescending(p => p.Nimi);
                            break;
                        default:
                            jarjestajat = jarjestajat.Where(p => p.Nimi.Contains(searchString)).OrderBy(p => p.Nimi);
                            break;
                    }
                }
                else
                {
                    switch (sortOrder)
                    {
                        case "customername_desc":
                            jarjestajat = jarjestajat.OrderByDescending(p => p.Nimi);
                            break;
                        case "Name":
                            jarjestajat = jarjestajat.OrderBy(p => p.Nimi);
                            break;
                        case "Name_desc":
                            jarjestajat = jarjestajat.OrderByDescending(p => p.Nimi);
                            break;
                        default:
                            jarjestajat = jarjestajat.OrderBy(p => p.Nimi);
                            break;
                    }
                };

                int pageSize = (pagesize ?? 10); //Tämä palauttaa sivukoon taikka jos pagesize on null, niin palauttaa koon 10 riviä per sivu
                int pageNumber = (page ?? 1); //int pageNumber on sivuparametrien arvojen asetus. Tämä palauttaa sivunumeron taikka jos page on null, niin palauttaa numeron yksi
                return View(jarjestajat.ToPagedList(pageNumber, pageSize));

                //var jarjestajat = db.Jarjestajat.Include(j => j.Tilaisuudet);
                //return View(jarjestajat.ToList());
            }
        }

        // GET: Jarjestajat/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Kayttajanimi"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Jarjestajat jarjestajat = db.Jarjestajat.Find(id);
                if (jarjestajat == null)
                {
                    return HttpNotFound();
                }
                return View(jarjestajat);
            }
        }

        // GET: Jarjestajat/Create
        public ActionResult Create()
        {
            if (Session["Kayttajanimi"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.TilaisuusID = new SelectList(db.Tilaisuudet, "TilaisuusID", "Nimi");
                return View();
            }
        }

        // POST: Jarjestajat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JarjestajaID,Nimi,TilaisuusID")] Jarjestajat jarjestajat)
        {
            if (ModelState.IsValid)
            {
                db.Jarjestajat.Add(jarjestajat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TilaisuusID = new SelectList(db.Tilaisuudet, "TilaisuusID", "Nimi", jarjestajat.TilaisuusID);
            return View(jarjestajat);
        }

        // GET: Jarjestajat/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["Kayttajanimi"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Jarjestajat jarjestajat = db.Jarjestajat.Find(id);
                if (jarjestajat == null)
                {
                    return HttpNotFound();
                }
                ViewBag.TilaisuusID = new SelectList(db.Tilaisuudet, "TilaisuusID", "Nimi", jarjestajat.TilaisuusID);
                return View(jarjestajat);
            }
        }

        // POST: Jarjestajat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JarjestajaID,Nimi,TilaisuusID")] Jarjestajat jarjestajat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jarjestajat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TilaisuusID = new SelectList(db.Tilaisuudet, "TilaisuusID", "Nimi", jarjestajat.TilaisuusID);
            return View(jarjestajat);
        }

        // GET: Jarjestajat/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["Kayttajanimi"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Jarjestajat jarjestajat = db.Jarjestajat.Find(id);
                if (jarjestajat == null)
                {
                    return HttpNotFound();
                }
                return View(jarjestajat);
            }
        }

        // POST: Jarjestajat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try {
            Jarjestajat jarjestajat = db.Jarjestajat.Find(id);
            db.Jarjestajat.Remove(jarjestajat);
            db.SaveChanges();
            return RedirectToAction("Index");
            }
            catch
            {
                Jarjestajat jarjestajat = db.Jarjestajat.Find(id);
                ViewBag.Error = "Et voi poistaa kyseistä järjestäjää, sillä tämä on järjestäjänä tilaisuudessa. Poistaaksesi järjestäjän sinun täytyy ensin poistaa tilaisuus, jossa järjestäjä on järjestäjänä tai vaihtaa tilaisuuden järjestäjää.";
                return View(jarjestajat);
            }
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

