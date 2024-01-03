using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingElement : MonoBehaviour, IInteractInterface
{
    [SerializeField] private float amountHeal;
    private bool isUsed;

    void IInteractInterface.Interact()
    {
        if (!isUsed)
        {
            //Heal the player
        }
        else
        {
            //Debug.Log("TheHealingElement is empty...");
        }
    }

}
