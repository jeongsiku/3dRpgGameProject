using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDraggingItem : MonoBehaviour
{
    public Image background;
    public Image icon;
    public UIItem dragItem;
    public TMP_Text countUI;

    public Vector3 Position
    {
        set
        {
            background.transform.position = value;
        }
    }

    public void Init()
    {
        Transform t = transform.Find("Background");
        if(t != null)
        {
            background = t.GetComponent<Image>();
            background.raycastTarget = false;

            Transform iconT = t.Find("ItemIcon");
            if(iconT != null)
            {
                icon = iconT.GetComponent<Image>();
                icon.gameObject.SetActive(true);
            }

            Transform countT = t.Find("Count");
            if (countT != null)
            {
                countUI = countT.GetComponent<TMP_Text>();
                countUI.gameObject.SetActive(true);
            }
        }
    }
    
    public void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
        //icon.SetNativeSize();
    }

    public void SetActive(bool state)
    {
        gameObject.SetActive(state);
    }

    public void SetCount(int count)
    {
        countUI.text = count.ToString();
    }

    public void Show(bool state, UIItem item)
    {
        dragItem = item;
        SetIcon(item.IconSprite);
        if(item.itemCategory == ItemCategory.Quest)
        {
            SetCount(item.count);
            countUI.gameObject.SetActive(true);
        }
        else
            countUI.gameObject.SetActive(false);

        SetActive(state);
    }
}
