using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterNarration : MonoBehaviour
{
    Text narrationText;
    MainManager mm;
    public float duration;

    private void Start()
    {
        narrationText= GameObject.Find("Final Fortune Canvas").transform.GetChild(7).GetComponent<Text>();
        narrationText.text = null;
        mm = GameObject.Find("MainManager").GetComponent<MainManager>();
    }

    public IEnumerator EnterText(int narrationIndex, float sec, IEnumerator nextCoroutine)
    {
        if (GameManager.Instance.isExitNarrationStart == false)
        {

            narrationText.text = mm.currentMain.doorCaption[narrationIndex];
            yield return new WaitForSeconds(sec);
            StartCoroutine(nextCoroutine);
        }
        else
            print("Enter interrupted by exit");
    }
    public IEnumerator EnterTextEnd(int narrationIndex, float sec)
    {
        if (GameManager.Instance.isExitNarrationStart == false)
        {
            narrationText.text = mm.currentMain.doorCaption[narrationIndex];
            yield return new WaitForSeconds(sec);
            narrationText.text = null;
        }
        else
            print("Enter interrupted by exit");
    }
    public void Enter(int n)
    {
        GameManager.Instance.isExitNarrationStart = false;
        GameManager.Instance.isExitNarrationEnd = false;

        if (n== 0)
            StartCoroutine(EnterText(0, 4f, EnterText(1, 4.9f, EnterTextEnd(2, duration - 8.9f))));
        else if (n == 1)
            StartCoroutine(EnterText(0, 4f, EnterText(1, 8.6f, EnterTextEnd(2, 5.3f)))); 
        else if (n == 2)
            StartCoroutine(EnterText(0, 3.7f, EnterText(1, 8.5f, EnterTextEnd(2, duration - 12.2f))));
        else if (n == 3)
            StartCoroutine(EnterText(0, 3.4f, EnterText(1, 4.5f, EnterTextEnd(2, duration - 7.9f))));
        else if (n == 4)
            StartCoroutine(EnterText(0, 7.2f, EnterText(1, 3.2f, EnterTextEnd(2, duration - 10.4f))));
        else if (n == 5)
            StartCoroutine(EnterText(0, 3.92f, EnterTextEnd(1, duration - 3.92f)));
        else
            print("입장 텍스트를 보여줄 수 없음");

    }
}
