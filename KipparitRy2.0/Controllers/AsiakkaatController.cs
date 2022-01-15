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
    public class AsiakkaatController : Controller
    {
        private KipparitRyEntities db = new KipparitRyEntities();

        // GET: Asiakkaat
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

                var asiakkaat = from p in db.Asiakkaat
                                select p;

                if (!String.IsNullOrEmpty(searchString)) //Jos hakufiltteri on käytössä, niin käytetään sitä ja sen lisäksi lajitellaan tulokset
                {
                    switch (sortOrder)
                    {
                        case "customername_desc":
                            asiakkaat = asiakkaat.Where(p => p.Nimi.Contains(searchString)).OrderByDescending(p => p.Nimi);
                            break;
                        case "Date":
                            asiakkaat = asiakkaat.Where(p => p.Nimi.Contains(searchString)).OrderBy(p => p.RekisterointiPvm);
                            break;
                        case "Date_desc":
                            asiakkaat = asiakkaat.Where(p => p.Nimi.Contains(searchString)).OrderByDescending(p => p.RekisterointiPvm);
                            break;
                        default:
                            asiakkaat = asiakkaat.Where(p => p.Nimi.Contains(searchString)).OrderBy(p => p.Nimi);
                            break;
                    }
                }
                else
                {
                    switch (sortOrder)
                    {
                        case "customername_desc":
                            asiakkaat = asiakkaat.OrderByDescending(p => p.Nimi);
                            break;
                        case "Date":
                            asiakkaat = asiakkaat.OrderBy(p => p.RekisterointiPvm);
                            break;
                        case "Date_desc":
                            asiakkaat = asiakkaat.OrderByDescending(p => p.RekisterointiPvm);
                            break;
                        default:
                            asiakkaat = asiakkaat.OrderBy(p => p.Nimi);
                            break;
                    }
                };

                int pageSize = (pagesize ?? 10); //Tämä palauttaa sivukoon taikka jos pagesize on null, niin palauttaa koon 10 riviä per sivu
                int pageNumber = (page ?? 1); //int pageNumber on sivuparametrien arvojen asetus. Tämä palauttaa sivunumeron taikka jos page on null, niin palauttaa numeron yksi
                return View(asiakkaat.ToPagedList(pageNumber, pageSize));
            }
        }

        // GET: Asiakkaat/Details/5
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
                Asiakkaat asiakkaat = db.Asiakkaat.Find(id);
                if (asiakkaat == null)
                {
                    return HttpNotFound();
                }
                return View(asiakkaat);
            }
        }

        // GET: Asiakkaat/Create
        public ActionResult Create()
        {
            if (Session["Kayttajanimi"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var postit = db.Postitoimipaikat.OrderBy(s => s.Postitoimipaikka)
                    .Select(s => new
                    {
                        Text = s.Postitoimipaikka + ", " + s.Postinumero,
                        Value = s.PostiID
                    })
                    .ToList();

                ViewBag.PostiLista = new SelectList(postit, "Value", "Text");
                return View();
            }
        }

        // POST: Asiakkaat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Asiakkaat asiakkaat)
        {
            if (ModelState.IsValid)
            {
                db.Asiakkaat.Add(asiakkaat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PostiID = new SelectList(db.Postitoimipaikat, "PostiID", "Postinumero", asiakkaat.PostiID);
            return View(asiakkaat);
        }

        // GET: Asiakkaat/Edit/5
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
                Asiakkaat asiakkaat = db.Asiakkaat.Find(id);
                if (asiakkaat == null)
                {
                    return HttpNotFound();
                }
                var postit = db.Postitoimipaikat.OrderBy(s => s.Postitoimipaikka)
                    .Select(s => new
                    {
                        Text = s.Postitoimipaikka + ", " + s.Postinumero,
                        Value = s.PostiID
                    })
                    .ToList();

                ViewBag.PostiLista = new SelectList(postit, "Value", "Text");
                return View(asiakkaat);
            }
        }

        // POST: Asiakkaat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Asiakkaat asiakkaat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(asiakkaat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostiID = new SelectList(db.Postitoimipaikat, "PostiID", "Postinumero", asiakkaat.PostiID);
            return View(asiakkaat);
        }

        // GET: Asiakkaat/Delete/5
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
                Asiakkaat asiakkaat = db.Asiakkaat.Find(id);
                if (asiakkaat == null)
                {
                    return HttpNotFound();
                }
                return View(asiakkaat);
            }
        }

        // POST: Asiakkaat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Asiakkaat asiakkaat = db.Asiakkaat.Find(id);
                db.Asiakkaat.Remove(asiakkaat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                Asiakkaat asiakkaat = db.Asiakkaat.Find(id);
                ViewBag.Error = "Et voi poistaa asiakasta, sillä asiakas on rekisteröitynyt tilaisuuteen. Poistaaksesi asiakkaan sinun täytyy ensin poistaa asiakas rekisteröitymisistä.";
                return View(asiakkaat);
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