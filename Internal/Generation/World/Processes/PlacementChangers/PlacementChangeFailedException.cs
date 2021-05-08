using System;

namespace Metozis.TeTwo.Internal.Generation.World.Processes
{
    public class PlacementChangeFailedException : Exception
    {
        public PlacementChangeFailedException() { }

        public PlacementChangeFailedException(string message)
            : base(message) { }

        public PlacementChangeFailedException(string message, Exception inner)
            : base(message, inner) { }
    }
}