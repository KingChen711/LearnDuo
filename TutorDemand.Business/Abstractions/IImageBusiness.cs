using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorDemand.Business.Abstractions
{
    public interface IImageBusiness
    {
        Task<string> UploadAsynce(IFormFile file);
    }
}
