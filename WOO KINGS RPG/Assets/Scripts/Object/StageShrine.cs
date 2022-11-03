using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageShrine : ScanableObject
{
    [SerializeField]
    int shrineNum = 0;
    public bool isOpened = false;

    public int ShrineNum { get { return shrineNum; } }   

    public void SetOpen(int nowShrine)
    {
        if(nowShrine == shrineNum)
            isOpened = true;
    }

    public void ActiveShrine(int num)
    {
        if (shrineNum != num) return;

        Transform t = transform.GetChild(0);
        if(t != null)
            t.gameObject.SetActive(true);
        t = transform.GetChild(1);
        if (t != null)
            t.gameObject.SetActive(true);
        t = transform.GetChild(2);
        if (t != null)
            t.gameObject.SetActive(true);
    }

}
