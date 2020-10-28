using Microsoft.AspNetCore.Mvc;

namespace RemindersManagement.API.Controllers
{
    public class HomeController : ControllerBase
    {
        [Route(""), HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public RedirectResult RedirectToSwaggerUI()
        {
            return Redirect("/swagger/");
        }
    }
}
