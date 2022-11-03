using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        collision.transform.parent = transform;
    }

    private void OnCollisionExit(Collision collision)
    {
        collision.transform.parent = null;
    }
}
