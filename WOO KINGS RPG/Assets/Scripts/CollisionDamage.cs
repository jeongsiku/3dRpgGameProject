using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Monster,
    Trap
}

public class CollisionDamage : MonoBehaviour 
{
    public EnemyType enemyType;

    [SerializeField]
    int collisionDamage = 0;

    public int GetCollisionDamage
    {
        get { return collisionDamage; }
    }

    public int SetCollisionDamage
    {
        set { collisionDamage = value; }
    }

    private void Start()
    {
        SetDamage();
    }

    void SetDamage()
    {
        switch(enemyType)
        {
            case EnemyType.Monster:
                collisionDamage = 3;
                break;

            case EnemyType.Trap:
                collisionDamage = 6;
                break;
        }
    }
}
