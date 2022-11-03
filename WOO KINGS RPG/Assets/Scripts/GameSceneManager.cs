using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    void Start()
    {
        Invoke("SignalStart", 0.2f);
    }

    void SignalStart()
    {
        AudioMng.Instance.PlayBackground("music_harp_peaceful_loop", 0.2f);
    }
}
