using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSceneManager : MonoBehaviour
{
    

    void Start()
    {
        AudioMng.Instance.PlayBackground("music_epic_fallen_empire", 0.2f);
        
        Invoke("SignalStart", 0.2f);
    }

    void SignalStart()
    {
        MapManager mapManager = FindObjectOfType<MapManager>(); ;
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.transform.position = mapManager.SetStartPointBoss();
        }

        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager?.ShowUI(true);

        UIFade uiFade = FindObjectOfType<UIFade>();
        uiFade.FadeIn();
    }
}
