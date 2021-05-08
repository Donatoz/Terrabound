using System;
using System.Collections.Generic;
using Metozis.TeTwo.Internal.Entities.Factories;
using Metozis.TeTwo.Internal.Entities.Meta;
using Metozis.TeTwo.Internal.Interaction;
using Metozis.TeTwo.Internal.Management;
using Metozis.TeTwo.Internal.Pawns;
using Metozis.TeTwo.Internal.Rendering;
using Metozis.TeTwo.Internal.Rendering.Blocks;
using Metozis.TeTwo.Internal.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.U2D;

namespace Metozis.TeTwo.Internal.Entities.Archetypes
{
    public class Block : Pawn, ISpriteSheetRenderTarget
    {
        public Action OnRendered { get; set; }
        public Action OnSpriteChanged { get; set; }
        public Renderer Renderer => blockRenderer;
        public Sprite[] SpriteSheet { get; set; }

        [SerializeField]
        [BoxGroup("Block properties")]
        private Sprite blockSprite;
        private SpriteRenderer blockRenderer;

        private BlockStateModule state;
        
        private void Awake()
        {
            blockRenderer = GetComponentInChildren<SpriteRenderer>();
            AddModule(new EffectModule(this)
            {
                ChangeColorContext = (renderer, color) =>
                {
                    ((SpriteRenderer) renderer).color = color;
                }
            });
            AddModule(new BlockStateModule(this));
            state = GetModule<BlockStateModule>();
        }

        private void Start()
        {
            if (blockSprite != null)
            {
                Render();
            }
            
            foreach (var tile in ((BlockMeta)Meta).CustomTileData)
            {
                state.VisualStateProcessor[tile.State] = tile.Tiles;
            }
            state.StaticRefresh();
        }

        public void Render()
        {
            blockRenderer.sprite = blockSprite;
            OnRendered?.Invoke();
        }

        public void ChangeSprite(Sprite sprite)
        {
            blockSprite = sprite;
            OnSpriteChanged?.Invoke();
        }

        public void PickSprite(int id)
        {
            throw new NotImplementedException();
        }

        public Vector2Int WorldPosition()
        {
            return new Vector2Int((int) transform.position.x, (int) transform.position.y);
        }

        public override void Destroy()
        {
            var pos = WorldPosition();
            WorldManager.PawnRegistry.Remove((pos.x, pos.y));
            
            var neighbors = WorldPointer.GetBlockNeighbors(this);
            foreach (var block in neighbors.Active())
            {
                block.state.StaticRefresh();
            }
            
            base.Destroy();
        }
    }
}