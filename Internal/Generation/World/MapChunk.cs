using System;
using System.Collections.Generic;
using Metozis.TeTwo.Internal.Generation.World.Rules;
using UnityEngine;

namespace Metozis.TeTwo.Internal.Generation.World
{
    [Serializable]
    public class MapChunk
    {
        public List<BlockGenerationVariant> Blocks;
        public Dictionary<int, List<Vector3>> PlacementVariants;
    }
}