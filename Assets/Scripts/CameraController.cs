using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera Camera3D;
    public CinemachineVirtualCamera Camera2D;
    public CinemachineVirtualCamera ActiveCamera;


    PlayerInputHandler playerInputHandler;
    float cameraVerticalAngle = 0f;

    void Start()
    {
        playerInputHandler = GameRoot.Instance.PlayerInputHandler;
        
    }

    void Update()
    {

        

    }

    public void CamSwitchTo3D()
    {
        Camera3D.Priority = 10;
        Camera2D.Priority = 5;
    }

    public void CamSwitchTo2D()
    {
        Camera3D.Priority = 5;
        Camera2D.Priority = 10;
    }


    public Vector3 CameraRotation(float rotationValue)
    {
        cameraVerticalAngle += rotationValue;
        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -89f, 89f);
        
        Camera3D.transform.localEulerAngles = new Vector3(cameraVerticalAngle,0f,0f);

        return Camera3D.transform.localEulerAngles;
    }

    public void GetCameraHeightRatio(float ratio)
    {
        Camera3D.transform.localPosition = new Vector3(0f,ratio,0f);
    }


}
