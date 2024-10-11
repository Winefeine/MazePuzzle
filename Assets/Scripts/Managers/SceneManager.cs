using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager
{

    private Dictionary<SceneType, GameObject> dicScene;

    BaseScene sceneState;
    
    /// <param name="scene"></param>
    public void SetScene(BaseScene scene)
    {
        //if(sceneState != null)
        //{
        //    sceneState.OnExit();
        //}
        //sceneState = scene;
        //if(sceneState != null)
        //{
        //    sceneState.OnEnter();
        //}
        sceneState?.OnExit();
        sceneState = scene;
        sceneState?.OnEnter();
    }

    public SceneManager()
    {
        dicScene = new Dictionary<SceneType, GameObject>();
    }

    public GameObject GetSingleScene(SceneType type)
    {
        //GameObject parent = Ga
        return null;
    }
}
