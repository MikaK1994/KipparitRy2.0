using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KipparitRy2._0.Models;

namespace KipparitRy2._0.Controllers
{
    public class HomeController : Controller
    {
        private KipparitRyEntitiesX db = new KipparitRyEntitiesX();
        
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Authorize(Logins LoginModel)
        {
            //Haetaan käyttäjän/Loginin tiedot annetuilla tunnustiedoilla tietokannasta LINQ -kyselyllä
            var LoggedUser = db.Logins.SingleOrDefault(x => x.Kayttajanimi == LoginModel.Kayttajanimi && x.Salasana == LoginModel.Salasana);
            if (LoggedUser != null)
            {
                //ViewBag.LoginMessage = "Kirjautuminen onnistui!";
                ViewBag.LoggedStatus = "In";
                ViewBag.LoginError = 0; //Ei virhettä
                Session["Kayttajanimi"] = LoggedUser.Kayttajanimi;
                return RedirectToAction("Index", "Home"); //Tässä määritellään mihin onnistunut kirjautuminen johtaa
            }
            else
            {
                ViewBag.ErrorMessage = "Kirjautuminen epäonnistui.";
                ViewBag.LoggedStatus = "Out";
                ViewBag.LoginError = 1; //Pakoketaan modaali login-ruutu uudelleen, koska kirjautumisyritys epäonnistui
                return View("ReDirect", LoginModel);
            }
        }
        public ActionResult LogOut()
        {
            Session.Abandon();
            ViewBag.LoggedStatus = "Out";
            return RedirectToAction("Index", "Home"); //Uloskirjautumisen jälkeen pääsivulle
        }

        //GET: Home/Index
        public ActionResult Index()
        {
            ViewBag.LoginError = 0; //ei virhettä sisäänkirjautuessa

            var postit = db.Postitoimipaikat.OrderBy(s => s.Postitoimipaikka)
            .Select(s => new
            {
                Text = s.Postitoimipaikka + ", " + s.Postinumero,
                Value = s.PostiID
            })
            .ToList();
            ViewBag.PostiLista = new SelectList(postit, "Value", "Text");

            // Tilaisuus
            List<Tilaisuudet> tilaisuusList = db.Tilaisuudet.ToList();
            ViewBag.TilaisuusID = new SelectList(tilaisuusList.OrderBy(s => s.Nimi), "TilaisuusID", "Nimi");

            return View();
        }

        //POST : Home/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Rekisteroinutasiakas rekisteroinutasiakas)
        {
            string msg = "";
            string msg2 = "";
            if (ModelState.IsValid)
            {
                Asiakkaat asiakas = new Asiakkaat(); //luokan instassi/olio/objekti
                asiakas.AsiakasID = rekisteroinutasiakas.AsiakasID; //olion ominaisuus
                asiakas.Nimi = rekisteroinutasiakas.Nimi;
                asiakas.Sposti = rekisteroinutasiakas.Sposti;
                asiakas.Osoite = rekisteroinutasiakas.Osoite;
                asiakas.PostiID = rekisteroinutasiakas.PostiID;
                asiakas.RekisterointiPvm = DateTime.Now;
                db.Asiakkaat.Add(asiakas);

                Rekisteroitymiset rekisteroitymiset = new Rekisteroitymiset();
                rekisteroitymiset.AsiakasID = rekisteroinutasiakas.AsiakasID;

                var ilmoittautumiset = db.Rekisteroitymiset.Where(r => r.TilaisuusID == rekisteroinutasiakas.TilaisuusID).ToList().Count();
                Tilaisuudet tilaisuus = db.Tilaisuudet.Where(r => r.TilaisuusID == rekisteroinutasiakas.TilaisuusID).FirstOrDefault(); //haetaan tilaisuus
                int? maxMaara = tilaisuus.MaxMaara;
                if(maxMaara == ilmoittautumiset || maxMaara < ilmoittautumiset)
                {
                    msg = "Tilaisuuden osallistujamäärä on valitettavasti täynnä";
                    TempData["ErrorMessage"] = msg;
                    return RedirectToAction("Index");
                }

                var kayttoehdot = rekisteroinutasiakas.EhdotBox;
                if(kayttoehdot != true)
                {
                    msg2 = "Käyttöehdot pitää hyväksyä, jotta voit ilmoittautua tilaisuuteen.";
                    TempData["ErrorMessage2"] = msg2;
                    return RedirectToAction("Index");
                }
                ViewBag.Error = "";
                rekisteroitymiset.TilaisuusID = (int)rekisteroinutasiakas.TilaisuusID;
                db.Rekisteroitymiset.Add(rekisteroitymiset);
                db.SaveChanges();
                return RedirectToAction("ReDirect2", "Home");
            }

            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postitoimipaikka", rekisteroinutasiakas.PostiID);
            return View(rekisteroinutasiakas);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Porvoon Kipparit";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Porvoon Kipparit ry";

            return View();
        }

        public ActionResult Privacy()
        {
            ViewBag.Message = "Tietosuojaseloste";

            return View();
        }
        public ActionResult ReDirect()
        {
            return View();
        }
        public ActionResult ReDirect2()
        {
            return View();
        }
    }
}