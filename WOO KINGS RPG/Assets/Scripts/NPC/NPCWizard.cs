using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWizard : NPC
{
    public string titleText = "�������� ä��";
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
        npcType = NPCType.Wizard;
        contentsText = new Dictionary<int, string>();
        contentsText.Add(0, "���� �����������Դϴ�. �� �ٴ� �����Ǹ����� �����ϱ� ���� �� �ֺ��� �ִ� ���� ������ ������ ����ֽñ� �ٶ��ϴ�.");
        contentsText.Add(1, "���� ������ �����ֽø� �����Ǹ����� ����� ����� �����غ��ڽ��ϴ�.");
        contentsText.Add(2, "Ȥ�� ������ ã�� ���ϼ̳���? ������ ä���� �����̶� ��ٸ��� �ٽ� ������ ���� �� �ֽ��ϴ�.");
        contentsText.Add(3, "���������� ��� ��ƿ��̱���! ���� �Ƚ��Դϴ�. ���մ�.");
        contentsText.Add(4, "���� �� ���� ���Դϴ�. � ������ �������ֽʽÿ�. ���մ�!");
    }

    public void AcceptQuest()
    {
        if (step <= 1)
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
