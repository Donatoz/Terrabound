using System;
using System.Collections.Generic;
using Metozis.TeTwo.Internal.Generation.World.Framework;

namespace Metozis.TeTwo.Internal.Generation.World.Processes
{
    public class ObjectGenerationProcess : IGenerationProcess
    {
        public Type DataPieceType => typeof(MapDataPiece);
        public string Logs { get; }
        public GenerationProcessType Type { get; }
        public bool Complete { get; protected set; }

        protected readonly IPlacementMap placementMap;
        protected readonly int mainMetaId;

        private Dictionary<IPlacementChanger, bool> placementChangers = new Dictionary<IPlacementChanger, bool>();
        
        public ObjectGenerationProcess(
            GenerationProcessType type,
            IPlacementMap placementMap,
            int mainMetaId
        )
        {
            Type = type;
            this.placementMap = placementMap;
            this.mainMetaId = mainMetaId;
        }

        public void AddPlacementChanger(IPlacementChanger changer, bool drop)
        {
            placementChangers[changer] = drop;
        }

        public virtual void Generate(ref WorldGenerationMeta result, GenerationDataPiece piece)
        {
            Complete = false;
            var positions = placementMap.GetPlacementPositions();

            foreach (var position in positions)
            {
                var pos = position;
                var drop = false;
                foreach (var changer in placementChangers)
                {
                    try
                    {
                        pos = changer.Key.Change(pos, result);
                    }
                    catch (PlacementChangeFailedException)
                    {
                        if (changer.Value)
                        {
                            drop = true;
                        }
                    }
                }
                if (!drop) result.BlockData[pos] = mainMetaId;
            }

            Complete = true;
        }
    }
}