using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera playerCamera;
    Vector3 forward;
    Transform followTarget;

    float targetVerticalAngle = 0;
    float minVerticalAngle = -60f;
    float maxVerticalAngle = 60f;

    float defaultDistance = 10; // 줌의 기본값
    float zoomDistance = 0;
    float minDistance = 1;
    float maxDistance = 10f;
    float zoomUnit = 3f;
    
    void Start()
    {
        forward = transform.forward;
        followTarget = GameObject.FindGameObjectWithTag("CameraPoint").transform;
        playerCamera = Camera.main;
        zoomDistance = defaultDistance;
    }

    public Quaternion CalculateRotationMoveByTheYAxis()
    {
        targetVerticalAngle = Mathf.Clamp(targetVerticalAngle + Input.GetAxisRaw("Mouse Y"), 
            minVerticalAngle, maxVerticalAngle);
        return Quaternion.Euler(targetVerticalAngle * -1, 0, 0);
    }

    Quaternion RotationValueOfTheCamera()
    {
        Quaternion yRotation = CalculateRotationMoveByTheYAxis();
        //Quaternion lookRotation = Quaternion.LookRotation(forward) * yRotation;
        return yRotation;
    }

    //Vector3 CalculatePositionOfCamera(Quaternion rotation)
    //{
    //    return followTarget.position - rotation * Vector3.forward * currentDistance;
    //}

    void ZoomCamera()
    {
        float currDistance = Vector3.Distance(playerCamera.transform.position, followTarget.position);
        Vector3 targetDist = playerCamera.transform.position - followTarget.position;
        
        RaycastHit[] hits = Physics.SphereCastAll(followTarget.position, 0.01f,
            (playerCamera.transform.position - followTarget.position).normalized, currDistance,1<<LayerMask.NameToLayer("Environment"));
        targetDist = Vector3.Normalize(targetDist);

        if(hits != null)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (currDistance > hits[i].distance)
                {
                    if (minDistance < currDistance)
                        Camera.main.transform.position -= targetDist * 0.1f;
                    //currentDistance = hits[i].distance - 0.5f;
                }
            }
        }
        

        

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            Vector3 calCamPos = Camera.main.transform.position - (targetDist * scroll * zoomUnit);
            float calDist = Vector3.Distance(calCamPos, followTarget.position);
            
            if(minDistance < calDist && calDist < maxDistance)
            {
                Camera.main.transform.position -= (targetDist * scroll * zoomUnit);
            }
        }

        Camera.main.transform.LookAt(followTarget.transform);

        



    }

    void Update()
    {
        if (playerCamera == null) return;
        if (GameData.isUI) return;
        followTarget.transform.localRotation = RotationValueOfTheCamera(); // 카메라 상하 시점
        ZoomCamera(); // 카메라 줌

    }
}
