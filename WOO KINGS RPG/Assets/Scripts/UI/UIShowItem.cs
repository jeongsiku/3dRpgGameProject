using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIShowItem : MonoBehaviour
{
    public RectTransform root;
    public RectTransform background;
    public Image icon;
    public UIItem showItem;
    public TMP_Text itemName;
    public TMP_Text itemType;
    public TMP_Text itemContents;
    public TMP_Text itemStatAtk;
    public TMP_Text itemStatDef;
    public TMP_Text itemStatHp;
    VerticalLayoutGroup verticalLayoutGroup; 

    public Vector3 Position
    {
        set
        {
            root.transform.position = value;
        }
    }

    public void Init()
    {
        Transform rootTrans = transform.Find("Root");
        root = rootTrans.GetComponent<RectTransform>();
        Transform t = transform.Find("Root/Background");
        if (t != null)
        {
            background = t.GetComponent<RectTransform>();
            Transform ic = t.Find("IconBg/Icon");
            if (ic != null)
            {
                icon = ic.GetComponent<Image>();
                icon.gameObject.SetActive(true);
            }

            Transform name = t.Find("IconBg/Name");
            if (name != null)
            {
                itemName = name.GetComponent<TMP_Text>();
                itemName.gameObject.SetActive(true);
            }

            Transform type = t.Find("IconBg/ItemType");
            if (type != null)
            {
                itemType = type.GetComponent<TMP_Text>();
                itemType.gameObject.SetActive(true);
            }

            Transform cont = t.Find("TextGroup/Content");
            if (cont != null)
            {
                itemContents = cont.GetComponent<TMP_Text>();
                itemContents.gameObject.SetActive(true);
            }

            Transform statAtk = t.Find("TextGroup/StatAtk");
            if (statAtk != null)
            {
                itemStatAtk = statAtk.GetComponent<TMP_Text>();
                itemStatAtk.gameObject.SetActive(true);
            }

            Transform statDef = t.Find("TextGroup/StatDef");
            if (statDef != null)
            {
                itemStatDef = statDef.GetComponent<TMP_Text>();
                itemStatDef.gameObject.SetActive(true);
            }

            Transform statHp = t.Find("TextGroup/StatHp");
            if (statHp != null)
            {
                itemStatHp = statHp.GetComponent<TMP_Text>();
                itemStatHp.gameObject.SetActive(true);
            }

            verticalLayoutGroup = t.GetComponent<VerticalLayoutGroup>();
        }
    }

    public void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
        //icon.SetNativeSize();
    }

    public void SetName(string name)
    {
        itemName.text = name;
    }

    public void SetType(ItemCategory category)
    {
        
        switch(category)
        {
            case ItemCategory.Keys:
                itemType.text = "[ ¿­ ¼è ]";
                itemType.color = Color.red;
                break;
            case ItemCategory.Equipable:
                itemType.text = "[ Àå ºñ ]";
                itemType.color = Color.green;
                break;
            case ItemCategory.Quest:
                itemType.text = "[ Äù ½º Æ® ]";
                itemType.color = Color.magenta;
                break;
        }
    }

    public void SetContent(string text)
    {
        itemContents.text = string.Empty;
        itemContents.text = text;
    }

    public void SetStatText(int atk, int def, int hp)
    {
        itemStatAtk.gameObject.SetActive(false);
        itemStatDef.gameObject.SetActive(false);
        itemStatHp.gameObject.SetActive(false);
        if (atk>0)
        {
            itemStatAtk.text = "ATK + " + atk;
            itemStatAtk.gameObject.SetActive(true);
        }
        if(def>0)
        {
            itemStatDef.text = "DEF + " + def;
            itemStatDef.gameObject.SetActive(true);
        }
        if(hp>0)
        {
            itemStatHp.text = "HP + " + hp;
            itemStatHp.gameObject.SetActive(true);
        }
    }

    public void SetActive(bool state)
    {
        if(state)
        {
            gameObject.SetActive(state);
        }
        else
        {
            itemContents.text = null;
            itemStatAtk.text = null;
            itemStatDef.text = null;
            itemStatHp.text = null;
            gameObject.SetActive(state);
        }
    }

    public void Show(bool state, UIItem item)
    {
        SetIcon(item.IconSprite);
        SetName(item.itemName);
        SetType(item.itemCategory);
        SetContent(item.itemContents);
        SetStatText(item.addAtk, item.addDef, item.addHp);
        
        SetActive(state);
    }
}
