using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager?.ShowUI(false);
            gameManager.ShowMinimap(false);

            UIFade uiFade = GameObject.FindObjectOfType<UIFade>();
            if (uiFade != null)
                uiFade.FadeOut();

            GameData.currScene++;
            Invoke("MoveToBoss", 1f);
        }
    }

    void MoveToBoss()
    {
        AudioMng.Instance.StopBackground("music_harp_peaceful_loop");
        SceneManager.LoadSceneAsync("BossScene");
    }

}
