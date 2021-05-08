using System;
using System.Linq;
using Metozis.TeTwo.Extensions;
using Metozis.TeTwo.Internal.Generation.World.Framework.Filters;
using UnityEngine;

namespace Metozis.TeTwo.Internal.Generation.World.Processes
{
    public readonly struct PositionNormalizer : IPlacementChanger
    {
        private readonly Vector2 direction;
        private readonly MetaFilter[] mask;
        private readonly int maxDepth;
        private readonly bool forceStop;

        public PositionNormalizer(Vector2 direction, int maxDepth, MetaFilter[] mask = null, bool forceStop = false)
        {
            this.direction = direction;
            this.mask = mask;
            this.maxDepth = maxDepth;
            this.forceStop = forceStop;
        }
        
        public Vector3 Change(Vector3 input, WorldGenerationMeta meta)
        {
            var currentPos = input;

            if (meta.BlockData.ContainsKey(currentPos))
            {
                throw new PlacementChangeFailedException();
            }
            
            var currentDepth = 0;
            
            while (currentDepth <= maxDepth)
            {
                var next = currentPos + direction.Upper();
                if (meta.BlockData.ContainsKey(next))
                {
                    if (mask != null)
                    {
                        var pass = mask.All(filter => filter.Resolve(meta.BlocksMeta[meta.BlockData[next]]));
                        if (!pass)
                        {
                            if (forceStop)
                            {
                                throw new PlacementChangeFailedException();
                            }

                            continue;
                        }
                    }

                    return currentPos;
                }
                
                currentPos = next;
                currentDepth++;
            }

            throw new PlacementChangeFailedException();
        }
    }
}