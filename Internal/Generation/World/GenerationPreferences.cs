using System;
using System.Collections.Generic;
using Metozis.TeTwo.Internal.Entities.Meta;
using Metozis.TeTwo.Internal.Modification;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace Metozis.TeTwo.Internal.Generation.World
{
    public class GenerationPreferences : MonoBehaviour
    {
        public static GenerationPreferences Instance;

        public SpriteAtlas DefaultBlockAtlas;
        public float DefaultBlockVisualScale;
        public List<BlockMeta> BlockMeta;
        
        private Dictionary<string, GameObject> templates = new Dictionary<string, GameObject>();

        private void Awake()
        {
            Instance = gameObject.GetComponent<GenerationPreferences>();

            templates["Block"] = Resources.Load<GameObject>("Templates/Block");
        }

        internal GameObject GetTemplate(string templateName)
        {
            return templates[templateName];
        }
        
        [Button]
        public void InitializeDefaultBlocks()
        {
            BlockMeta.Add(new BlockMeta {Id = 0, Tint = Color.white, BlockAtlas = DefaultBlockAtlas, Name = "block", VisualScale = DefaultBlockVisualScale});
            BlockMeta.Add(new BlockMeta {Id = 1, Tint = Color.white, BlockAtlas = DefaultBlockAtlas, Name = "block", VisualScale = DefaultBlockVisualScale});
            BlockMeta.Add(new BlockMeta {Id = 2, Tint = Color.white, BlockAtlas = DefaultBlockAtlas, Name = "block", VisualScale = DefaultBlockVisualScale});
            BlockMeta.Add(new BlockMeta {Id = 3, Tint = Color.white, BlockAtlas = DefaultBlockAtlas, Name = "block", VisualScale = DefaultBlockVisualScale});
            BlockMeta.Add(new BlockMeta {Id = 4, Tint = Color.white, BlockAtlas = DefaultBlockAtlas, Name = "block", VisualScale = DefaultBlockVisualScale});
            BlockMeta.Add(new BlockMeta {Id = 5, Tint = Color.white, BlockAtlas = DefaultBlockAtlas, Name = "block", VisualScale = DefaultBlockVisualScale});
        }
    }
}