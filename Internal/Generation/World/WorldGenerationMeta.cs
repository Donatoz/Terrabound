using System;
using System.Collections.Generic;
using System.Linq;
using Metozis.TeTwo.Internal.Entities.Meta;
using Metozis.TeTwo.Internal.Generation.World.Rules;
using UnityEngine;

namespace Metozis.TeTwo.Internal.Generation.World
{
    [Serializable]
    public class WorldGenerationMeta
    {
        public List<BlockGenerationVariant> Blocks;
        // TODO: Process-shared part must be separated
        public Dictionary<Vector3, int> BlockData;
        public Dictionary<int, BlockMeta> BlocksMeta;
        
        private readonly Dictionary<string, object> generationData = new Dictionary<string, object>();

        public void AppendData(object data, string id)
        {
            generationData[id] = data;
        }

        public void Initialize()
        {
            Blocks = new List<BlockGenerationVariant>();
            BlockData = new Dictionary<Vector3, int>();
            BlocksMeta = new Dictionary<int, BlockMeta>();
        }

        public T GetData<T>(string id)
        {
            return (T)generationData[id];
        }

        public IEnumerable<KeyValuePair<Vector3, int>> GetPlacementsSlice(int start, int end)
        {
            return BlockData.Where(kvp => kvp.Key.x >= start && kvp.Key.x <= end);
        }
    }
}