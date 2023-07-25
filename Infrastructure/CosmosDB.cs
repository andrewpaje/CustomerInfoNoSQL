using Microsoft.Azure.Cosmos;
using System.Net;
using CustomerInfoNoSQL.Models;

namespace CustomerInfoNoSQL.Infrastructure;

public class CosmosDB
{
    private CosmosClient cosmosClient { get; set; }
    public CosmosDB()
    {
        cosmosClient = new(
            accountEndpoint: Environment.GetEnvironmentVariable("COSMOS_ENDPOINT")!,
            authKeyOrResourceToken: Environment.GetEnvironmentVariable("COSMOS_KEY")!
        );
    }

    public async Task<Database> GetDatabase(string databaseId)
    {
        try
        {
            await cosmosClient.CreateDatabaseIfNotExistsAsync(id: databaseId);
            return cosmosClient.GetDatabase(databaseId);
        }
        catch (CosmosException exception) when (exception.StatusCode != HttpStatusCode.Created)
        {
            throw exception;
        }
    }

    public async Task<Container> GetContainer(string databaseId, string containerId, string partitionKey)
    {
        try
        {
            Database cosmosDatabase = await GetDatabase(databaseId);
            await cosmosDatabase.CreateContainerIfNotExistsAsync(
                id: containerId,
                partitionKeyPath: partitionKey,
                throughput: 400
            );
            return cosmosClient.GetContainer(databaseId, containerId);
        }
        catch (CosmosException exception) when (exception.StatusCode != HttpStatusCode.Created)
        {
            throw exception;
        }
    }

    public async Task<CustomerInfo> GetItem(
        string databaseId,
        string containerId,
        string partitionKey,
        string partitionKeyPath,
        string customerId
    )
    {
        try
        {
            Container container = await GetContainer(databaseId, containerId, partitionKeyPath);
            return await container.ReadItemAsync<CustomerInfo>(
                id: customerId,
                partitionKey: new PartitionKey(partitionKey)
            );
        }
        catch (CosmosException exception) when (exception.StatusCode != HttpStatusCode.Created)
        {
            throw exception;
        }
    }

    public async Task<CustomerInfo> AddItem(
        string databaseId,
        string containerId,
        string partitionKey,
        string partitionKeyPath,
        CustomerInfo customerInfo
    )
    {
        try
        {
            Container container = await GetContainer(databaseId, containerId, partitionKeyPath);
            return await container.CreateItemAsync<CustomerInfo>(
                item: customerInfo,
                partitionKey: new PartitionKey(partitionKey)
            );
        }
        catch (CosmosException exception) when (exception.StatusCode != HttpStatusCode.Created)
        {
            throw exception;
        }
    }
}
