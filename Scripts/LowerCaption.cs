using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LowerCaption : MonoBehaviour
{
    public static LowerCaption instance;

    GameObject canvas;
    GameObject lower;
    [SerializeField]
    Text lowerText;

    IntroManager im;
    MainManager mm;
    OutroManager om;

    public delegate void ShowDelegate(GameObject go);
    ShowDelegate ShowCaption;
    public delegate void HideDelegate();
    HideDelegate HideCaption;

    public float readTime = 10f;
    private Fade fade;

    Stage stage;

    float currentTime;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Start is called before the first frame update
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        canvas = GameObject.Find("Final Fortune Canvas");
        lower = canvas.transform.GetChild(10).gameObject;
        lowerText = lower.transform.GetChild(0).GetComponent<Text>();
        lowerText.text = null;
        lower.SetActive(false);
        stage = GameManager.Instance.stage;
        switch (stage)
        {
            case Stage.Intro:
                im = GameObject.Find("IntroManager").GetComponent<IntroManager>();
                break;
            case Stage.Main:
                mm = GameObject.Find("MainManager").GetComponent<MainManager>();
                break;
            case Stage.Outro:
                om = GameObject.Find("OutroManager").GetComponent<OutroManager>();
                break;
        }
        print("LowerCaption이 받은 stage: " + stage);

        if (GameObject.FindWithTag("Fade") != null)
        {
            fade = GameObject.FindWithTag("Fade").GetComponent<Fade>();
        }

    }
    private void Update()
    {
        currentTime += Time.deltaTime;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void ArtCaptionOn(GameObject go)
    {
        if (lowerText == null) Debug.Log("Null lowerText");
        lower.SetActive(true);
        lowerText.text = mm.currentMain.artPair[go];
    }

    public void ArtCaptionOff()
    {
        lower.SetActive(false);
        lowerText.text = null;
    }

    public IEnumerator ReadArtCaption(GameObject go)
    {
        currentTime = 0;
        Debug.Log("ReadArtCaption in " + go.gameObject.name);
        CanvasManager.Instance.magClicked = false;
        ArtCaptionOn(go);
        if (!UpperCaption.instance.viewedArt.Contains(go))
        {
            GameManager.Instance.isExitNarrationEnd = false;
            print("isExitNarrationEnd" + GameManager.Instance.isExitNarrationEnd);
            UpperCaption.instance.viewedArt.Add(go);
        }
        yield return new WaitUntil(() => currentTime >= readTime);
        if (lower.activeSelf == true)
        {
            ArtCaptionOff();
            CanvasManager.Instance.isMagFading = false;
        }
        else
            print("Already Off");
    }
}
