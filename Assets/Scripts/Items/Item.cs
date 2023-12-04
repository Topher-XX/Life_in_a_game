using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private int strength;
    [SerializeField] private int dexterity;
    [SerializeField] private int intelligence;

    [SerializeField] private int charism;
    [SerializeField] private int luck;

    [SerializeField] private int hp;
    [SerializeField] private int critRate;


    

    public int Strength { get { return strength; } }
    public int Dexterity { get { return dexterity; } }
    public int Intelligence { get { return intelligence; } }

    public int Charism { get { return charism; } }
    public int Luck { get { return luck; } }

    public int Hp { get { return hp; } }
    public int CritRate { get {  return critRate; } }
}
