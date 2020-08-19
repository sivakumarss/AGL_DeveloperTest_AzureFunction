using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Net.Http;
using AGL_DeveloperTestFunc.Infrastructure;
using AGL_DeveloperTestFunc.Model;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AGL_DeveloperTestFunc
{
    public static class AglDeveloperTestFunction
    {
        [FunctionName("getCats")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var newClient = new HttpClient();
            var aglDeveloperTestApi = newClient.GetStreamAsync(Constants.AGL_DEVELOPERTEST_URL);
            var petOwners = await JsonSerializer.DeserializeAsync<List<PetOwner>>(await aglDeveloperTestApi);

            if (petOwners.Count <= 0)
                throw new NullReferenceException();

            var maleCats = new List<Cat>();
            var femaleCats = new List<Cat>();
            foreach (var owner in petOwners)
            {
                if (owner.Pets != null && owner.Pets.Count > 0)
                {
                    foreach (var pet in owner.Pets)
                    {
                        if (pet.PetType == Constants.CAT)
                        {
                            var cat = new Cat()
                            {
                                CatName = pet.PetName,
                                CatOwnerGender = owner.Gender
                            };

                            if (cat.CatOwnerGender == Constants.MALE)
                            {
                                maleCats.Add(cat);
                            }
                            else if (cat.CatOwnerGender == Constants.FEMALE)
                            {
                                femaleCats.Add(cat);
                            }
                        }
                    }
                }
            }


            var sb = new StringBuilder();
            sb.AppendLine($"---{Constants.MALE}--");
            foreach (var cat in maleCats.Select(c => c).OrderBy(c => c.CatName))
            {
                sb.AppendLine(cat.CatName);
            }

            sb.AppendLine($"---{Constants.FEMALE}--");
            foreach (var cat in femaleCats.Select(c => c).OrderBy(c => c.CatName))
            {
                sb.AppendLine(cat.CatName);
            }

            return new OkObjectResult(sb.ToString());
        }
    }
}
