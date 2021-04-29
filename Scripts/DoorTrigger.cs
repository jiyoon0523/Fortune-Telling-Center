using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    GameObject mm;
    
    void Start()
    {
        mm = GameObject.Find("MainManager");
    }

    private void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.tag != "Player"){return;}
        mm.GetComponent<UpperCaption>().ShowExitNarration();
        Debug.Log("Door Delegate in doorRespawn");
    }
}
