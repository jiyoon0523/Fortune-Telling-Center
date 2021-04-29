using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GazeRay : MonoBehaviour
{
    [SerializeField]
    Camera mainCam;
    [SerializeField]
    float rayDist;
    [SerializeField]
    float radius;

    Stage stage;

    GameObject managerObj;
    GazeInterface gazeInterface;

    private bool isObserveOff = false;

    void Start()
    {
        stage = GameManager.Instance.stage;
        switch (stage)
        {
            case Stage.Intro:
                managerObj = GameObject.Find("IntroManager");
                break;
            case Stage.Main:
                managerObj = GameObject.Find("MainManager");
                break;
            case Stage.Outro:
                managerObj = GameObject.Find("OutroManager");
                break;
        }
        gazeInterface = managerObj.GetComponent<GazeInterface>();
        mainCam = Camera.main;
    }

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
                gazeInterface.GazeRespond(hit);
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
