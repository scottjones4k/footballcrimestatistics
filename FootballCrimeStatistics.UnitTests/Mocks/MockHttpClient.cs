using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FootballCrimeStatistics.UnitTests.Mocks
{
    public class MockHttpClient
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        public HttpClient Client { get; init; }

        public MockHttpClient()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            Client = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://testing/")
            };
        }

        public void SetReturn(string path, string content)
        {
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(m => m.RequestUri.PathAndQuery == path), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(content),
                });
        }
    }
}
