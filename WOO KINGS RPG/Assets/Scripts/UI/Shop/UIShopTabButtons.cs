using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShopTabButtons : MonoBehaviour
{
    private List<UITabButton> tabBtns = new List<UITabButton>();
    
    public void Init()
    {
        tabBtns.AddRange(GetComponentsInChildren<UITabButton>(true));
        foreach(var button in tabBtns)
            button.Init();
    }

    public void SetListener(System.Action<UITabButton> action)
    {
        foreach(var button in tabBtns)
            button.SetListener(action);
    }

    public void SetTab(EquipType btnType)
    {
        foreach (var button in tabBtns)
        {
            if(button.EquipType == btnType)
                button.SetActiveHighlight(true);
            else
                button.SetActiveHighlight(false);
        }
    }
}
