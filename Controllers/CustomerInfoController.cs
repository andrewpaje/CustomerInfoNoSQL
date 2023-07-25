using Microsoft.AspNetCore.Mvc;
using CustomerInfoNoSQL.Models;
using CustomerInfoNoSQL.Infrastructure;

namespace CustomerInfoNoSQL.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class CustomerInfoController : ControllerBase
{
    private CosmosDB cosmosDB;

    // public CustomerInfoController(CosmosDB constructorCosmosDB) => cosmosDB = constructorCosmosDB;

    [HttpGet(Name = "GetCustomerInfo")]
    public async Task<CustomerInfo> Get([FromQuery] string id, [FromQuery] string lastName)
    {
        cosmosDB = new CosmosDB();
        return await cosmosDB.GetItem(
            "customerInfoDB",
            "customerInfoContainer",
            lastName,
            "/LastName",
            id
        );
    }

    [HttpPost(Name = "AddCustomerInfo")]
    public async Task<CustomerInfo> Add([FromBody] CustomerInfo info)
    {
        cosmosDB = new CosmosDB();
        CustomerInfo customerInfo = new CustomerInfo
        {
            id = Guid.NewGuid().ToString(),
            FirstName = info.FirstName,
            LastName = info.LastName,
            BirthdayInEpoch = info.BirthdayInEpoch,
            Email = info.Email
        };
        return await cosmosDB.AddItem(
            "customerInfoDB",
            "customerInfoContainer",
            customerInfo.LastName,
            "/LastName",
            customerInfo
        );
    }
}
