using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircularReference
{
    class Program
    {
        static void Main(string[] args)
        {
            var refA = new ReferenceTest() { Id = 1 };
            var refB = new ReferenceTest() { Id = 2, Ref = refA };
            var refC = new ReferenceTest() { Id = 3, Ref = refB };
            refA.Ref = refC;
            Reference.CheckCircular(refA);
        }
    }
}
