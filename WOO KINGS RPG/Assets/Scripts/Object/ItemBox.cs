using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : ScanableObject
{
    public int boxNum = 0;

    public List<int> itemList = null;
    bool isOpened = false;
    
    public void Interaction()
    {
        DataManager.onBoxNum = boxNum;
        DataManager.isBoxOpen = true;
        if(isOpened)
            Open();
        else
            FirstOpen();
    }

    void FirstOpen()
    {
        ClearField();
        isOpened = true;
        UIItemBox uiItemBox = FindObjectOfType<UIItemBox>();
        if(uiItemBox != null)
        {
            for(int i = 0; i < itemList.Count; i++)
            {
                uiItemBox.ItemAdd(itemList[i]);
                DataManager.AddItemInBox(itemList[i]);
            }
        }
    }

    void Open()
    {
        ClearField();
        UIItemBox uiItemBox = FindObjectOfType<UIItemBox>();
        if (uiItemBox != null)
        {
            int count = DataManager.GetCount();
            for (int i = 0; i < count; i++)
            {
                int itemID = DataManager.GetID(i);
                uiItemBox.ItemAdd(itemID);
            }
        }
    }

    void ClearField()
    {
        UIItemBox uiItemBox = FindObjectOfType<UIItemBox>();
        if( uiItemBox != null )
        {
            for(int i=0; i < uiItemBox.transform.childCount; i++)
            {
                UIDragItem item = uiItemBox.transform.GetChild(i).GetComponent<UIDragItem>();
                item.HasItem = false;
                item.SetActive(false);
            }
        }
    }
}
