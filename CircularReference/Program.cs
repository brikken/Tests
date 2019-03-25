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
            try
            {
                Reference.CheckCircular(refA);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Caught circular reference exception: {e.Message}");
            }

            var refVal = new ReferenceValidator<ReferenceTest>(r => r.Ref, refA);
            try
            {
                refVal.Validate();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Caught CircularReferenceException: {e.Message}");
            }
        }
    }
}
