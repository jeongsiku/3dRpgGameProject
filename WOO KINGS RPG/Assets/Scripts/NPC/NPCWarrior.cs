using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWarrior : NPC
{
    public string titleText = "좀비퇴치";
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
        contentsText.Add(0, "안녕하십니까? 국왕님! 저는 이 무덤을 지키는 경비병입니다. 저는 무서워서 좀비를 잡지 못하니, 국왕님께서 좀비를 없애주시면 제가 아끼는 보물을 드리겠습니다!");
        contentsText.Add(1, "아직 마음이 준비가 되지 않으셨나요? 좀비를 꼭 퇴치해주시길 바랍니다!!");
        contentsText.Add(2, "아직 좀비가 다 사라지지 않았습니다.");
        contentsText.Add(3, "좀비를 다 해치우셨군요! 감사합니다 국왕님! 선물을 드리겠습니다!");
        contentsText.Add(4, "이제 이 곳은 제가 잘 지키고 있겠습니다! 국왕님!! 성소의 젬은 뒤에 보이는 산에서 찾아보시기 바랍니다.");
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
