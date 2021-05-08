using UnityEngine;

namespace Metozis.TeTwo.Internal.Generation.World.Processes
{
    public interface IPlacementChanger
    {
        Vector3 Change(Vector3 input, WorldGenerationMeta meta);
    }
}