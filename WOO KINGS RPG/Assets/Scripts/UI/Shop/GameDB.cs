using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GameDB
{
    public static Dictionary<int, ShopItemInfo> itemDic = new Dictionary<int, ShopItemInfo>();
    public static SaveData saveData = new SaveData();

    public static void SetItem(int uniqueID, int itemID)
    {
        //DataManager.Load(TableType.ItemTable);
        ShopItemInfo item = new ShopItemInfo();
        item.itemID = itemID;
        item.name = DataManager.ToS(TableType.ItemTable, itemID, "ITEMNAME");
        item.note = DataManager.ToS(TableType.ItemTable, itemID, "CONTENTS");
        item.atkPoint = DataManager.ToI(TableType.ItemTable, itemID, "ATK");
        item.defPoint = DataManager.ToI(TableType.ItemTable, itemID, "DEF");
        item.hpPoint = DataManager.ToI(TableType.ItemTable, itemID, "HP");
        item.cost = DataManager.ToI(TableType.ItemTable, itemID, "COST");
        string et = DataManager.ToS(TableType.ItemTable, itemID, "EQUIPTYPE");
        item.equipType = (EquipType)System.Enum.Parse(typeof(EquipType), et);
        item.bitEquipType = (BitEquipType)System.Enum.Parse(typeof(BitEquipType), et);

        if (itemDic.ContainsKey(uniqueID) == false)
            itemDic.Add(uniqueID, item);
    }
       

    public static void CreateAllItem()
    {
        int uniqueCount = 0;
        
        for(int i=0; i<18; i++)
        {
            uniqueCount = i + 1;
            int tableID = i + 4;
            SetItem(uniqueCount, tableID);
        }
    }

    public static int Sort(ShopItemInfo left, ShopItemInfo right)
    {
        if (left.equipType > right.equipType)
            return 1;
        else if (left.equipType < right.equipType)
            return -1;
        else
        {
            int leftValue = left.atkPoint * 10000 + left.defPoint * 100 + left.hpPoint;
            int rightValue = right.atkPoint * 10000 + right.defPoint * 100 + right.hpPoint;

            if (leftValue > rightValue)
                return -1;
            else if (leftValue < rightValue)
                return 1;

            return 0;
        }
    }

    public static List<ShopItemInfo> GetItems(int category)
    {
        List<ShopItemInfo> itemList = new List<ShopItemInfo>();
        foreach(var pair in itemDic)
        {
            int state = category & (int)pair.Value.bitEquipType;
            if(state == (int)pair.Value.bitEquipType)
                itemList.Add(pair.Value);
        }

        itemList.Sort(Sort);

        return itemList;
    }

    public static void Save(string filename)
    {
        Player player = Transform.FindObjectOfType<Player>();
        saveData.charPosition = player.transform.position;
        saveData.currentHp = player.currentHp;
        saveData.currGold = GameData.gold;

        EquipGroup equipGroup = Transform.FindObjectOfType<EquipGroup>();
        for(int i =0; i<equipGroup.transform.childCount; i++)
        {
            UIDragItem item = equipGroup.transform.GetChild(i).GetComponent<UIDragItem>();
            if (item.HasItem)
                saveData.equipItemInfo[i] = item.itemID;
            else
                saveData.equipItemInfo[i] = -1;
        }

        UIInventory uiInventory = Transform.FindObjectOfType<UIInventory>();
        for(int i = 0; i<uiInventory.transform.childCount; i++)
        {
            UIDragItem item = uiInventory.transform.GetChild(i).GetComponent<UIDragItem>();

            InvenItemList invenitem = null; 
            if (item.HasItem)
            {
                invenitem = new InvenItemList(item.itemID, item.count);
            }
            else
            {
                invenitem = new InvenItemList(-1, 0);
            }
            saveData.invenItemList[i] = invenitem;

        }


        string json = JsonConvert.ToJson<SaveData>(saveData, true);
        WriteFile(GetPath(filename), json);
    }

    public static void Load(string filename)
    {
        string json = ReadFile(GetPath(filename));
        saveData = JsonConvert.FromJson<SaveData>(json);

        Player player = Transform.FindObjectOfType<Player>();
        player.transform.position = saveData.charPosition;
        GameData.gold = saveData.currGold;

        EquipGroup equipGroup = Transform.FindObjectOfType<EquipGroup>();
        for (int i = 0; i < equipGroup.transform.childCount; i++)
        {
            if (saveData.equipItemInfo[i] == -1) continue;
            UIDragItem item = equipGroup.transform.GetChild(i).GetComponent<UIDragItem>();
            item.CreateItemInChar(saveData.equipItemInfo[i]);
        }

        UIInventory uiInventory = Transform.FindObjectOfType<UIInventory>();
        for (int i = 0; i < uiInventory.transform.childCount; i++)
        {
            if (saveData.invenItemList[i].itemID == -1) continue;
            UIDragItem item = uiInventory.transform.GetChild(i).GetComponent<UIDragItem>();
            uiInventory.SetItem(item, saveData.invenItemList[i].itemID);
        }


        player.currentHp = saveData.currentHp;

        player.UpdateStatUI();
    }

    public static void WriteFile(string path, string content)
    {
        FileStream fileStream = new FileStream(path, FileMode.Create);
        using(StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(content);
        }
    }

    public static string ReadFile(string path)
    {
        FileStream fileStream = new FileStream(path, FileMode.Open);
        string readAllText = string.Empty;
        using(StreamReader reader = new StreamReader(fileStream))
        {
            readAllText = reader.ReadToEnd();
        }
        return readAllText;
    }

    public static string GetPath(string filename)
    {
        string filePath = "";
        if (Application.isMobilePlatform || Application.isConsolePlatform)
            filePath = Application.persistentDataPath;
        else
            filePath = Application.dataPath;

        return Path.Combine(filePath, filename);
    }
}
