using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private PlayerDatas playerDatas;

    void Start()
    {
        Instantiate(playerDatas.PlayerClass, transform.position, new Quaternion());
    }

}
