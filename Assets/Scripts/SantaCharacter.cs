using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SantaCharacter : MonoBehaviour
{
    public GameObject dialogBox;
    public GameObject optionsBox;
    Text dialog;
    int dialogBoxID;
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
        fullText = JsonClass.Instance.Smalltalk[0];
    }

    void Update()
    {
        Type();

        if (Input.GetButtonDown("Submit"))
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

        if (optionsOn)
        {
            optionsOn = false;
            optionsBox.SetActive(false);
        }

        dialog.text = "";
        typedCharacters = "";

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
        optionsBox.transform.Find("Option 1").GetComponentInChildren<Text>().text = fullTextOptions[1];
        optionsBox.GetComponentInChildren<Button>().Select();
        optionsBox.transform.Find("Option 2").GetComponentInChildren<Text>().text = fullTextOptions[2];
    }

    //public void Option1()
    //{
    //    Debug.Log("Yo, I am working");
    //    //set all the booleans

    //    DecisionTracker.achiever += JsonClass.Instance.SantaPoints[0];
    //    DecisionTracker.explorer += JsonClass.Instance.SantaPoints[1];
    //    DecisionTracker.socializer += JsonClass.Instance.SantaPoints[2];
    //    DecisionTracker.killer += JsonClass.Instance.SantaPoints[3];
    //}

    //public void Option2()
    //{
    //    Debug.Log("Yo, I am working");

    //    //set all the booleans

    //    DecisionTracker.achiever += JsonClass.Instance.SantaPoints[4];
    //    DecisionTracker.explorer += JsonClass.Instance.SantaPoints[5];
    //    DecisionTracker.socializer += JsonClass.Instance.SantaPoints[6];
    //    DecisionTracker.killer += JsonClass.Instance.SantaPoints[7];
    //}

    
}
