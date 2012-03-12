﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Ohb.Mvc.Api.ActionFilters;
using Ohb.Mvc.Api.Models;
using Ohb.Mvc.Storage.Books;
using Ohb.Mvc.Storage.PreviousReads;
using Raven.Client.Linq;

namespace Ohb.Mvc.Api.Controllers
{
    public class PreviousReadsController : OhbApiController
    {
        readonly IBookImporter importer;
        readonly IApiModelMapper mapper;

        public PreviousReadsController(IBookImporter importer, IApiModelMapper mapper)
        {
            if (importer == null) throw new ArgumentNullException("importer");
            if (mapper == null) throw new ArgumentNullException("mapper");
            this.importer = importer;
            this.mapper = mapper;
        }

        [RequiresAuthCookie]
        public IEnumerable<PreviousReadModel> Get()
        {
            return DocumentSession
                .Query<PreviousRead, PreviousReadsWithBook>()
                .Where(p => p.UserId == User.Id)
                .OrderByDescending(p => p.MarkedByUserAt)
                .Take(100)
                .As<PreviousReadWithBook>()
                .ToList()
                .Select(mapper.ToPreviousRead);
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
                        Id = PreviousRead.MakeId(User.Id, book.GoogleVolumeId),
                        UserId = User.Id,
                        BookId = book.Id,
                        GoogleVolumeId = book.GoogleVolumeId,
                        GoogleVolumeIdBase64 = ConvertGoogleVolumeId.ToBase64String(volumeId),
                        MarkedByUserAt = DateTime.UtcNow
                    };

            // Overwrite existing
            DocumentSession.Store(previousRead);
            DocumentSession.SaveChanges();
        }

        [RequiresAuthCookie]
        public void Delete(string volumeId)
        {
            if (String.IsNullOrWhiteSpace(volumeId))
                throw new HttpResponseException("Missing parameter: 'volumeId' (Google Book Volume ID)", HttpStatusCode.BadRequest);

            var id = PreviousRead.MakeId(User.Id, volumeId);
            var previousRead = DocumentSession.Load<PreviousRead>(id);
            if (previousRead == null)
                return;

            DocumentSession.Delete(previousRead);
            DocumentSession.SaveChanges();
        }
    }
}