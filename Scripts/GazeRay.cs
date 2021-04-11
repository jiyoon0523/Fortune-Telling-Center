using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class GazeRay : MonoBehaviour
{
    [SerializeField]
    Camera mainCam;
    [SerializeField]
    float rayDist;
    [SerializeField]
    float radius;

    Stage stage;

    MainManager mm;
    IntroManager im;
    OutroManager om;
    private bool isObserveOff = false;

    void Start()
    {
        stage = GameManager.Instance.stage;
        switch (stage)
        {
            case Stage.Intro:
                im = GameObject.Find("IntroManager").GetComponent<IntroManager>();
                break;
            case Stage.Main:
                mm = GameObject.Find("MainManager").GetComponent<MainManager>();
                break;
            case Stage.Outro:
                om = GameObject.Find("OutroManager").GetComponent<OutroManager>();
                break;
        }
        print("GazeRay가 받은 stage: " + stage);

        mainCam = Camera.main;
        Debug.Log("mainCam: " + mainCam);
    }

    public abstract void GazeRespond(RaycastHit hit);

    void Update()
    {
        Gaze();
    }

    public void Gaze()
    {
        Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
        RaycastHit hit;
        int layerMask = 1 << 12;

        if (Physics.SphereCast(ray, radius, out hit, rayDist, layerMask))
        {
            if (CanvasManager.Instance.below.activeSelf == false)
            {

                if ((int)stage == 1)
                {
                    im.GazeRespond(hit);
                }
                else if ((int)stage == 3)
                {
                    om.GazeRespond(hit);
                }
                else
                {
                    mm.GazeRespond(hit);
                }
                isObserveOff = false;
            }
        }
        else
        {
            if (CanvasManager.Instance)
            {
                CanvasManager.Instance.InitializeMag();
                if (!isObserveOff)
                {
                    CanvasAudioManager.Instance.OnObserveOff();
                    isObserveOff = true;
                }
            }
        }
    }
}
