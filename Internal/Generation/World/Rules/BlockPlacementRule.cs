using System;
using System.Linq;
using Metozis.TeTwo.Internal.Entities.Meta;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Metozis.TeTwo.Internal.Generation.World.Rules
{
    [Serializable]
    public class BlockPlacementRule : GenerationRule
    {
        [SerializeField]
        private BlockMeta block;
        [SerializeField] 
        private float coverage;

        public BlockPlacementRule(BlockMeta block, float coverage)
        {
            this.block = block;
            this.coverage = Mathf.Clamp01(coverage);
        }
        
        public override void Resolve(ref WorldGenerationMeta result)
        {
            var positions = result.BlockData.Where(kvp => kvp.Value == block.Id).Select(kvp => kvp.Key).ToList();
            if (coverage < 1)
            {
                for (int i = 0; i < positions.Count; i++)
                {
                    var rnd = Random.Range(0, 101);
                    if (rnd > coverage * 100)
                    {
                        positions.RemoveAt(i);
                    }
                }
            }

            result.Blocks.Add(new BlockGenerationVariant {Meta = block, Positions = positions.ToArray()});
        }
    }
}