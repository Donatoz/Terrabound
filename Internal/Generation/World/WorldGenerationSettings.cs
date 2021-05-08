using System;
using System.Collections.Generic;
using Metozis.TeTwo.Internal.Generation.World.Rules;
using UnityEngine;

namespace Metozis.TeTwo.Internal.Generation.World
{
    public class WorldGenerationSettings
    {
        public int Seed;
        public Vector2 ChunkSize;
        public List<GenerationRule> Rules;
        
        private readonly Dictionary<Type, GenerationDataPiece> dataChunks = new Dictionary<Type, GenerationDataPiece>();

        public void DistributeChunks()
        {
            dataChunks[typeof(MapDataPiece)] = new MapDataPiece
            {
                Seed = Seed,
                ChunkSize = ChunkSize
            };
        }

        public GenerationDataPiece GetDataChunk(Type chunkType)
        {
            return dataChunks[chunkType];
        }
    }
}