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
    public class JarjestajatController : Controller
    {
        private KipparitRyEntitiesX db = new KipparitRyEntitiesX();

        // GET: Jarjestajat
        public ActionResult Index()
        {
            var jarjestajat = db.Jarjestajat.Include(j => j.Tilaisuudet);
            return View(jarjestajat.ToList());
        }

        // GET: Jarjestajat/Details/5
        public ActionResult Details(int? id)
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

        // GET: Jarjestajat/Create
        public ActionResult Create()
        {
            ViewBag.TilaisuusID = new SelectList(db.Tilaisuudet, "TilaisuusID", "Nimi");
            return View();
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

        // POST: Jarjestajat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Jarjestajat jarjestajat = db.Jarjestajat.Find(id);
            db.Jarjestajat.Remove(jarjestajat);
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
