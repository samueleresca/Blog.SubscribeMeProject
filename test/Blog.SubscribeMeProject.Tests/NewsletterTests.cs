using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace Blog.SubscribeMeProject.Tests
{
    public class NewsletterTests : IClassFixture<FunctionalTestFixture<Startup>>
    {
        public HttpClient Client { get; set; }
        
        public NewsletterTests(FunctionalTestFixture<Startup> fixture)
        {
            Client = fixture.CreateClient();
            if (Client.BaseAddress.ToString().StartsWith("https://", StringComparison.Ordinal) == false)
                Client.BaseAddress = new Uri("https://localhost");
        }
        
        [Fact]
        public async Task GetNewsletter_ShouldReturn_Ok()
        {
            // Arrange & Act
            var response = await Client.GetAsync("/api/newsletter/samuele.resca@gmail.com");
 
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetNewsletter_ShouldReturn_Not_Found()
        {
            // Arrange & Act
            var response = await Client.GetAsync("/api/newsletter/not.found@gmail.com");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetNewsletter_ShouldReturn_BadRequest_With_Invalid_Email()
        {
            // Arrange & Act
            var response = await Client.GetAsync("/api/newsletter/invalid.com");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PostNewsletter_ShouldReturn_BedRequest_With_Existing_Email()
        {
            // Arrange & Act
            var response = await Client.PostAsync("/api/newsletter/", new StringContent("{\"email\": \"samuele.resca@gmail.com\",\"isActive\": false}", Encoding.UTF8, "application/json"));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [Fact]
        public async Task PostNewsletter_ShouldReturn_Created()
        {
            // Arrange & Act
            var response = await Client.PostAsync("/api/newsletter/", new StringContent("{\"email\": \"samuele.resca"+Guid.NewGuid()+"@gmail.com\",\"isActive\": false}", Encoding.UTF8, "application/json") );

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task DeleteNewsletter_ShouldReturn_NoContent()
        {
            // Arrange & Act
            var response = await Client.DeleteAsync("/api/newsletter/samuele.resca@gmail.com");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteNewsletter_ShouldReturn_BadRequest_With_Invalid_Email()
        {
            // Arrange & Act
            var response = await Client.DeleteAsync("/api/newsletter/invalid.com");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        
    }
}