using System.Linq;
using System.Reflection;
using Ohb.Mvc.Models;

namespace Ohb.Mvc.Api.Controllers
{
    public class VersionController : OhbApiController
    {
        public AppVersionModel Get()
        {
            return AppVersionModel.Current;
        }
    }
}