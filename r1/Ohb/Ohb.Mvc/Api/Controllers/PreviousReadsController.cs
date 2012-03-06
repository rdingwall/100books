﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Ohb.Mvc.Api.ActionFilters;
using Ohb.Mvc.Storage.Books;
using Ohb.Mvc.Storage.PreviousReads;

namespace Ohb.Mvc.Api.Controllers
{
    public class PreviousReadsController : OhbApiController
    {
        readonly IBookImporter importer;

        public PreviousReadsController(IBookImporter importer)
        {
            if (importer == null) throw new ArgumentNullException("importer");
            this.importer = importer;
        }

        [RequiresAuthCookie]
        public IEnumerable<PreviousRead> Get()
        {
            return DocumentSession.Query<PreviousRead>()
                .Where(p => p.UserId == User.Id)
                .OrderByDescending(p => p.MarkedByUserAt);
        }

        [RequiresAuthCookie]
        public void Put(string volumeId)
        {
            if (String.IsNullOrWhiteSpace(volumeId))
                throw new HttpResponseException("Missing parameter: 'volumeId' (Google Book Volume ID)", HttpStatusCode.BadRequest);

            var book = importer.GetBook(DocumentSession, volumeId);
            if (book == null)
                throw new HttpResponseException("Book not found (bad Google Book Volume ID?)",
                                                HttpStatusCode.NotFound);

            var previousRead =
                new PreviousRead
                    {
                        Id = String.Format("PreviousReads/{0}-{1}", User.Id, book.GoogleVolumeId),
                        UserId = User.Id,
                        Book = book,
                        MarkedByUserAt = DateTime.UtcNow
                    };

            // Overwrite existing
            DocumentSession.Store(previousRead);
            DocumentSession.SaveChanges();
        }
    }
}