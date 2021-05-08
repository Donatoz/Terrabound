using System;
using System.Collections.Generic;
using Metozis.External.WeightedRandomization;
using Metozis.TeTwo.Internal.Entities.Meta;
using Metozis.TeTwo.Internal.Generation.World.Framework;
using Metozis.TeTwo.Internal.Generation.World.Framework.Filters;
using Metozis.TeTwo.Internal.Generation.World.Processes;
using Metozis.TeTwo.Internal.Generation.World.Rules;
using Sirenix.Utilities;
using UnityEngine;

namespace Metozis.TeTwo.Internal.Generation.World
{
    internal static class DefaultGeneration
    {
        public static readonly List<GenerationRule> DefaultRules = AssembleDefaultRules();
        
        private static List<GenerationRule> AssembleDefaultRules()
        {
            var rules = new List<GenerationRule>();
            foreach (var block in GenerationPreferences.Instance.BlockMeta)
            {
                rules.Add(new BlockPlacementRule(block, 1));
            }

            return rules;
        }

        private static (HashSet<IGenerationProcess>, Noise) GenerateCaves(WorldGenerationSettings settings)
        {
            var cavesNoiseGenerator =
                new NoiseGenerators.FastNoiseGenerator(settings.Seed, FastNoiseLite.NoiseType.OpenSimplex2, 10);
            
            var cavesNoise = new Noise(cavesNoiseGenerator, settings.ChunkSize * new Vector2(1, 1f), 0.3f);

            var cavesGenerationProcess = new BlockGenerationProcess(GenerationProcessType.Preprocess, cavesNoise, 1);
            cavesGenerationProcess.DistributeProbability<StaticWeightedRandomizer<int>>(new Dictionary<int, int>
            {
                {1, 6},
                {2, 3}
            }, settings.Seed);
            cavesGenerationProcess.SetRandomizationRules(new Dictionary<int, Predicate<Vector3>>
            {
                {2, pos => pos.y > settings.ChunkSize.y * 0.65f}
            });
            cavesGenerationProcess.SetStaticPlacements(new Dictionary<Predicate<Vector3>, int>
            {
                {
                    pos => pos.y > settings.ChunkSize.y * 0.8f,
                    2
                }
            });

            return (new HashSet<IGenerationProcess> {cavesGenerationProcess}, cavesNoise);
        }

        private static HashSet<IGenerationProcess> GenerateOres(WorldGenerationSettings settings, Noise mask)
        {
            var simplexNoiseGenerator =
                new NoiseGenerators.FastNoiseGenerator(settings.Seed, FastNoiseLite.NoiseType.OpenSimplex2S, 10);
            
            #region Iron Ore

            var ironOreNoise = new Noise(simplexNoiseGenerator, settings.ChunkSize * new Vector2(2, 2f), -0.2f,
                ((int) settings.ChunkSize.x, 0)) * mask;

            var ironOreProcess = new BlockGenerationProcess(GenerationProcessType.Preprocess, ironOreNoise, -1);
            
            ironOreProcess.DistributeProbability<StaticWeightedRandomizer<int>>(new Dictionary<int, int>
            {
                {3, 1}
            }, settings.Seed);
            ironOreProcess.SetRandomizationRules(new Dictionary<int, Predicate<Vector3>>
            {
                {3, pos => pos.y < settings.ChunkSize.y * 0.85f}
            });

            #endregion

            return new HashSet<IGenerationProcess>
            {
                ironOreProcess
            };
        }
        
        public static HashSet<IGenerationProcess> AssembleDefaultProcessor(WorldGenerationSettings settings)
        {
            var result = new HashSet<IGenerationProcess>();
            var noiseGenerator = new NoiseGenerators.FastNoiseGenerator(settings.Seed, FastNoiseLite.NoiseType.OpenSimplex2, 10);

            var firstNoise = new Noise(noiseGenerator, settings.ChunkSize * new Vector2(1, 1f), 0.3f);


            var biomeProcess = new BiomeGenerationProcess(new Dictionary<int, int>
            {
                {1, 4},
                {2, 4},
                {3, 5}
            }, (0, 40));

            var availablePlaces =
                new ObjectGenerationProcess(GenerationProcessType.Preprocess, firstNoise.OneMinus().Eliminate(0.99f), 0);
            availablePlaces.AddPlacementChanger(new PositionNormalizer(new Vector2(0, -1), 20, new []
            {
                new MetaFilter(new List<Predicate<EntityMeta>>
                {
                    meta => ((BlockMeta) meta).Type != BlockType.Trigger
                })
            }, true), true);

            var caves = GenerateCaves(settings);

            result.AddRange(caves.Item1);
            result.AddRange(GenerateOres(settings, caves.Item2));
            result.Add(biomeProcess);
            result.Add(availablePlaces);

            return result;
        }
    }
}