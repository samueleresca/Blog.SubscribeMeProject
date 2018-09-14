using System;
using System.Threading.Tasks;
using Blog.SubscribeMeProject.Infrastructure.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Blog.SubscribeMeProject.Infrastructure.Repositories
{
    class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly DocumentClient _client;
        private readonly string _databaseName;
        private readonly string _collectionName;
        private readonly ILogger<SubscriptionRepository> _logger;

        public SubscriptionRepository(IOptions<ConnectionStrings> dataSourceConfig, ILogger<SubscriptionRepository> logger)
        {
            _client = new DocumentClient(new Uri(dataSourceConfig.Value.EndpointURI),
                dataSourceConfig.Value.PrimaryKey);
            _databaseName = dataSourceConfig.Value.DatabaseName;
            _collectionName = dataSourceConfig.Value.CollectionName;
            _logger = logger;
        }

        public async Task<Subscription> Get(string email)
        {
            try
            {
                return await _client.ReadDocumentAsync<Subscription>(
                    UriFactory.CreateDocumentUri(_databaseName, _collectionName, email));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode.HasValue &&
                   e.StatusCode.Value == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                throw e;
            }

        }

        public async Task Add(Subscription subscription)
        {
            await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_databaseName, _collectionName),
                subscription);
        }

        public async Task Remove(string email)
        {
            var updated = new Subscription { IsActive = false, Email = email };
            await _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_databaseName, _collectionName, email), updated);
        }
    }
}