using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ZoomWalk : MonoBehaviour
{
    private SteamVR_PlayArea area;
    public SteamVR_Action_Boolean grabPinch;
    public float TRIGGER_WALK_MULTIPLIER;
    private Vector3 lastLocalPos;
    void Awake ()
    {
        area = GetComponent<SteamVR_PlayArea>();
    }

    // Update is called once per frame
    void Update ()
    {
        var cam = SteamVR_Render.Top();
        if (grabPinch.GetState(SteamVR_Input_Sources.Any))
        {
            Vector3 posDelta = cam.transform.localPosition - lastLocalPos;
            posDelta.y = 0f;
            area.transform.position += posDelta * TRIGGER_WALK_MULTIPLIER;
        }

        lastLocalPos = cam.transform.localPosition;
    }
}