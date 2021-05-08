using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Metozis.External.WeightedRandomization;
using Metozis.TeTwo.Extensions;
using Metozis.TeTwo.Internal.Generation.World.Framework;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Metozis.TeTwo.Internal.Generation.World.Processes
{
    public sealed class BlockGenerationProcess : ObjectGenerationProcess
    {
        private Dictionary<Predicate<Vector3>, int> staticPlacements;
        private Dictionary<int, Predicate<Vector3>> randomizationRules;
        
        private IWeightedRandomizer<int> randomizer;
        private Dictionary<int, int> distribution;
        
        public BlockGenerationProcess(
            GenerationProcessType type, 
            IPlacementMap placementMap, 
            int mainMetaId
        ) : base(type, placementMap, mainMetaId)
        {
        }

        public void SetStaticPlacements(Dictionary<Predicate<Vector3>, int> staticPlacements)
        {
            this.staticPlacements = staticPlacements;
        }

        public void DistributeProbability<T>(Dictionary<int, int> distribution, int seed) where T : IWeightedRandomizer<int>, new()
        {
            randomizer = new T();
            randomizer.ReSeed(seed);
            this.distribution = distribution;
            RefreshRandomizer();
        }

        public void SetRandomizationRules(Dictionary<int, Predicate<Vector3>> rules)
        {
            randomizationRules = rules;
        }

        private void RefreshRandomizer()
        {
            foreach (var kvp in distribution)
            {
                randomizer[kvp.Key] = kvp.Value;
            }
        }
        
        public override void Generate(ref WorldGenerationMeta result, GenerationDataPiece dataPiece)
        {
            Complete = false;

            if (randomizer != null)
            {
                RefreshRandomizer();
            }
            
            var mapData = (MapDataPiece) dataPiece;
            var positions = placementMap.GetPlacementPositions();
            
            foreach (var position in positions)
            {
                if (staticPlacements != null && staticPlacements.Any(kvp => kvp.Key(position)))
                {
                    foreach (var staticPlacement in staticPlacements)
                    {
                        if (staticPlacement.Key(position))
                        {
                            result.BlockData[position] = staticPlacement.Value;
                        }
                    }
                }
                else
                {
                    var blockId = randomizer?.NextWithReplacement() ?? mainMetaId;
                    if (randomizationRules != null && randomizer != null)
                    {
                        if (randomizationRules.ContainsKey(blockId) && !randomizationRules[blockId](position))
                        {
                            blockId = mainMetaId;
                        }
                    }
                    result.BlockData[position] = blockId;
                }
            }

            Complete = true;
        }

    }
}