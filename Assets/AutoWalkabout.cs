using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class AutoWalkabout : MonoBehaviour {
    private SteamVR_PlayArea area;
    public float EDGE_THRESHOLD; // .3
    private bool startedFade = false;
    private float fadeStarted = 0f;
    public float FADE_OUT_TIME; //.1
    public float FADE_IN_TIME; //.1
    private bool finishedFade = false;
    private bool flipped = false;
    private bool returnedToCenter = true;
    private Vector3 initialTurnLookDir;
    public float TURN_DOT_THRESH; //.9
    void Awake() {
        area = GetComponent<SteamVR_PlayArea>();
    }

    // TODO test?
    private void OnDisable()
    {
        flipped = false;
        transform.localRotation = Quaternion.identity;
    }
	
    // Update is called once per frame
    void Update () {
        var cam = SteamVR_Render.Top();
        HmdQuad_t areaRect = new HmdQuad_t();
        if (cam && SteamVR_PlayArea.GetBounds(SteamVR_PlayArea.Size.Calibrated, ref areaRect)) {
            float xNorm = cam.transform.localPosition.x / Mathf.Abs(areaRect.vCorners0.v0) / 2f;
            float zNorm = cam.transform.localPosition.z / Mathf.Abs(areaRect.vCorners0.v2) / 2f;
            //Debug.Log("NORMALIZED POS " + xNorm + "," + zNorm);
            //Debug.Log("Cam pos " + cam.transform.localPosition);
            //Debug.Log("Rect " + PV(areaRect.vCorners0) + " " + PV(areaRect.vCorners1) + " " + PV(areaRect.vCorners2) + " " + PV(areaRect.vCorners3));
            
            Vector3 camLookFlat = cam.transform.forward;
            camLookFlat.y = 0f;
            camLookFlat = camLookFlat.normalized * (flipped ? -1f : 1f);

            bool outsideNormalArea = Mathf.Abs(xNorm) > EDGE_THRESHOLD || Mathf.Abs(zNorm) > EDGE_THRESHOLD;
            if (outsideNormalArea) {
                if (!startedFade && returnedToCenter) {
                    startedFade = true;
                    fadeStarted = Time.time;
                    SteamVR_Fade.Start(Color.black, FADE_OUT_TIME);
                    initialTurnLookDir = camLookFlat;
                    returnedToCenter = false;
                    Debug.Log("FADE OUT");
                }
            }
            bool lookingOpposite = Vector3.Dot(initialTurnLookDir, camLookFlat) < -TURN_DOT_THRESH;
            bool lookingOppositeIsh = Vector3.Dot(initialTurnLookDir, camLookFlat) < 0f;
            if (!outsideNormalArea || lookingOpposite) { // looking away
                if (finishedFade) {
                    startedFade = false;
                    finishedFade = false;
                    SteamVR_Fade.Start(Color.clear, FADE_IN_TIME);
                    Debug.Log("FADE IN AND MAYBE FLIP");
                    // don't flip if youre still looking the same way
                    if (lookingOppositeIsh) {
                        Debug.Log("Flipitt");
                        Vector3 localXZ = cam.transform.localPosition * (flipped ? -1f : 1f);
                        localXZ.y = 0f;
                        transform.position += 2f * localXZ;
                        transform.Rotate(new Vector3(0f, 180f, 0f));
                        flipped = !flipped;
                    }
                }
            }

            if (!outsideNormalArea) {
                returnedToCenter = true;
            }

            Debug.Log("camdir " + camLookFlat + " initdir " + initialTurnLookDir + " lookingopp " + lookingOpposite + " ish " + lookingOppositeIsh);
            if (startedFade && Time.time > fadeStarted + FADE_OUT_TIME && !finishedFade) {
                finishedFade = true;
            }
        } else {
            Debug.Log("No area I guess :O cam: " + cam);
        }
    }

    string PV (HmdVector3_t v) {
        return "{" + v.v0.ToString("f2") + " " + v.v1.ToString("f2") + " " + v.v2.ToString("f2") + "}";
    }
}
