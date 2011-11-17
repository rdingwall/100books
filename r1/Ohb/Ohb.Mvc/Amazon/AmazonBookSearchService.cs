using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Web;
using Ohb.Mvc.Models;

namespace Ohb.Mvc.Amazon
{
    public class AmazonBookSearchService : IBookSearchService
    {
        readonly string accessKeyId;
        readonly string secretKey;
        readonly AWSECommerceServicePortTypeClient client;

        public AmazonBookSearchService(string accessKeyId, string secretKey)
        {
            if (accessKeyId == null) throw new ArgumentNullException("accessKeyId");
            if (secretKey == null) throw new ArgumentNullException("secretKey");
            this.accessKeyId = accessKeyId;
            this.secretKey = secretKey;
            client = CreateClient(accessKeyId, secretKey);
        }

        static AWSECommerceServicePortTypeClient CreateClient(string accessKeyId, string secretKey)
        {
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport)
                              {
                                  MaxReceivedMessageSize = int.MaxValue,
                                  ReaderQuotas = { MaxStringContentLength = 2 * 1024 * 1024 }
                              };

            var client = new AWSECommerceServicePortTypeClient(
                binding,
                new EndpointAddress("https://webservices.amazon.com/onca/soap?Service=AWSECommerceService"));

            // add authentication to the ECS client
            client.ChannelFactory.Endpoint.Behaviors.Add(new AmazonSigningEndpointBehavior(accessKeyId, secretKey));

            return client;
        }

        public Task<IEnumerable<BookSearchResult>> Search(string terms)
        {
            if (terms == null) throw new ArgumentNullException("terms");

            var itemSearch = CreateRequest(terms);

            return Task.Factory.FromAsync(
                (cb, o) => client.BeginItemSearch(itemSearch, cb, null),
                r => GetResults(client.EndItemSearch(r)),
                null);
        }

        public BookDetails GetBook(string id)
        {
            throw new NotImplementedException();
        }

        static IEnumerable<BookSearchResult> GetResults(ItemSearchResponse response)
        {
            ThrowIfContainsError(response);

            foreach (var item in response.Items[0].Item.OrEmpty())
            {
                string coverImageUrl = "";
                if (item.ImageSets != null)
                    coverImageUrl = item.ImageSets[0].ImageSet[0].ThumbnailImage.URL;

                yield return new BookSearchResult
                                 {
                                     Title = item.ItemAttributes.Title,
                                     Id = item.ASIN,
                                     Author = String.Join(", ", item.ItemAttributes.Author.OrEmpty()),
                                     SmallThumbnailUrl = coverImageUrl
                                 };
            }
        }

        static void ThrowIfContainsError(ItemSearchResponse response)
        {
            if (!response.Items[0].Request.Errors.OrEmpty()
                .Any(e => e.Code != "AWS.ECommerceService.NoExactMatches"))
                return;

            throw new AmazonQueryFailedException(response.Items[0].Request.Errors.First().Message);
        }


        ItemSearch CreateRequest(string terms)
        {
            var request = new ItemSearchRequest
                              {
                                  SearchIndex = "Books",
                                  //Power = String.Format("keywords: {0} and binding: Hardcover or Mass Market Paperback or Paperback or Perfect Paperback or Textbook Binding", terms),
                                  Power = String.Format("binding: Hardcover or Paperback or Textbook"),
                                  Keywords = HttpUtility.UrlEncode(terms),
                                  MerchantId = "ATVPDKIKX0DER",
                                  ResponseGroup = new[] { "Large", "Images" },
                                  //Sort = "reviewrank"
                                  //Power = "binding:Hardcover or Mass Market Paperback or Perfect Paperback or Paperback or Tankobon Hardcover"
                              };

            return new ItemSearch
            {
                Request = new[] { request },
                AWSAccessKeyId = accessKeyId,
                AssociateTag = accessKeyId
            };
        }
    }

    public static class ArrayExtensions
    {
        public static T[] OrEmpty<T>(this T[] items)
        {
            return items ?? new T[0];
        }
    }
}