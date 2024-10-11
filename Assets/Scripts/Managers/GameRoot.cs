using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameRoot : MonoBehaviour
{
    public static GameRoot Instance;
    
    [Header("Managers")]
    public PlayerInputHandler PlayerInputHandler;
    public PlayerCharacterController PlayerCharacterController;
    public PlayerController2D PlayerController2D;
    public CameraController CameraController;

    [Header("General")]
    public bool Is3D;
    public bool Is2D;

    public bool CanSwitchTo3D;
    public bool CanSwitchTo2D;

    public Tilemap FloorTilemap;
    public TileBase StartTile;
    public TileBase FloorTile;
    
    public Vector3Int StartPos2D;
    public Vector3 StartPos;

    public PhantomWall pw;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }


    }

    void Start()
    {
        SwitchTo3D();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            if(JudgeCanSwitchTo3D())
            {
                SwitchTo3D();
            }
            
        }
        if(Input.GetKeyDown(KeyCode.T))
        {
            SwitchTo2D();
        }

    }

    private void SwitchTo3D()
    {   
        if(Is2D)
        {
            FloorTilemap.SetTile(StartPos2D,FloorTile);
            PlayerController2D.transform.position = StartPos;
        }
        if(pw.IsPhantom)
        {
            pw.ResumeFromPhantom();
        }


        CameraController.CamSwitchTo3D();

        Is3D = true;
        Is2D = false;
    }


    private void SwitchTo2D()
    {
        CameraController.CamSwitchTo2D();

        StartPos = PlayerController2D.transform.position;
        FloorTilemap.SetTile(StartPos2D,StartTile);

        //Point Out the Start Pos


        Is3D = false;
        Is2D = true;
    }    

    private bool JudgeCanSwitchTo3D()
    {
        float distance = Vector3.Distance(PlayerController2D.transform.position,StartPos);
        if(distance <= 1.5f)
        {
            return true;
        }

        Debug.Log("离进入点太远了");
        return false;
    }


    private void OnDestroy()
    {
        DestroyImmediate(gameObject);
    }

    public void Active3DCharacter()
    {

    }

    public void Active2DCharacter()
    {


    }


}