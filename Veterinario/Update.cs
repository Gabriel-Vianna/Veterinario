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
using Veterinario.Core;

namespace Veterinario
{
    public static class Update
    {
        [FunctionName("Update")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)] HttpRequest req,
            ILogger log)
        {
            Guid id = new Guid(req.Query["id"]);

            var repository = new CosmoDbRepository();

            var pet = repository.GetById(id.ToString());

            if (pet == null)
                return new NotFoundObjectResult(new { message = "Não existe pet com este Id" });

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            await repository.Update(pet);

            var newPet = pet;

            return new OkObjectResult(newPet);
        }
    }
}
