using System;
using System.Collections.Generic;
using Metozis.TeTwo.Internal.Rendering.Blocks;
using UnityEngine;
using UnityEngine.U2D;

namespace Metozis.TeTwo.Internal.Entities.Meta
{
    public enum BlockType
    {
        Solid,
        Trigger
    }
    
    [Serializable]
    public class BlockMeta : EntityMeta
    {
        [Serializable]
        public struct CustomTile
        {
            public BlockVisualState State;
            public int[] Tiles;
        }
        
        public SpriteAtlas BlockAtlas;
        public float VisualScale;
        public Color Tint;
        public BlockType Type;
        public string DefaultSpriteName;
        public Vector2 VisualOffset;
        public int TileId;
        public List<CustomTile> CustomTileData;
    }
}