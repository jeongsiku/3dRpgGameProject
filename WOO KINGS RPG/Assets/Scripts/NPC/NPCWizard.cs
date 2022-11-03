using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWizard : NPC
{
    public string titleText = "버섯수액 채집";
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
        contentsText.Add(0, "저는 궁정마법사입니다. 날 뛰는 버섯악마들을 연구하기 위해 이 주변에 있는 빨간 버섯의 수액을 모아주시기 바랍니다.");
        contentsText.Add(1, "버섯 수액을 구해주시면 버섯악마들을 잠재울 방법을 연구해보겠습니다.");
        contentsText.Add(2, "혹시 버섯을 찾지 못하셨나요? 수액을 채집한 버섯이라도 기다리면 다시 수액을 얻을 수 있습니다.");
        contentsText.Add(3, "버섯수액을 모두 모아오셨군요! 이제 안심입니다. 국왕님.");
        contentsText.Add(4, "이제 곧 끝이 보입니다. 어서 앞으로 전진해주십시오. 국왕님!");
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
