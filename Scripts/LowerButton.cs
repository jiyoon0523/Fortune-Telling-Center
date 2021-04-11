using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerButton : MonoBehaviour
{
    GameObject lower;

    void Start()
    {
        lower = this.transform.parent.gameObject;
    }

    public void LowerClose()
    {
        if (lower.activeSelf == true)
            lower.SetActive(false);
    }

}
