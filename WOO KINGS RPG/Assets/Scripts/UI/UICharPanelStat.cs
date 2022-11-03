using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharPanelStat : MonoBehaviour
{
    int currentATKPoint = 0;
    int maxATKPoint = 0;
    Image atkBar;

    int currentDEFPoint = 0;
    int maxDEFPoint = 0;
    Image defBar;

    int currentHPPoint = 0;
    int maxHPPoint = 0;
    Image hpBar;

    public void Init()
    {
        Transform t = transform.Find("AttackGroup/AttackBar");
        if (t != null)
            atkBar = t.GetComponent<Image>();

        t = transform.Find("DefenseGroup/DefenseBar");
        if (t != null)
            defBar = t.GetComponent<Image>();

        t = transform.Find("HealthGroup/HealthBar");
        if (t != null)
            hpBar = t.GetComponent<Image>();
    }

    public void SetCurrentAtk(int currAtk)
    {
        currentATKPoint = currAtk;
        atkBar.fillAmount = (float)currentATKPoint / (float)maxATKPoint;
    }

    public void SetCurrentAtk()
    {
        SetCurrentAtk(currentATKPoint);
    }

    public void SetMaxAtk(int maxAtk)
    {
        maxATKPoint = maxAtk;
        SetCurrentAtk();
    }

    public void SetCurrentDef(int currDef)
    {
        currentDEFPoint = currDef;
        defBar.fillAmount = (float)currentDEFPoint / (float)maxDEFPoint;
    }

    public void SetCurrentDef()
    {
        SetCurrentDef(currentDEFPoint);
    }

    public void SetMaxDef(int maxDef)
    {
        maxDEFPoint = maxDef;
        SetCurrentDef();
    }

    public void SetCurrentHp(int currHp)
    {
        currentHPPoint = currHp;
        hpBar.fillAmount = (float)currentHPPoint / (float)maxHPPoint;
    }

    public void SetCurrentHp()
    {
        SetCurrentHp(currentHPPoint);
    }

    public void SetMaxHp(int maxHp)
    {
        maxHPPoint = maxHp;
        SetCurrentHp();
    }
}
