using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    GameObject mainCamera;
    GameObject subCamera;

    private void Start()
    {
        mainCamera = GameObject.FindObjectOfType<CameraController>(true).gameObject;
        if(mainCamera != null)
        {
            mainCamera.SetActive(true);
        }
        subCamera = transform.Find("SubCamera").gameObject;
        if(subCamera != null)
        {
            subCamera.SetActive(false);
        }
    }

    public void activeCamera(int cameraNum, bool state)
    {
        if(cameraNum == 1)
        {
                mainCamera.SetActive(state);
        }
        else if(cameraNum == 2)
        {
                subCamera.SetActive(state);
        }
    }

    public void CameraPos (Vector3 gatePosition, Vector3 camPos)
    {
        transform.position = camPos;
        transform.LookAt(gatePosition);
    }

    public void npcCameraPos(Vector3 pos, Vector3 camPos)
    {
        transform.position = camPos;
        pos.y = camPos.y;
        transform.LookAt(pos);
    }

    public void ShakeCamera(int cameraNum = 1, float interval = 0.01f, float distance = 0.05f,  int count = 20)
    {
        StartCoroutine(IEShakeCamera(cameraNum, interval, distance, count));
    }

    public IEnumerator IEShakeCamera(int cameraNum, float interval, float distance, int count)
    {
        GameObject camera = mainCamera;
        switch (cameraNum)
        {
            case 1:
                camera = mainCamera;
                break;
            case 2:
                camera = subCamera;
                break;
        }

        Vector3 originPos = camera.transform.position;
        for (int i = 0; i < count; i++)
        {
            Vector2 unitCircle = Random.insideUnitCircle;
            camera.transform.position = originPos + new Vector3(unitCircle.x, unitCircle.y, 0) * distance;
            yield return new WaitForSeconds(interval);
        }
        camera.transform.position = originPos;
    }


    void Update()
    {
        
    }
}
