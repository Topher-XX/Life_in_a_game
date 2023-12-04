using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemSpawnerManager : MonoBehaviour
{
    [Header("Spawnable Items")]
    [SerializeField] private GameObject[] items;

    [Header("Items Rarity Multiplicator")]
    [SerializeField] private static int mPrimaryStat;//Primary stat multiplicator
    [SerializeField] private static int mSecondaryStat;//Secondary stat multiplicator
    [SerializeField] private static int mDirectStat;//Direct stat multiplicator

    [Header("Bonus Item")]
    [Tooltip("Luck stat to have for ensure a Item bonus.\n" +
             "If the luck stat if inferior of the tier, there are a random chance to get a bonus item.")]
    [SerializeField] private int itemBonusTier;


    private void Start()
    {
        SpawnSpecificNbItems(20, transform.position, transform.rotation);
        
    }


    public void SpawnSpecificNbItems(int _nbItemToSpawn, Vector3 _spawnPosition, Quaternion _spawnRotation)
    {
        List<GameObject> itemsCanBeSpawned = new List<GameObject>(items);

        if (_nbItemToSpawn > itemsCanBeSpawned.Count)
        {
            _nbItemToSpawn = itemsCanBeSpawned.Count;
            Debug.LogWarning("Nb Item to spawn is superior to nb item in the array.\n" +
                             "Nb Item to spawn is resize to nb item in the array");
        }


        for (int i = 0; i < _nbItemToSpawn; i++)
        {
            int maxRandom = 0;
            foreach (GameObject item in itemsCanBeSpawned)
            {
                maxRandom += ItemRarity(item.GetComponent<Item>());
            }

            int random = Random.Range(0, maxRandom);

            int checkRandom = 0;
            for (int j = 0; j < itemsCanBeSpawned.Count; j++)
            {
                checkRandom += ItemRarity(itemsCanBeSpawned[j].GetComponent<Item>());
                if (random <= checkRandom)
                {
                    SpawnItem(itemsCanBeSpawned[j], _spawnPosition, _spawnRotation);
                    itemsCanBeSpawned.Remove(itemsCanBeSpawned[j]);
                    break;
                }
            }



        }
    }

    //Spawn items from a number
    private void SpawnItem(GameObject itemChosen, Vector3 _spawnPosition, Quaternion _spawnRotation)
    {
        float x = Random.Range(-10f, 10f);
        float y = Random.Range(-10f, 10f);
        Vector3 spawnPosition = _spawnPosition + new Vector3(x, y, _spawnPosition.z);

        Instantiate(itemChosen, spawnPosition, _spawnRotation);

    }

    //Determine the number of Item to spawn
    public int AddBonusItemToSpawn(int _luckState)
    {
        int nbBonusItemsToSpawn = (_luckState / itemBonusTier);
        if(Random.Range(0.0f, 1.0f) >= (_luckState / itemBonusTier) % 1)
            nbBonusItemsToSpawn += 1;

        return nbBonusItemsToSpawn;
    }

    public int ItemRarity(Item item)
    {
        return (item.Strength + item.Dexterity + item.Intelligence) * mPrimaryStat +
               (item.Charism + item.Luck) * mSecondaryStat +
               (item.Hp + item.CritRate) * mDirectStat;
    }
}
