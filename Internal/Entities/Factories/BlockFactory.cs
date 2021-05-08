using System.Collections.Generic;
using Metozis.TeTwo.Extensions;
using Metozis.TeTwo.Internal.Entities.Archetypes;
using Metozis.TeTwo.Internal.Entities.Meta;
using Metozis.TeTwo.Internal.Generation.World;
using Metozis.TeTwo.Internal.Management;
using Metozis.TeTwo.Internal.Pawns;
using UnityEngine;

namespace Metozis.TeTwo.Internal.Entities.Factories
{
    internal class BlockFactory : EntityFactory
    {
        private Vector3 position;

        public BlockFactory(Vector3 spawningPosition)
        {
            SetPosition(spawningPosition);
        }
        
        public void SetPosition(Vector3 pos)
        {
            position = pos;
        }
        
        public override T Create<T>(EntityMeta options)
        {
            var resource = GenerationPreferences.Instance.GetTemplate("Block");
            var block = Object.Instantiate(resource).GetComponent<Block>();
            var blockMeta = options as BlockMeta;
            block.Meta = blockMeta;
            
            var atlas = blockMeta.BlockAtlas;
            
            block.Name = options.Name;
            block.GetModule<EffectModule>().ChangeScale(blockMeta.VisualScale);
            block.GetModule<EffectModule>().ChangeColor(blockMeta.Tint);

            block.transform.Find("Sprite").transform.position += blockMeta.VisualOffset.Upper();
            block.transform.position = position;

            WorldManager.PawnRegistry[((int) block.transform.position.x, (int) block.transform.position.y)] = block;
            
            return block as T;
        }
    }
}