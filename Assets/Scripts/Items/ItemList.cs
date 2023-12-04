using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu()]
public class ItemList : ScriptableObject
{
    [SerializeField] private int mPrimaryStat;//Primary stat multiplicator
    [SerializeField] private int mSecondaryStat;//Secondary stat multiplicator
    [SerializeField] private int mDirectStat;//Direct stat multiplicator

    [SerializeField] private GameObject[] items;



    public int MPrimaryStat
    {
        get { return mPrimaryStat; }
    }
    public int MSecondaryStat
    {
        get { return mSecondaryStat; }
    }
    public int MDirectStat
    {
        get { return mDirectStat; }
    }

    public GameObject[] Items
    {
        get { return items; }
    }
}
