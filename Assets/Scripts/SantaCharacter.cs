using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SantaCharacter : MonoBehaviour
{
    //TODO: jesse get back to work!!!
    //TODO: no u
    //TODO: i do beat-them-up, u novel. so no u, get back to work!

    public GameObject dialogBox;
    public GameObject optionsBox;
    public int ammountOfOptions;
    List<Action> optionsAction;
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

        optionsAction = new List<Action>();
        optionsAction.Insert(0, Option1);
        optionsAction.Insert(1, Option2);
        optionsAction.Insert(2, Option3);
    }

    void Update()
    {
        Type();

        if (Input.GetButtonUp("Fire1") && !optionsOn)
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
        for (int i = 0; i < ammountOfOptions; i++)
        {
            GameObject option = optionsBox.transform.GetChild(i).gameObject;
            option.SetActive(true);
            option.GetComponentInChildren<Text>().text = fullTextOptions[i];
            option.GetComponent<Button>().Select();
            //So here very intersting thing happens. If u think this is retarded, I dare u to try it
            int x = i;
            option.GetComponent<Button>().onClick.AddListener(() => optionsAction[x]());
        }

        optionsOn = true;
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

    public void Option3()
    {
        ClearText();
        fullText = JsonClass.Instance.SmalltalkAnswer3[0];

        CloseOptions();
    }
}
