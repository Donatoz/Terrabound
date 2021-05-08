using UnityEngine;

namespace Metozis.TeTwo.Internal.Generation.World.Framework
{
    public interface IPlacementMap
    {
        Vector3[] GetPlacementPositions();
        bool CanPlace(float x, float y);
    }
}