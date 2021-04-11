using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitNone : ExitNarration
{
    void Start()
    {
        upperText = base.upperText;
        mm = base.mm;
        duration = base.duration;
    }

    public override void ShowExit(int i)
    {
        Debug.Log("ExitNone");
            if (PlayerManager.Instance.currentRoomNo == 0)
                StartCoroutine(ExitText(3, 5f, ExitTextEnd(4, duration - 5f)));
            else if (PlayerManager.Instance.currentRoomNo == 1)
                StartCoroutine(ExitText(3, 6f, ExitTextEnd(4, duration - 6f)));
            else if (PlayerManager.Instance.currentRoomNo == 2)
                StartCoroutine(ExitText(3, 4f, ExitText(4, 4.7f, ExitTextEnd(5, duration - 8.7f))));
            else if (PlayerManager.Instance.currentRoomNo == 3)
                StartCoroutine(ExitText(3, 3.8f, ExitTextEnd(4, duration - 3.8f)));
            else if (PlayerManager.Instance.currentRoomNo == 4)
                StartCoroutine(ExitText(3, 3.3f, ExitTextEnd(4, duration - 3.3f)));
            else if (PlayerManager.Instance.currentRoomNo == 5)
                StartCoroutine(ExitText(2, 2.4f, ExitText(3, 3.4f, ExitTextEnd(4, duration - 5.8f))));
            else
                print("None 퇴장 텍스트를 보여줄 수 없음");
    }
}


