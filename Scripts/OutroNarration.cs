using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutroNarration : MonoBehaviour
{
    protected Text narrationText;
    protected OutroData outro;

    void Awake()
    {
        narrationText = GameObject.Find("Final Fortune Canvas").transform.GetChild(7).GetComponent<Text>();
        outro = CSVReader.CSVtoDataManager().outro;
    }

    public IEnumerator OutroText(int narrationIndex, float sec, IEnumerator nextCoroutine)
    {
        narrationText.text = outro.NarrationList[narrationIndex];
        yield return new WaitForSeconds(sec);
        StartCoroutine(nextCoroutine);
    }

    public IEnumerator OutroTextEnd(int narrationIndex, float sec)
    {
        narrationText.text = outro.NarrationList[narrationIndex];
        yield return new WaitForSeconds(sec);
        narrationText.text = null;
    }
}
