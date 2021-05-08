using Metozis.TeTwo.Internal.Entities.Meta;

namespace Metozis.TeTwo.Internal.Generation.World.Rules
{
    public abstract class GenerationRule
    {
        public string Name;
        public bool Pass;
        public int Order;

        public abstract void Resolve(ref WorldGenerationMeta result);
    }
}