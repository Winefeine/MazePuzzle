using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera ResetCamera;
    public CinemachineVirtualCamera ChangeCamera;
    public CinemachineVirtualCamera ActiveCamera;


    PlayerInputHandler playerInputHandler;
    float cameraVerticalAngle = 0f;

    void Start()
    {
        playerInputHandler = GameManager.Instance.PlayerInputHandler;
        
        ResetCurrentCam();
    }

    void Update()
    {

        

    }

    public void ResetCurrentCam()
    {

    }

    public Vector3 CameraRotation(float rotationValue)
    {
        cameraVerticalAngle += rotationValue;
        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -89f, 89f);
        
        //ChangeCamera.transform.localEulerAngles = new Vector3(cameraVerticalAngle,0f,ChangeCamera.transform.rotation.eulerAngles.z);
        ResetCamera.transform.localEulerAngles = new Vector3(cameraVerticalAngle,0f,0f);

        return ResetCamera.transform.localEulerAngles;
    }

    public void GetCameraHeightRatio(float ratio)
    {
        ResetCamera.transform.localPosition = new Vector3(0f,ratio,0f);
    }


}
