using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCNurse : NPC
{
    public string titleText = "����ä��";
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
        npcType = NPCType.Nurse;
        contentsText = new Dictionary<int, string>();
        contentsText.Add(0, "�ȳ��ϼ���? ���մ�. ���� 5������ �����߸� �� ���ƿ��ø� ������ �帮�ھ��!");
        contentsText.Add(1, "������ �ñ��ϼ̳���? �ٷ� ������������ �̵��� �� �ִ� ���迡��.");
        contentsText.Add(2, "������ ��� ������ �� �ٽ� ���ּ���.");
        contentsText.Add(3, "���� ���մ� �� �س��� �� �˾Ҿ��. ���� ������ �޾��ּ���.");
        contentsText.Add(4, "���� ���� �; �ٽ� ���ƿ��̳���?");
    }

    public void AcceptQuest()
    {
        if(step <=1)
        {
            step = 2;
        }
    }

    public string TitleText()
    {
        return titleText;
    }

    public string ContentsText()
    {
        string text = contentsText[step];
        if(step == 0)
        {
            step++;
        }
        return text;

    }

    public bool CheckClear()
    {
        if(step == 3)
            return true;
        return false;
    }

    public void SuccessQuest()
    {
        step = 3;
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
