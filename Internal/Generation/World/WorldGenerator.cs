using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Metozis.External.WeightedRandomization;
using Metozis.TeTwo.Internal.Entities.Archetypes;
using Metozis.TeTwo.Internal.Entities.Factories;
using Metozis.TeTwo.Internal.Entities.Meta;
using Metozis.TeTwo.Internal.Generation.World.Framework;
using Metozis.TeTwo.Internal.Generation.World.Processes;
using Metozis.TeTwo.Internal.Generation.World.Rules;
using Sirenix.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Metozis.TeTwo.Internal.Generation.World
{
    public class WorldGenerator : MonoBehaviour
    {
        public Sprite BlockSprite;
        public bool CustomGeneration;
        public Action<WorldGenerationMeta> OnWorldGenerated = delegate {  };

        private readonly HashSet<IGenerationProcess> generationProcessor = new HashSet<IGenerationProcess>();
        
        private void InitializeDefaultGeneration(WorldGenerationSettings settings)
        {
            generationProcessor.AddRange(DefaultGeneration.AssembleDefaultProcessor(settings));
        }
        
        private void Start()
        {

            var options = new WorldGenerationSettings
            {
                Rules = DefaultGeneration.DefaultRules, 
                ChunkSize = new Vector2(100, 100), 
                Seed = 219
            };

            Random.InitState(options.Seed);

            OnWorldGenerated += CreateWorld;
            GenerateWorld(options);

            if (false)
            {
                //StartCoroutine(CreateWorldAsync(GenerateWorld(options)));
            }
        }
        
        public void GenerateWorld(WorldGenerationSettings options)
        {
            StartCoroutine(GenerationRoutine(options));
        }

        private IEnumerator GenerationRoutine(WorldGenerationSettings options)
        {
            var result = new WorldGenerationMeta();

            if (!CustomGeneration)
            {
                InitializeDefaultGeneration(options);
            }
            
            result.Initialize();
            options.DistributeChunks();
            
            foreach (var meta in GenerationPreferences.Instance.BlockMeta)
            {
                result.BlocksMeta[meta.Id] = meta;
            }
            
            foreach (var process in generationProcessor.Where(p => p.Type == GenerationProcessType.Preprocess))
            {
                process.Generate(ref result, options.GetDataChunk(process.DataPieceType));
                yield return new WaitUntil(() => process.Complete);
            }
            
            options.Rules.Sort((rule, another) =>
            {
                if (rule.Order > another.Order) return 1;
                if (rule.Order < another.Order) return -1;
                return 0;
            } );
            
            foreach (var rule in options.Rules)
            {
                rule.Resolve(ref result);
            }
            
            foreach (var process in generationProcessor.Where(p => p.Type == GenerationProcessType.Postprocess))
            {
                process.Generate(ref result, options.GetDataChunk(process.DataPieceType));
                yield return new WaitUntil(() => process.Complete);
            }
            OnWorldGenerated.Invoke(result);
            Debug.Log("World generation complete!");
        }

        public void CreateWorld(WorldGenerationMeta meta)
        {
            var world = new GameObject("World Chunk");
            var blockFactory = new BlockFactory(Vector3.zero);
            foreach (var variant in meta.Blocks)
            {
                foreach (var pos in variant.Positions)
                {
                    blockFactory.SetPosition(pos);
                    var block = blockFactory.Create<Block>(variant.Meta);
                    block.transform.parent = world.transform;
                }
            }
        }

        private IEnumerator CreateWorldAsync(WorldGenerationMeta meta)
        {
            yield return new WaitForSeconds(1);
            var blockFactory = new BlockFactory(Vector3.zero);
            foreach (var variant in meta.Blocks)
            {
                foreach (var pos in variant.Positions)
                {
                    yield return new WaitForSeconds(Time.deltaTime);
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        blockFactory.SetPosition(pos);
                        blockFactory.Create<Block>(variant.Meta);
                    });
                }
            }
        }
    }
}