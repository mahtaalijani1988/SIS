using Elmah;
using System.Web.Mvc;

namespace Student_Information_System.Extentions.Filters
{
    public class ElmahHandledErrorLoggerFilter:IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
                ErrorSignal.FromCurrentContext().Raise(filterContext.Exception);
        }
    }
}