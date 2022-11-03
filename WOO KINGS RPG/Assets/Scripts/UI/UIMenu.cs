using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    Button save;
    Button exit;

    public void Init()
    {
        save = transform.Find("ButtonGroup/Save").GetComponent<Button>();
        save?.onClick.AddListener(Save);
        exit = transform.Find("ButtonGroup/Exit").GetComponent<Button>();
        exit?.onClick.AddListener(Exit);
    }

    void Save()
    {
        GameDB.Save("GameSave.json");
        UIManager uiManager = FindObjectOfType<UIManager>();
        uiManager?.PrintMessage("게임이 저장되었습니다.");
        GameData.isUI = false;
        GameData.isMenu = false;
        gameObject.SetActive(false);
    }

    void Exit()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager?.ShowUI(false);
        gameManager.ShowMinimap(false);

        UIFade uiFade = GameObject.FindObjectOfType<UIFade>();
        if (uiFade != null)
            uiFade.FadeOut();

        Invoke("LoadTitleScene", 2f);
    }

    void SetDeactive()
    {
        gameObject.SetActive(false);
    }

    void LoadTitleScene()
    {
        AudioMng.Instance.StopBackground("music_epic_fallen_empire");
        SceneManager.LoadSceneAsync("TitleScene");
        gameObject.SetActive(false);
    }
    
}
