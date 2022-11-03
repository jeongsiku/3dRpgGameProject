using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIPlayerAction : MonoBehaviour
{
    GameObject gaugeGroup;
    Image bar;
    TMP_Text text;
    Image itemIcon;
    Vector3 startPos1;
    Vector3 headPos1;
    Vector3 headPos2;
    Vector3 headPos3;
    Vector3 invenPos1;
    Vector3 invenPos2;



    public void Init()
    {
        // 게이지바
        gaugeGroup = transform.Find("GaugeGroup").gameObject;
        bar = transform.Find("GaugeGroup/Bar").GetComponent<Image>();
        text = transform.Find("GaugeGroup/TextBg/Text").GetComponent<TMP_Text>();
        Set();

        // 아이템 인벤에 던지기
        Transform t = transform.Find("ThrowItem");
        itemIcon = t.GetComponent<Image>();
        if(itemIcon != null)
            itemIcon.gameObject.SetActive(false);
        startPos1 = transform.Find("StartPos1").position;
        headPos1 = transform.Find("HeadUpPos1").position;
        headPos2 = transform.Find("HeadUpPos2").position;
        invenPos1 = transform.Find("MenuInvenPos1").position;
        invenPos2 = transform.Find("MenuInvenPos2").position;
    }

    void Set()
    {
        bar.fillAmount = 0;
        gaugeGroup.SetActive(false);
        text.text = "수확 중...";
    }

    public void GaugeGroupSetActive(bool state)
    {
        gaugeGroup.SetActive(state);
    }

    public void BarProgress(GameObject scanObj,int setTime = 3)
    {
        StartCoroutine( IEBarProgress(scanObj,setTime));
        StartCoroutine(IEHarvestSound(setTime));
    }

    public void StopBarProgress()
    {
        StopAllCoroutines();
        Set();
    }

    IEnumerator IEBarProgress(GameObject scanObj,int setTime = 3)
    {
        gaugeGroup.SetActive(true);
        bar.fillAmount = 0;
        float currTime = 0;

        while(currTime < setTime)
        {
            currTime += Time.deltaTime / setTime;

            bar.fillAmount = Mathf.Lerp(0, 1, currTime);
            
            yield return null;
            if (bar.fillAmount == 1) break;
        }
        GameData.isHarvesting = false;
        text.text = "성공!";
        StartCoroutine(IEThrowItem(scanObj));
        GetItem();
        AudioMng.Instance.PlayUIEffect("ui_menu_button_confirm_02");
        
        QMushroom currMushroom = scanObj.GetComponent<QMushroom>();
        if(currMushroom != null)
            currMushroom.SuccessHarvest();
        
        yield return new WaitForSeconds(1);
        
        gaugeGroup.SetActive(false);
        Set();
    }

    void GetItem()
    {
        int itemID = GameData.nowHarvestItemID;
        if(itemID >= 1)
        {
            UIInventory uiInventory = FindObjectOfType<UIInventory>();
            if (uiInventory != null)
                uiInventory.ItemAdd(itemID);
        }
        else
        {
            print("아이템 정보를 읽어오지 못했습니다.");
        }
    }

    IEnumerator IEThrowItem(GameObject scanObj)
    {
        Vector3 startPos2 = Camera.main.WorldToScreenPoint(scanObj.transform.position);
        List<Vector3> path = new List<Vector3>();
        path.Add(startPos1);
        path.Add(startPos2);
        path.Add(headPos1);
        path.Add(headPos2);
        path.Add(invenPos1);
        path.Add(invenPos2);

        int itemID = GameData.nowHarvestItemID;
        string icon = DataManager.ToS(TableType.ItemTable, itemID, "ICON");
        Sprite itemSprite = Resources.Load<Sprite>("Images/" + icon);
        itemIcon.sprite = itemSprite;
        itemIcon.gameObject.SetActive(true);
        float time = 0;
        int currIndex = 0;
        bool isThrow = true;
        while (isThrow)
        {
            time += Time.deltaTime * 4f ;
            time = Mathf.Clamp01(time);

            Vector3 currPos = CatmullRom(time,
                path[0 + currIndex],
                path[1 + currIndex],
                path[2 + currIndex],
                path[3 + currIndex]);
            itemIcon.transform.position = currPos;
            if (time >= 1)
            {
                currIndex++;
                if(currIndex >= 3)
                {
                    currIndex = 0;
                    isThrow = false;
                }
                time = 0;
            }
            yield return null;
        }


        itemIcon.gameObject.SetActive(false);
        
    }

    IEnumerator IEHarvestSound(int setTime = 3)
    {
        int count = 0;
        while(count < setTime * 2)
        {
            count++;
            if(count%2==0)
                AudioMng.Instance.PlayActionEffect("harvesting1", 0.3f);
            else
                AudioMng.Instance.PlayActionEffect("harvesting2", 0.3f);

            yield return new WaitForSeconds(0.5f);
        }
    }

    public Vector3 CatmullRom(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        return 0.5f * ((2.0f * p1) + (-p0 + p2) * t + (2.0f * p0 - 5.0f * p1 + 4 * p2 - p3) * t2 +
            (-p0 + 3.0f * p1 - 3.0f * p2 + p3) * t3);
    }
}
