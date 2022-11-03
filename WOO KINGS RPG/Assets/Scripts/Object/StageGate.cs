using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGate : MonoBehaviour
{
    [SerializeField]
    int gateNum = 1;
    float openSpeed = 3f;

    Vector3 gatePos;
    Vector3 cameraPos;

    private void Start()
    {
        gatePos = transform.position;
        cameraPos = transform.Find("CameraPos").position;
    }

    public void OpenGate(int shrineNum)
    {
        if (shrineNum != gateNum) return;
        StartCoroutine(OpenGate());
    }

    IEnumerator OpenGate()
    {
        yield return new WaitForSeconds(1.5f);

        CameraManager cameraManager = GameObject.FindObjectOfType<CameraManager>();
        if(cameraManager != null)
        {
            cameraManager.activeCamera(1,false);
            cameraManager.activeCamera(2,true);
            cameraManager.CameraPos(gatePos, cameraPos);
        }

        float elapsedTime = 0;
        Vector3 originScale = transform.localScale;
        AudioMng.Instance.PlayUIEffect("hum_electric_neon_light_01");
        while (elapsedTime < 1f) // 게이트 작아지기
        {
            elapsedTime += Time.deltaTime / openSpeed;
            Vector3 currentScale = transform.localScale;
            currentScale.x = Mathf.Lerp(2,0,elapsedTime);
            currentScale.y = Mathf.Lerp(2,0,elapsedTime);
            currentScale.z = Mathf.Lerp(2,0,elapsedTime);
            transform.localScale = currentScale;

            yield return null;
        }
        yield return new WaitForSeconds(1f);

        cameraManager.activeCamera(1, true);
        cameraManager.activeCamera(2, false);
        GameData.isFreeze = false;
        Destroy(gameObject);
    }
}
