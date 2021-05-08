using System;
using Metozis.TeTwo.Internal.Interaction.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Metozis.TeTwo.Internal.Entities.Archetypes
{
    public class ControllableActor : Actor
    {
        public PlayerInput Input;
        
        private void Start()
        {
            var movementSharedData = new MovementSharedData(this)
            {
                MovementSpeed = 5,
                MovementSpeedMinMax = new Vector2(-5, 5)
            };
            AddModule(new MovementModule(this, Input, movementSharedData));
            var movement = GetModule<MovementModule>();
            
            movement.AddAction("Move", (data, ctx) =>
            {
                data.MovementDelta = ctx.ReadValue<Vector2>();
            });
            movement.AddAction("Jump", (data, ctx) =>
            {
                if (ctx.started) data.Physics.AddForce(new Vector2(0, 200 * data.Physics.gravityScale));
            });
        }
    }
}