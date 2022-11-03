using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndingManager : MonoBehaviour
{
    UIFade uiFade;
    TMP_Text textEnd1;
    TMP_Text textEnd2;

    void Start()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null)
            Destroy(player.gameObject);

        UIManager uiManager = FindObjectOfType<UIManager>();
        if(uiManager != null)
            uiManager.gameObject.SetActive(false);

        uiFade = FindObjectOfType<UIFade>();
        if( uiFade != null )
            uiFade.FadeIn();

        Transform t = transform.Find("Character");
        Animator[] anims = t.GetComponentsInChildren<Animator>();
        foreach(Animator anim in anims)
            anim.SetBool("Run", true);

        GameObject objects = GameObject.FindGameObjectWithTag("EndTextGroup");
        textEnd1 = objects.transform.Find("Text1").GetComponent<TMP_Text>();
        Color color1 = textEnd1.color;
        color1.a = 0;
        textEnd1.color = color1;
        textEnd2 = objects.transform.Find("Text2").GetComponent<TMP_Text>();
        Color color2 = textEnd2.color;
        color2.a = 0;
        textEnd2.color = color2;

        AudioMng.Instance.PlayBackground("music_comedy_quirky_fun_knockout",0.2f);
        

        StartCoroutine(IEEnding());
    }

    IEnumerator IEEnding()
    {
        yield return new WaitForSeconds(1);
        float time = 0;
        while(time < 1)
        {
            time += Time.deltaTime;
            Color color = textEnd1.color;
            color.a = Mathf.Lerp(0,1,time);
            textEnd1.color=color;
            yield return null;
        }
        yield return new WaitForSeconds(3);

        time = 0;
        while (time < 1)
        {
            time += Time.deltaTime;
            Color color = textEnd1.color;
            color.a = Mathf.Lerp(1, 0, time);
            textEnd1.color = color;
            yield return null;
        }

        yield return new WaitForSeconds(1);

        time = 0;
        while (time < 1)
        {
            time += Time.deltaTime;
            Color color = textEnd2.color;
            color.a = Mathf.Lerp(0, 1, time);
            textEnd2.color = color;
            yield return null;
        }
        yield return new WaitForSeconds(3);

        time = 0;
        while (time < 1)
        {
            time += Time.deltaTime;
            Color color = textEnd2.color;
            color.a = Mathf.Lerp(1, 0, time);
            textEnd2.color = color;
            yield return null;
        }
        yield return new WaitForSeconds(2);
        uiFade.FadeOut();
        Invoke("LoadTitle", 1.1f);
    }

    void LoadTitle()
    {
        AudioMng.Instance.StopBackground("music_comedy_quirky_fun_knockout");
        SceneManager.LoadSceneAsync("TitleScene");
    }

}
