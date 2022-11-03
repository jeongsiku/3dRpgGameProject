using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPolice : NPC
{
    public string titleText = "ª�� ��Ż";
    public Dictionary<int, string> contentsText;
    [SerializeField]
    int step = 0;

    public new void Init()
    {
        base.Init();
        SetContents();
    }

    void SetContents()
    {
        npcType = NPCType.Police;
        contentsText = new Dictionary<int, string>();
        contentsText.Add(0, "���̰�. ���� ã�ҳ׿� �Ҿƹ���. ���ڴ�� �������� �����ø� �ȵȴٰ��! ���� �׸���������.");
        contentsText.Add(1, "�� ���� �������� ���ư����ؿ�. ��Ŵ� �帮�ڽ��ϴ�.");
        contentsText.Add(2, "");
        contentsText.Add(3, "");
        contentsText.Add(4, "");
    }

    public void AcceptQuest()
    {
        if (step <= 1)
        {
            step = 2;
        }

        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
            gameManager.Clear();
    }

    public string TitleText()
    {
        return titleText;
    }

    public string ContentsText()
    {
        string text = contentsText[step];
        if (step == 0)
        {
            step++;
        }
        return text;

    }

    public bool CheckClear()
    {
        if (step == 3)
            return true;
        return false;
    }

    public void SuccessQuest()
    {
        step = 3;
        //Vector3 pos = transform.parent.transform.Find("WarriorPos").transform.position;
        //transform.position = pos;
    }

    public int CompleteQuest()
    {
        step = 4;
        return step;
    }

    public int GetNPCStep()
    {
        return step;
    }

    private void Start()
    {
        Init();
    }
}
