using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PhantomWall : MonoBehaviour
{
    public List<GameObject> ObjectsToSee = new List<GameObject>();
    public GameObject PhantomObject;
    public Tilemap RelativeTilemap;
    public LayerMask TriggerLayers;

    TilemapRenderer tilemapRender;
    TilemapCollider2D tilemapCollider;

    public bool IsPhantom;

    void Start()
    {
        tilemapRender = RelativeTilemap.transform.GetComponent<TilemapRenderer>();
        tilemapCollider = RelativeTilemap.transform.GetComponent<TilemapCollider2D>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            if(JudgePhantom())
            {
                SwitchToPhantom();
            }else
            {
                Debug.Log("Not Triggered");
            }
        }
    }

    public void SwitchToPhantom()
    {
        tilemapRender.enabled = false;
        tilemapCollider.enabled = false;
        IsPhantom = true;
        Debug.Log("幻影墙壁消失了");
    }

    public void ResumeFromPhantom()
    {
        tilemapRender.enabled = true;
        tilemapCollider.enabled = true;
        IsPhantom = false;
        Debug.Log("幻影墙壁恢复了");
    }

    
    public bool JudgePhantom()
    {
        Vector3 playerPos = GameRoot.Instance.PlayerCharacterController.transform.position;
        foreach(GameObject obj in ObjectsToSee)
        {
            //Debug.Log(obj.name+ obj.GetComponent<MeshRenderer>().isVisible);
            if(!obj.GetComponent<MeshRenderer>().isVisible)
            {
                return false;
            }
            if(CanRayHit(obj.transform.position,playerPos))
            {
                return false;
            }
        }

        if(!CanRayHit(PhantomObject.transform.position,playerPos))
        {
            return false;
        }

        return true;
    }

    private bool CanRayHit(Vector3 startPos,Vector3 endPos)
    {
        RaycastHit hit;
        Vector3 dir = endPos - startPos;
        Physics.Raycast(startPos,dir,out hit,50f,TriggerLayers);
        //Debug.Log(hit.transform);
        if(hit.transform != null)
        {
            Debug.Log(hit.transform.gameObject.layer);
            Debug.Log(hit.transform.gameObject.name);
            return true;
        }
        return false;

        //return Physics.Raycast(startPos,dir,out hit,500f,~IgnoreLayers);
    }

    private void OnDrawGizmos()
    {

        //Gizmos.DrawLine(GameRoot.Instance.PlayerCharacterController.transform.position,PhantomObject.transform.position);
        //Gizmos.DrawLine(GameRoot.Instance.PlayerCharacterController.transform.position,ObjectsToSee[0].transform.position);
    }
    

    
}
