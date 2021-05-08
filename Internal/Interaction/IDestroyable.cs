using System;

namespace Metozis.TeTwo.Internal.Interaction
{
    public interface IDestroyable
    {
        Action OnDestroyed { get; set; }
        void Destroy();
    }
}