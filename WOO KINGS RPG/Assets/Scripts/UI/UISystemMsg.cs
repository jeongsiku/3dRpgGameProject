using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISystemMsg : MonoBehaviour
{
    TMP_Text systemMsg;
    Image backgournd;

    public void Init()
    {
        systemMsg = GetComponentInChildren<TMP_Text>(true);
        if(systemMsg != null) systemMsg.text = string.Empty;
        backgournd = GetComponentInChildren<Image>(true);

        systemMsg.gameObject.SetActive(false);
        backgournd.gameObject.SetActive(false);
    }

    public void Print(string text)
    {
        systemMsg.gameObject.SetActive(true);
        backgournd.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(IEPrint(text));
    }

    IEnumerator IEPrint(string text)
    {
        systemMsg.text = text;
        Color textColor = systemMsg.color;
        Color BgColor = backgournd.color;
        textColor.a = 1;
        BgColor.a = 1;
        systemMsg.color = textColor;
        backgournd.color = BgColor;
        yield return new WaitForSeconds(3f);
        float time = 0;
        while(time < 1f)
        {
            time += Time.deltaTime;
            textColor = systemMsg.color;
            BgColor = backgournd.color;
            textColor.a = Mathf.Lerp(1, 0, time);
            BgColor.a = Mathf.Lerp(1, 0, time);
            systemMsg.color = textColor;
            backgournd.color = BgColor;
            yield return null;
        }
        systemMsg.gameObject.SetActive(false);
        backgournd.gameObject.SetActive(false);
    }
    
}
