using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScanObjectType
{
    None,
    NPC,
    Shrine,
    ItemBox,
    Collect,
    Shop

}
public class ScanableObject : MonoBehaviour
{
    public ScanObjectType scanObjectType = ScanObjectType.None;

    int harvestItemID = 22;
    public int HarvestItemID { get { return harvestItemID; } }


}
