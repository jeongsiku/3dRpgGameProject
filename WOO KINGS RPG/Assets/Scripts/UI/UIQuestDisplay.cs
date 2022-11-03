using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIQuestDisplay : MonoBehaviour
{
    TMP_Text questText;
    Image iconResolving;
    Image iconSuccess;
    public bool isSuccess = false;
    
    public void Init()
    {
        Transform t = transform.Find("QuestList/QuestText");
        if (t != null)
            questText = t.GetComponent<TMP_Text>();

        t = transform.Find("QuestList/QuestText/IconResolving");
        if(t != null)
        {
            iconResolving = t.GetComponent<Image>();
        }

        t = transform.Find("QuestList/QuestText/IconSuccess");
        if( t != null)
        {
            iconSuccess = t.GetComponent<Image>();
        }

        gameObject.SetActive(false);
        Resetting();
    }

    public void Resetting()
    {
        iconResolving?.gameObject.SetActive(true);
        iconSuccess.gameObject.SetActive(false);
    }

    public void ActiveDisplay(bool state)
    {
        gameObject.SetActive(state);
    }

    public void SetText(string subject, int current, int target)
    {
        questText.text = subject + " " + current + "/" + target;
        if(current == 0)
            questText.color = Color.red;
        else if( current == target)
            questText.color = Color.green;
        else 
            questText.color = Color.yellow;
        CheckQuest();
    }

    public void CheckQuest()
    {
        if(isSuccess)
        {
            iconResolving.gameObject.SetActive(false);
            iconSuccess.gameObject.SetActive(true);
        }
    }
}
