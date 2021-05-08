using System;
using System.Collections.Generic;
using Metozis.TeTwo.Internal.Entities;
using Metozis.TeTwo.Internal.Entities.Archetypes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Metozis.TeTwo.Internal.Interaction.Movement
{
    public class MovementSharedData
    {
        public Vector2 MovementDelta;
        public Rigidbody2D Physics;
        public float MovementSpeed = 0;
        public Vector2 MovementSpeedMinMax;

        public MovementSharedData(Actor target)
        {
            MovementDelta = Vector2.zero;
            MovementSpeedMinMax = new Vector2(-7, 7);
            Physics = target.GetComponent<Rigidbody2D>();
        }
    }
    
    public class MovementModule : IRuntimeModule
    {
        public bool Enabled { get; }
        public RuntimeModuleType Type { get; }
        public readonly MovementSharedData SharedData;
        
        private PlayerInput input;
        private Actor target;

        private readonly Dictionary<InputAction, Action<MovementSharedData, InputAction.CallbackContext>> actions =
            new Dictionary<InputAction, Action<MovementSharedData, InputAction.CallbackContext>>();

        public MovementModule(Actor target, PlayerInput input, MovementSharedData sharedData)
        {
            this.target = target;
            this.input = input;
            SharedData = sharedData;
            
            input.onActionTriggered += ReadAction;
        }

        public void AddAction(string actionName, Action<MovementSharedData, InputAction.CallbackContext> context)
        {
            var action = input.actions.FindAction(actionName);
            if (action == null) return;
            
            actions[action] = context;
        }

        private void ReadAction(InputAction.CallbackContext ctx)
        {
            var action = ctx.action;
            if (actions.ContainsKey(action))
            {
                actions[action].Invoke(SharedData, ctx);
            }
        }

        public void Update()
        {
            var delta = SharedData.MovementDelta;
            SharedData.Physics.AddForce(delta * (10 * SharedData.MovementSpeed));
        }
    }
}