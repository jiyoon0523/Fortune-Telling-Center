using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OKButton : MonoBehaviour
{
    public Text inputFieldText;
    Button button;
    
    Color32 disabledText;
    Color32 enabledText;

    // Start is called before the first frame update
    void Start()
    {
        button = this.GetComponent<Button>();
        button.gameObject.SetActive(true);
        button.interactable=false;
        disabledText = new Color32(90, 60, 224, 255);
        enabledText = new Color32(255, 255, 255, 255);
    }

    // Update is called once per frame
    void Update()
    {
            if(inputFieldText.text.Length>=1)
            {
                button.interactable=true;
            }
            else
            {
                button.interactable=false;
            }
    }

    public void SaveName()
    {
        GameManager.Instance.userName = inputFieldText.text;
    }

    public void SaveConcern()
    {
        GameManager.Instance.concern = inputFieldText.text;
    }

}
