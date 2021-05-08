using System;
using System.Collections.Generic;
using System.Linq;
using Metozis.TeTwo.Internal.Generation.World.Framework;
using UnityEngine;

namespace Metozis.TeTwo.Internal.Generation.World.Processes
{
    public sealed class BiomeGenerationProcess : IGenerationProcess
    {
        public Type DataPieceType => typeof(MapDataPiece);
        public string Logs { get; }
        public bool Complete { get; private set; }
        public GenerationProcessType Type { get; }

        private readonly Dictionary<int, int> replacements;
        private readonly (int, int) startEnd;

        public BiomeGenerationProcess(Dictionary<int, int> replacements, (int, int) startEnd)
        {
            this.replacements = replacements;
            this.startEnd = startEnd;
        }
        
        public void Generate(ref WorldGenerationMeta result, GenerationDataPiece piece)
        {
            Complete = false;
            var mapData = (MapDataPiece) piece;
            
            lock (result)
            {
                foreach (var replacement in replacements)
                {
                    if (!result.BlockData.ContainsValue(replacement.Key)) continue;

                    var positions = result.GetPlacementsSlice(startEnd.Item1, startEnd.Item2)
                        .Where(kvp => kvp.Value == replacement.Key)
                        .Select(kvp => kvp.Key)
                        .ToList();

                    for (int i = positions.Count - 1; i >= 0; i--)
                    {
                        result.BlockData[positions[i]] = replacement.Value;
                    }
                }
            }

            Complete = true;
        }
    }
}