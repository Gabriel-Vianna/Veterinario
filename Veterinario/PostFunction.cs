using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Veterinario.Core;
using Infra.Repository;

namespace Veterinario
{
    public static class PostFunction
    {
        [FunctionName("SaveTodos")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            Pet data = JsonConvert.DeserializeObject<Pet>(requestBody);

            if (data == null)
                return new BadRequestObjectResult(new { message = "Dados para cria��o de uma tarefa � obrigatoria" });

            var repository = new CosmoDbRepository();

            data.Id = Guid.NewGuid().ToString();

            await repository.Save(data);

            return new CreatedResult("", data);
        }
    }
}
