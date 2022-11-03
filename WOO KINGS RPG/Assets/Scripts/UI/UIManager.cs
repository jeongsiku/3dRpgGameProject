using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    UICharInfo uiCharInfo;
    UIQuest uiQuest;
    UIInventory uiInventory;
    EquipGroup charEquipGroup;
    UISystemMsg uiSystemMsg;
    UIPlayerAction uiPlayerAction;
    UICharPanelStat uiCharPanelStat;
    UIMinimap uiMinimap;
    UIShop uiShop;
    UIMenu uiMenu;
    Equipment equipment;
    TMP_Text gold;
    UIItemBox itemBox;

    public void Init()
    {
        DontDestroyOnLoad(gameObject);
        
        uiCharInfo = GetComponentInChildren<UICharInfo>();
        if(uiCharInfo != null) uiCharInfo.Init();
        
        uiQuest = GetComponentInChildren<UIQuest>();
        if(uiQuest != null) uiQuest.Init();

        uiInventory = GetComponentInChildren<UIInventory>();
        if(uiInventory != null) uiInventory.Init();

        charEquipGroup = GetComponentInChildren<EquipGroup>();
        charEquipGroup?.Init();

        itemBox = GetComponentInChildren<UIItemBox>();
        itemBox?.Init();

        uiSystemMsg = GetComponentInChildren<UISystemMsg>(true);
        if(uiSystemMsg != null) uiSystemMsg.Init();

        uiCharPanelStat = GetComponentInChildren<UICharPanelStat>();
        if(uiCharPanelStat != null) uiCharPanelStat.Init();

        uiMinimap = GetComponentInChildren<UIMinimap>();
        uiMinimap?.Init();

        uiShop = GetComponentInChildren<UIShop>(true);
        uiShop?.gameObject.SetActive(true);
        uiShop?.Init();

        equipment = GetComponentInChildren<Equipment>();
        equipment?.Init();

        uiMenu = GetComponentInChildren<UIMenu>(true);
        uiMenu?.Init();
        

        gold = transform.Find("MenuGroup/Gold/Gold").GetComponent<TMP_Text>(); 
    }

    public void SetPlayerHp(int hp)
    {
        uiCharInfo.SetPlayerHp(hp);
    }

    public void SetPlayerMaxHp(int hp)
    {
        uiCharInfo.SetPlayerMaxHp(hp);
    }

    #region  Input to CharPanelStat START

    public void SetCurrentPlayerATKPoint(int atk)
    {
        uiCharPanelStat.SetCurrentAtk(atk);
    }

    public void SetMaxPlayerATKPoint(int maxAtk)
    {
        uiCharPanelStat.SetMaxAtk(maxAtk);
    }

    public void SetCurrentPlayerDEFPoint(int def)
    {
        uiCharPanelStat.SetCurrentDef(def);
    }

    public void SetMaxPlayerDEFPoint(int maxDef)
    {
        uiCharPanelStat.SetMaxDef(maxDef);
    }

    public void SetCurrentPlayerHPPoint(int hp)
    {
        uiCharPanelStat.SetCurrentHp(hp);
    }

    public void SetMaxPlayerHPPoint(int maxHp)
    {
        uiCharPanelStat.SetMaxHp(maxHp);
    }
    #endregion Input to CharPanelStat END

    public void RefreshGold()
    {
        this.gold.text = string.Format("{0:N0}", GameData.gold);
    }

    public void OpenBox()
    {

    }

    public void PrintMessage(string msg)
    {
        uiSystemMsg.Print(msg);
    }

    private void OnGUI()
    {
        //if (GUI.Button(new Rect(100, 150, 200, 100), "Random Item"))
        //{
        //    uiInventory.GetRandomItem(); // 4 ~ 21번 아이템 중 랜덤 생성
        //}

        //if (GUI.Button(new Rect(400, 150, 200, 100), "Do Test"))
        //{
        //    //uiInventory.ItemAdd(22);
        //    //uiQuest.HarvestMushroom();
        //}
    }

    void Update()
    {
        
    }
}
