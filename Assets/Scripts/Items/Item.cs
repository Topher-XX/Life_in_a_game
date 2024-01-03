using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Item : MonoBehaviour, IInteractInterface
{
    [SerializeField] private int strength;
    [SerializeField] private int dexterity;
    [SerializeField] private int intelligence;

    [SerializeField] private int charism;
    [SerializeField] private int luck;

    [SerializeField] private int hp;
    [SerializeField] private int critRate;

    void IInteractInterface.Interact()
    {
        //Add item's stat to player's stat
        //Add item to the player item list

        Destroy(gameObject);
    }



    #region Getter & Setter
    public int Strength { get { return strength; } }
    public int Dexterity { get { return dexterity; } }
    public int Intelligence { get { return intelligence; } }

    public int Charism { get { return charism; } }
    public int Luck { get { return luck; } }

    public int Hp { get { return hp; } }
    public int CritRate { get {  return critRate; } }
    #endregion
}
