using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : MonoBehaviour
{
    private int strengthNeed;
    //Remplacer par une interface
    public void HitWall(int Strength)
    {
        if (Strength < strengthNeed)
        {
            //Do something
            return;
        }

        //Do Something

        Destroy(gameObject);
    }


    #region Getter & Setter
    public int StrengthNeed
    {
        get { return StrengthNeed; }
        set { strengthNeed = value; }
    }
    #endregion
}
