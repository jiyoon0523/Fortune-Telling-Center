using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ExitNarration : MonoBehaviour
{
    protected Text upperText;
    protected MainManager mm;
    public float duration;

    ExitNarration en;

    Fade fade;

    void Awake()
    {
        upperText = GameObject.Find("Final Fortune Canvas").transform.GetChild(7).GetComponent<Text>();
        mm = GameObject.Find("MainManager").GetComponent<MainManager>();
    }
    private void Start()
    {
        if (GameObject.FindWithTag("Fade"))
        {
            fade = GameObject.FindWithTag("Fade").GetComponent<Fade>();
        }
    }

    public abstract void ShowExit(int i);

    public IEnumerator ExitText(int narrationIndex, float sec, IEnumerator nextCoroutine)
    {
        if(narrationIndex<mm.currentMain.doorCaption.Count){
            GameManager.Instance.isExitNarrationStart = true;
            upperText.text = mm.currentMain.doorCaption[narrationIndex];
            print(mm.currentMain.doorCaption[narrationIndex]);
            yield return new WaitForSeconds(sec);
            StartCoroutine(nextCoroutine);
        }
    }

    public IEnumerator ExitTextEnd(int narrationIndex, float sec)
    {
        Debug.Log("ExitTextEnd");
        upperText.text = mm.currentMain.doorCaption[narrationIndex];
        print(mm.currentMain.doorCaption[narrationIndex]);
        yield return new WaitForSeconds(sec);
        print("ExitTextEnd-isExitNarrationEnd-" + GameManager.Instance.isExitNarrationEnd);
        GameManager.Instance.isExitNarrationEnd = true;
        print("ExitTextEnd-isExitNarrationEnd-: " + GameManager.Instance.isExitNarrationEnd);
        upperText.text = null;
    }
}
