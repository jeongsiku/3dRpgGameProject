using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eGizmoType
{
    Sphere,
    WireCube
}

public class DrawGizmos : MonoBehaviour
{
    public eGizmoType gizmoType;
    public float radius = 0.1f;
    public Color color = Color.red;
    public Vector3 size = Vector3.one;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        switch(gizmoType)
        {
            case eGizmoType.Sphere:
                Gizmos.DrawSphere(transform.position, radius);
                break;

            case eGizmoType.WireCube:
                Gizmos.DrawWireCube(transform.position, size);
                break;
        }
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
