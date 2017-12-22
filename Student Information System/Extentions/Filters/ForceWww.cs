using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;

namespace Student_Information_System.Extentions.Filters
{
    public class ForceWww : ActionFilterAttribute
    {
        private readonly string _baseHost;
        private ActionExecutingContext _filterContext;
        private HttpRequestBase _request;

        public ForceWww(string siteRootUrl)
        {
            _baseHost = new Uri(siteRootUrl).Host.ToLowerInvariant();
        }

        private bool CanIgnoreRequest
        {
            get
            {
                var url = _request.Url;
                return url != null && 
                    (_filterContext.IsChildAction ||
                     _filterContext.HttpContext.Request.IsAjaxRequest() ||
                     url.AbsoluteUri.Contains("?"));
            }
        }

        private bool IsDomainSetCorrectly
        {
            get
            {
                return (_request.Url != null) && (_request.Url.Host == _baseHost);
            }
        }

        private bool IsLocalRequest
        {
            get { return _request.IsLocal; }
        }

        private bool IsRootRequest
        {
            get
            {
                var url = _request.Url;
                return url != null && url.AbsolutePath == "/";
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _filterContext = filterContext;
            _request = _filterContext.RequestContext.HttpContext.Request;
            ModifyUrlAndRedirectPermanent();

            base.OnActionExecuting(filterContext);
        }

        private string AvoidTrailingSlashes(string url)
        {
            if (!IsRootRequest && url.EndsWith("/"))
            {
                url = url.TrimEnd(new[] { '/' });
            }
            return url;
        }

        private void ModifyUrlAndRedirectPermanent()
        {
            if (IsLocalRequest || IsDomainSetCorrectly || CanIgnoreRequest)
                return;

            var url = _request.Url;
            if (url == null)
                return;

            var newUri = new UriBuilder(url) { Host = _baseHost };
            var absoluteUrl = HttpUtility.UrlDecode(newUri.Uri.AbsoluteUri.ToString(CultureInfo.InvariantCulture));
            if (string.IsNullOrWhiteSpace(absoluteUrl))
                return;

            var redirectUrl = absoluteUrl.ToLowerInvariant();
            redirectUrl = AvoidTrailingSlashes(redirectUrl);
            _filterContext.Controller.ViewBag.CanonicalUrl = redirectUrl;

            _filterContext.Result = new RedirectResult(redirectUrl, permanent: true);
        }
    }
}