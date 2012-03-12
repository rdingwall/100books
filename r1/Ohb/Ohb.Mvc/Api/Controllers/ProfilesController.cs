using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using Ohb.Mvc.Api.Models;
using Ohb.Mvc.Storage.PreviousReads;
using Ohb.Mvc.Storage.Users;
using Raven.Client.Linq;

namespace Ohb.Mvc.Api.Controllers
{
    public class ProfilesController : OhbApiController
    {
        readonly IApiModelMapper mapper;

        public ProfilesController(IApiModelMapper mapper)
        {
            if (mapper == null) throw new ArgumentNullException("mapper");
            this.mapper = mapper;
        }

        public ProfileModel Get(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
                throw new HttpResponseException("Missing parameter: 'id' (User ID)", HttpStatusCode.BadRequest);

            var user = DocumentSession.Load<User>(id);

            if (user == null)
                throw new HttpResponseException("User not found (bad user ID?)", HttpStatusCode.NotFound);

            // Todo extract this into a query object
            var previousReads = DocumentSession
                .Query<PreviousRead, PreviousReadsWithBook>()
                .Where(p => p.UserId == id)
                .OrderByDescending(p => p.MarkedByUserAt)
                .Take(100)
                .As<PreviousReadWithBook>()
                .ToList();

            return mapper.ToProfile(user, previousReads);
        }
    }
}