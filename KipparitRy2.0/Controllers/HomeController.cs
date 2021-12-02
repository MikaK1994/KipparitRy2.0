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
                //ViewBag.LoginMessage = "Kirjautuminen epäonnistui.";
                ViewBag.LoggedStatus = "Out";
                ViewBag.LoginError = 1; //Pakoketaan modaali login-ruutu uudelleen, koska kirjautumisyritys epäonnistui
                LoginModel.ErrorMessage = "Tuntematon käyttäjätunnus tai salasana.";
                return View("Index", LoginModel);
            }
        }
        public ActionResult LogOut()
        {
            Session.Abandon();
            ViewBag.LoggedStatus = "Out";
            return RedirectToAction("Index", "Home"); //Uloskirjautumisen jälkeen pääsivulle
        }
        public ActionResult Index()
        {
            ViewBag.LoginError = 0; //ei virhettä sisäänkirjautuessa
            return View();
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
    }
}