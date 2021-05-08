using Metozis.TeTwo.Internal.Entities.Meta;

namespace Metozis.TeTwo.Internal.Entities.Factories
{
    public abstract class EntityFactory
    {
        public static string TemplatePath = "Templates/";
        
        public abstract T Create<T>(EntityMeta options) where T : Entity;
    }
}