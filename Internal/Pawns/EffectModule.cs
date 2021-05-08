using System;
using Metozis.TeTwo.Internal.Entities;
using Metozis.TeTwo.Internal.Rendering;
using UnityEngine;

namespace Metozis.TeTwo.Internal.Pawns
{
    public class EffectModule : IModule
    {
        public bool Enabled { get; private set; }
        private readonly IRenderTarget target;

        public Action<Renderer, Color> ChangeColorContext = delegate(Renderer renderer, Color color)
        {
            renderer.material.color = color;
        };
        
        public EffectModule(IRenderTarget target)
        {
            this.target = target;
        }

        public void ChangeColor(Color color)
        {
            ChangeColorContext(target.Renderer, color);
        }

        public void ChangeScale(float scale)
        {
            target.Renderer.gameObject.transform.localScale = Vector3.one * scale;
        }
    }
}