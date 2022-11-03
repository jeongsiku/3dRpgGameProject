using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InvenItemList
{
    public int itemID = -1;
    public int count = 1;

    public InvenItemList(int id, int cnt)
    {
        itemID = id;
        count = cnt;
    }
}


[System.Serializable]
public class SaveData
{
    public Vector3 charPosition;
    public int currentHp;
    public int maxHp;
    public int currGold;

    public int[] equipItemInfo =new int[6];

    public InvenItemList[] invenItemList = new InvenItemList[16];

}
