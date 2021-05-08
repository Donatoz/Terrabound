using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Metozis.TeTwo.Internal.Generation.World.Framework
{
    public struct Noise : IPlacementMap
    {
        private INoiseGenerator generator;
        private Vector3[] placementPositions;
        private Vector2 constraints;
        private float step;
        private (int, int) offset;
        
        public Noise(INoiseGenerator generator, Vector2 constraints, float step = 0, (int, int) offset = default)
        {
            this.generator = generator;
            this.constraints = constraints;
            this.step = Mathf.Clamp(step, -1f, 1f);
            this.offset = offset.Equals(default) ? (0, 0) : offset;

            placementPositions = new Vector3[0];
            InitializePlacementPositions();
        }

        private void InitializePlacementPositions(bool negate = false)
        {
            var positions = new List<Vector3>();
            for (int i = 0; i < constraints.x; i++)
            {
                for (int j = 0; j < constraints.y; j++)
                {
                    if (!negate)
                    {
                        if (CanPlace(i, j))
                        {
                            positions.Add(new Vector3(i, j, 0));
                        }
                    }
                    else
                    {
                        if (!CanPlace(i, j))
                        {
                            positions.Add(new Vector3(i, j, 0));
                        }
                    }
                }
            }
            
            placementPositions = positions.ToArray();
        }

        public float Generate(float x, float y)
        {
            var result = generator.Generate(offset.Item1 + x, offset.Item2 + y);
            return Mathf.Clamp01(result);
        }
        
        public Vector3[] GetPlacementPositions()
        {
            return placementPositions;
        }

        public Noise OneMinus()
        {
            var newNoise = this;
            newNoise.InitializePlacementPositions(true);
            return newNoise;
        }

        public Noise WithStep(float step)
        {
            this.step = step;
            return this;
        }

        public Noise WithOffset((int, int) offset)
        {
            this.offset = offset;
            return this;
        }

        public Noise Eliminate(float elimination)
        {
            var positions = placementPositions.ToList();
            for (int i = Mathf.RoundToInt((positions.Count - 1) * Mathf.Clamp01(elimination)) ; i >= 0; i--)
            {
                positions.RemoveAt(Random.Range(0, positions.Count - 1));
            }

            placementPositions = positions.ToArray();
            return this;
        }

        public bool CanPlace(float x, float y)
        {
            return x <= constraints.x && y <= constraints.y && Mathf.RoundToInt(Generate(x, y) + step) == 1;
        }
        
        public static Noise operator *(Noise a, Noise b)
        {
            var result = a;
            var newPositions = new List<Vector3>();
            for (int i = 0; i < b.placementPositions.Length; i++)
            {
                if (a.CanPlace(b.placementPositions[i].x, b.placementPositions[i].y))
                {
                    newPositions.Add(b.placementPositions[i]);
                }
            }
            result.placementPositions = newPositions.ToArray();
            return result;
        }

        public static Noise operator +(Noise a, Noise b)
        {
            var positions = a.placementPositions.ToList();
            foreach (var position in b.placementPositions)
            {
                if (!positions.Contains(position))
                {
                    positions.Add(position);
                }
            }

            a.placementPositions = positions.ToArray();
            return a;
        }

        public Noise MultipliedOffset(int offset, float newBias = 0)
        {
            return new Noise(generator, constraints, Mathf.Clamp(newBias, -1, 1)) * this;
        }
    }
}