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

        public SubscriptionRepository(IOptions<DataSourceConfig> dataSourceConfig, ILogger<SubscriptionRepository> logger)
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
            catch (DocumentClientException de)
            {
                    return null;
            }

        }

        public async Task Add(Subscription subscription)
        {
            try
            {
                await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_databaseName, _collectionName),
                    subscription);
            }
            catch (Exception  e)
            {
                _logger.LogError(e.Message);
            }
          
        }

        public async Task Remove(string email)
        {
            try
            {
                var updated = new Subscription {IsActive = false, Email = email};
                await _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_databaseName, _collectionName, email), updated);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
           
        }
    }
}