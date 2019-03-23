using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQLTests
{
    public class Parameters
    {
        public class Droid
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        public class Query
        {
            private List<Droid> _droids = new List<Droid>
                {
                    new Droid { Id = "123", Name = "R2-D2" },
                    new Droid { Id = "456", Name = "R2-D2" }
                };

            [GraphQLMetadata("droid")]
            public Droid GetDroid(string id)
            {
                return _droids.FirstOrDefault(x => x.Id == id);
            }
        }

        public static void Run()
        {
            var schema = Schema.For(@"
  type Droid {
    id: ID
    name: String
  }

  type Query {
    droid(id: ID): Droid
  }
", _ =>
        {
            _.Types.Include<Query>();
        });

            var json = schema.Execute(_ =>
            {
                _.Query = $"{{ droid(id: \"123\") {{ id name }} }}";
            });

            Console.WriteLine(json);
        }
    }
}