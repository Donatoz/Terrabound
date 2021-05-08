using System;
using Metozis.TeTwo.Internal.Generation.World.Framework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Metozis.TeTwo.Components.Visual
{
    [ExecuteInEditMode]
    public class NoiseRenderer : MonoBehaviour
    {
        public FastNoiseLite.NoiseType Type = FastNoiseLite.NoiseType.Perlin;
        [Range(0, 4096)]
        public int Width = 256;
        [Range(0, 4096)]
        public int Height = 256;
        public int Seed = 0;
        public float Scale = 1;
        public Renderer renderer;

        public bool Discrete;

        [Range(0, 1)]
        public float Step;

        private Noise noise;
        private INoiseGenerator generator;
        
        private void OnDrawGizmos()
        {
            generator = new NoiseGenerators.FastNoiseGenerator(Seed, Type);

            if (Discrete)
            {
                noise = new Noise(generator, new Vector2(Width, Height), Step);
                renderer.material.mainTexture = GenerateDiscrete();
            }
            else
            {
                renderer.material.mainTexture = Generate();
            }
        }

        private Texture2D Generate()
        {
            var tex = new Texture2D(Width, Height);
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    var color = GetPixelColor(i, j);
                    tex.SetPixel(i, j, color);
                }
            }
            tex.Apply();
            return tex;
        }
        
        private Texture2D GenerateDiscrete()
        {
            var tex = new Texture2D(Width, Height);
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    tex.SetPixel(i, j, noise.CanPlace(i, j) ? Color.white : Color.black);
                }
            }
            tex.Apply();
            return tex;
        }

        private Color GetPixelColor(int x, int y)
        {
            var val = generator.Generate(x * Scale, y * Scale);
            return new Color(val + Step, val + Step, val + Step);
        }
    }
}