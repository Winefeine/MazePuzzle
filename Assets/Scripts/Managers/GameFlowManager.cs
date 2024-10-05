using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager
{
    private List<BasePhase> fixedPhaseList;
    private int fixedPhaseNum = 0;
    private bool pauseFlag = false;
    private BasePhase curPhase;

    public PhaseManager()
    {
        fixedPhaseList = new List<BasePhase>();
    }

    public void SwitchProcess(BasePhase basePhase = null)
    {
        Pause();
        // 处理特殊事件
        if (basePhase != null)
        {
            curPhase = basePhase;
            curPhase.OnEnter();
        }
        // 处理固定事件
        else
        {
            if (fixedPhaseList.Count > 0)
            {
                curPhase = fixedPhaseList[fixedPhaseNum];
                fixedPhaseNum++;
                fixedPhaseNum %= fixedPhaseList.Count;
                curPhase.OnEnter();
                pauseFlag = false;
            }
            else
            {
                curPhase = null;
                pauseFlag = true;
            }
        }
    }

    public void Pause()
    {
        if (curPhase != null)
        { 
            curPhase.OnPause(); 
        }
    }

    public void Resume()
    {
        if (curPhase != null)
        {
            curPhase.OnResume();
        }
    }
}
