using UnityEngine;
using UnityEngine.InputSystem.Controls;

namespace Metozis.TeTwo.Extensions
{
    public static class UnityExtensions
    {
        public static Vector3 Upper(this Vector2 vec2, float z = 0)
        {
            return new Vector3(vec2.x, vec2.y, z);
        }
        
        public static Vector2 Lower(this Vector3 vec3)
        {
            return new Vector2(vec3.x, vec3.y);
        }

        public static Vector2 Starndardize(this Vector2Control vec)
        {
            return new Vector2(vec.x.ReadValue(), vec.y.ReadValue());
        }
    }
}