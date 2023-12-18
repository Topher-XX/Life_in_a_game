using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ItemSpawnerManager : MonoBehaviour
{
    [Header("Spawnable Items")]
    [SerializeField] private GameObject[] items;

    [Header("Items Rarity Multiplicator")]
    [SerializeField] private float mPrimaryStat;//Primary stat multiplicator
    [SerializeField] private float mSecondaryStat;//Secondary stat multiplicator
    [SerializeField] private float mDirectStat;//Direct stat multiplicator

    [Header("Bonus Item")]
    [Tooltip("Luck stat to have for ensure a Item bonus.\n" +
             "If the luck stat if inferior of the tier, there are a random chance to get a bonus item.")]
    [SerializeField] private int itemBonusTier;

    public static ItemSpawnerManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Warning ! They are more than 1 ItemSpawnerManager in this scene !");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        //SpawnSpecificNbItems(20, transform.position, transform.rotation);
        
    }

    /// <summary>
    /// spawn a number of items requested at a given position
    /// </summary>
    /// <param name="_nbItemToSpawn"></param>
    /// <param name="_spawnPosition"></param>
    /// <param name="randomPos"></param>
    /// <param name="_spawnRotation"></param>
    public void SpawnSpecificNbItems(int _nbItemToSpawn, Vector3 _spawnPosition, float randomPos, Quaternion _spawnRotation)
    {
        List<GameObject> itemsCanBeSpawned = new List<GameObject>(items);

        //Check is the number items to spawn is superior than the number items in the list
        if (_nbItemToSpawn > itemsCanBeSpawned.Count)
        {
            _nbItemToSpawn = itemsCanBeSpawned.Count;
            Debug.LogWarning("Nb Item to spawn is superior to nb item in the array.\n" +
                             "Nb Item to spawn is resize to nb item in the array");
        }

        //Spawn items depending of the request
        for (int i = 0; i < _nbItemToSpawn; i++)
        {
            //Set the maxRandom
            float maxRandom = 0;
            foreach (GameObject item in itemsCanBeSpawned)
            {
                maxRandom += ItemRarity(item.GetComponent<Item>());
            }

            //Set a random value depending of the maxRandom for the item's spawn
            float random = Random.Range(0, maxRandom);

            #region Spawn a item from the random value
            float checkRandom = 0;
            for (int j = 0; j < itemsCanBeSpawned.Count; j++)
            {
                checkRandom += ItemRarity(itemsCanBeSpawned[j].GetComponent<Item>());
                if (random <= checkRandom) //true -> it's the item chosen
                {
                    SpawnItem(itemsCanBeSpawned[j], _spawnPosition, randomPos, _spawnRotation);
                    itemsCanBeSpawned.Remove(itemsCanBeSpawned[j]);
                    break;
                }
                else if (random > maxRandom) //prevent the float imprecision
                {
                    SpawnItem(itemsCanBeSpawned[j], _spawnPosition, randomPos, _spawnRotation);
                    itemsCanBeSpawned.Remove(itemsCanBeSpawned[j]);
                    break;
                }
            }
            #endregion


        }
    }

    /// <summary>
    /// Spawn an element at a given position
    /// </summary>
    /// <param name="itemChosen"></param>
    /// <param name="_spawnPosition"></param>
    /// <param name="randomPos"></param>
    /// <param name="_spawnRotation"></param>
    private void SpawnItem(GameObject itemChosen, Vector3 _spawnPosition, float randomPos, Quaternion _spawnRotation)
    {
        float x = Random.Range(-randomPos, randomPos);
        float y = Random.Range(-randomPos, randomPos);
        Vector3 spawnPosition = _spawnPosition + new Vector3(x, y, _spawnPosition.z);

        Instantiate(itemChosen, spawnPosition, _spawnRotation);
    }

    /// <summary>
    /// Determine the number of objects to spawn based on the player's chance stat
    /// </summary>
    public int AddBonusItemToSpawn(int _luckState)
    {
        int nbBonusItemsToSpawn = (_luckState / itemBonusTier);
        if(Random.Range(0.0f, 1.0f) >= (_luckState / itemBonusTier) % 1)
            nbBonusItemsToSpawn += 1;

        return nbBonusItemsToSpawn;
    }

    /// <summary>
    /// return the rarity of the item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public float ItemRarity(Item item)
    {
        return (item.Strength + item.Dexterity + item.Intelligence) * mPrimaryStat +
               (item.Charism + item.Luck) * mSecondaryStat +
               (item.Hp + item.CritRate) * mDirectStat;
    }
}
