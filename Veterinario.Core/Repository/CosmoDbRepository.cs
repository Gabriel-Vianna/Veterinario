using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinario.Core;

namespace Infra.Repository
{
    public class CosmoDbRepository
    {
        private string ConnectionString = "AccountEndpoint=https://cosmos-db-infnet.documents.azure.com:443/;AccountKey=aJfYGYHj2M82OzRQHsv5JRSsrO8SqcRd9JSDKLFXYisXESaDheQIqdN2k7dHolDGacRIbYqSCx94SK7lcftIDg==;";
        private string Container = "pet";
        private string Database = "veterinario";

        private CosmosClient CosmosClient { get; set; }

        public CosmoDbRepository()
        {
            this.CosmosClient = new CosmosClient(this.ConnectionString);
        }

        public List<Pet> GetAll()
        {
            var container = this.CosmosClient.GetContainer(Database, Container);

            QueryDefinition queryDefinition = new QueryDefinition("SELECT * FROM c");

            var result = new List<Pet>();

            var queryResult = container.GetItemQueryIterator<Pet>(queryDefinition);

            while (queryResult.HasMoreResults)
            {
                FeedResponse<Pet> currentResultSet = queryResult.ReadNextAsync().Result;
                result.AddRange(currentResultSet.Resource);
            }

            return result;

        }

        public Pet GetById(string id)
        {
            var container = this.CosmosClient.GetContainer(Database, Container);

            QueryDefinition queryDefinition = new QueryDefinition($"SELECT * FROM c where c.id = '{id}'");

            var queryResult = container.GetItemQueryIterator<Pet>(queryDefinition);

            Pet item = null;

            while (queryResult.HasMoreResults)
            {
                FeedResponse<Pet> currentResultSet = queryResult.ReadNextAsync().Result;
                item = currentResultSet.Resource.FirstOrDefault();
            }

            return item;
        }

        public async Task Save(Pet item)
        {
            var container = this.CosmosClient.GetContainer(Database, Container);
            await container.CreateItemAsync<Pet>(item, new PartitionKey(item.PartitionKey));
        }

        public async Task Update(Pet item)
        {
            var container = this.CosmosClient.GetContainer(Database, Container);
            await container.ReplaceItemAsync<Pet>(item, item.Id.ToString(), new PartitionKey(item.PartitionKey));
        }

        public async Task Delete(Pet item)
        {
            var container = this.CosmosClient.GetContainer(Database, Container);
            await container.DeleteItemAsync<Pet>(item.Id.ToString(), new PartitionKey(item.PartitionKey));
        }
    }
}
