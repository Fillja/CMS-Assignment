using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using UmbracoCMS.Models;
using UmbracoCMS.Services;

namespace UmbracoCMS.Controllers;

public class ContactSurfaceController : SurfaceController
{
    private readonly EmailService _emailService;

    public ContactSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
    {
        _emailService = new EmailService();
    }

    public async Task<IActionResult> HandleSubmit(ContactFormModel form, bool isMessageRequired, bool isPhoneRequired)
    {
        var sectionId = "contact-form";
        var page = CurrentPage?.Url();

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

        var emailSent = await _emailService.SendConfirmationEmailAsync(form.Email);

        if (emailSent)
        {
            TempData["success"] = "Form submitted successfully! A confirmation has been sent to your email.";
        }
        else
        {
            TempData["success"] = "Form submitted successfully, but there was an error in sending confirmation email.";
        }


        return Redirect($"{page}#{sectionId}");
    }

    public async Task<IActionResult> HandleAsideForm(AsideFormModel form)
    {
        var page = CurrentPage?.Url();

        if (!ModelState.IsValid)
        {
            ViewData["email_error"] = "You must enter an email!";

            return CurrentUmbracoPage();
        }

        var emailSent = await _emailService.SendConfirmationEmailAsync(form.Email);

        if (emailSent)
        {
            TempData["success"] = "Form submitted successfully! A confirmation has been sent to your email";
        }
        else
        {
            TempData["success"] = "Form submitted successfully, but there was an error in sending confirmation email.";
        }

        return Redirect($"{page}#aside-form");
    }
}