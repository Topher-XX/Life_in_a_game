using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractInterface
{
    [SerializeField] private int nbItemToSpawn;
    [SerializeField] private float maxRandomSpawnPos;


    private void Start()
    {
        
    }

    // Wait to create interface Interact
    void IInteractInterface.Interact()
    {
        ItemSpawnerManager.instance.SpawnSpecificNbItems(nbItemToSpawn, transform.position, maxRandomSpawnPos, new Quaternion());
        //Jouer l'animation d'ouverture
    }


}
