using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomSpawner : MonoBehaviour
{
    List<Transform> mushroomSpawnPoint = new List<Transform>();
    GameObject mushroomPrefab;
    List<GameObject> mushrooms = new List<GameObject>();
    public int mushroomCount = 0;
    bool mushroomSpawning = false;

    void Start()
    {
        mushroomPrefab = Resources.Load<GameObject>("Prefabs/Character/Monster_Mushroom");

        for (int i = 0; i < transform.childCount; i++)
        {
            mushroomSpawnPoint.Add(transform.GetChild(i).transform);
        }
        MushroomSpawn();
    }

    void MushroomSpawn()
    {
        for(int i = 0; i < mushroomSpawnPoint.Count; i++)
        {
            GameObject instantEnemy = Instantiate(mushroomPrefab, mushroomSpawnPoint[i].position, mushroomSpawnPoint[i].rotation);
            Monster mushroom = instantEnemy.GetComponent<Monster>();
            mushroomCount++;
        }
        mushroomSpawning = false;
    }

    void Update()
    {
        if (mushroomCount == 0 && !mushroomSpawning)
        {
            mushroomSpawning = true;
            Invoke("MushroomSpawn", 5f);
        }

    }
}
