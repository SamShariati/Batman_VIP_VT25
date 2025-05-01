using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TooltipHUD : HudBase
{
    
    struct TooltipData
    {
        public string message;
        public float endTime;
        public float prio;
    }

    List<TooltipData> tooltipList = new();

    TooltipData currentTooltip;
    public TMP_Text tooltipText;
    bool showTooltip;

    public override void OnNoPlayer()
    {
        
    }

    public override void OnPlayerObject(GameObject obj)
    {
        
    }

    
    void Start()
    {
        currentTooltip = new TooltipData()
        {
            message = "",
            endTime = 0,
            prio = 0
        };
        tooltipText.enabled = false;
    }

    private void OnEnable()
    {
        TooltipUtil.OnTooltipTimed += TooltipUtil_OnTooltipTimed;
        TooltipUtil.OnCancelTooltip += TooltipUtil_OnCancelTooltip;
    }

    private void OnDisable()
    {
        TooltipUtil.OnTooltipTimed -= TooltipUtil_OnTooltipTimed;
        TooltipUtil.OnCancelTooltip -= TooltipUtil_OnCancelTooltip;
    }

    private void TooltipUtil_OnCancelTooltip(string msg)
    {
        if(currentTooltip.message == msg)
        {
            tooltipText.enabled = false;
            tooltipText.text = "";
            currentTooltip.endTime = 0;
        }
    }

    private void TooltipUtil_OnTooltipTimed(string msg, float time, float prio)
    {
        
        if(string.IsNullOrEmpty(msg)) return; //no message so dont do anything
        if (prio < currentTooltip.prio) return; //message is lower prio dont show it

        float endTime = Time.time + time;
        currentTooltip = new TooltipData()
        {
            message = msg,
            endTime = endTime,
            prio = prio
        };
        showTooltip = true;
        tooltipText.enabled = true;
        tooltipText.text = msg;

       // Debug.Log("Show " + msg + " " + prio);
        //enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!showTooltip)
        {
            tooltipText.enabled = false;
           // Debug.Log("Disabling " + currentTooltip.message + " " + currentTooltip.prio);
            currentTooltip.prio = 0; //reset the prio!
            //enabled = false;
        }
        showTooltip = currentTooltip.endTime >= Time.time;//after to allow atleast one frame of tooltip
    }
}

public static class TooltipUtil
{
    public static event Action<string, float, float> OnTooltipTimed;
    public static event Action<string> OnCancelTooltip;
    public static void Show(string message, float time = 0, float prio = 0)
    {
        OnTooltipTimed?.Invoke(message, time, prio);
    }

    public static void Cancel(string message)
    {
        OnCancelTooltip?.Invoke(message);
    }
}
