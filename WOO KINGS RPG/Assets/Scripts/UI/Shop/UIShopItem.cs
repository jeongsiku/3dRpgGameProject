using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIShopItem : UIShopItemBase
{
    TMP_Text uiName;
    TMP_Text uiNote;
    TMP_Text uiCost;
    Image icon;
    Image coin;
    Transform atk;
    Transform def;
    Transform hp;
    Image atkBar;
    Image defBar;
    Image hpBar;
    Image select;
    Button button;

    public System.Action<ShopItemInfo, UIShopItem> onClick;

    public void Init()
    {
        
        icon = transform.Find("Icon").GetComponent<Image>();
        coin = transform.Find("Coin").GetComponent<Image>();
        uiName = transform.Find("Name").GetComponent<TMP_Text>();
        uiNote = transform.Find("Note").GetComponent<TMP_Text>();
        uiCost = transform.Find("Cost").GetComponent<TMP_Text>();
        atk = transform.Find("ATK");
        def = transform.Find("DEF");
        hp = transform.Find("HP");
        atkBar = transform.Find("ATK/AtkBar").GetComponent<Image>();
        defBar = transform.Find("DEF/DefBar").GetComponent<Image>();
        hpBar = transform.Find("HP/HpBar").GetComponent<Image>();
        select = transform.Find("Select").GetComponent<Image>();
        button = GetComponent<Button>();
        button?.onClick.AddListener(OnClick);

        Clear();
    }

    public void Clear()
    {
        icon?.gameObject.SetActive(false);
        uiName?.gameObject.SetActive(false);
        uiNote?.gameObject.SetActive(false);
        uiCost?.gameObject.SetActive(false);
        coin?.gameObject.SetActive(false);
        atk?.gameObject.SetActive(false);
        def?.gameObject.SetActive(false);
        hp?.gameObject.SetActive(false);
        select?.gameObject.SetActive(false);
        if (button != null)
            button.enabled = false;
    }

    public void SetIcon(Sprite sprite)
    {
        if(icon != null)
            icon.sprite = sprite;
    }

    public void SetStat(int atk = 0, int def= 0, int hp= 0)
    {
        atkBar.fillAmount = (float)atk / 40;
        defBar.fillAmount = (float)def / 40;
        hpBar.fillAmount = (float)hp / 40;
    }
    public void SetActiveStat(bool state)
    {
        atk.gameObject.SetActive(state);
        def.gameObject.SetActive(state);
        hp.gameObject.SetActive(state);
        atkBar.gameObject.SetActive(state);
        defBar.gameObject.SetActive(state);
        hpBar.gameObject.SetActive(state);
        button.enabled=state;
    }
    
    public void SetUIName(string name)
    {
        uiName.text = name;
    }

    public void SetUINote(string note)
    {
        uiNote.text = note;
    }

    public void SetUICost(int cost)
    {
        uiCost.text = string.Format($"{cost:N0}");
    }

    public override void SetInfo(ShopItemInfo info)
    {
        base.SetInfo(info);
        if(info != null)
        {
            string icon = DataManager.ToS(TableType.ItemTable, info.itemID, "ICON");
            Sprite itemSprite = Resources.Load<Sprite>("Images/" + icon);
            SetIcon(itemSprite);
            this.icon.gameObject.SetActive(true);
            SetStat(info.atkPoint, info.defPoint, info.hpPoint);
            SetActiveStat(true);
            SetUIName(info.name);
            uiName.gameObject.SetActive(true);
            SetUINote(info.note);
            uiNote.gameObject.SetActive(true);
            SetUICost(info.cost);
            uiCost.gameObject.SetActive(true);
            coin.gameObject.SetActive(true);
        }
        
    }

    public void SetClickEvent(System.Action<ShopItemInfo, UIShopItem> onClick)
    {
        this.onClick = onClick;
    }

    void OnClick()
    {
        onClick?.Invoke(info, this);
    }

    public void SetSelect(bool state)
    {
        select.gameObject.SetActive(state);
    }
}
