using RPG.Attributes;
using RPG.Combat;
using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class CombatTarget : MonoBehaviour, IRaycastable
{
    public CursorType GetCursorType()
    {
        return CursorType.Combat;
    }

    public bool TryHandleRaycast(PlayerController callingController)
    {
        Fighter fighter = callingController.GetComponent<Fighter>();

        if (!fighter.CanAttack(gameObject))  return false;

        if (Input.GetMouseButton(0))
        {
            fighter.Attack(gameObject);
        }

        return true;
    }
}
