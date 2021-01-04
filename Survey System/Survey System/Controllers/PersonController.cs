using Survey_System.Models;
using Survey_System.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Survey_System.Controllers
{

    public class PersonController : BaseController
    {
        public ActionResult Index()
        {
            if (Session["ADMIN"] == null)
            {
                return RedirectToAction("SignIn", "Login");
            }
            var model = db.PERSON.ToList();
            return View(model);
        }

        public ActionResult Create(PERSON person,string Answer)
        {
            if (person.NAMESURNAME != null)
            {
                person.CREATEDATE = DateTime.Now;
                person.CREATEBY = NAMESURNAME;
                if (Answer == "Evet")
                {
                    person.ISADMIN ="Y";
                }
                else
                {
                    person.ISADMIN = "N";
                }
                db.PERSON.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Edit(int? ID)
        {
            if (ID == null || ID ==0) 
            {
                return HttpNotFound();
            }
            var model = db.PERSON.Find(ID);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(PERSON person, string Answer)
        {
            db.Entry(person).State = System.Data.Entity.EntityState.Modified;
            db.Entry(person).Property(e => e.CREATEBY).IsModified = false;
            db.Entry(person).Property(e => e.CREATEDATE).IsModified = false;

            person.MODIFYBY = NAMESURNAME;
            person.MODIFYDATE = DateTime.Now;
            if (Answer == "Evet")
            {
                person.ISADMIN = "Y";
            }
            else
            {
                person.ISADMIN = "N";
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? ID)
        {
            if (ID == null || ID == 0)
            {
                return HttpNotFound();
            }
            var person = db.PERSON.Find(ID);
            db.PERSON.Remove(person);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}