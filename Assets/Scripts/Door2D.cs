using UnityEngine;
using DG.Tweening;

public class Door2D : MonoBehaviour
{
    public Vector3 ClosedAngle;
    public Vector3 OpenedAngle;
    public float TriggerDistance;
    public bool IsOpen = false;
    public float Duration;

    public DoorController RelativeDoor;

    void Start()
    {
        
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
                Debug.Log(name+"is not close enough.");
            }
        }
    }

    public void ToggleDoor()
    {
        if(IsOpen)
        {
            transform.DOLocalRotateQuaternion(Quaternion.Euler(ClosedAngle),Duration);
            if(RelativeDoor)
            {
                RelativeDoor.ToggleDoor();
            }
        }else
        {
            transform.DOLocalRotateQuaternion(Quaternion.Euler(OpenedAngle),Duration);
            if(RelativeDoor)
            {
                RelativeDoor.ToggleDoor();
            }
        }

        IsOpen = !IsOpen;
    }

    private bool NearDoor()
    {
        Vector3 pos = GameRoot.Instance.PlayerController2D.transform.position;
        float distance = Vector3.Distance(pos,this.transform.position);
        if(distance < TriggerDistance)
        {
            if(pos.y > transform.position.y)
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
