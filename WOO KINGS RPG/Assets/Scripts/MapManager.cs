using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    Vector3 startPoint1;
    Vector3 startPoint2;
    Vector3 startPoint3;
    Vector3 startPoint4;
    
    public Vector3 StartPoint1      { get { return startPoint1;     } }
    public Vector3 StartPoint2      { get { return startPoint2;     } }
    public Vector3 StartPoint3      { get { return startPoint3;     } }
    public Vector3 StartPoint4      { get { return startPoint4;     } }

    public void Init()
    {
        startPoint1 = transform.Find("StartPoint1").position;
        startPoint2 = transform.Find("StartPoint2").position;
        startPoint3 = transform.Find("StartPoint3").position;
        startPoint4 = transform.Find("StartPoint4").position;
    }

    public Vector3 SetStartPointBoss()
    {
        return transform.Find("StartPointBoss").position;
    }
    
}
