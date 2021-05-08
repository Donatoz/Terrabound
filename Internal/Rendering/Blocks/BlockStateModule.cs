using System;
using System.Collections.Generic;
using Metozis.TeTwo.Internal.Entities;
using Metozis.TeTwo.Internal.Entities.Archetypes;
using Metozis.TeTwo.Internal.Entities.Meta;
using Metozis.TeTwo.Internal.Management;
using Metozis.TeTwo.Internal.Pawns;
using Metozis.TeTwo.Internal.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Metozis.TeTwo.Internal.Rendering.Blocks
{
    public enum BlockVisualState
    {
        Single = 0,
        TopLeftCorner = 12,
        TopRightCorner = 6,
        BottomRightCorner = 3,
        BottomLeftCorner = 9,
        TopWallPart = 14,
        RightWallPart = 7,
        BottomWallPart = 11,
        LeftWallPart = 13,
        MiddleWall = 15,
        SingleVerticalWall = 5,
        SingleVerticalWallTop = 4,
        SingleVerticalWallBottom = 1,
        SingleHorizontalWall = 10,
        SingleHorizontalWallRight = 2,
        SingleHorizontalWallLeft = 8
    }
    
    public class BlockStateModule : IModule
    {
        public bool Enabled { get; }
        private readonly Block target;

        /// <summary>
        /// Precalculated sprite indexes for different visual states.
        /// </summary>
        public readonly Dictionary<BlockVisualState, int[]> VisualStateProcessor = new Dictionary<BlockVisualState, int[]>
        {
            {BlockVisualState.Single, new []{57, 58, 59}},
            
            {BlockVisualState.TopLeftCorner, new []{48, 50, 52}},
            {BlockVisualState.TopRightCorner, new []{49, 51, 53}},
            {BlockVisualState.BottomRightCorner, new []{64, 66, 68}},
            {BlockVisualState.BottomLeftCorner, new []{63, 65, 67}},
            
            {BlockVisualState.TopWallPart, new []{1, 2, 3}},
            {BlockVisualState.RightWallPart, new []{4, 20, 36}},
            {BlockVisualState.BottomWallPart, new []{33, 34, 35}},
            {BlockVisualState.LeftWallPart, new []{0, 16, 32}},
            {BlockVisualState.MiddleWall, new []{17, 18, 19}},
            
            {BlockVisualState.SingleVerticalWall, new []{5, 21, 37}},
            {BlockVisualState.SingleVerticalWallTop, new []{6, 7, 8}},
            {BlockVisualState.SingleVerticalWallBottom, new []{54, 55, 56}},
            
            {BlockVisualState.SingleHorizontalWall, new []{69, 70, 71}},
            {BlockVisualState.SingleHorizontalWallLeft, new []{9, 25, 41}},
            {BlockVisualState.SingleHorizontalWallRight, new []{12, 28, 44}},
        };
        
        public BlockStateModule(Block target)
        {
            this.target = target;
        }

        private BlockVisualState CalculateState()
        {
            var matrix = WorldPointer.GetBlockNeighbors(target);
            
            var index = 0;
            if (matrix.Top != null) index += 1;
            if (matrix.Left != null) index += 2;
            if (matrix.Bottom != null) index += 4;
            if (matrix.Right != null) index += 8; 
            
            return (BlockVisualState)index;
        }

        public void StaticRefresh()
        {
            var blockMeta = target.Meta as BlockMeta;
            var spriteVariants = VisualStateProcessor[CalculateState()];
            target.ChangeSprite(blockMeta.BlockAtlas.GetSprite($"Tiles_{blockMeta.TileId}_{spriteVariants[Random.Range(0, spriteVariants.Length - 1)]}"));
            target.Render();
        }
    }
}