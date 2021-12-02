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
    public class TilaisuudetController : Controller
    {
        private KipparitRyEntitiesX db = new KipparitRyEntitiesX();

        // GET: Tilaisuudet
        public ActionResult Index()
        {
            var tilaisuudet = db.Tilaisuudet.Include(t => t.Jarjestajat1);
            return View(tilaisuudet.ToList());
        }

        // GET: Tilaisuudet/Details/5
        public ActionResult Details(int? id)
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

        // GET: Tilaisuudet/Create
        public ActionResult Create()
        {
            ViewBag.JarjestajaID = new SelectList(db.Jarjestajat, "JarjestajaID", "Nimi");
            return View();
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
