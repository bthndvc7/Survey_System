using Survey_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Survey_System.Controllers
{
    public class QuestionController : Controller
    {
        SurveyEntities db = new SurveyEntities();
        public ActionResult Index()
        {
            var model = db.QUESTION.ToList();
            return View(model);
        }

        public ActionResult Create(QUESTION question)
        {
            if (question.QUESTIONLINE != null)
            {
                question.CREATEDATE = DateTime.Now;
                question.CREATEBY = "System";
                db.QUESTION.Add(question);
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
            if (ID == null || ID == 0)
            {
                return HttpNotFound();
            }
            var model = db.QUESTION.Find(ID);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(QUESTION question)
        {
            db.Entry(question).State = System.Data.Entity.EntityState.Modified;
            db.Entry(question).Property(e => e.CREATEBY).IsModified = false;
            db.Entry(question).Property(e => e.CREATEDATE).IsModified = false;

            question.MODIFYBY = "SYSTEM Edit";
            question.MODIFYDATE = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? ID)
        {
            if (ID == null || ID == 0)
            {
                return HttpNotFound();
            }
            var question = db.QUESTION.Find(ID);
            db.QUESTION.Remove(question);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}