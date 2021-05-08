using System;
using UnityEngine;

namespace Metozis.TeTwo.Internal.Cinema
{
    public enum CameraState
    {
        Follow,
        Static
    }
    
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        public CameraState State;
        public float Speed;

        private Camera target;

        private void Start()
        {
            target = GetComponent<Camera>();
        }
    }
}