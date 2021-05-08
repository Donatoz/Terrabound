using System;
using System.Collections.Generic;
using Metozis.TeTwo.Internal.Modification;
using Metozis.TeTwo.Internal.Pawns;

namespace Metozis.TeTwo.Internal.Entities.Archetypes
{
    public struct ActorDefaultStats
    {
        public const string Health = "Health";
    }
    
    public abstract class Actor : Pawn
    {
        public Dictionary<string, Stat> stats = new Dictionary<string, Stat>();
    }
}