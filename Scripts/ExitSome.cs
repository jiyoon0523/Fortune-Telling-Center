using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitSome : ExitNarration
{
    void Start()
    {
        upperText = base.upperText;
        mm = base.mm;
        duration= base.duration;
    }

    public override void ShowExit(int i)
    {
        if (PlayerManager.Instance.currentRoomNo == 0)
            StartCoroutine(ExitText(5, 2f, ExitTextEnd(6, duration - 2f)));
        else if (PlayerManager.Instance.currentRoomNo == 1)
            StartCoroutine(ExitText(5, 5f, ExitTextEnd(6, duration - 5f)));
        else if (PlayerManager.Instance.currentRoomNo == 2)
            StartCoroutine(ExitText(6, 4.5f, ExitTextEnd(7,duration-4.5f)));
        else if (PlayerManager.Instance.currentRoomNo == 3)
            StartCoroutine(ExitText(5,6.2f, ExitTextEnd(6, duration - 6.2f)));
        else if (PlayerManager.Instance.currentRoomNo == 4)
            StartCoroutine(ExitText(5,6f, ExitTextEnd(6, duration - 6f)));
        else if (PlayerManager.Instance.currentRoomNo == 5)
            StartCoroutine(ExitText(5,4.6f, ExitTextEnd(6, duration - 4.6f)));
        else
            print("Some 퇴장 텍스트를 보여줄 수 없음");
    }

}
