using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using MarketplaceWeb.Models;
using MarketplaceWeb.Models.Binders;
using SendGrid;

namespace MarketplaceWeb.Controllers
{
	[RoutePrefix("mail")]
	public class MailController : Controller
	{
		// GET: Mail
		//[ValidateAntiForgeryToken]
		[Route("send")]
		public ActionResult Send([ModelBinder(typeof(MailModelBinder))]MailModel model, string redirectUrl)
		{
			var username = ConfigurationManager.AppSettings["SendGridUsername"];
			var password = ConfigurationManager.AppSettings["SendGridPassword"];

			var message = new SendGridMessage();

			message.AddTo(ConfigurationManager.AppSettings["SupportToEmail"]);
			message.From = new MailAddress(model.To, model.FullName);
			message.Subject = model.Subject;
			message.Html = model.FullMailBody;

			var credentials = new NetworkCredential(username, password);
			var transportWeb = new Web(credentials);
			transportWeb.Deliver(message);


		    return Redirect(redirectUrl);
		}
	}
}