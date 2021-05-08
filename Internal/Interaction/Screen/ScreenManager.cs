using System;
using Metozis.TeTwo.Extensions;
using Metozis.TeTwo.Internal.Entities.Archetypes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Metozis.TeTwo.Internal.Interaction.Screen
{
    public class ScreenManager : MonoBehaviour
    {
        public PlayerInput Input;

        private void Start()
        {
            Input.onActionTriggered += HandleAction;
        }

        private void HandleClick()
        {
            var pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.Starndardize());
            var hit = Physics2D.Raycast(pos.Lower(), Vector2.zero);
            if (hit.collider != null && hit.collider.GetComponent<ISelectable>() != null)
            {
                hit.collider.GetComponent<Block>().Destroy();
            }
        }

        private void HandleAction(InputAction.CallbackContext ctx)
        {
            if (ctx.action == Input.actions.FindAction("Fire"))
            {
                if (ctx.started)
                {
                    HandleClick();
                }
            }
        }
    }
}