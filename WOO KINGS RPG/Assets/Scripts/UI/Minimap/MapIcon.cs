using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapIcon : MonoBehaviour
{
    private Transform target;

    private Image icon;

    public Vector3 TargetPos { get { return target.position; } }

    public void Init()
    {
        icon = GetComponent<Image>();
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void UpdatePos(Vector3 position)
    {
        transform.localPosition = position;
    }


    void Update()
    {
        transform.localPosition = MinimapHelper.WorldPosToMapPos(target.position);
    }
}
