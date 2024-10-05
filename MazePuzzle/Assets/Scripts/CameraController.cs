using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class CameraController : MonoBehaviour
{
    public CinemachineFreeLook MainCam; 
    public float mouseSensitivity = 2f;

    public CinemachineFreeLook CurrentCam;

    PlayerInputHandler playerInputHandler;

    void Start()
    {
        playerInputHandler = GameManager.Instance.PlayerInputHandler;
        CurrentCam = MainCam;
        ResetCurrentCam();
    }

    void Update()
    {

        

    }

    public void ResetCurrentCam()
    {
        CurrentCam.m_XAxis.Value = 0;
        CurrentCam.m_YAxis.Value = 0.5f;
    }

    public void CameraRotation(float xValue,float yValue)
    {
        CurrentCam.m_XAxis.Value += xValue;
        CurrentCam.m_YAxis.Value += yValue;

        CurrentCam.m_YAxis.Value = Mathf.Clamp(CurrentCam.m_YAxis.Value, 0.2f, 0.8f);

        CurrentCam.transform.rotation = Quaternion.Euler(transform.eulerAngles.x,transform.eulerAngles.y,0f);
    }


}
