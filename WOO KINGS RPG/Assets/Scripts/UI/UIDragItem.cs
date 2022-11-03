using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public enum EquipFieldType
{
    None,
    Weapon,
    Armour,
    Gauntlet,
    Helmet,
    Boots,
    Shield
}

public class UIDragItem : UIItem, IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler, IDropHandler, IPointerClickHandler
{
    private static UIDraggingItem draggingItem;
    private static UIShowItem showItem;
    private UIInventory uiInventory;
    public EquipFieldType equipFieldType = EquipFieldType.None;
    
    float interval = 0.25f;
    float doubleClickTime = -1.0f;



    public override void Init()
    {
        base.Init();
        if(draggingItem == null)
        {
            draggingItem = Instantiate(Resources.Load<UIDraggingItem>("Prefabs/UI/DraggingItem"));
            draggingItem.gameObject.SetActive(false);
            draggingItem.Init();
        }

        if(showItem == null)
        {
            showItem = Instantiate(Resources.Load<UIShowItem>("Prefabs/UI/ShowItem"));
            showItem.gameObject.SetActive(false);
            showItem.Init();
        }

        uiInventory = FindObjectOfType<UIInventory>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(draggingItem != null && HasItem)
        {
            draggingItem.Show(true, this);
            SetActive(false);
            SetActiveCount(false);
            AudioMng.Instance.PlayUIEffect("ItemMoveSwitch", 0.5f);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggingItem != null)
        {
            draggingItem.Position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggingItem != null)
        {
            draggingItem.SetActive(false);
            if (HasItem)
            {
                SetActive(true);
                if(itemCategory == ItemCategory.Quest)
                    SetActiveCount(true);
                AudioMng.Instance.PlayUIEffect("ItemMoveSwitch", 0.5f);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetSelect(true);
        if (showItem != null && HasItem)
        {
            if (eventData.position.x > 960)
            {
                Vector2 pos = eventData.position;
                pos.x = pos.x - 300;
                showItem.Position = pos;
            }
            else
            {
                Vector2 pos = eventData.position;
                pos.x = pos.x + 300;
                showItem.Position = pos;
            }

            showItem.Show(true, this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetSelect(false);
        if (showItem != null)
        {
            showItem.SetActive(false);
        }
    }

    public void DeleteItem()
    {
        HasItem = false;
        itemID = -1;
        ItemRef = null;
        itemName = null;
        itemCategory = ItemCategory.None;
        count = 0;
        SetActiveCount(false);
        itemContents = null;
        equipType = EquipType.None;
        addAtk = 0;
        addDef = 0;
        addHp = 0;
        SetActive(false);
    }

    public void CreateItemInChar(int itemID)
    {
        string icon = DataManager.ToS(TableType.ItemTable, itemID, "ICON");
        Sprite itemSprite = Resources.Load<Sprite>("Images/" + icon);
        SetIcon(itemSprite);
        SetNativeSize();
        SetActive(true);
        HasItem = true;
        this.itemID = itemID;
        ItemRef = DataManager.ToS(TableType.ItemTable, itemID, "ITEMREF");
        itemName = DataManager.ToS(TableType.ItemTable, itemID, "ITEMNAME");
        string ct = DataManager.ToS(TableType.ItemTable, itemID, "CATEGORY");
        itemCategory = (ItemCategory)Enum.Parse(typeof(ItemCategory), ct);
        itemContents = DataManager.ToS(TableType.ItemTable, itemID, "CONTENTS");
        string et = DataManager.ToS(TableType.ItemTable, itemID, "EQUIPTYPE");
        equipType = (EquipType)Enum.Parse(typeof(EquipType), et);
        addAtk = DataManager.ToI(TableType.ItemTable, itemID, "ATK");
        addDef = DataManager.ToI(TableType.ItemTable, itemID, "DEF");
        addHp = DataManager.ToI(TableType.ItemTable, itemID, "HP");
        count = 0;

        if (equipFieldType != EquipFieldType.None) // 아이템 착용시 스탯 조정하기
        {
            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                player.SetItemStat(addAtk, 0, addDef, 0, addHp, 0);
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        UIItem draggedItem = eventData.pointerDrag.GetComponent<UIItem>();
        if (!draggedItem.HasItem) return;

        if(equipFieldType == EquipFieldType.None ||
            equipFieldType == EquipFieldType.Weapon && draggedItem.equipType == EquipType.Weapon ||
            equipFieldType == EquipFieldType.Armour && draggedItem.equipType == EquipType.Armour ||
            equipFieldType == EquipFieldType.Gauntlet && draggedItem.equipType == EquipType.Gauntlet ||
            equipFieldType == EquipFieldType.Helmet && draggedItem.equipType == EquipType.Helmet ||
            equipFieldType == EquipFieldType.Boots && draggedItem.equipType == EquipType.Boots ||
            equipFieldType == EquipFieldType.Shield && draggedItem.equipType == EquipType.Shield)
        {
            int tempAddAtk = 0;
            int tempAddDef = 0;
            int tempAddHp = 0;
            if (IsEmpty() && !HasItem)
            {
                SetIcon(draggedItem.IconSprite);
                SetNativeSize();
                SetActive(true);
                HasItem = true;
                itemID = draggedItem.itemID;
                ItemRef = draggedItem.ItemRef;
                itemName = draggedItem.itemName;
                itemCategory = draggedItem.itemCategory;
                itemContents = draggedItem.itemContents;
                equipType = draggedItem.equipType;
                addAtk = draggedItem.addAtk;
                addDef = draggedItem.addDef;
                addHp = draggedItem.addHp;
                count = draggedItem.count;
                InputCount(count);
                if(itemCategory == ItemCategory.Quest)
                    SetActiveCount(true);
                draggedItem.SetActive(false);
                draggedItem.HasItem = false;
                draggedItem.itemID = -1;
                draggedItem.ItemRef = null;
                draggedItem.itemName = null;
                draggedItem.itemCategory = ItemCategory.None;
                draggedItem.count = 0;
                draggedItem.SetActiveCount(false);
                draggedItem.itemContents = null;
                draggedItem.equipType = EquipType.None;
                draggedItem.addAtk = 0;
                draggedItem.addDef = 0;
                draggedItem.addHp = 0;
                if(DataManager.isBoxOpen) // 아이템 상자에 넣고 빼는 조건
                {
                    if((draggedItem.transform.parent.tag == "UIItemBox") && (transform.parent.tag != "UIItemBox"))
                        DataManager.DelItemInBox(itemID);
                    else if((draggedItem.transform.parent.tag != "UIItemBox") && (transform.parent.tag == "UIItemBox"))
                        DataManager.AddItemInBox(itemID);
                }

            }
            else
            {
                Sprite prevSprite = IconSprite;

                SetIcon(draggedItem.IconSprite);
                SetNativeSize();
                SetActive(true);
                HasItem = true;
                int tempItemID = itemID;
                string tempItemRef = ItemRef;
                string tempItemName = itemName;
                ItemCategory tempItemType = itemCategory;
                string tempItemContents = itemContents;
                EquipType tempEquipType = equipType;
                tempAddAtk = addAtk;
                tempAddDef = addDef;
                tempAddHp = addHp;
                int tempCount = count;

                itemID = draggedItem.itemID;
                ItemRef = draggedItem.ItemRef;
                itemName = draggedItem.itemName;
                itemCategory = draggedItem.itemCategory;
                itemContents = draggedItem.itemContents;
                equipType = draggedItem.equipType;
                addAtk = draggedItem.addAtk;
                addDef = draggedItem.addDef;
                addHp = draggedItem.addHp;
                count = draggedItem.count;
                InputCount(count);
                if (itemCategory == ItemCategory.Quest)
                    SetActiveCount(true);
                else
                    SetActiveCount(false);

                draggedItem.SetIcon(prevSprite);
                draggedItem.SetNativeSize();
                draggedItem.itemID = tempItemID;
                draggedItem.ItemRef = tempItemRef;
                draggedItem.itemName = tempItemName;
                draggedItem.itemCategory = tempItemType;
                draggedItem.itemContents = tempItemContents;
                draggedItem.equipType = tempEquipType;
                draggedItem.addAtk = tempAddAtk;
                draggedItem.addDef = tempAddDef;
                draggedItem.addHp = tempAddHp;
                draggedItem.count = tempCount;
                draggedItem.InputCount(count);
                if (draggedItem.itemCategory == ItemCategory.Quest)
                    draggedItem.SetActiveCount(true);
                else
                    draggedItem.SetActiveCount(false);

                if (DataManager.isBoxOpen) // 아이템 상자에 넣고 빼는 조건
                {
                    if ((draggedItem.transform.parent.tag == "UIItemBox") && (transform.parent.tag != "UIItemBox"))
                    {
                        DataManager.DelItemInBox(itemID);
                        DataManager.AddItemInBox(draggedItem.itemID);
                    }
                    else if ((draggedItem.transform.parent.tag != "UIItemBox") && (transform.parent.tag == "UIItemBox"))
                    {
                        DataManager.DelItemInBox(draggedItem.itemID);
                        DataManager.AddItemInBox(itemID);
                    }
                }
            }
            AudioMng.Instance.PlayUIEffect("ItemMoveSwitch", 0.5f);


            if(equipFieldType != EquipFieldType.None) // 아이템 착용시 스탯 조정하기
            {
                Player player = FindObjectOfType<Player>();
                if(player != null)
                {
                    player.SetItemStat(addAtk, tempAddAtk, addDef, tempAddDef, addHp, tempAddHp);
                }
            }

            UIDragItem offedField = draggedItem.GetComponent<UIDragItem>();
            if (offedField.equipFieldType != EquipFieldType.None)
            {
                Player player = FindObjectOfType<Player>();
                if (player != null)
                {
                    int minusAtk = addAtk;
                    int plusAtk = tempAddAtk;
                    int minusDef = addDef;
                    int plusDef = tempAddDef;
                    int minusHp = addHp;
                    int plusHp = tempAddHp;
                    player.SetItemStat(plusAtk, minusAtk, plusDef, minusDef, plusHp, minusHp);
                }
            }

        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if((Time.time - doubleClickTime) < interval)
        {
            doubleClickTime = -1.0f;

            if(HasItem && equipFieldType == EquipFieldType.None && equipType != EquipType.None)
            {
                EquipGroup equipGroup = FindObjectOfType<EquipGroup>();
                if(equipGroup != null)
                {
                    UIDragItem[] fields = equipGroup.GetComponentsInChildren<UIDragItem>();
                    foreach(UIDragItem field in fields)
                    {
                        if(field.equipFieldType == EquipFieldType.Weapon && equipType == EquipType.Weapon ||
                            field.equipFieldType == EquipFieldType.Armour && equipType == EquipType.Armour ||
                            field.equipFieldType == EquipFieldType.Gauntlet && equipType == EquipType.Gauntlet ||
                            field.equipFieldType == EquipFieldType.Helmet && equipType == EquipType.Helmet ||
                            field.equipFieldType == EquipFieldType.Boots && equipType == EquipType.Boots ||
                            field.equipFieldType == EquipFieldType.Shield && equipType == EquipType.Shield)
                        {
                             
                            if (field.HasItem)
                            {
                                Sprite sprite = field.IconSprite;
                                bool tempHasItem = field.HasItem;
                                int tempItemID = field.itemID;
                                string tempItemRef = field.ItemRef;
                                string tempItemName = field.itemName;
                                ItemCategory tempCategory = field.itemCategory;
                                int tempCount = field.count;
                                string tempContents = field.itemContents;
                                EquipType tempEquipType = field.equipType;
                                int tempAtk = field.addAtk;
                                int tempHp = field.addHp;
                                int tempDef = field.addDef;
                                
                                field.SetIcon(IconSprite);
                                field.HasItem = tempHasItem;
                                field.itemID = itemID;
                                field.ItemRef = ItemRef;
                                field.itemName = itemName;
                                field.itemCategory = itemCategory;
                                field.count = count;
                                field.itemContents = itemContents;
                                field.equipType = equipType;
                                field.addAtk = addAtk;
                                field.addHp = addHp;
                                field.addDef = addDef;
                                field.SetActive(true);

                                SetIcon(sprite);
                                HasItem = true;
                                itemID = tempItemID;
                                ItemRef = tempItemRef;
                                itemName = tempItemName;
                                itemCategory = tempCategory;
                                count = tempCount;
                                SetActiveCount(false);
                                itemContents = tempContents;
                                equipType = tempEquipType;
                                addAtk = tempAtk;
                                addDef = tempDef;
                                addHp = tempHp;
                                SetActive(true);

                                Player player = FindObjectOfType<Player>();
                                if (player != null)
                                {
                                    player.SetItemStat(field.addAtk, addAtk, field.addDef, addDef, field.addHp, addHp);
                                }
                            }
                            else
                            {
                                field.SetIcon(IconSprite);
                                field.HasItem = true;
                                field.itemID = itemID;
                                field.ItemRef = ItemRef;
                                field.itemName = itemName;
                                field.itemCategory = itemCategory;
                                field.count = count;
                                field.itemContents = itemContents;
                                field.equipType = equipType;
                                field.addAtk = addAtk;
                                field.addHp = addHp;
                                field.addDef = addDef;
                                field.SetActive(true);

                                HasItem = false;
                                itemID = -1;
                                ItemRef = null;
                                itemName = null;
                                itemCategory = ItemCategory.None;
                                count = 0;
                                SetActiveCount(false);
                                itemContents = null;
                                equipType = EquipType.None;
                                addAtk = 0;
                                addDef = 0;
                                addHp = 0;
                                SetActive(false);

                                Player player = FindObjectOfType<Player>();
                                if (player != null)
                                {
                                    player.SetItemStat(field.addAtk, 0, field.addDef, 0, field.addHp, 0);
                                }
                            }

                            if (DataManager.isBoxOpen) // 아이템 상자에 넣고 빼는 조건
                            {
                                if ((field.transform.parent.tag != "UIItemBox") && (transform.parent.tag == "UIItemBox"))
                                {
                                    DataManager.DelItemInBox(field.itemID);
                                    DataManager.AddItemInBox(itemID);
                                }
                            }
                            AudioMng.Instance.PlayUIEffect("ItemMoveSwitch", 0.5f);
                        }
                    }
                        
                }

            }
            else if (HasItem && equipFieldType != EquipFieldType.None)
            {
                if(uiInventory!=null)
                {
                    uiInventory.ItemAdd(itemID);

                    int minusAtk = addAtk;
                    int minusDef = addDef;
                    int minusHp = addHp;

                    HasItem = false;
                    itemID = -1;
                    ItemRef = null;
                    itemName = null;
                    itemCategory = ItemCategory.None;
                    count = 0;
                    SetActiveCount(false);
                    itemContents = null;
                    equipType = EquipType.None;
                    addAtk = 0;
                    addDef = 0;
                    addHp = 0;
                    SetActive(false);

                    Player player = FindObjectOfType<Player>();
                    if (player != null)
                    {
                        player.SetItemStat(0,minusAtk, 0, minusDef, 0, minusHp);
                    }

                    AudioMng.Instance.PlayUIEffect("ItemMoveSwitch", 0.5f);
                }
            }

        }
        else
        {
            doubleClickTime = Time.time;
        }
    }
}
