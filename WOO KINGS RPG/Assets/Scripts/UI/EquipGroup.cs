using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipGroup : MonoBehaviour
{
    // Start is called before the first frame update
    public void Init()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            UIDragItem item = transform.GetChild(i).GetComponent<UIDragItem>();
            item?.Init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
