using System;
using UnityEngine;

namespace Metozis.TeTwo.Internal.Rendering
{
    public interface IRenderTarget
    {
        Action OnRendered { get; set; }
        Renderer Renderer { get; }
        void Render();
    }
}