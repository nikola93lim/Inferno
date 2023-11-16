using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    [RequireComponent(typeof(Pickup))]
    public class ClickablePickup : MonoBehaviour, IRaycastable
    {
        private Pickup _pickup;

        private void Awake()
        {
            _pickup = GetComponent<Pickup>();
        }

        public CursorType GetCursorType()
        {
            if (_pickup.CanBePickedUp())
            {
                return CursorType.Pickup;
            }
            else
            {
                return CursorType.FullPickup;
            }
        }

        public bool TryHandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _pickup.PickupItem();
            }

            return true;
        }
    }
}
