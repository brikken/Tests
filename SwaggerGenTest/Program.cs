using Newtonsoft.Json;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SwaggerGenTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            var httpConfig = new HttpConfiguration();

            WebApiConfig.Register(httpConfig);
            httpConfig.EnsureInitialized();

            var swaggerProvider = new SwaggerGenerator(
                httpConfig.Services.GetApiExplorer(),
                httpConfig.Formatters.JsonFormatter.SerializerSettings,
                new Dictionary<string, Info> { { "v1", new Info { version = "v1", title = "My API" } } },
                new SwaggerGeneratorOptions(
                    schemaIdSelector: (type) => type.FriendlyId(true),
                    conflictingActionsResolver: (apiDescriptions) => apiDescriptions.First()
                )
            );
            var swaggerDoc = swaggerProvider.GetSwagger("http://tempuri.org/api", "v1");

            var swaggerString = JsonConvert.SerializeObject(
                swaggerDoc,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }
            );

            Console.WriteLine(swaggerString);
        }
    }
}