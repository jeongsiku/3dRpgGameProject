using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UITitle : MonoBehaviour
{
    void Start()
    {
        AudioMng.Instance.Init();
        AudioMng.Instance.PlayBackground("music_oriental_sunrise", 0.5f);

        Transform playBtn = transform.Find("MenuButtonGroup/Play");
        if(playBtn != null)
        {
            Button uiBtn = playBtn.GetComponent<Button>();
            if(uiBtn != null)
            {
                uiBtn.onClick.AddListener(OnClickPlay);
            }
        }

        Transform continueBtn = transform.Find("MenuButtonGroup/Continue");
        if (continueBtn != null)
        {
            Button uiBtn = continueBtn.GetComponent<Button>();
            if (uiBtn != null)
            {
                uiBtn.onClick.AddListener(OnClickContinue);
            }
        }

        Transform quitBtn = transform.Find("MenuButtonGroup/Quit");
        if (quitBtn != null)
        {
            Button uiBtn = quitBtn.GetComponent<Button>();
            if (uiBtn != null)
            {
                uiBtn.onClick.AddListener(OnClickQuit);
            }
        }

        

        UIFade uiFade = GameObject.FindObjectOfType<UIFade>();
        if(uiFade == null)
        {
            uiFade = Instantiate(Resources.Load<UIFade>("Prefabs/UI/UIFade"));
            uiFade.Init();
        }
        else
        {
            uiFade.FadeIn();
        }

        Player player = FindObjectOfType<Player>();
        if(player!=null) Destroy(player.gameObject);
    }

    void OnClickPlay()
    {
        AudioMng.Instance.PlayUIEffect("ui_menu_button_confirm_03");
        UIFade uiFade = GameObject.FindObjectOfType<UIFade>();
        if (uiFade != null)
            uiFade.FadeOut();

        Invoke("MoveToGame", 1f);
    }

    void OnClickContinue()
    {
        GameData.isContinue = true;
        OnClickPlay();
        
    }

    void MoveToGame()
    {
        AudioMng.Instance.StopBackground("music_oriental_sunrise");
        SceneManager.LoadSceneAsync("GameScene");
    }

    void OnClickQuit()
    {
        AudioMng.Instance.PlayUIEffect("ui_menu_button_cancel_02");
        Invoke("Quit", 1f);
    }

    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.visible = true;
    }
}
