using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    PlayerController playerController;
    Player player;

    public GameObject scanObject = null; // 스캔 오브젝트

    //UIManager uiManager;
    MapManager mapManager;
    
    GameObject uiCanvas;
    public GameObject questPanel;
    public GameObject shrinePanel;
    public GameObject inventoryPanel;
    public GameObject charPanel;
    public GameObject itemBoxPanel;
    public UIPlayerAction uiPlayerAction;
    public UISystemMsg uiSystemMsg;
    UICharInfo uiCharInfo;
    Transform menuUI;
    UIMinimap uiMinimap;
    UIShop uiShop;
    UIMenu uiMenu;


    Animator questAnimator;
    Animator inventoryAnimator;
    Animator charPanelAnimator;
    Animator itemBoxAnimator;
    UIFade uiFade;

    bool isOpenQuest = false;
    bool isOpenInventory = false;
    bool isOpenCharPanel = false;

    //1층 버섯존
    public Transform[] mushroomSpawnPoint;

    // 채집하기
    

    void Start()
    {
        DataManager.Load(TableType.ItemTable);

        playerController = GameObject.FindObjectOfType<PlayerController>();
        if(playerController != null)
            playerController.Init();

        player = GameObject.FindObjectOfType<Player>();
        if (player != null)
            player.Init();

        mapManager = FindObjectOfType<MapManager>();
        if(mapManager != null) mapManager.Init();

        AudioMng.Instance.Init();

        uiFade = GameObject.FindObjectOfType<UIFade>();
        if (uiFade != null)
            uiFade.FadeIn();

        questAnimator = questPanel.GetComponent<Animator>();
        inventoryAnimator = inventoryPanel.GetComponent<Animator>();
        charPanelAnimator = charPanel.GetComponent<Animator>();
        itemBoxAnimator = itemBoxPanel.GetComponent<Animator>();
        uiPlayerAction = FindObjectOfType<UIPlayerAction>(true);

        Cursor.visible = false;
        //uiManager = GameObject.FindObjectOfType<UIManager>();
        //if(uiManager != null) uiManager.Init();

        uiCharInfo = FindObjectOfType<UICharInfo>();
        uiMinimap = FindObjectOfType<UIMinimap>();
        uiShop = FindObjectOfType<UIShop>(true);
        uiMenu = FindObjectOfType<UIMenu>(true);
        menuUI = uiCharInfo.transform.parent.transform.Find("MenuGroup");

        DontDestroyOnLoad(gameObject);

        if(GameData.isContinue) LoadSaveData();
            
    }

    void LoadSaveData()
    {
        GameDB.Load("GameSave.json");
    }

    public void ShowUI(bool state)
    {
        uiCharInfo.gameObject.SetActive(state);
        menuUI.gameObject.SetActive(state);
        
    }

    public void ShowMinimap(bool state)
    {
        uiMinimap.gameObject.SetActive(state);
    }

    public void Interaction(GameObject scanObj)
    {
        scanObject = scanObj;
        ScanObjectType type = scanObject.GetComponent<ScanableObject>().scanObjectType;
        

        switch(type)
        {
            case ScanObjectType.NPC:
                {
                    questAnimator.SetBool("QuestShow", true);
                    NPC npc = scanObject.GetComponent<NPC>();
                    if (npc != null)
                        npc.Interaction(true, scanObject);
                    AfterInteractionWithUI();
                }
                break;
            
            case ScanObjectType.Shrine:
                {
                    int shrineNum = 0;
                    bool isOpen = false;
                    StageShrine shrine = scanObj.GetComponent<StageShrine>();
                    if (shrine != null)
                    {
                        shrineNum = shrine.ShrineNum;
                        isOpen = shrine.isOpened;
                    }
                    UIShrineItemField uiShrineItemField = FindObjectOfType<UIShrineItemField>(true);
                    if (uiShrineItemField != null)
                        uiShrineItemField.SetField(shrineNum,isOpen);
                    shrinePanel.SetActive(true);
                    if (!isOpenInventory)
                        OpenInventory();
                    AfterInteractionWithUI();
                }
                break;
            case ScanObjectType.ItemBox:
                {
                    ItemBox itemBox = scanObject.GetComponent<ItemBox>();
                    if (itemBox != null)
                    {
                        itemBox.Interaction();
                        itemBoxAnimator.SetBool("ItemBoxShow", true);
                        if (!isOpenInventory)
                            OpenInventory();
                    }
                    AfterInteractionWithUI();
                }
                break;
            case ScanObjectType.Collect:
                {
                    QMushroom obj = scanObj.GetComponent<QMushroom>();
                    if (obj != null)
                    {
                        if (obj.IsCanHarvest == false)
                        {
                            uiSystemMsg.Print("다 자랄 때까지 기다려야 합니다.");
                            return;
                        }
                        GameData.nowHarvestItemID = obj.HarvestItemID;
                    }
                    if(uiPlayerAction != null)
                        uiPlayerAction.BarProgress(scanObj);
                    GameData.isHarvesting = true;
                }
                break;
            case ScanObjectType.Shop:
                {
                    uiShop.SetActive(true);
                    if (!isOpenInventory)
                        OpenInventory();
                    AfterInteractionWithUI();
                }
                break;

        }
        
    }

    public void CloseShopUI()
    {
        uiShop.SetActive(false);
    }

    void AfterInteractionWithUI()
    {
        isOpenQuest = true;
        CheckState();
        AudioMng.Instance.PlayUIEffect("car_window_open_close_whir_03");
    }

    void AfterInteractionWithOutUI()
    {

    }

    public void StopHarvesting()
    {
        uiPlayerAction.StopBarProgress();
    }

    public void CloseQuestPanel()
    {
        shrinePanel.SetActive(false);
        questAnimator.SetBool("QuestShow", false);
        inventoryAnimator.SetBool("InventoryShow", false);
        itemBoxAnimator.SetBool("ItemBoxShow", false);
        isOpenCharPanel = false;
        charPanelAnimator.SetBool("CharPanelShow", false);
        AudioMng.Instance.PlayUIEffect("car_window_open_close_whir_03");
        isOpenInventory = false;
        isOpenQuest = false;
        DataManager.isBoxOpen = false;
        CheckState();

        ChangeSubToMainCamera();
    }

    public void ChangeSubToMainCamera()
    {
        CameraManager cameraManager = FindObjectOfType<CameraManager>();
        if (cameraManager != null)
        {
            cameraManager.activeCamera(1, true);
            cameraManager.activeCamera(2, false);
        }
    }

    public void OpenInventory()
    {
        if(!isOpenInventory)
        {
            isOpenInventory = true;
            inventoryAnimator.SetBool("InventoryShow", true);
        }
        else
        {
            isOpenInventory = false;
            inventoryAnimator.SetBool("InventoryShow", false);
        }
        CheckState();
        AudioMng.Instance.PlayUIEffect("car_window_open_close_whir_03");
    }

    public void OpenCharPanel()
    {
        if (!isOpenCharPanel)
        {
            isOpenCharPanel = true;
            charPanelAnimator.SetBool("CharPanelShow", true);
        }
        else
        {
            isOpenCharPanel = false;
            charPanelAnimator.SetBool("CharPanelShow", false);
        }
        CheckState();
        AudioMng.Instance.PlayUIEffect("car_window_open_close_whir_03");
    }

    public void OpenMenu(bool state)
    {
        uiMenu.gameObject.SetActive(state);
        GameData.isUI = state;
        GameData.isMenu = state;
    }

    public void DeleteScanObject()
    {
        scanObject = null;
    }

    public void FadeOut()
    {
        uiFade.FadeOut();
    }

    public void FadeIn()
    {
        uiFade.FadeIn();
    }

    void CheckState()
    {
        if (!isOpenInventory && !isOpenQuest && !isOpenCharPanel)
        {
            GameData.isUI = false;
        }
        else
        {
            GameData.isUI = true;
        }
    }

    public void Clear()
    {
        GameData.isFreeze = true;
        ShowUI(false);

        uiFade.FadeOut(5);
        Invoke("LoadEndingScene", 5);

    }

    void LoadEndingScene()
    {
        AudioMng.Instance.StopBackground("music_epic_fallen_empire");
        SceneManager.LoadSceneAsync("EndingScene");
    }

    void Update()
    {
        if (GameData.isUI || GameData.isMenu)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
            
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
            
    }
}
