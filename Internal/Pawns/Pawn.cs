using System;
using Metozis.TeTwo.Internal.Entities;
using Metozis.TeTwo.Internal.Entities.Factories;
using Metozis.TeTwo.Internal.Interaction;

namespace Metozis.TeTwo.Internal.Pawns
{
    public abstract class Pawn : Entity, ISelectable, IDestroyable
    {
        public Action<bool> OnSelected { get; set; }
        public Action OnDestroyed { get; set; }
        
        public virtual void Select()
        {
            OnSelected?.Invoke(true);
        }

        public virtual void DeSelect()
        {
            OnSelected?.Invoke(false);
        }

        public virtual void Destroy()
        {
            OnDestroyed?.Invoke();
            UnityEngine.MonoBehaviour.Destroy(gameObject);
        }
    }
}