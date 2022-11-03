using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICharInfo : MonoBehaviour
{
    TMP_Text hpText;
    
    Image hpBar;
    int currentHp;
    int maxHp;
    
    public void Init()
    {
        Transform t = transform.Find("HPText");
        if(t != null)
            hpText = t.gameObject.GetComponent<TMP_Text>();

        t = transform.Find("HPBarFront");
        if (t != null)
            hpBar = t.gameObject.GetComponent<Image>();

        
    }

    public void SetPlayerHp(int hp)
    {
        hpText.text = hp.ToString();
        hpBar.fillAmount = (float)hp / (float)maxHp;
    }

    public void SetPlayerMaxHp(int mHp)
    {
        maxHp = mHp;
        hpBar.fillAmount = (float)currentHp / (float)maxHp;
    }

   
}
