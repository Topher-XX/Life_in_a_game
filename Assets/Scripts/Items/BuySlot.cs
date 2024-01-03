using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuySlot : MonoBehaviour, IInteractInterface
{
    private GameObject itemToSell;
    private int itemPrice;

    private void Start()
    {
        itemToSell = ItemSpawnerManager.instance.GetRandomItem();
        GetComponent<SpriteRenderer>().sprite = itemToSell.GetComponent<SpriteRenderer>().sprite;

        //Add item's price
    }

    void IInteractInterface.Interact()
    {
        //if have money -> Buy
        Instantiate(itemToSell, transform.position, new Quaternion());

        Destroy(gameObject); // test
    }
}
