using Survey_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Survey_System.Utils
{
    public class BaseController : Controller
    {
        public SurveyEntities db = new SurveyEntities();
        public string UserCODE { get; set;}
        public string NAMESURNAME { get; set;}

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["CODE"]==null)
            {
                filterContext.Result = new RedirectResult("/Login/SignIn");
            }
            else
            {
                UserCODE = Session["CODE"].ToString();
                NAMESURNAME = Session["NAMESURNAME"].ToString();
            }

            base.OnActionExecuting(filterContext);
        }
    }
}