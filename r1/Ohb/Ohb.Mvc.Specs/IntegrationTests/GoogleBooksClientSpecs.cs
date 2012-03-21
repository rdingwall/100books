using System.Linq;
using Machine.Specifications;
using Ohb.Mvc.Google;

namespace Ohb.Mvc.Specs.IntegrationTests
{
    [Subject(typeof(GoogleBooksClient))]
    public class GoogleBooksClientSpecs
    {
        public class when_looking_up_a_book_by_id
        {
            Establish context =
                () => service = new GoogleBooksClient(apiKey: "AIzaSyAwesvnG7yP5wCqiNv21l8g7mo-ehkcVJs");

            Because of = () => volume = service.GetVolume("a52a_F-OKUcC");

            It should_return_some_results = () => volume.ShouldNotBeNull();

            It should_get_the_books_id =
                () => volume.Id.ShouldEqual("a52a_F-OKUcC");

            It should_get_the_books_title = 
                () => volume.VolumeInfo.Title.ShouldEqual("LEGO");

            It should_get_the_books_subtitle =
                () => volume.VolumeInfo.SubTitle.ShouldEqual("A Love Story");

            It should_get_the_books_authors = 
                () => volume.VolumeInfo.Authors.ShouldContainOnly("Jonathan Bender");

            It should_get_the_books_publishers =
                () => volume.VolumeInfo.Publisher.ShouldEqual("John Wiley and Sons");

            It should_get_the_books_published_date = 
                () => volume.VolumeInfo.PublishedDate.ShouldEqual("2010-05-03");

            It should_get_the_books_description =
                () => volume.VolumeInfo.Description.ShouldEqual("\u003cb\u003eAn adult LEGO fan's dual quest: to build with bricks and build a family\u003c/b\u003e \u003cp\u003e There are 62 LEGO bricks for every person in the world, and at age 30, Jonathan Bender realized that he didn't have a single one of them. While reconsidering his childhood dream of becoming a master model builder for The LEGO Group, he discovers the men and women who are skewing the averages with collections of hundreds of thousands of LEGO bricks. What is it about the ubiquitous, brightly colored toys that makes them so hard for everyone to put down? \u003cp\u003e In search of answers and adventure, Jonathan Bender sets out to explore the quirky world of adult fans of LEGO (AFOLs) while becoming a builder himself. As he participates in challenges at fan conventions, searches for the largest private collection in the United States, and visits LEGO headquarters (where he was allowed into the top secret set vault), he finds his LEGO journey twinned with a second creative endeavor—to have a child. His two worlds intertwine as he awaits the outcome: Will he win a build competition or bring a new fan of LEGO into the world? Like every really good love story, this one has surprises—and a happy ending. \u003cul\u003e \u003cli\u003eExplores the world of adult fans of LEGO, from rediscovering the childhood joys of building with LEGO to evaluating LEGO's place in culture and art \u003cli\u003eTakes an inside look at LEGO conventions, community taboos, and build challenges and goes behind-the-scenes at LEGO headquarters and LEGOLAND \u003cli\u003eTells a warm and personal story about the attempt to build with LEGO and build a family \u003c/ul\u003e \u003cp\u003e Whether you're an avid LEGO freak or a onetime fan who now shares LEGO bricks with your children, this book will appeal to the inner builder in you and reignite a love for all things LEGO.");

            It should_get_the_books_isbn_10 =
                () =>
                volume.VolumeInfo.IndustryIdentifiers.Single(i => i.Type == "ISBN_10")
                    .Identifier.ShouldEqual("0470407026");

            It should_get_the_books_isbn_13 =
                () =>
                volume.VolumeInfo.IndustryIdentifiers.Single(i => i.Type == "ISBN_13")
                    .Identifier.ShouldEqual("9780470407028");

            It should_get_the_books_page_count =
                () => volume.VolumeInfo.PageCount.ShouldEqual(290);

            It should_get_the_books_thumbnail_url =
                () => volume.VolumeInfo.ImageLinks.Thumbnail.ShouldEqual("http://bks2.books.google.co.uk/books?id=a52a_F-OKUcC&printsec=frontcover&img=1&zoom=1&edge=curl&source=gbs_api");
           
            It should_get_the_books_small_thumbnail_url =
                () => volume.VolumeInfo.ImageLinks.SmallThumbnail.ShouldEqual("http://bks2.books.google.co.uk/books?id=a52a_F-OKUcC&printsec=frontcover&img=1&zoom=5&edge=curl&source=gbs_api");

            It should_get_the_books_small_img_url =
                () => volume.VolumeInfo.ImageLinks.Small.ShouldEqual("http://bks2.books.google.co.uk/books?id=a52a_F-OKUcC&printsec=frontcover&img=1&zoom=2&edge=curl&source=gbs_api");

            It should_get_the_books_medium_img_url =
                () => volume.VolumeInfo.ImageLinks.Medium.ShouldEqual("http://bks2.books.google.co.uk/books?id=a52a_F-OKUcC&printsec=frontcover&img=1&zoom=3&edge=curl&source=gbs_api");

            It should_get_the_books_langage =
                () => volume.VolumeInfo.Language.ShouldEqual("en");

            It should_get_the_books_canonical_url =
                () =>
                volume.VolumeInfo.CanonicalVolumeLink.ShouldEqual(
                    "http://books.google.co.uk/books/about/LEGO.html?id=a52a_F-OKUcC");

            // seems to be the same as the google URL?
            It should_get_the_books_preview_url =
                () =>
                volume.VolumeInfo.PreviewLink.ShouldEqual(
                    "http://books.google.co.uk/books?id=a52a_F-OKUcC&source=gbs_api");

            It should_get_the_books_web_reader_url =
                () =>
                volume.AccessInfo.WebReaderLink.ShouldEqual(
                    "http://books.google.co.uk/books/reader?id=a52a_F-OKUcC&printsec=frontcover&output=reader&source=gbs_api");

            It should_get_the_books_categories =
                () => volume.VolumeInfo.Categories.ShouldContain("Bender, Jonathan - Travel",
                                                                 "Handicraft - Competitions",
                                                                 "Handicraft/ Competitions",
                                                                 "Handicraft",
                                                                 "Journalists - Travel - United States",
                                                                 "Journalists/ Travel/ United States",
                                                                 "Journalists",
                                                                 "LEGO Group",
                                                                 "LEGO koncernen (Denmark)",
                                                                 "LEGO toys",
                                                                 "Toys - Psychological aspects",
                                                                 "Toys",
                                                                 "Antiques & Collectibles / Toys",
                                                                 "Biography & Autobiography / Editors, Journalists, Publishers",
                                                                 "Biography & Autobiography / Personal Memoirs",
                                                                 "Crafts & Hobbies / General",
                                                                 "Crafts & Hobbies / Models",
                                                                 "Sports & Recreation / General",
                                                                 "Antiques & Collectibles / Reference");

            It should_get_the_books_main_category =
                () =>
                volume.VolumeInfo.MainCategory.ShouldEqual(
                    "Biography & Autobiography / Editors, Journalists, Publishers");

            static IGoogleBooksClient service;
            static GoogleVolume volume;
        }
    }
}
