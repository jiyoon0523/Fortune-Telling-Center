using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitAll : ExitNarration
{

    void Start()
    {
        upperText = base.upperText;
        mm = base.mm;
        duration = base.duration;
    }

    public override void ShowExit(int i)
    {
        if (PlayerManager.Instance.currentRoomNo == 0)
            StartCoroutine(ExitText(7, 2.5f, ExitText(8, 8f, ExitTextEnd(9, duration-10.5f))));
        else if (PlayerManager.Instance.currentRoomNo == 1)
            StartCoroutine(ExitText(7, 8.1f, ExitTextEnd(8, duration - 8.1f)));
        else if (PlayerManager.Instance.currentRoomNo == 2)
            StartCoroutine(ExitText(8, 5f, ExitTextEnd(9, duration - 5f)));
        else if (PlayerManager.Instance.currentRoomNo == 3)
            StartCoroutine(ExitText(7, 3.5f, ExitTextEnd(8, duration - 3.5f)));
        else if (PlayerManager.Instance.currentRoomNo == 4)
            StartCoroutine(ExitText(7, 3.8f, ExitText(8, 5.2f, ExitTextEnd(9, duration - 9f))));
        else if (PlayerManager.Instance.currentRoomNo == 5)
            StartCoroutine(ExitText(5, 4.6f, ExitTextEnd(6, duration - 4.6f)));
        else
            print("All 퇴장 텍스트를 보여줄 수 없음");
    }
}
