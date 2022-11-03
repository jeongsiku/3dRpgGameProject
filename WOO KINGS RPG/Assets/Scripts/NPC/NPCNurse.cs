using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCNurse : NPC
{
    public string titleText = "버섯채집";
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
        contentsText.Add(0, "안녕하세요? 국왕님. 버섯 5마리를 쓰러뜨린 후 돌아오시면 선물을 드리겠어요!");
        contentsText.Add(1, "선물이 궁금하셨나요? 바로 다음지역으로 이동할 수 있는 열쇠에요.");
        contentsText.Add(2, "버섯을 모두 잡으신 후 다시 와주세요.");
        contentsText.Add(3, "역시 국왕님 잘 해내실 줄 알았어요. 여기 보석을 받아주세요.");
        contentsText.Add(4, "제가 보고 싶어서 다시 돌아오셨나요?");
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
