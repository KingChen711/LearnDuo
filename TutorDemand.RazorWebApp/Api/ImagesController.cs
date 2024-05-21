using Microsoft.AspNetCore.Mvc;
using System.Net;
using TutorDemand.Business.Abstractions;

namespace TutorDemand.RazorWebApp.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : Controller
    {
        private readonly IImageBusiness imageBusiness;

        public ImagesController(IImageBusiness imageBusiness)
        {
            this.imageBusiness = imageBusiness;
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            var imageUrl = await imageBusiness.UploadAsynce(file);
            if (imageUrl == null)
            {
                return Problem("Something went wrong!", null, (int)HttpStatusCode.InternalServerError);
            }
            return Json(new { link = imageUrl });
        }
    }
}
