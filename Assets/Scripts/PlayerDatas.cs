using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerDatas : ScriptableObject
{
    [SerializeField] private GameObject playerClass;
    public int levelFinished;




    //Getter & Setter
    public GameObject PlayerClass
    {
        get { return playerClass; }
        set { playerClass = value; }
    }
}
