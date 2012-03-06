using System.Linq;
using Ohb.Mvc.Storage.Books;
using Raven.Client.Indexes;

namespace Ohb.Mvc.Storage.PreviousReads
{
    public class PreviousReadsWithBook : AbstractIndexCreationTask<PreviousRead>
    {
        public PreviousReadsWithBook()
        {
            Map = previousReads => from p in previousReads
                                   select new {p.UserId, p.MarkedByUserAt, p.BookId};

            TransformResults =
                (database, previousReads) =>
                from p in previousReads
                select
                    new
                        {
                            p.MarkedByUserAt,
                            Book = database.Load<Book>(p.BookId)
                        };

        }
    }
}