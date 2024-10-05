using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;  
    public static GameManager Instance;
    
    public PlayerInputHandler PlayerInputHandler;
    public PlayerCharacterController PlayerCharacterController;


    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        DestroyImmediate(gameObject);
    }

}