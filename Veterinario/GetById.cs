using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Infra.Repository;

namespace Veterinario
{
    public static class GetById
    {
        [FunctionName("GetById")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            Guid id = new Guid(req.Query["id"]);

            var repository = new CosmoDbRepository();

            var pet = repository.GetById(id.ToString());

            if (pet == null)
                return new NotFoundObjectResult(new { message = "Não existe pet com este Id" });

            return new OkObjectResult(pet);
        }
    }
}
