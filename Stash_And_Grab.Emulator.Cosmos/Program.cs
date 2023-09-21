using Microsoft.Azure.Cosmos;

const string endpointUrl = "https://localhost:8081";
const string
    primaryKey =
        "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="; // Emulator Key
const string databaseId = "TestDatabase";
const string containerId = "TestContainer";


var cosmosClient = new CosmosClient(endpointUrl, primaryKey);

// Create database
var database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);

// Create container
var container = await database.Database.CreateContainerIfNotExistsAsync(containerId, "/partitionKey");

// Insert a sample item
var testItem = new { id = "1", name = "Test Item", partitionKey = "testPartition" };
await container.Container.CreateItemAsync(testItem, new PartitionKey(testItem.partitionKey));

Console.WriteLine("Test data inserted!");