using JetBrains.Annotations;

namespace Metozis.TeTwo.Internal.Modification
{
    public readonly struct Modifier<T>
    {
        [CanBeNull] 
        public readonly string Id;
        public readonly T Value;

        public Modifier(T value, string id = null)
        {
            Id = id;
            Value = value;
        }
    }
}