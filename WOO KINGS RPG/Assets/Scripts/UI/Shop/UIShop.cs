using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : MonoBehaviour
{
    EquipType currType = EquipType.None;

    private UIShopTabButtons uiShopTabButton;
    
    private UIShopItem uiShopItemPrefab;
    private VerticalLayoutGroup verticalLayoutGroup;
    private int slotCount = 18;

    List<UIShopItem> shopItems = new List<UIShopItem>();

    private Dictionary<int , UIShopItem> itemDic = new Dictionary<int , UIShopItem>();

    UIShopItem nowItem = new UIShopItem();
    Button buyButton;

    public void Init()
    {
        uiShopTabButton = GetComponentInChildren<UIShopTabButtons>();
        uiShopTabButton.Init();

        uiShopItemPrefab = Resources.Load<UIShopItem>("Prefabs/UI/ShopItem");
        verticalLayoutGroup = GetComponentInChildren<VerticalLayoutGroup>(true);

        GameDB.CreateAllItem();

        CreateEmptySlot(slotCount);

        SetTabClickEvent(OnClickTab);
        SetClickEvent(OnClickItem);

        buyButton = transform.Find("Background/ButtonGroup/BuyButton").GetComponent<Button>();
        buyButton?.onClick.AddListener(BuyItem);

        InitTab();

        gameObject.SetActive(false);
    }

    public void OnClickItem(ShopItemInfo itemInfo, UIShopItem uiShopItem)
    {
        if (itemInfo != null)
            SelectShopItem(itemInfo, uiShopItem);
    }


    public void OnClickTab(UITabButton tab)
    {
        BitEquipType bitEquipType = BitEquipType.Weapon;
        System.Enum.TryParse<BitEquipType>(tab.EquipType.ToString(), out bitEquipType);
        SetTab(tab.EquipType);
        SetItemList(GameDB.GetItems((int)bitEquipType));
        AudioMng.Instance.PlayUIEffect("textbeep");

    }

    public void InitTab()
    {
        SetTab(EquipType.All);
        SetItemList(GameDB.GetItems((int)BitEquipType.All));
    }

    public void SetActive(bool state)
    {
        gameObject.SetActive(state);
    }

    public void Clear()
    {
        for(int i = 0; i < shopItems.Count; i++)
            shopItems[i].Clear();
    }

    void CreateEmptySlot(int count)
    {
        for(int i = 0; i < count; i++)
        {
            UIShopItem item = Instantiate(uiShopItemPrefab, verticalLayoutGroup.transform);
            item.Init();
            shopItems.Add(item);
        }
    }

    public void SetItemList(List<ShopItemInfo> itemList)
    {
        itemDic.Clear();

        for(int i = 0; i < itemList.Count; i++)
        {
            shopItems[i].SetInfo(itemList[i]);
            itemDic.Add(itemList[i].itemID, shopItems[i]);
        }
    }

    public void SetTabClickEvent(System.Action<UITabButton> onClick)
    {
        uiShopTabButton.SetListener(onClick);
    }
    
    public void SetTab(EquipType equipType)
    {
        if(currType == equipType) return;

        currType = equipType;
        uiShopTabButton.SetTab(currType);
        

        Clear();
    }

    public void SetClickEvent(System.Action<ShopItemInfo, UIShopItem> onClick)
    {
        for(int i = 0; i < shopItems.Count; i++)
        {
            shopItems[i].SetClickEvent(onClick);
        }
    }

    void SelectShopItem(ShopItemInfo itemInfo, UIShopItem uiShopItem)
    {
        DeselectAll();
        uiShopItem.SetSelect(true);
        GameData.selectedItemID = itemInfo.itemID;
        GameData.selectedItemCost = itemInfo.cost;
        GameData.seletedItemName = itemInfo.name;
        nowItem = uiShopItem;
        AudioMng.Instance.PlayUIEffect("textbeep");
    }

    void DeselectAll()
    {
        for(int i =0; i < shopItems.Count; i++)
        {
            shopItems[i].SetSelect(false);
        }
    }

    public void BuyItem()
    {
        if (GameData.selectedItemID == -1) return;

        UIInventory uIInventory = FindObjectOfType<UIInventory>();
        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uIInventory != null)
        {
            if(GameData.selectedItemCost > GameData.gold)
            {
                uiManager.PrintMessage("골드가 부족합니다.");
                AudioMng.Instance.PlayUIEffect("ui_menu_button_cancel_02");
            }
            else
            {
                uIInventory.ItemAdd(GameData.selectedItemID);
                GameData.gold -= GameData.selectedItemCost;
                uiManager.RefreshGold();
                uiManager.PrintMessage($"{GameData.seletedItemName}을(를) 구입했습니다.");
                AudioMng.Instance.PlayUIEffect("ui_menu_button_confirm_02");
            }
        }
    }
}
