﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircularReference
{
    public class Reference
    {
        public static void CheckCircular(ReferenceTest initial)
        {
            var refs = new List<ReferenceTest>();
            WalkReferences(initial, refs);
        }

        private static void WalkReferences(ReferenceTest @ref, IList<ReferenceTest> refs)
        {
            if (refs.Contains(@ref))
                throw new Exception($"Circular reference ({refs.Last().Id} referenced {@ref.Id} which was element {refs.IndexOf(@ref)} in the stack)");
            refs = refs.Append(@ref).ToList();
            WalkReferences(@ref.Ref, refs);
        }
    }

    public class ReferenceTest
    {
        public ReferenceTest Ref { get; set; }
        public int Id { get; set; }
    }
}
