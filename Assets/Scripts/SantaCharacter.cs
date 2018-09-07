using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SantaCharacter : MonoBehaviour
{
    //TODO: jesse get back to work!!!

    public GameObject dialogBox;
    public GameObject optionsBox;
    Text dialog;
    Text name;
    int dialogBoxID = 1;
    bool optionsOn;

    //Variables for slow type
    public float typeDelay = 0.1f;
    float time;
    string fullText = "";
    string[] fullTextOptions;
    string typedCharacters = "";

    void Start()
    {
        dialog = dialogBox.GetComponentInChildren<Text>();
        name = dialogBox.transform.Find("Name").GetComponent<Text>();
        name.text = JsonClass.Instance.Smalltalk[0];
        fullText = JsonClass.Instance.Smalltalk[1];                                   
    }

    void Update()
    {
        Type();

        if (Input.GetButtonUp("Fire1"))
        {
            Continue();
        }
    }

    void Type()
    {
        time += Time.deltaTime;
        while (time >= typeDelay && typedCharacters.Length < fullText.Length)
        {
            typedCharacters += fullText[typedCharacters.Length];
            time = 0;
        }
        dialog.text = typedCharacters;

        if (typedCharacters.Length == fullText.Length && optionsOn)
        {
            optionsBox.SetActive(true);
        }
    }

    void Continue()
    {
        if (typedCharacters.Length < fullText.Length)
        {
            typedCharacters = fullText;
            return;
        }

        ClearText();

        if (dialogBoxID < JsonClass.Instance.Smalltalk.Length - 1)
        {
            dialogBoxID++;
        }
        else
        {
            dialogBox.SetActive(false);
            return;
        }

        if (JsonClass.Instance.Smalltalk[dialogBoxID].Contains("|"))
        {
            fullTextOptions = JsonClass.Instance.Smalltalk[dialogBoxID].Split('|');
            fullText = fullTextOptions[0];
            ShowOptions();
        }
        else
        {
            fullText = JsonClass.Instance.Smalltalk[dialogBoxID];
        }
    }

    void ShowOptions()
    {
        optionsOn = true;
        GameObject option1 = optionsBox.transform.Find("Option 1").gameObject;
        GameObject option2 = optionsBox.transform.Find("Option 2").gameObject;
        option1.GetComponentInChildren<Text>().text = fullTextOptions[1];
        option2.GetComponentInChildren<Text>().text = fullTextOptions[2];
        option1.GetComponent<Button>().onClick.AddListener(() => Option1());
        option2.GetComponent<Button>().onClick.AddListener(() => Option2());
        option1.GetComponent<Button>().Select();
    }

    void CloseOptions()
    {
        optionsOn = false;
        optionsBox.SetActive(false);
    }

    void ClearText()
    {
        dialog.text = "";
        typedCharacters = "";
    }

    public void Option1()
    {
        DecisionTracker.acceptedSanta = true;
        ClearText();
        fullText = JsonClass.Instance.SmalltalkAnswer1[0];

        CloseOptions();
    }

    public void Option2()
    {
        DecisionTracker.declineSanta = true;
        ClearText();
        fullText = JsonClass.Instance.SmalltalkAnswer2[0];

        CloseOptions();
    }
}
