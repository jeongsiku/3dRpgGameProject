using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWarrior : NPC
{
    public string titleText = "������ġ";
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
        npcType = NPCType.Warrior;
        contentsText = new Dictionary<int, string>();
        contentsText.Add(0, "�ȳ��Ͻʴϱ�? ���մ�! ���� �� ������ ��Ű�� ����Դϴ�. ���� �������� ���� ���� ���ϴ�, ���մԲ��� ���� �����ֽø� ���� �Ƴ��� ������ �帮�ڽ��ϴ�!");
        contentsText.Add(1, "���� ������ �غ� ���� �����̳���? ���� �� ��ġ���ֽñ� �ٶ��ϴ�!!");
        contentsText.Add(2, "���� ���� �� ������� �ʾҽ��ϴ�.");
        contentsText.Add(3, "���� �� ��ġ��̱���! �����մϴ� ���մ�! ������ �帮�ڽ��ϴ�!");
        contentsText.Add(4, "���� �� ���� ���� �� ��Ű�� �ְڽ��ϴ�! ���մ�!! ������ ���� �ڿ� ���̴� �꿡�� ã�ƺ��ñ� �ٶ��ϴ�.");
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
        Vector3 pos = transform.parent.transform.Find("WarriorPos").transform.position;
        transform.position = pos;
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
