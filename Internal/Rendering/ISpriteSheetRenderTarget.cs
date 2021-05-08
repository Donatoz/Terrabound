using UnityEngine;

namespace Metozis.TeTwo.Internal.Rendering
{
    public interface ISpriteSheetRenderTarget : ISpriteRenderTarget
    {
        Sprite[] SpriteSheet { get; set; }
        void PickSprite(int id);
    }
}