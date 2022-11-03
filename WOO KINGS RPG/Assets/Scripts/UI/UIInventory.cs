using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class UIInventory : MonoBehaviour
{


    public void Init()
    {
        for(int i=0; i < transform.childCount; i++)
        {
            UIDragItem item = transform.GetChild(i).GetComponent<UIDragItem>();
            item?.Init();
        }
    }

    public void GetItem(string ItemName)
    {
        UIDragItem field = CheckEmptyField();
        if(field != null)
        {
            field.HasItem = true;
            field.ItemRef = ItemName;
            Sprite sprite = Resources.Load<Sprite>("Images/shrineKey");
            field.SetIcon(sprite);
            field.SetActive(true);
        }
        else
        {

        }
    }

    public void ItemAdd(int itemID)
    {
        UIDragItem field = CheckHaveSameItem(itemID);
        if(field != null)
        {
            AddItemCount(field);
        }
        else
        {
            field = CheckEmptyField();
            if (field != null)
            {
                SetItem(field, itemID);
            }
        }
    }

    public void DeleteItem(int itemID)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            UIDragItem field = transform.GetChild(i).GetComponent<UIDragItem>();
            if(field != null)
            {
                if (field.itemID == itemID && field.itemCategory == ItemCategory.Quest)
                {
                    field.DeleteItem();
                }
            }
        }
    }

    UIDragItem CheckHaveSameItem(int itemID)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            UIDragItem field = transform.GetChild(i).GetComponent<UIDragItem>();
            if (field != null)
            {
                if (field.itemID == itemID && field.itemCategory == ItemCategory.Quest)
                {
                    return field;
                }
            }
        }
        return null;
    }

    void AddItemCount(UIDragItem field)
    {
        field.count++;
        field.InputCount(field.count);
    }

    UIDragItem CheckEmptyField()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            UIDragItem field = transform.GetChild(i).GetComponent<UIDragItem>();
            if(field != null)
            {
                if (field.HasItem == false)
                    return field;
            }
        }
        return null;
    }

    public void GetRandomItem()
    {
        int itemID = UnityEngine.Random.Range(4, 22);
        ItemAdd(itemID);
    }

    public void SetItem(UIDragItem field, int itemID)
    {
        field.itemID = itemID;
        field.HasItem = true;
        field.ItemRef = DataManager.ToS(TableType.ItemTable, itemID, "ITEMREF");
        field.itemName = DataManager.ToS(TableType.ItemTable, itemID, "ITEMNAME");
        string et = DataManager.ToS(TableType.ItemTable, itemID, "EQUIPTYPE");
        field.equipType = (EquipType)Enum.Parse(typeof(EquipType), et);
        string ct = DataManager.ToS(TableType.ItemTable, itemID, "CATEGORY");
        field.itemCategory = (ItemCategory)Enum.Parse(typeof(ItemCategory), ct);
        field.itemContents = DataManager.ToS(TableType.ItemTable, itemID, "CONTENTS");
        field.addAtk = DataManager.ToI(TableType.ItemTable, itemID, "ATK");
        field.addDef = DataManager.ToI(TableType.ItemTable, itemID, "DEF");
        field.addHp = DataManager.ToI(TableType.ItemTable, itemID, "HP");
        string icon = DataManager.ToS(TableType.ItemTable, itemID, "ICON");
        Sprite itemSprite = Resources.Load<Sprite>("Images/" + icon);
        field.count = 0;
        field.SetActiveCount(false);
        field.SetIcon(itemSprite);
        field.SetActive(true);
        if(field.itemCategory == ItemCategory.Quest)
        {
            field.count = 1;
            field.InputCount(field.count);
            field.SetActiveCount(true);
        }
        
    }
}
