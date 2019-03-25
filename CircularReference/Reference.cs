using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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

        public override string ToString()
        {
            return Id.ToString();
        }
    }

    public class ReferenceValidator<T>
    {
        private readonly Func<T, T> refExtract;
        private readonly T initial;
        private HashSet<T> refs = new HashSet<T>();

        public ReferenceValidator(Func<T, T> refExtract, T initial)
        {
            this.refExtract = refExtract;
            this.initial = initial;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="CircularReferenceException">Thrown if an object is encountered more than once</exception>
        public void Validate()
        {
            WalkReferences(initial);
        }

        private void WalkReferences(T @ref)
        {
            if (!refs.Add(@ref))
                throw new CircularReferenceException($"{@ref.ToString()} already in the reference stack");
            WalkReferences(refExtract.Invoke(@ref));
        }
    }

    public class CircularReferenceException : Exception
    {
        public CircularReferenceException()
        {
        }

        public CircularReferenceException(string message) : base(message)
        {
        }

        public CircularReferenceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CircularReferenceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
