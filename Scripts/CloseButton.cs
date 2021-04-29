using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButton : MonoBehaviour
{
    GameObject finalResult;
    GameObject joystick;
    OutroManager om;

    private void Start()
    {
        if (GameManager.Instance.stage == Stage.Outro)
        {
            finalResult = GameObject.Find("Final Fortune Canvas").transform.GetChild(3).gameObject;
            joystick = GameObject.Find("Final Fortune Canvas").transform.GetChild(0).gameObject;
            om = GameObject.Find("OutroManager").GetComponent<OutroManager>();
        }
    }
    public void CloseResult()
    {
        finalResult.SetActive(false);
        finalResult.GetComponent<FortuneResultManager>().InitializeFade();
        CanvasManager.Instance.ActiveJoysticks();
        joystick.SetActive(true);
        om.isResultClosed = true;
        om.isMagPause = false;
    }
}
