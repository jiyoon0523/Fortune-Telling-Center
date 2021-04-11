using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class IntroManager : IntroNarration, GazeInterface
{
    GameObject canvas;
    Text narrationText;
    GameObject joystickNButton;
    
    GameObject lower;
    Text lowerText;
    public Animator doorAnimator;  
    public List<AudioClip> introAudio = new List<AudioClip>();
    AudioSource audioSource;

    IntroData intro;

    [SerializeField]
    enum PlayerState
    {
        cantMove, canMove, nameDone, issueDone
    }
    PlayerState state;

    [SerializeField]
    float duration;
    [SerializeField]
    float audioTime;

    [SerializeField] UnityEvent joystickOn;
    [SerializeField] UnityEvent guideOff;
    [SerializeField] UnityEvent infoDesk;
    [SerializeField] UnityEvent nameOn;
    [SerializeField] UnityEvent nameDone;
    [SerializeField] UnityEvent concernOn;
    [SerializeField] UnityEvent concernDone;

    string clockText;

    float currentTime;

    GameObject viewGuide;
    GameObject nameField;
    GameObject concernField;

    bool isArrive;
    bool isGreetingDone;
    bool infoMayStart;

    public AudioSource doorOpenSound;

    public float readTime = 10f;

    void Start()
    {
        intro = base.intro;
        narrationText= base.narrationText;
        narrationText.text = null;

        doorAnimator.SetBool("isOpen",false);
        
        state = PlayerState.cantMove;
        canvas = GameObject.Find("Final Fortune Canvas");
        joystickNButton = canvas.transform.GetChild(0).gameObject;
        joystickNButton.SetActive(true);

        audioSource = GetComponent<AudioSource>();
        viewGuide = canvas.transform.GetChild(1).gameObject;
        nameField = canvas.transform.GetChild(8).gameObject;
        concernField = canvas.transform.GetChild(9).gameObject;
        lower = canvas.transform.GetChild(10).gameObject;
        lowerText= lower.transform.GetChild(0).GetComponent<Text>();
        lowerText.text = null;

        GameManager.Instance.isExitNarrationEnd = false;
        SetClip(0);
        audioSource.Play();
        audioTime = audioSource.time;
        StartCoroutine(GreetingDelay());
    }

    void Update()
    {
        audioTime = audioSource.time;
        currentTime += Time.deltaTime;
        if (isGreetingDone == false)
            Greeting();
        if (audioSource.isPlaying == false)
            narrationText.text = null;
    }

    public void GazeRespond(RaycastHit hit)
    {
        if (hit.transform.tag == "InfoDesk")
        {
            if (isArrive == false&& infoMayStart==true)
            {
                isArrive = true;
                infoDesk.Invoke();
            }
        }
        else
        {
            CanvasManager.Instance.MagActive();
            if (CanvasManager.Instance.magClicked == true)
            {
                StartCoroutine(CompassCaption());
            }
        }
    }
    public IEnumerator CompassCaption()
    {
        currentTime = 0;
        Debug.Log("CompassCaption");
        CanvasManager.Instance.magClicked = false;

        if (lowerText == null) Debug.Log("Null lowerText");
        lower.SetActive(true);
        lowerText.text = clockText;

        yield return new WaitUntil(() => currentTime >= readTime);

        if (lower.activeSelf == true)
        {
            lower.SetActive(false);
            lowerText.text = null;
            CanvasManager.Instance.isMagFading = false;
            CanvasManager.Instance.magClicked = false;
        }
        else
            print("Already Off");
    }
    void SetClip(int i)
    {
        audioSource.clip = introAudio[i];
        duration = audioSource.clip.length;
    }
    private IEnumerator GreetingDelay()
    {
        yield return new WaitForSeconds(duration);
        state = PlayerState.canMove;
        guideOff.Invoke();
    }
    private void InfoDesk()
    {
        narrationText.gameObject.SetActive(true);
        IntroText(8, 2, IntroText(9, 4.5f, InfoDeskDelayP3()));
    }
    private IEnumerator InfoDeskDelayP3()
    {
        narrationText.text = intro.NarrationList[10];
        yield return new WaitForSeconds(duration - 6.5f);
        nameOn.Invoke();
        narrationText.text = null;
        if (canvas.GetComponent<CanvasManager>().isIntroBtnClicked == false)
        {
            StartCoroutine(NameClickDelay());
        }
    }
    private IEnumerator NameClickDelay()
    {
        yield return new WaitUntil(() => canvas.GetComponent<CanvasManager>().isIntroBtnClicked == true);
        nameDone.Invoke();
    }
    private void Concern()
    {
        IntroText(11, 8.5f, ConcernDelayP2());
    }
    private IEnumerator ConcernDelayP2()
    {
        narrationText.text = intro.NarrationList[12];
        yield return new WaitForSeconds(duration - 8.5f);
        concernOn.Invoke();
        narrationText.text = null;
        if (canvas.GetComponent<CanvasManager>().isIntroBtnClicked == true)
        {
            StartCoroutine(ConcernClickDelay());
        }
    }
    private IEnumerator ConcernClickDelay()
    {
        yield return new WaitUntil(() => canvas.GetComponent<CanvasManager>().isIntroBtnClicked == false);
        concernDone.Invoke();
    }
    private IEnumerator LastLines()
    {
        narrationText.text = intro.NarrationList[13];
        joystickNButton.SetActive(true);
        joystickNButton.GetComponent<JoysticknButtonController>().ShowJoystick();
        yield return new WaitForSeconds(duration);
        SetClip(4);
        audioSource.Play();
        doorAnimator.SetBool("isOpen",true);
        doorOpenSound.Play();
        doorOpenSound.loop=false;
        StartCoroutine(IntroText(14, 7.5f, IntroText(15, 6.4f, IntroText(16, 4.8f, IntroText(17, 7.5f, IntroText(18, 6.2f, IntroText(19, 2.9f, IntroTextEnd(20, duration - 35.3f))))))));
    }
    public void GuideOff()
    {
        infoMayStart = true;
    }
    public void JoystickOn()
    {
        joystickNButton.GetComponent<JoysticknButtonController>().ShowJoystick();
        isGreetingDone = true;
    }
    private void Greeting()
    {
        if (0 < audioTime && audioTime < 3)
        {
            narrationText.text = intro.NarrationList[0]; 
            viewGuide.gameObject.SetActive(false);
            joystickNButton.GetComponent<JoysticknButtonController>().DontShowAll();
        }
        else if (audioTime < 6)
        {
            narrationText.text = intro.NarrationList[1]; 
            viewGuide.gameObject.SetActive(true);
        } 
        else if (audioTime < 8.1f)
        {
            narrationText.text = intro.NarrationList[1];
        }
        else if (audioTime < 12)
        {
            narrationText.text = intro.NarrationList[2];
        }
        else if (audioTime < 18.3f){
            narrationText.text = intro.NarrationList[3];
            viewGuide.gameObject.SetActive(false);
        }
        else if (audioTime < 22.4f){
            narrationText.text = intro.NarrationList[4];
            joystickNButton.GetComponent<JoysticknButtonController>().ShowJumpButton();
        }
        else if (audioTime < 29.2f)
        {
            narrationText.text = intro.NarrationList[5];
        }
        else if (audioTime < 38.7f)
        {
            narrationText.text = intro.NarrationList[6];
            joystickNButton.GetComponent<JoysticknButtonController>().ShowObserveButton();
        }
        else if (audioTime < duration)
            narrationText.text = intro.NarrationList[7];
        else
            narrationText.text = null;
            if (audioTime >= 39)
                joystickOn.Invoke();
    }

    public void InfoDeskArrive()
    {
        infoMayStart = false;
        SetClip(1);
        audioSource.Play();
        InfoDesk();
    }
    public void NameOn()
    {
        nameField.SetActive(true);
    }
    public void NameDone()
    {
        nameField.SetActive(false);
        SetClip(2);
        audioSource.Play();
        Concern();
    }
    public void ConcernOn()
    {
        concernField.SetActive(true);
    }
    public void ConcernDone()
    {
        concernField.SetActive(false);
        SetClip(3);
        audioSource.Play();
        StartCoroutine(LastLines());
    }
}
