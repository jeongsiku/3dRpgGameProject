using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBackground : MonoBehaviour
{
    private Transform player;
    private RectTransform minimapBg;

    public Vector2 SizeDelta
    {
        get { return minimapBg.sizeDelta; }
    }

    public void Init()
    {
        minimapBg = GetComponent<RectTransform>();
    }

    public void SetTarget(Transform target)
    {
        player = target;
    }

    private void Update()
    {
        MinimapHelper.MarkOnTheMap(player, minimapBg);
    }
}
