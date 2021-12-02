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
    public class TilaisuudetController : Controller
    {
        private KipparitRyEntitiesX db = new KipparitRyEntitiesX();

        // GET: Tilaisuudet
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
                //ViewBag.EventNameSortParm = String.IsNullOrEmpty(sortOrder) ? "event_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "Date_desc" : "Date";


                //Hakufiltterin laitto muistiin
                if (searchString != null) //tarkistetaan onko käyttäjän antama arvo (esim. kirjain a tai sana villa) eri suuruinen kuin null
                {
                    page = 1; //jos a-kirjainta etitään, niin vie sivulle 1 kaikki tuotteet, jossa a-kirjain
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.currentFilter = searchString;

                var tilaisuudet = from p in db.Tilaisuudet
                                  select p;

                if (!String.IsNullOrEmpty(searchString)) //Jos hakufiltteri on käytössä, niin käytetään sitä ja sen lisäksi lajitellaan tulokset
                {
                    switch (sortOrder)
                    {
                        case "event_desc":
                            tilaisuudet = tilaisuudet.Where(p => p.Nimi.Contains(searchString)).OrderByDescending(p => p.Nimi);
                            break;
                        case "Date":
                            tilaisuudet = tilaisuudet.Where(p => p.Nimi.Contains(searchString)).OrderBy(p => p.Pvm);
                            break;
                        case "Date_desc":
                            tilaisuudet = tilaisuudet.Where(p => p.Nimi.Contains(searchString)).OrderByDescending(p => p.Pvm);
                            break;
                        default:
                            tilaisuudet = tilaisuudet.Where(p => p.Nimi.Contains(searchString)).OrderBy(p => p.Nimi);
                            break;
                    }
                }
                else
                {
                    switch (sortOrder)
                    {
                        case "event_desc":
                            tilaisuudet = tilaisuudet.OrderByDescending(p => p.Nimi);
                            break;
                        case "Date":
                            tilaisuudet = tilaisuudet.OrderBy(p => p.Pvm);
                            break;
                        case "Date_desc":
                            tilaisuudet = tilaisuudet.OrderByDescending(p => p.Pvm);
                            break;
                        default:
                            tilaisuudet = tilaisuudet.OrderBy(p => p.Nimi);
                            break;
                    }
                };

                int pageSize = (pagesize ?? 10); //Tämä palauttaa sivukoon taikka jos pagesize on null, niin palauttaa koon 10 riviä per sivu
                int pageNumber = (page ?? 1); //int pageNumber on sivuparametrien arvojen asetus. Tämä palauttaa sivunumeron taikka jos page on null, niin palauttaa numeron yksi
                return View(tilaisuudet.ToPagedList(pageNumber, pageSize));
                //var tilaisuudet = db.Tilaisuudet.Include(t => t.Jarjestajat1);
                //return View(tilaisuudet.ToList());
            }
        }

        // GET: Tilaisuudet/Details/5
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
                Tilaisuudet tilaisuudet = db.Tilaisuudet.Find(id);
                if (tilaisuudet == null)
                {
                    return HttpNotFound();
                }
                return View(tilaisuudet);
            }
        }

        // GET: Tilaisuudet/Create
        public ActionResult Create()
        {
            if (Session["Kayttajanimi"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.JarjestajaID = new SelectList(db.Jarjestajat, "JarjestajaID", "Nimi");
                return View();
            }
        }

        // POST: Tilaisuudet/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TilaisuusID,Nimi,JarjestajaID,Pvm,MaxMaara")] Tilaisuudet tilaisuudet)
        {
            if (ModelState.IsValid)
            {
                db.Tilaisuudet.Add(tilaisuudet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.JarjestajaID = new SelectList(db.Jarjestajat, "JarjestajaID", "Nimi", tilaisuudet.JarjestajaID);
            return View(tilaisuudet);
        }

        // GET: Tilaisuudet/Edit/5
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
                Tilaisuudet tilaisuudet = db.Tilaisuudet.Find(id);
                if (tilaisuudet == null)
                {
                    return HttpNotFound();
                }
                ViewBag.JarjestajaID = new SelectList(db.Jarjestajat, "JarjestajaID", "Nimi", tilaisuudet.JarjestajaID);
                return View(tilaisuudet);
            }
        }

        // POST: Tilaisuudet/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TilaisuusID,Nimi,JarjestajaID,Pvm,MaxMaara")] Tilaisuudet tilaisuudet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tilaisuudet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JarjestajaID = new SelectList(db.Jarjestajat, "JarjestajaID", "Nimi", tilaisuudet.JarjestajaID);
            return View(tilaisuudet);
        }

        // GET: Tilaisuudet/Delete/5
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
                Tilaisuudet tilaisuudet = db.Tilaisuudet.Find(id);
                if (tilaisuudet == null)
                {
                    return HttpNotFound();
                }
                return View(tilaisuudet);
            }
        }

        // POST: Tilaisuudet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tilaisuudet tilaisuudet = db.Tilaisuudet.Find(id);
            db.Tilaisuudet.Remove(tilaisuudet);
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
