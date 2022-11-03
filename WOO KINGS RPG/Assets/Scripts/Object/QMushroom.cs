using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QMushroom : ScanableObject
{
    GameObject msgInteract;
    GameObject msgWait;
    Vector3 HUDPos;
    bool isShow = false;
    bool isCanHarvest = true;

    UIQuest uiQuest;

    SphereCollider sphereCollider;

    public bool IsCanHarvest { get { return isCanHarvest; } }
    

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        msgInteract = transform.Find("Canvas/MsgInteract").gameObject;
        msgWait = transform.Find("Canvas/MsgWait").gameObject;
        msgInteract.gameObject.SetActive(false);
        HUDPos = transform.Find("HeadMsgPos").position;

        uiQuest = FindObjectOfType<UIQuest>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        if (GameData.isInteractView) return;

        if (isShow && isCanHarvest)
        {
            msgInteract.transform.position = Camera.main.WorldToScreenPoint(HUDPos);
        }
        else if(isShow && !isCanHarvest)
        {
            msgWait.transform.position = Camera.main.WorldToScreenPoint(HUDPos);
        }
    }

    public void SuccessHarvest()
    {
        isCanHarvest = false;
        StartCoroutine(Shrink());
        StartCoroutine(IERegen());
        uiQuest.HarvestMushroom();
        msgInteract.gameObject.SetActive(false);
        msgWait.gameObject.SetActive(true);

    }

    IEnumerator Shrink() // 캤을 때 줄어들기
    {
        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime;
            float scaleX = gameObject.transform.localScale.x;
            float scaleY = gameObject.transform.localScale.y;
            float scaleZ = gameObject.transform.localScale.z;
            scaleX = Mathf.Lerp(5,2,time);
            scaleY = Mathf.Lerp(5,2,time);
            scaleZ = Mathf.Lerp(5,2,time);
            gameObject.transform.localScale = new Vector3(scaleX,scaleY,scaleZ);

            yield return null;
        }
    }

    public IEnumerator IERegen() // 다시 성장하기
    {
        yield return new WaitForSeconds(5);
        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime * 0.2f;
            float scaleX = gameObject.transform.localScale.x;
            float scaleY = gameObject.transform.localScale.y;
            float scaleZ = gameObject.transform.localScale.z;
            scaleX = Mathf.Lerp(2, 5, time);
            scaleY = Mathf.Lerp(2, 5, time);
            scaleZ = Mathf.Lerp(2, 5, time);
            gameObject.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);

            yield return null;
        }
        isCanHarvest = true;

        if(isShow)
        {
            msgInteract.gameObject.SetActive(true);
            msgWait.gameObject.SetActive(false);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (isCanHarvest)
            {
                msgInteract.gameObject.SetActive(true);
                msgWait.gameObject.SetActive(false);
            }
            else
            {
                msgInteract.gameObject.SetActive(false);
                msgWait.gameObject.SetActive(true);
            }
            isShow = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            msgInteract.gameObject.SetActive(false);
            msgWait.gameObject.SetActive(false);
            isShow = false;
        }
    }

    
}
