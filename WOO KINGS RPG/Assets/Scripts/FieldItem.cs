using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None,
    HealHeart,
    MaxHeart,
    Coin
}

public class FieldItem : MonoBehaviour
{
    //ItemType itemType;

    int healPoint = 6;
    int maxHeartUp = 10;
    int gold = 1234;

    public int HealPoint { get { return healPoint; } }
    public int MaxHeartUp { get { return maxHeartUp; } }
    public int Coin { get { return gold; } }

    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
