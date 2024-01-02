using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private int nbItemToSpawn;
    [SerializeField] private float maxRandomSpawnPos;


    private void Start()
    {
        Interact();
    }

    // Wait to create interface Interact
    public void Interact()
    {
        ItemSpawnerManager.instance.SpawnSpecificNbItems(nbItemToSpawn, transform.position, maxRandomSpawnPos, new Quaternion());
        //Jouer l'animation d'ouverture
    }


}
