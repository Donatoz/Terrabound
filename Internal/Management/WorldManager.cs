using System.Collections.Generic;
using Metozis.TeTwo.Internal.Pawns;

namespace Metozis.TeTwo.Internal.Management
{
    public static class WorldManager
    {
        public static Dictionary<(int, int), Pawn> PawnRegistry = new Dictionary<(int, int), Pawn>();
    }
}