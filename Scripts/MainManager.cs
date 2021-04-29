using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;


public class MainManager : MonoBehaviour, GazeInterface
{
    Language language;

    DataManager dm;
    public MainData currentMain;

    Dictionary<int, List<GameObject>> Room_ArtList = new Dictionary<int, List<GameObject>>();
    public List<GameObject> Main1Art = new List<GameObject>();
    public List<GameObject> Main2Art = new List<GameObject>();
    public List<GameObject> Main3Art = new List<GameObject>();
    public List<GameObject> Main4Art = new List<GameObject>();
    public List<GameObject> Main5Art = new List<GameObject>();
    public List<GameObject> Main6Art = new List<GameObject>();

    Dictionary<int, List<AudioClip>> Room_AudioList = new Dictionary<int, List<AudioClip>>();
    public List<AudioClip> Main1Audio = new List<AudioClip>();
    public List<AudioClip> Main2Audio = new List<AudioClip>();
    public List<AudioClip> Main3Audio = new List<AudioClip>();
    public List<AudioClip> Main4Audio = new List<AudioClip>();
    public List<AudioClip> Main5Audio = new List<AudioClip>();
    public List<AudioClip> Main6Audio = new List<AudioClip>();

    Fade fade;

    public bool isEnterPlay;
    void Start()
    {
        dm = CSVReader.CSVtoDataManager();

        language = GameManager.Instance.language;

        fade = GameObject.FindWithTag("Fade").GetComponent<Fade>();
        if (fade)
        {
            fade.fadeInCompleted += CheckCurrent;
            fade.fadeInCompleted += EnterPlay;
        }
        FortuneManager.Instance.isCharacterDone = false;

        FillArts();
        FillAudio();
        CheckLanguage();
        CheckCurrent();
    }

    public void EnterPlay()
    {
        if (isEnterPlay == true)
        {
            isEnterPlay = false;
        }
    }

    void FillArts()
    {
        Room_ArtList.Add(1, Main1Art);
        Room_ArtList.Add(2, Main2Art);
        Room_ArtList.Add(3, Main3Art);
        Room_ArtList.Add(4, Main4Art);
        Room_ArtList.Add(5, Main5Art);
        Room_ArtList.Add(6, Main6Art);

        foreach (MainData main in dm.mainArray)
        {
            for (int i = 1; i <= 6; i++)
            {
                if (main.roomNo == i)
                {
                    main.artPieces = Room_ArtList[i];
                    main.artPair_Kor = new Dictionary<GameObject, string>();
                    main.artPair_Eng = new Dictionary<GameObject, string>();
                    Debug.Log("artCount: "+main.artCount);
                    Debug.Log("mainart Piece: "+main.artPieces.Count);
                    for (int j = 0; j < main.artCount; j++)
                    {
                        main.artPair_Kor.Add(main.artPieces[j], main.artCaption_Kor[j]);
                        main.artPair_Eng.Add(main.artPieces[j], main.artCaption_Eng[j]);
                    }
                }
            }
        }
    }

    void FillAudio()
    {
        Debug.Log("FillAudio");
        Room_AudioList.Add(1, Main1Audio);
        Room_AudioList.Add(2, Main2Audio);
        Room_AudioList.Add(3, Main3Audio);
        Room_AudioList.Add(4, Main4Audio);
        Room_AudioList.Add(5, Main5Audio);
        Room_AudioList.Add(6, Main6Audio);

        foreach (MainData main in dm.mainArray)
        {
            for (int i = 1; i <= 6; i++)
            {
                if (main.roomNo == i)
                {
                    main.doorAudio = Room_AudioList[i];
                }
            }
        }
    }


    
    public void CheckLanguage()
    {
        if ((int)language == 0) 
        {
            foreach (MainData main in dm.mainArray)
            {
                main.artCaption = main.artCaption_Kor;
                main.artPair = main.artPair_Kor;
                main.doorCaption = main.doorCaption_Kor;
            }
        }
        else 
        {
            foreach (MainData main in dm.mainArray)
            {
                main.artCaption = main.artCaption_Eng;
                main.artPair = main.artPair_Eng;
                main.doorCaption = main.doorCaption_Eng;
            }
        }
    }

    public void CheckCurrent()
    {

        foreach (MainData main in dm.mainArray)
        {
            if (main.roomNo == PlayerManager.Instance.currentRoomNo + 1)
            {
                currentMain = main;
            }
        }
    }

    public void GazeRespond(RaycastHit hit)
    {
        if (CanvasManager.Instance.below.activeSelf == false)
        {
            if (currentMain.artPieces.Contains(hit.transform.gameObject))
            {
                CanvasManager.Instance.MagActive();
                if (CanvasManager.Instance.magClicked)
                {
                    StartCoroutine(LowerCaption.instance.ReadArtCaption(hit.transform.gameObject));
                }
            }
        }
    }

}
