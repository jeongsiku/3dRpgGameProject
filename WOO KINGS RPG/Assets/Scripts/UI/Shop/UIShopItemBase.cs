using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShopItemBase : MonoBehaviour
{
    //public ShopItemInfo info = new ShopItemInfo();
    public ShopItemInfo info;

    public void SetName(string strName)
    {
        info.name = strName;
    }

    public void SetNote(string strNote)
    {
        info.note = strNote;
    }

    public void SetEquipType(EquipType type)
    {
        info.equipType = type;
    }

    public virtual void SetInfo(ShopItemInfo info)
    {
        this.info = info;
    }
}
