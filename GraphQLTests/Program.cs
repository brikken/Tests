using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;

namespace GraphQLTests
{
    class Program
    {
        static void Main(string[] args)
        {
            //HelloWorld();
            //SchemaFirst();
            Parameters.Run();
        }

        static void HelloWorld()
        {
            var schema = Schema.For(@"
  type Query {
    hello: String
  }
");

            var root = new { Hello = "Hello World!" };
            var json = schema.Execute(_ =>
            {
                _.Query = "{ hello }";
                _.Root = root;
            });

            Console.WriteLine(json);
        }

        public class Droid
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        public class Query
        {
            [GraphQLMetadata("droid")]
            public Droid GetDroid()
            {
                return new Droid { Id = "123", Name = "R2-D2" };
            }
        }

        static void SchemaFirst()
        {
            var schema = Schema.For(@"
  type Droid {
    id: ID
    name: String
  }

  type Query {
    droid: Droid
  }
", _ => {
                _.Types.Include<Query>();
            });

            var json = schema.Execute(_ =>
            {
                _.Query = "{ droid { id name } }";
            });
            Console.WriteLine(json);
        }
    }
}
