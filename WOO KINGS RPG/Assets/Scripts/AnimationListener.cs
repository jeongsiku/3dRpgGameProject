using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationListener : MonoBehaviour
{
    public void CallEvent(string functionName)
    {
        transform.SendMessageUpwards(functionName,SendMessageOptions.DontRequireReceiver);
    }
}
