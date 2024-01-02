using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingElement : MonoBehaviour
{
    [SerializeField] private float amountHeal;
    private bool isUsed;

    // Wait to create interface Interact
    public void Interact()
    {
        if (!isUsed)
        {
            //Soigner le joueur
        }
        else
        {
            //Debug.Log("TheHealingElement is empty...");
        }
    }

}
