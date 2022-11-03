using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static bool isDead = false;
    public static bool isUI = false;
    public static bool isMenu = false;
    public static bool isFreeze = false;
    public static bool isBattle = false;
    public static bool isHarvesting = false;
    public static int nowHarvestItemID = -1;
    public static bool isInteractView = false;

    public static int currScene = 1;

    public static int gold = 0;
    public static int selectedItemID = -1;
    public static int selectedItemCost = -1;
    public static string seletedItemName = string.Empty;

    public static int currentQuest = 1;

    public static bool isContinue = false;
}
