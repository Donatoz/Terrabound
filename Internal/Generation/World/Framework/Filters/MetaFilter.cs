using System;
using System.Collections.Generic;
using Metozis.TeTwo.Internal.Entities.Meta;

namespace Metozis.TeTwo.Internal.Generation.World.Framework.Filters
{
    public readonly struct MetaFilter
    {
        private readonly List<Predicate<EntityMeta>> filters;
        
        public MetaFilter(List<Predicate<EntityMeta>> filters)
        {
            this.filters = filters;
        }

        public bool Resolve(EntityMeta meta)
        {
            foreach (var filter in filters)
            {
                if (!filter.Invoke(meta)) return false;
            }
            return true;
        }
    }
}