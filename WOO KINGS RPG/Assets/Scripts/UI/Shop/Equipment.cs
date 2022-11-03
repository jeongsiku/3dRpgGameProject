using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    UIShop uiShop;

    public void Init()
    {
        uiShop = GetComponent<UIShop>();
        if(uiShop != null)
        {
            uiShop.SetTab(EquipType.All);
            uiShop.SetItemList(GameDB.GetItems((int)BitEquipType.All));
        }
    }

    void Update()
    {
        
    }
}
