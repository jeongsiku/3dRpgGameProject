using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIItemBox : UIInventory
{
    public void Init()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            UIDragItem item = transform.GetChild(i).GetComponent<UIDragItem>();
            item?.Init();
        }
    }
}