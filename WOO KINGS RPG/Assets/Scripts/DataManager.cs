using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TableType
{
    ItemTable
}

public class ItemListInBox
{
    public List<int> itemIDList = new List<int>();
}

public static class DataManager
{
    private static Dictionary<TableType, LowBase> tableList = new Dictionary<TableType, LowBase>();
    private static Dictionary<int , ItemListInBox> itemListInBox = new Dictionary<int , ItemListInBox>();
    public static int onBoxNum = -1;
    public static int onUIShrineNum = -1;
    public static bool isBoxOpen = false;

    public static int GetCount()
    {
        return itemListInBox[onBoxNum].itemIDList.Count;
    }

    public static int GetID(int value)
    {
        return itemListInBox[onBoxNum].itemIDList[value];
    }

    public static void DelItemInBox(int itemNum)
    {
        itemListInBox[onBoxNum].itemIDList.Remove(itemNum);
        //for(int i=0; i<itemListInBox[onBoxNum].itemIDList.Count; i++)
        //{
        //    Debug.Log(GetID(itemListInBox[onBoxNum].itemIDList[i]));
        //}
    }

    public static void AddItemInBox(int itemNum)
    {
        
        if (itemListInBox.ContainsKey(onBoxNum))
        {
            itemListInBox[onBoxNum].itemIDList.Add((int)itemNum);
        }
        else
        {
            ItemListInBox items = new ItemListInBox();
            items.itemIDList.Add(itemNum);
            itemListInBox.Add(onBoxNum, items);
        }
    }


    public static LowBase Get(TableType tableType)
    {
        LowBase lowBase = null;
        if(tableList.ContainsKey(tableType))
            lowBase = tableList[tableType];
        return lowBase;
    }

    public static string ToS(TableType tableType, int tableID, string subKey)
    {
        string str = string.Empty;
        if (tableList.ContainsKey(tableType))
        {
            str = tableList[tableType].ToS(tableID, subKey);
        }
        return str;
    }


    public static int ToI(TableType tableType, int tableID, string subKey)
    {
        int val = -1;
        if (tableList.ContainsKey(tableType))
        {
            val = tableList[tableType].ToI(tableID, subKey);
        }
        return val;
    }

    public static float ToF(TableType tableType, int tableID, string subKey)
    {
        float val = -1;
        if (tableList.ContainsKey(tableType))
        {
            val = tableList[tableType].ToF(tableID, subKey);
        }
        return val;
    }

    public static LowBase Load(TableType tableType, string path = "Tables/")
    {
        LowBase lowBase = null;
        if (tableList.ContainsKey(tableType))
            lowBase = tableList[tableType];
        else
        {
            TextAsset textAsset = Resources.Load<TextAsset>(path + tableType);
            if(textAsset != null)
            {
                lowBase = new LowBase();
                lowBase.Load(textAsset.text);
                tableList.Add(tableType, lowBase);
            }
        }
        return lowBase;
    }
}
