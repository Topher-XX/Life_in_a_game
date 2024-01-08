using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[]
[CreateAssetMenu]
public class BaseStats : ScriptableObject
{
   
    [Header("Base Stats")]
    public float healthPoint;
    public float strenght;
    public float intel;
    public float dext;
    [Header(" def")]
    //public bool ranged;
    public int defPercent;
    [Header("Secondary Stats")]
    public float Charisma, Chance;
    [Header("Max Weight = 500")]
    [Range(0,500)] public int percentOfSpawn;





}
