using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroNarration : MonoBehaviour
{
    protected Text narrationText;
    protected IntroData intro;

    string clockText;

    void Awake()
    {
        narrationText = GameObject.Find("Final Fortune Canvas").transform.GetChild(7).GetComponent<Text>();
        intro = intro = CSVReader.CSVtoDataManager().intro;
    }
    void Start()
    {
        if (GameManager.Instance.language == Language.KOREAN)
        {
            intro.NarrationList = intro.Narration_Kor;
            clockText = intro.Narration_Kor[23];
        }
        else
        {
            intro.NarrationList = intro.Narration_Eng;
            clockText = intro.Narration_Eng[21];
        }
    }

    public IEnumerator IntroText(int narrationIndex, float sec, IEnumerator nextCoroutine)
    {
        narrationText.text = intro.NarrationList[narrationIndex];
        yield return new WaitForSeconds(sec);
        StartCoroutine(nextCoroutine);
    }

    public IEnumerator IntroTextEnd(int narrationIndex, float sec)
    {
        narrationText.text = intro.NarrationList[narrationIndex];
        yield return new WaitForSeconds(sec);
        narrationText.text = null;
    }
}
