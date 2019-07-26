using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TiltWalk : MonoBehaviour {
    private SteamVR_PlayArea area;
    public SteamVR_Action_Boolean grabPinch;
    public float minTiltDist;
    public float moveMult;
    public bool accelerate;
    public float accelMult;
    public float maxSpeed;
    private Vector3 velocity = new Vector3(0f, 0f, 0f);
    void Awake ()
    {
        area = GetComponent<SteamVR_PlayArea>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (grabPinch.GetState(SteamVR_Input_Sources.Any))
        {
            var cam = SteamVR_Render.Top();
            Vector3 camLocal = cam.transform.localPosition;
            Vector3 camUpFlat = cam.transform.up;
            camUpFlat.y = 0f;

            if (camUpFlat.magnitude < minTiltDist)
            {
                camUpFlat = Vector3.zero;
            }

            if (accelerate)
            {
                velocity += camUpFlat * accelMult * Time.deltaTime;
            }
            else
            {
                velocity = camUpFlat * moveMult;
            }

            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
            area.transform.position += velocity * Time.deltaTime;
        }
    }
}
