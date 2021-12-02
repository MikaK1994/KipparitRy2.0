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
    public class RekisteroitymisetController : Controller
    {
        private KipparitRyEntitiesX db = new KipparitRyEntitiesX();

        // GET: Rekisteroitymiset
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
                ViewBag.NameSortParm = sortOrder == "Date" ? "Date_desc" : "Date";

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

                var rekisteroitymiset = db.Rekisteroitymiset.Include(r => r.Asiakkaat).Include(r => r.Tilaisuudet);

                if (!String.IsNullOrEmpty(searchString)) //Jos hakufiltteri on käytössä, niin käytetään sitä ja sen lisäksi lajitellaan tulokset
                {
                    switch (sortOrder)
                    {
                        case "customername_desc":
                            rekisteroitymiset = rekisteroitymiset.Where(r => r.Asiakkaat.Nimi.Contains(searchString)).OrderByDescending(r => r.Asiakkaat.Nimi);
                            break;
                        case "Date":
                            rekisteroitymiset = rekisteroitymiset.Where(r => r.Tilaisuudet.Nimi.Contains(searchString)).OrderBy(r => r.Tilaisuudet.Nimi);
                            break;
                        case "Date_desc":
                            rekisteroitymiset = rekisteroitymiset.Where(r => r.Tilaisuudet.Nimi.Contains(searchString)).OrderByDescending(r => r.Tilaisuudet.Nimi);
                            break;
                        default:
                            rekisteroitymiset = rekisteroitymiset.Where(r => r.Asiakkaat.Nimi.Contains(searchString)).OrderBy(r => r.Asiakkaat.Nimi);
                            break;
                    }
                }
                else
                {
                    switch (sortOrder)
                    {
                        case "customername_desc":
                            rekisteroitymiset = rekisteroitymiset.OrderByDescending(r => r.Asiakkaat.Nimi);
                            break;
                        case "Date":
                            rekisteroitymiset = rekisteroitymiset.OrderBy(r => r.Tilaisuudet.Nimi);
                            break;
                        case "Date_desc":
                            rekisteroitymiset = rekisteroitymiset.OrderByDescending(r => r.Tilaisuudet.Nimi);
                            break;
                        default:
                            rekisteroitymiset = rekisteroitymiset.OrderBy(r => r.Asiakkaat.Nimi);
                            break;
                    }
                };

                int pageSize = (pagesize ?? 10); //Tämä palauttaa sivukoon taikka jos pagesize on null, niin palauttaa koon 10 riviä per sivu
                int pageNumber = (page ?? 1); //int pageNumber on sivuparametrien arvojen asetus. Tämä palauttaa sivunumeron taikka jos page on null, niin palauttaa numeron yksi
                return View(rekisteroitymiset.ToPagedList(pageNumber, pageSize));

                //var asiakkaats = db.Asiakkaat.Include(a => a.Postitoimipaikat);
                //return View(asiakkaats.ToList());
            }
        }

        // GET: Rekisteroitymiset/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rekisteroitymiset rekisteroitymiset = db.Rekisteroitymiset.Find(id);
            if (rekisteroitymiset == null)
            {
                return HttpNotFound();
            }
            return View(rekisteroitymiset);
        }

        // GET: Rekisteroitymiset/Create
        public ActionResult Create()
        {
            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi");
            ViewBag.TilaisuusID = new SelectList(db.Tilaisuudet, "TilaisuusID", "Nimi");
            return View();
        }

        // POST: Rekisteroitymiset/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RekisterointiID,TilaisuusID,AsiakasID")] Rekisteroitymiset rekisteroitymiset)
        {
            if (ModelState.IsValid)
            {
                db.Rekisteroitymiset.Add(rekisteroitymiset);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi", rekisteroitymiset.AsiakasID);
            ViewBag.TilaisuusID = new SelectList(db.Tilaisuudet, "TilaisuusID", "Nimi", rekisteroitymiset.TilaisuusID);
            return View(rekisteroitymiset);
        }

        // GET: Rekisteroitymiset/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rekisteroitymiset rekisteroitymiset = db.Rekisteroitymiset.Find(id);
            if (rekisteroitymiset == null)
            {
                return HttpNotFound();
            }
            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi", rekisteroitymiset.AsiakasID);
            ViewBag.TilaisuusID = new SelectList(db.Tilaisuudet, "TilaisuusID", "Nimi", rekisteroitymiset.TilaisuusID);
            return View(rekisteroitymiset);
        }

        // POST: Rekisteroitymiset/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RekisterointiID,TilaisuusID,AsiakasID")] Rekisteroitymiset rekisteroitymiset)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rekisteroitymiset).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi", rekisteroitymiset.AsiakasID);
            ViewBag.TilaisuusID = new SelectList(db.Tilaisuudet, "TilaisuusID", "Nimi", rekisteroitymiset.TilaisuusID);
            return View(rekisteroitymiset);
        }

        // GET: Rekisteroitymiset/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rekisteroitymiset rekisteroitymiset = db.Rekisteroitymiset.Find(id);
            if (rekisteroitymiset == null)
            {
                return HttpNotFound();
            }
            return View(rekisteroitymiset);
        }

        // POST: Rekisteroitymiset/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rekisteroitymiset rekisteroitymiset = db.Rekisteroitymiset.Find(id);
            db.Rekisteroitymiset.Remove(rekisteroitymiset);
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