using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMinimap : MonoBehaviour
{
    private UIBackground background;
    private Transform player;
    private Transform uiPlayer;
    private List<MapIcon> mapIcons = new List<MapIcon>();

    //private List<NPC> npcList = new List<NPC>();
    //private List<StageShrine> stageShrineList = new List<StageShrine>();

    public void Init()
    {
        GameObject playerObj = FindObjectOfType<Player>().gameObject;
        if(playerObj != null)
            player = playerObj.transform;

        background = GetComponentInChildren<UIBackground>();
        if(background != null)
        {
            background.Init();
            background.SetTarget(player);
        }

        GameObject min = GameObject.Find("FlagMin");
        GameObject max = GameObject.Find("FlagMax");
        GameObject uiCenter = GameObject.Find("MinimapCenter");
        GameObject worldCenter = GameObject.Find("WorldCenter");

        float worldWidth = 0;
        float worldDepth = 0;
        float disttanceZ = 0;
        float disttanceX = 0;

        if(min != null && max != null && background != null && uiCenter != null && worldCenter != null)
        {
            worldWidth = Mathf.Abs(min.transform.position.x - max.transform.position.x);
            worldDepth = Mathf.Abs(min.transform.position.z - max.transform.position.z);
            disttanceZ = Mathf.Abs(uiCenter.transform.position.z - worldCenter.transform.position.z);
            disttanceX = Mathf.Abs(uiCenter.transform.position.x - worldCenter.transform.position.x);
            Vector3 sizeDelta = background.SizeDelta;
            MinimapHelper.Setting(worldWidth, worldDepth, sizeDelta.x, sizeDelta.y, disttanceX, disttanceZ);
        }

        uiPlayer = transform.Find("Mask/Player");

        SearchObjectForIcon();
    }

    void SearchObjectForIcon()
    {
        NPC[] npcList = FindObjectsOfType<NPC>();
        foreach (NPC npc in npcList)
            AddIcon(npc.transform);

        StageShrine[] shrineList = FindObjectsOfType<StageShrine>();
        foreach (StageShrine shrine in shrineList)
            AddIcon(shrine.transform);
    }

    public void AddIcon(Transform target)
    {
        MapIcon prefab = null;
        ScanableObject obj = target.GetComponent<ScanableObject>();
        if(obj.scanObjectType == ScanObjectType.NPC)
            prefab = Resources.Load<MapIcon>("Prefabs/UI/NpcIcon");
        else if (obj.scanObjectType == ScanObjectType.Shrine)
            prefab = Resources.Load<MapIcon>("Prefabs/UI/ShrineIcon");
        else if (obj.scanObjectType == ScanObjectType.Shop)
            prefab = Resources.Load<MapIcon>("Prefabs/UI/ShopIcon");

        MapIcon mapIcon = Instantiate(prefab, background.transform);
        mapIcon.Init();
        mapIcon.SetTarget(target);
        mapIcons.Add(mapIcon);
    }

    private void Update()
    {
        MinimapHelper.LookAt(player, uiPlayer);
    }
}
