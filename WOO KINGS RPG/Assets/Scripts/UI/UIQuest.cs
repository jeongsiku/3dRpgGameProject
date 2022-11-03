using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIQuest : MonoBehaviour
{
    Animator anim;
    GameManager gameManager;

    NPCType npcType;
    int npcStep = 0;
    TMP_Text titleText;
    TMP_Text contentsText;
    string originText;
    float textInterval = 0.1f;
    TMP_Text acceptButtonText;

    AudioSource textBeep;
    bool isTexting = false;

    UIQuestDisplay uIQuestDisplay;

    GameObject nowNPC;
    bool isNurseSuccess = false;
    bool isWarriorSuccess = false;
    bool isWizardSuccess = false;
    public int MushroomKillcount = 0;
    public int ZombieKillcount = 0;
    public int mushroomHarvestCount = 0;

    UIInventory uiInventory;
    UISystemMsg uISystemMsg;

    public void Init()
    {
        Transform exitBtn = transform.Find("ExitButton");
        if (exitBtn != null)
        {
            Button uiBtn = exitBtn.GetComponent<Button>();
            if (uiBtn != null)
            {
                uiBtn.onClick.AddListener(OnClickExitButton);
            }
        }

        Transform AcceptBtn = transform.Find("AcceptButton");
        if (AcceptBtn != null)
        {
            Button uiBtn = AcceptBtn.GetComponent<Button>();
            if (uiBtn != null)
            {
                uiBtn.onClick.AddListener(OnClickAcceptButton);
            }
        }
        Transform t = AcceptBtn.Find("ButtonText");
        if(t != null)
            acceptButtonText = t.GetComponent<TMP_Text>();

        t = transform.Find("ContentsGroup/TitleText");
        if(t != null)
            titleText = t.GetComponent<TMP_Text>();

        t = transform.Find("ContentsGroup/ContentsText");
        if (t != null)
            contentsText = t.GetComponent<TMP_Text>();

        anim = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();

        textBeep = GetComponent<AudioSource>();
        if(textBeep != null)
        {
            textBeep.clip = Resources.Load<AudioClip>("Sounds/UI/textBeep");
            textBeep.volume = 0.1f;
        }

        uIQuestDisplay = FindObjectOfType<UIQuestDisplay>(true);
        if (uIQuestDisplay != null) uIQuestDisplay.Init();

        uiInventory = FindObjectOfType<UIInventory>();
        uISystemMsg = FindObjectOfType<UISystemMsg>();
    }

    void OnClickExitButton()
    {
        anim.SetBool("QuestShow", false);
        AudioMng.Instance.PlayUIEffect("car_window_open_close_whir_03");
        GameData.isUI = false;
        gameManager.ChangeSubToMainCamera();
        isTexting = false;
    }

    void OnClickAcceptButton()
    {
        anim.SetBool("QuestShow", false);
        AudioMng.Instance.PlayUIEffect("ui_menu_button_confirm_03");
        GameData.isUI = false;
        gameManager.ChangeSubToMainCamera();
        GameData.isInteractView = false;
        isTexting = false;
        switch (npcType)
        {
            case NPCType.Nurse:
                NPCNurse nurse = nowNPC.GetComponent<NPCNurse>();
                nurse.AcceptQuest();
                uIQuestDisplay.ActiveDisplay(true);
                uIQuestDisplay.SetText(titleText.text, MushroomKillcount, 5);
                isNurseSuccess = nurse.CheckClear();
                if(isNurseSuccess)
                {
                    uIQuestDisplay.ActiveDisplay(false);
                    npcStep = nurse.CompleteQuest();
                    SetButtonText();
                    uiInventory.ItemAdd(1);
                    uISystemMsg.Print("����Ʈ �������� ���� ���� ȹ���Ͽ����ϴ�!!");
                }
                break;
            case NPCType.Warrior:
                NPCWarrior warrior = nowNPC.GetComponent<NPCWarrior>();
                warrior.AcceptQuest();
                uIQuestDisplay.ActiveDisplay(true);
                uIQuestDisplay.SetText(titleText.text, ZombieKillcount, 3);
                isWarriorSuccess = warrior.CheckClear();
                if (isWarriorSuccess)
                {
                    uIQuestDisplay.ActiveDisplay(false);
                    npcStep = warrior.CompleteQuest();
                    SetButtonText();
                    uiInventory.ItemAdd(19);
                    uISystemMsg.Print("����Ʈ �������� ���� ���и� ȹ���Ͽ����ϴ�!!");
                }
                break;
            case NPCType.Wizard:
                NPCWizard wizard = nowNPC.GetComponent<NPCWizard>();
                wizard.AcceptQuest();
                uIQuestDisplay.ActiveDisplay(true);
                uIQuestDisplay.SetText(titleText.text, mushroomHarvestCount, 3);
                isWizardSuccess = wizard.CheckClear();
                if (isWizardSuccess)
                {
                    uIQuestDisplay.ActiveDisplay(false);
                    npcStep = wizard.CompleteQuest();
                    SetButtonText();
                    uiInventory.DeleteItem(22);
                    uiInventory.ItemAdd(21);
                    uISystemMsg.Print("����Ʈ �������� �հ��� ī��Ʈ�ǵ带 ȹ���Ͽ����ϴ�!!");
                }
                break;
            case NPCType.Police:
                NPCPolice police = nowNPC.GetComponent<NPCPolice>();
                police.AcceptQuest();
                break;
        }
    }

    public void ReQuestQuest(GameObject npc)
    {
        npcType = npc.GetComponent<NPC>().npcType;
        nowNPC = npc;

        switch (npcType)
        {
            case NPCType.Nurse:
                NPCNurse nurse = npc.GetComponent<NPCNurse>();
                if(nurse != null)
                {
                    titleText.text = nurse.TitleText();
                    originText = nurse.ContentsText();
                    npcStep = nurse.GetNPCStep();
                    SetButtonText();
                }
                break;
            case NPCType.Warrior:
                NPCWarrior warrior = npc.GetComponent<NPCWarrior>();
                if (warrior != null)
                {
                    titleText.text = warrior.TitleText();
                    originText = warrior.ContentsText();
                    npcStep = warrior.GetNPCStep();
                    SetButtonText();
                }
                break;
            case NPCType.Wizard:
                NPCWizard wizard = npc.GetComponent<NPCWizard>();
                if (wizard != null)
                {
                    titleText.text = wizard.TitleText();
                    originText = wizard.ContentsText();
                    npcStep = wizard.GetNPCStep();
                    SetButtonText();
                }
                break;
            case NPCType.Police:
                NPCPolice police = npc.GetComponent<NPCPolice>();
                if (police != null)
                {
                    titleText.text = police.TitleText();
                    originText = police.ContentsText();
                    npcStep = police.GetNPCStep();
                    SetButtonText();
                }
                break;
            case NPCType.Bear:
                return;
        }
        StartCoroutine(ContentsTextShow());
    }

    public void SetButtonText()
    {
        switch(npcStep)
        {
            case 0:
                acceptButtonText.text = "����";
                break;
            case 1:
                acceptButtonText.text = "����";
                break;
            case 2:
                acceptButtonText.text = "Ȯ��";
                break;
            case 3:
                acceptButtonText.text = "�Ϸ�";
                break;
            case 4:
                acceptButtonText.text = "Ȯ��";
                break;
            case 5:
                acceptButtonText.text = "����";
                break;

        }
    }

    IEnumerator ContentsTextShow()
    {
        isTexting = true;
        contentsText.text = string.Empty;
        int index = 0;
        yield return new WaitForSeconds(1f);
        while(contentsText.text.Length != originText.Length)
        {
            if(isTexting == false)
            {
                contentsText.text = originText;
                yield break;
            }
            contentsText.text += originText[index];
            if (originText[index] != ' ' || originText[index] != '.')
                textBeep.Play();
            index++;
            yield return new WaitForSeconds(textInterval);
        }
        isTexting = false;
    }

    public void KillMushroom()
    {
        if (MushroomKillcount >= 5) return;
        MushroomKillcount++;
        uIQuestDisplay.SetText(titleText.text, MushroomKillcount, 5);
        if(MushroomKillcount == 5)
        {
            uIQuestDisplay.isSuccess = true;
            uIQuestDisplay.CheckQuest();
            AudioMng.Instance.PlayUIEffect("ui_menu_button_confirm_02");
            NPCNurse nurse = FindObjectOfType<NPCNurse>();
            nurse.SuccessQuest();
            //npcStep = nurse.GetNPCStep();
            SetButtonText();
            uISystemMsg.Print("���� ä�� ����Ʈ�� �Ϸ��߽��ϴ�!");
            GameData.currentQuest = 2;
        }
    }

    public void KillZombie()
    {
        if (ZombieKillcount >= 3) return;
        ZombieKillcount++;
        uIQuestDisplay.SetText(titleText.text, ZombieKillcount, 3);
        if (ZombieKillcount == 3)
        {
            uIQuestDisplay.isSuccess = true;
            uIQuestDisplay.CheckQuest();
            AudioMng.Instance.PlayUIEffect("ui_menu_button_confirm_02");
            NPCWarrior warrior = FindObjectOfType<NPCWarrior>();
            warrior.SuccessQuest();
            SetButtonText();
            uISystemMsg.Print("���� ��ġ ����Ʈ�� �Ϸ��߽��ϴ�!");
            GameData.currentQuest = 3;
        }
    }

    public void HarvestMushroom()
    {
        if (mushroomHarvestCount >= 3) return;
        mushroomHarvestCount++;
        uIQuestDisplay.SetText(titleText.text, mushroomHarvestCount, 3);
        if (mushroomHarvestCount == 3)
        {
            uIQuestDisplay.isSuccess = true;
            uIQuestDisplay.CheckQuest();
            AudioMng.Instance.PlayUIEffect("ui_menu_button_confirm_02");
            NPCWizard wizard = FindObjectOfType<NPCWizard>();
            wizard.SuccessQuest();
            SetButtonText();
            uISystemMsg.Print("�������� ���� ����Ʈ�� �Ϸ��߽��ϴ�!");
            GameData.currentQuest = 4;
        }
    }

    void Update()
    {
        
    }
}
