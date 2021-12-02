using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KipparitRy2._0.Models;

namespace KipparitRy2._0.Controllers
{
    public class RekisteroitymisetController : Controller
    {
        private KipparitRyEntitiesX db = new KipparitRyEntitiesX();

        // GET: Rekisteroitymiset
        public ActionResult Index()
        {
            var rekisteroitymiset = db.Rekisteroitymiset.Include(r => r.Asiakkaat).Include(r => r.Tilaisuudet);
            return View(rekisteroitymiset.ToList());
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
