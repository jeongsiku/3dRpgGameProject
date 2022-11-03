using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum EquipType
{
    None,
    Weapon,
    Armour,
    Gauntlet,
    Helmet,
    Boots,
    Shield,
    All
}

public enum BitEquipType
{
    Weapon = 1,
    Armour = 2,
    Gauntlet = 4,
    Helmet = 8,
    Boots = 16,
    Shield = 32,
    All = Weapon | Armour | Gauntlet | Helmet | Boots | Shield
}

public enum ItemCategory
{
    None,
    Keys,
    Equipable,
    Quest
}

public class UIItem : MonoBehaviour
{
    public int itemID = -1;
    private Image icon;
    private Image background;
    private Image select;
    private TMP_Text countUI;
    [SerializeField]
    private bool hasItem = false;
    [SerializeField]
    private string itemRef = "";
    public EquipType equipType = EquipType.None;
    public string itemName = string.Empty;
    public ItemCategory itemCategory = ItemCategory.None;
    public string itemContents = string.Empty;
    public int count = 0;

    public int addAtk = 0;
    public int addDef = 0;
    public int addHp = 0;

    public Sprite IconSprite
    {
        get { return icon.sprite; }
    }

    public bool HasItem
    {
        get { return hasItem;}
        set { hasItem = value; }
    }

    public string ItemRef
    {
        get { return itemRef; }
        set { itemRef = value; }
    }

    public void InputCount(int count)
    {
        countUI.text = count.ToString();
        //countUI.gameObject.SetActive(true);
    }

    public bool IsEmpty()
    {
        return icon.IsActive() == false? true: false;
    }

    public void SetImage(Image image)
    {
        icon = image;
    }

    public void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
    }

    public void SetActive(bool state)
    {
        if(icon != null)
            icon.gameObject.SetActive(state);
    }

    public void SetActiveCount(bool state)
    {
        countUI.gameObject.SetActive(state);
    }


    public virtual void SetSelect(bool state)
    {
        select.gameObject.SetActive(state);
    }

    public virtual void Init()
    {
        gameObject.SetActive(true);
        background = GetComponent<Image>();
        Transform t = transform.Find("Select");
        if (t != null)
        {
            select = t.GetComponent<Image>();
            select.gameObject.SetActive(false);
        }

        t = transform.Find("ItemIcon");
        if(t != null)
            icon = t.GetComponent<Image>();

        t = transform.Find("Count");
        if (t != null)
            countUI = t.GetComponent<TMP_Text>();

        

        SetNativeSize();
    }

    public void SetNativeSize()
    {
        //icon.SetNativeSize();
        icon.rectTransform.sizeDelta = new Vector2(100, 100);
    }

    public void SwitchUI(bool state)
    {
        enabled = state;
        //SetActive(state);
    }
    
}
