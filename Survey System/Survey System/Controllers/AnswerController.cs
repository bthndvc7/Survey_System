using Survey_System.Models;
using Survey_System.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Survey_System.Controllers
{
    public class AnswerController : BaseController
    {

        public ActionResult Index()
        {
            var model = db.ANSWER.Where(m =>m.USERCODE==UserCODE).ToList();
            return View( model);
        }
        public ActionResult Create(string CODE)
        {
            
            if (CODE == null)
            {
                List<SelectListItem> personList = (from person in db.PERSON
                                                   where person.CODE != UserCODE
                                                   select new SelectListItem
                                                   {
                                                       Text = person.NAMESURNAME,
                                                       Value = person.CODE
                                                   }).ToList();
                ViewBag.PERSON = new SelectList(personList.OrderBy(m => m.Text), "Value", "Text");

                var questionModel = db.QUESTION.ToList();
                return View(questionModel);
            }
            else
            {
                CalculateScore(CODE);
                return RedirectToAction("Index");
            }
           
        }

        public void CalculateScore(string code)
        {
            double kkatiliyorum = 0, katiliyorum = 0, kararsizim = 0, katilmiyorum = 0, kkatilmiyorum = 0, result=0;
            var answer = db.ANSWER.FirstOrDefault(m => m.PERSONCODE == code && m.USERCODE == UserCODE );

            var answerLine = db.ANSWERLINE.Where(m => m.ANSWERID == answer.ID).ToList();

            foreach(var item in answerLine)
            {
                if (item.ANSWER == "Kesinlikle Katılıyorum") kkatiliyorum++;
                else if (item.ANSWER == "Katılıyorum") katiliyorum++;
                else if (item.ANSWER == "Kararsızım") kararsizim++;
                else if (item.ANSWER == "Katılmıyorum") katilmiyorum++;
                else kkatilmiyorum++; 
            }

            result = ((5*kkatiliyorum) + (4*katiliyorum) + (3*kararsizim) + (2*katilmiyorum) + kkatilmiyorum);
            answer.SCORE = result.ToString();
            db.SaveChanges();

        }

        public String SendData(AnswerModel answerModel)
        {
            int? month = DateTime.Now.Month;
            var model = db.ANSWER.FirstOrDefault(m =>m.PERSONCODE==answerModel.CODE && m.USERCODE == UserCODE && m.CREATEDATE.Value.Month == month);

            if (model!=null)
            {
                SaveAnswerLine(answerModel.QUESTION, answerModel.ANSWER, model.ID);
            }
            else
            {
                ANSWER answer = new ANSWER();
                answer.PERSONCODE = answerModel.CODE;
                answer.PERSONNAME = answerModel.NAMESURNAME;
                answer.USERCODE = UserCODE;
                answer.CREATEDATE = DateTime.Now;
                answer.CREATEBY = NAMESURNAME;
                db.ANSWER.Add(answer);
                db.SaveChanges();
                SaveAnswerLine(answerModel.QUESTION, answerModel.ANSWER, answer.ID);
            }
            
            return "True";
        }

        public void SaveAnswerLine(string question,string answer,decimal answerId)
        {
            var model = db.ANSWERLINE.FirstOrDefault(m => m.ANSWERID==answerId && m.QUESTION==question);

            if (model !=null)
            {
                model.ANSWER = answer;
                db.SaveChanges();
            }
            else
            {
                ANSWERLINE answerLine = new ANSWERLINE();
                answerLine.ANSWERID = answerId;
                answerLine.ANSWER = answer;
                answerLine.QUESTION = question;
                db.ANSWERLINE.Add(answerLine);
                db.SaveChanges();
            }
            
            
        }

        public ActionResult Detail(int? ID)
        {
            var model = db.ANSWERLINE.Where(m => m.ANSWERID == ID).ToList();
            return View(model);
        }
    }
}