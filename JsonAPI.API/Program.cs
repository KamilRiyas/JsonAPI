using System.Text.Json.Nodes;

namespace JsonAPI.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string fileName = "data.json";
            using FileStream openStream = File.OpenRead(fileName);
            var jsonContent = JsonNode.Parse(openStream);

            var builder = WebApplication.CreateBuilder(args);

            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            app.MapGet("/{entity}", (string entity) => {
                return jsonContent[entity] is not null ? jsonContent[entity].ToJsonString():"No Entity found named "+entity;
            });

            app.MapGet("/{entity}/{id}", (string entity, string id) =>
            {
                if (jsonContent[entity] != null)
                {
                    if (jsonContent[entity][0]["id"] != null)
                    {
                        var result = jsonContent[entity].AsArray().Where(x => x["id"].ToString() == id).ToList();
                        return result.FirstOrDefault().ToJsonString();
                    } else
                    {
                        return "No id field found for entity "+ entity;
                    }
                    return jsonContent.ToJsonString();
                }
                else
                {
                    return "No Entity found named " + entity;
                }
            });

            app.Run();
        }
    }
}