using System;

namespace Metozis.TeTwo.Internal.Interaction
{
    public interface ISelectable
    {
        Action<bool> OnSelected { get; set; }
        void Select();
        void DeSelect();
    }
}