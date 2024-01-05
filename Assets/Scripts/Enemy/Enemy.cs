using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy :MonoBehaviour
{
    //[]
    [Header("BaseStats")]
    public BaseStats  baseStats;
    protected float health;
    protected float dext;
    protected float intel;
    protected float strenght;
    protected bool ranged;
    protected int defPercent;
    protected GameObject player;
    protected GameManager gameManager;

    public enum State
    {
        _Idle,
        _Follow,
        _Attack,
        
    }



    public virtual void Awake()
    {
        defPercent = baseStats.defPercent;
        strenght=baseStats.strenght;
        intel = baseStats.intel;
        health = baseStats.healthPoint;
        dext = baseStats.dext;
        //ranged = baseStats.ranged;

    }

    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameManager.GMInstance;
    }

    

    public virtual void Attack()
    {

    }
    public  void CheckDamage(float Damage,out bool kill)
    {
        var tookDamage = health - Damage;
        if(tookDamage <= 0)
        {
            kill = true;
            //
             return;
        }
        else
        {
            kill = false;
            return;
        }
    }

    public virtual void TakeDamage(bool PlayerRanged,float Damage)
    {
        if (PlayerRanged) // range player
        {
            if (ranged)
            { // range monster
                //CheckDamage(player,Damage);
            }
            else// melee monster
            {

            }
        }
        else // melee player
        {
            if (ranged) //ranged monster
            {

            }
            else // melee monster
            {
                
            }
            
        }
    }

    public virtual void Movement()
    {

    }

    public virtual void Update()
    {

    }
    public virtual void FixedUpdate()
    {

    }

    public virtual void CheckState()
    {

    }
}
