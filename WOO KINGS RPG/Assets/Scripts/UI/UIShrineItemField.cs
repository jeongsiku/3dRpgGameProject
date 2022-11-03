using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIShrineItemField : UIDragItem, IDropHandler
{
    [SerializeField]
    int nowShrineNum = 0;
    bool nowShrineOpen = false;

    private void Start()
    {
        base.Init();
    }

    public void SetField(int shrineNum, bool isOpen)
    {
        nowShrineNum = shrineNum;
        nowShrineOpen = isOpen;
        if(nowShrineOpen) // 捞固 楷 己家
        {
            int iconNum = 0;
            switch (nowShrineNum)
            {
                case 1:
                    iconNum = 1;
                    break;
                case 2:
                    iconNum = 2;
                    break;
                case 3:
                    iconNum = 3;
                    break;
            }
            string icon = DataManager.ToS(TableType.ItemTable, iconNum, "ICON");
            Sprite itemSprite = Resources.Load<Sprite>("Images/" + icon);
            SetIcon(itemSprite);
            SetActive(true);
            SwitchUI(false);
        }
        else if(!nowShrineOpen)// 酒流 凯瘤 臼篮 己家
        {
            ClearField();
            SwitchUI(true);
            SetActive(false);
        }
    }

    void ClearField()
    {

    }

    public override void SetSelect(bool state)
    {
        return;
    }

    public new void OnDrop(PointerEventData eventData)
    {
        UIItem draggedItem = eventData.pointerDrag.GetComponent<UIItem>();

        if ((nowShrineNum == 1 && draggedItem.ItemRef == "Shrine1Gem") ||
            (nowShrineNum == 2 && draggedItem.ItemRef == "Shrine2Gem") ||
            (nowShrineNum == 3 && draggedItem.ItemRef == "Shrine3Gem"))
        {
            if (!draggedItem.HasItem) return;

            GameData.isFreeze = true;
            SetIcon(draggedItem.IconSprite);
            SetNativeSize();
            SetActive(true);
            HasItem = true;
            ItemRef = draggedItem.ItemRef;
            draggedItem.SetActive(false);
            draggedItem.HasItem = false;
            draggedItem.ItemRef = null;
            AudioMng.Instance.PlayUIEffect("ui_menu_button_confirm_03");
            StageShrine[] shrines = FindObjectsOfType<StageShrine>();
            if (shrines != null)
            {
                foreach (StageShrine shrine in shrines)
                {
                    shrine.SetOpen(nowShrineNum);
                }
            }
            ActiveShrine(nowShrineNum);
            Invoke("OpenGate", 1.5f);

            
        }
        else
        {
            AudioMng.Instance.PlayUIEffect("ui_menu_button_cancel_02");
            return;
        }
        

    }

    void ActiveShrine(int shrineNum) // 己家 备浇 难扁
    {
        StageShrine[] shrine = GameObject.FindObjectsOfType<StageShrine>();
        for (int i = 0; i < shrine.Length; i++)
            shrine[i].ActiveShrine(shrineNum);
    }

    void OpenGate()
    {
        CameraManager cameraManager = FindObjectOfType<CameraManager>();
        if (cameraManager != null)
            cameraManager.ShakeCamera(cameraNum: 1);
        AudioMng.Instance.PlayUIEffect("rock_door_slide_block_move_drag_01");

        StageGate[] gate = GameObject.FindObjectsOfType<StageGate>();
        for(int i = 0; i < gate.Length; i++)
        {
            gate[i].OpenGate(nowShrineNum);
        }

        
    }

    
}
