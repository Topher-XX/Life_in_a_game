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
    protected State currentState = State._idle;
    private Rigidbody rb;

    public enum State
    {
        _idle,
        _follow,
        _attack,
        _checkingPath,
        
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
        rb = GetComponent<Rigidbody>();
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
        switch (currentState)
        {
            case State._idle:
                IsIdling();
                break;
            case State._follow:
                IsFollowing();
                break;
            case State._attack:
                IsAttacking();
                break;
            case State._checkingPath:
                IsCheckingPath();
                break;


        }
        }
    public virtual void FixedUpdate()
    {

    }

    public virtual void CheckState()
    {

    }

    public virtual void IsIdling()
    {

    }

    public virtual void IsFollowing()
    {

    }

    public virtual void IsAttacking()
    {

    }

    public virtual void IsCheckingPath()
    {

    }

    public virtual void OnColliderEnter()
    {

    }


}
