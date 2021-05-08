namespace Metozis.TeTwo.Internal.Entities
{
    public enum RuntimeModuleType
    {
        Default,
        Fixed,
        Late
    }
    
    public interface IRuntimeModule : IModule
    {
        RuntimeModuleType Type { get; }
        void Update();
    }
}