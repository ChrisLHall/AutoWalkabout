using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class RotateWalk : MonoBehaviour
{
    private SteamVR_PlayArea area;
    public SteamVR_Action_Boolean grabPinch;
    public float deltaToDegreesFactor = 5f;
    private Vector3 lastPos;
    public SteamVR_Action_Boolean reverseDirAction;
    public bool useAngleFromCenter = false;
    public float minDistFromCenter = .3f;
    void Awake()
    {
        area = GetComponent<SteamVR_PlayArea>();
    }

    // Update is called once per frame
    void Update()
    {
        var cam = SteamVR_Render.Top();
        if (grabPinch.GetState(SteamVR_Input_Sources.Any))
        {
            Vector3 posDelta = cam.transform.position - lastPos;
            posDelta.y = 0f;

            Vector3 flatForward = cam.transform.forward;
            flatForward.y = 0f;
            float dot = Vector3.Dot(flatForward.normalized, posDelta.normalized);

            float dotSpeed = posDelta.magnitude * dot;

            float dirFactor = (reverseDirAction.GetState(SteamVR_Input_Sources.Any)) ? -1f : 1f;
            float angle = dotSpeed * deltaToDegreesFactor * dirFactor;
            if (useAngleFromCenter)
            {
                // TODO try this method later?
            }
            transform.RotateAround(cam.transform.position, Vector3.up, angle);
        }

        lastPos = cam.transform.position;
    }

    void OnDisable()
    {
        transform.localRotation = Quaternion.identity;
    }
}