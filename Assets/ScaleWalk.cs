using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ScaleWalk : MonoBehaviour
{
    private SteamVR_PlayArea area;
    public SteamVR_Action_Boolean grabPinch;
    public float SCALE_UP_MULT;
    private bool scaled = false;
    private float savedPosY;
    void Awake ()
    {
        area = GetComponent<SteamVR_PlayArea>();
    }

    // Update is called once per frame
    void Update ()
    {
        var cam = SteamVR_Render.Top();
        Vector3 camLocal = cam.transform.localPosition;
        //camLocal.y = 0f;
        if (grabPinch.GetStateDown(SteamVR_Input_Sources.Any) && !scaled)
        {
            scaled = true;
            savedPosY = area.transform.position.y;
            area.transform.localScale = Vector3.one * SCALE_UP_MULT;
            area.transform.position -= (SCALE_UP_MULT - 1) * camLocal;
        }
        else if (grabPinch.GetStateUp(SteamVR_Input_Sources.Any) && scaled)
        {
            scaled = false;
            area.transform.localScale = Vector3.one;
            area.transform.position += (SCALE_UP_MULT - 1) * camLocal;
            Vector3 newPos = area.transform.position;
            newPos.y = savedPosY;
            area.transform.position = newPos;
        }
    }
}