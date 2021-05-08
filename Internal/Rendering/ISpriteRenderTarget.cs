using System;
using UnityEngine;

namespace Metozis.TeTwo.Internal.Rendering
{
    public interface ISpriteRenderTarget : IRenderTarget
    {
        Action OnSpriteChanged { get; set; }
        void ChangeSprite(Sprite sprite);
    }
}