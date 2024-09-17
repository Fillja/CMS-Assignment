using Microsoft.AspNetCore.Mvc;
using StackExchange.Profiling.Internal;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using UmbracoCMS.Models;

namespace UmbracoCMS.Controllers;

public class ContactSurfaceController : SurfaceController
{
    public ContactSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
    {
    }

    public IActionResult HandleSubmit(ContactFormModel form, bool isMessageRequired, bool isPhoneRequired)
    {
        if (!ModelState.IsValid || isMessageRequired && form.Message == null || isPhoneRequired && form.Phone == null) 
        {
            ViewData["name"] = form.Name;
            ViewData["email"] = form.Email;
            ViewData["phone"] = form.Phone;
            ViewData["message"] = form.Message;

            ViewData["error_name"] = string.IsNullOrEmpty(form.Name);
            ViewData["error_email"] = string.IsNullOrEmpty(form.Email);
            ViewData["error_phone"] = isPhoneRequired && string.IsNullOrEmpty(form.Phone);
            ViewData["error_message"] = isMessageRequired && string.IsNullOrEmpty(form.Message);


            return CurrentUmbracoPage();
        }
        TempData["success"] = "Form submitted successfully!";
        return RedirectToCurrentUmbracoPage();
    }
}
