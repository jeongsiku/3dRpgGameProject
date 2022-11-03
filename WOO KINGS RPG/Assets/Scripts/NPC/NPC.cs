using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestStep
{
    Idle,       // 0
    Introduce,  // 1
    Accept,     // 2
    Complete    // 3
}

public enum NPCType
{
    None,
    Nurse,
    Warrior,
    Wizard,
    Police,
    Bear
}

public class NPC : ScanableObject
{
    public UIQuest questPanel;

    Transform camPos;
    public NPCType npcType = NPCType.None;

    public void Init()
    {
        questPanel = FindObjectOfType<UIQuest>();

        camPos = transform.Find("CameraPosition");
    }

    public void Interaction(bool state, GameObject scanObject)
    {
        CameraManager cameraManager = GameObject.FindObjectOfType<CameraManager>();
        if (cameraManager != null)
        {
            GameData.isInteractView = state;
            cameraManager.activeCamera(1, !state);
            cameraManager.activeCamera(2, state);
            cameraManager.npcCameraPos(transform.position,camPos.position);
        }

        CheckNPC(scanObject);
    }

    void CheckNPC(GameObject scanObject)
    {
        questPanel.ReQuestQuest(scanObject);
        
    }

    //public void TalkEnd()
    //{
    //    CameraManager cameraManager = GameObject.FindObjectOfType<CameraManager>();
    //    if (cameraManager != null)
    //    {
    //        cameraManager.activeCamera(2, false);
    //        cameraManager.activeCamera(1, true);
    //    }
    //}
}
