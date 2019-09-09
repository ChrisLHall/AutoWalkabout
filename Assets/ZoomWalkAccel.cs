using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ZoomWalkAccel : MonoBehaviour
{
    private SteamVR_PlayArea area;
    public SteamVR_Action_Boolean grabPinch;
    public float triggerSpeedThresh = 2f;
    // use a quadratic curve
    public float triggerWalkFactorSqr = .5f;
    public float triggerSpeedCap = 4f;
    public bool USE_FORWARD_VECTOR = false;
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
                posDelta *= Mathf.Clamp01(dot);
            }

            float speed = Mathf.Clamp(posDelta.magnitude / Time.deltaTime, 0f, triggerSpeedCap);
            if (speed > triggerSpeedThresh)
            {
                Debug.Log("SPEED OVER THRESHOLD");
                float extraSpeed = speed - triggerSpeedThresh;
                extraSpeed = (extraSpeed * extraSpeed) * triggerWalkFactorSqr;
                transform.position += posDelta * extraSpeed;
            }
        }

        lastLocalPos = cam.transform.localPosition;
    }
}