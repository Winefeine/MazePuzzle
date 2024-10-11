using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorController : MonoBehaviour
{

    public Vector3 ClosedAngle = new Vector3(0f,0f,0f); // 门关闭时的角度
    public Vector3 OpenAngle = new Vector3(0f,90f,0f); // 门打开时的角度
    public float Duration = 1f; // 旋转所需时间

    public float TriggerDistance;

    public bool IsOpen = false;
    //public bool IsOpen { get; private set;}

    private void Start()
    {
        // 初始化时，门保持关闭角度
        transform.localRotation = Quaternion.Euler(0, ClosedAngle.y, 0);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(NearDoor())
            {
                ToggleDoor();
            }else
            {
                //Debug.Log(name+"is not close enough.");
            }
        }
    }

    public void ToggleDoor()
    {
        if (IsOpen)
        {
            // 关闭门：从当前角度旋转到关闭角度
            transform.DOLocalRotateQuaternion(Quaternion.Euler(ClosedAngle), Duration);
        }
        else
        {
            // 打开门：从当前角度旋转到打开角度
            transform.DOLocalRotateQuaternion(Quaternion.Euler(OpenAngle), Duration);
        }

        IsOpen = !IsOpen;
    }

    private bool NearDoor()
    {
        Vector3 pos = GameRoot.Instance.PlayerCharacterController.transform.position;
        float distance = Vector3.Distance(pos,this.transform.position);
        if(distance < TriggerDistance)
        {
            if(pos.z > transform.position.z)
            {
                //UI
                Debug.Log("不能从这一侧打开");
                return false;
            }
            return true;
        }

        return false;
    }

}