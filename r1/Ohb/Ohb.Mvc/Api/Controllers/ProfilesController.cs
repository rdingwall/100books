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
        readonly IRecentReadsQuery recentReads;

        public ProfilesController(IApiModelMapper mapper, IRecentReadsQuery recentReads)
        {
            if (mapper == null) throw new ArgumentNullException("mapper");
            if (recentReads == null) throw new ArgumentNullException("recentReads");
            this.mapper = mapper;
            this.recentReads = recentReads;
        }

        public ProfileModel Get(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
                throw new HttpResponseException("Missing parameter: 'id' (User ID)", HttpStatusCode.BadRequest);

            var user = DocumentSession.Load<User>(id);
            if (user == null)
                throw new HttpResponseException("User not found (bad user ID?)", HttpStatusCode.NotFound);

            var previousReads = recentReads.Get(DocumentSession, id);

            return mapper.ToProfile(user, previousReads);
        }
    }
}