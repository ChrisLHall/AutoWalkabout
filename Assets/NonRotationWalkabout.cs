using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class NonRotationWalkabout : MonoBehaviour {
    private SteamVR_PlayArea area;
    public SteamVR_Action_Boolean grabPinch;
    private bool faded = false;
    private float fadeStarted = 0f;
    public float FADE_TIME; //.1
    private Vector3 fadeStartLocalPos;
    void Awake() {
        area = GetComponent<SteamVR_PlayArea>();
    }

    // Update is called once per frame
    void Update ()
    {
        var cam = SteamVR_Render.Top();
        Vector3 camLocalFlat = cam.transform.localPosition;
        camLocalFlat.y = 0f;
        if (grabPinch.GetState(SteamVR_Input_Sources.Any) && !faded)
        {
            faded = true;
            fadeStartLocalPos = camLocalFlat;
            SteamVR_Fade.Start(Color.black, FADE_TIME);
        }
        else if (!grabPinch.GetState(SteamVR_Input_Sources.Any) && faded)
        {
            faded = false;
            transform.position += (fadeStartLocalPos - camLocalFlat);
            SteamVR_Fade.Start(Color.clear, FADE_TIME);
        }
    }
}
