using Survey_System.Models;
using Survey_System.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Survey_System.Controllers
{
    public class QuestionController : BaseController
    {
        public ActionResult Index()
        {
            if (Session["ADMIN"]==null)
            {
                return RedirectToAction("SignIn","Login");
            }
            var model = db.QUESTION.ToList();
            return View(model);
        }
        public ActionResult Create(QUESTION question)
        {
            if (question.QUESTIONLINE != null)
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        // burada dilerseniz dosya tipine gore filtreleme yaparak sadece istediginiz dosya formatindaki dosyalari yukleyebilirsiniz
                        //if (file.ContentType == "image/jpeg" || file.ContentType == "image/jpg" || file.ContentType == "image/png" || file.ContentType == "image/gif")
                        //{
                        var fi = new FileInfo(file.FileName);
                        var fileName = Path.GetFileName(file.FileName);
                        fileName = Guid.NewGuid().ToString() + fi.Extension;
                        var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                        file.SaveAs(path);
                        var IMGLINE = "/Images/" + fileName;
                        question.IMGLINE =IMGLINE;
                        db.SaveChanges();
                        
                        //}
                    }
                }

                question.CREATEDATE = DateTime.Now;
                question.CREATEBY = NAMESURNAME;
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

            question.MODIFYBY = NAMESURNAME;
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