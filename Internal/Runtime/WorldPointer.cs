using System;
using System.Collections.Generic;
using Metozis.TeTwo.Internal.Entities;
using Metozis.TeTwo.Internal.Entities.Archetypes;
using Metozis.TeTwo.Internal.Management;
using Metozis.TeTwo.Internal.Pawns;
using UnityEngine;

namespace Metozis.TeTwo.Internal.Runtime
{
    public struct NeighborhoodMatrix
    {
        public Block Top;
        public Block Right;
        public Block Bottom;
        public Block Left;

        public IEnumerable<Block> Active()
        {
            var result = new List<Block>();
            
            if (Top != null) result.Add(Top);
            if (Right != null) result.Add(Right);
            if (Bottom != null) result.Add(Bottom);
            if (Left != null) result.Add(Left);
            
            return result;
        }
    }
    
    public static class WorldPointer
    {
        public static Pawn GetPawnAtPosition(int x, int y)
        {
            return WorldManager.PawnRegistry[(x, y)];
        }

        public static Block TryGetBlockAtPosition(int x, int y, Block comparer = null)
        {
            try
            {
                var pawn = WorldManager.PawnRegistry[(x, y)];
                if (comparer != null)
                {
                    return pawn.Meta.Id == comparer.Meta.Id ? pawn as Block : null;
                }

                return pawn as Block;
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        public static NeighborhoodMatrix GetBlockNeighbors(Block block)
        {
            var pos = new Vector2Int((int)block.transform.position.x, (int)block.transform.position.y);
            return new NeighborhoodMatrix
            {
                Top = TryGetBlockAtPosition(pos.x, pos.y + 1, block),
                Right = TryGetBlockAtPosition(pos.x + 1, pos.y, block),
                Bottom = TryGetBlockAtPosition(pos.x, pos.y - 1, block),
                Left = TryGetBlockAtPosition(pos.x - 1, pos.y, block)
            };
        }
    }
}