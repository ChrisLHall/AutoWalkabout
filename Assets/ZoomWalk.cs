using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ZoomWalk : MonoBehaviour
{
    private SteamVR_PlayArea area;
    public SteamVR_Action_Boolean grabPinch;
    public float TRIGGER_WALK_MULTIPLIER;
    public bool USE_FORWARD_VECTOR;
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
            if (USE_FORWARD_VECTOR)
            {
                Vector3 flatForward = cam.transform.forward;
                flatForward.y = 0f;
                float dot = Vector3.Dot(flatForward.normalized, posDelta.normalized);
                Debug.Log("DOING FORWARD DIR. DOT: " + dot);
                posDelta *= Mathf.Clamp01(dot);
            }
            area.transform.position += posDelta * TRIGGER_WALK_MULTIPLIER;
        }

        lastLocalPos = cam.transform.localPosition;
    }
}