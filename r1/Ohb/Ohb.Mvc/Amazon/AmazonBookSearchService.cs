using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Ohb.Mvc.Amazon
{
    public interface IAmazonBookSearchService
    {
        Task<IEnumerable<IBook>> Search(string terms);
    }

    public class AmazonBookSearchService : IAmazonBookSearchService
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
                                  MaxReceivedMessageSize = int.MaxValue
                              };

            var client = new AWSECommerceServicePortTypeClient(
                binding,
                new EndpointAddress("https://webservices.amazon.com/onca/soap?Service=AWSECommerceService"));

            // add authentication to the ECS client
            client.ChannelFactory.Endpoint.Behaviors.Add(new AmazonSigningEndpointBehavior(accessKeyId, secretKey));

            return client;
        }

        public Task<IEnumerable<IBook>> Search(string terms)
        {
            if (terms == null) throw new ArgumentNullException("terms");

            var itemSearch = CreateRequest(terms);

            return Task.Factory.FromAsync(
                (cb, o) => client.BeginItemSearch(itemSearch, cb, null),
                r => GetResults(client.EndItemSearch(r)),
                null);
        }

        static IEnumerable<IBook> GetResults(ItemSearchResponse response)
        {
            ThrowIfContainsError(response);

            foreach (var item in response.Items[0].Item.OrEmpty())
            {
                string coverImageUrl = "";
                if (item.ImageSets != null)
                    coverImageUrl = item.ImageSets[0].ImageSet[0].ThumbnailImage.URL;

                yield return new Book(title: item.ItemAttributes.Title,
                                      asin: item.ASIN,
                                      author: String.Join(", ", item.ItemAttributes.Author.OrEmpty()),
                                      coverImageUrl: coverImageUrl);
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
                                  Title = terms,
                                  ResponseGroup = new[] { "Small", "Images" },
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