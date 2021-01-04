using Survey_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Survey_System.Controllers
{
    public class LoginController : Controller
    {
        SurveyEntities db = new SurveyEntities();
        public ActionResult SignIn(string CODE,string PASSWORD)
        {
            if (CODE==null)
            {
                return View();
            }
            else
            {
                var person = db.PERSON.FirstOrDefault(m => m.CODE == CODE && m.PASSWORD == PASSWORD);
                if (person != null)
                {
                    Session["CODE"] = person.CODE;
                    Session["NAMESURNAME"] = person.NAMESURNAME;
                    if (person.ISADMIN=="Y") 
                    {
                        Session["ADMIN"] = "Admin";
                    }
                    return RedirectToAction("Index", "Answer");
                }
                else
                {
                    ViewBag.Error = "Kullanıcı kodu veya şifre hatalı.";
                    return View();
                }
            }
            
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("SignIn", "Login");
        }
    }
}