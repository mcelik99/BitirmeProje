using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BitirmeProjesi.Areas.Student.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var ID = this.HttpContext.Session.GetInt32("STUDENT_ID");
            if (Convert.ToInt32(ID) == 0)
            {
                /*  context.Result = new RedirectToRouteResult(
                       new RouteValueDictionary
                       {
                          {"controller", "Login"},
                          {"action", "Student"}
                       }
                  );*/

                context.Result = new RedirectResult("/Login/Student");
                return;
            }

            base.OnActionExecuting(context);
        }

        protected int getStudentID()
        {
            return (int)this.HttpContext.Session.GetInt32("STUDENT_ID");
        }
    }
}
