using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SantaCharacter : MonoBehaviour
{
    public GameObject dialogBox;
    public GameObject optionsBox;
    public GameObject sliders;
    public int ammountOfOptions;
    public bool timedDecision;
    public float givenTime;
    float _timeOnDecision;
    Text _dialog;
    Text _name;
    int _dialogBoxID = 1;
    bool _optionsOn;
    bool _questionAsked;

    List<Action> _optionsAction = new List<Action>();
 
    //Variables for slow type
    public float typeDelay = 0.1f;
    float _timeType;
    string _fullText = "";
    string[] _fullTextOptions;
    string _typedCharacters = "";

    void Start()
    {
        _dialog = dialogBox.GetComponentInChildren<Text>();
        _name = dialogBox.transform.Find("Name").GetComponent<Text>();
        _name.text = JsonClass.Instance.Smalltalk[0];
        _fullText = JsonClass.Instance.Smalltalk[1];

        _optionsAction.Add(Option1);
        _optionsAction.Add(Option2);
        _optionsAction.Add(Option3);
    }

    void Update()
    {
        Type();

        if (Input.GetButtonDown("Fire1") && !_optionsOn)
        {
            Continue();
        }

        if (_optionsOn && timedDecision)
        {
            CountDown();
        }
    }

    void Type()
    {
        _timeType += Time.deltaTime;
        while (_timeType >= typeDelay && _typedCharacters.Length < _fullText.Length)
        {
            _typedCharacters += _fullText[_typedCharacters.Length];
            _timeType = 0;
        }
        _dialog.text = _typedCharacters;

        if (_typedCharacters.Length == _fullText.Length && _questionAsked && !_optionsOn)
        {
            ShowOptions();
        }
    }

    void Continue()
    {
        if (_typedCharacters.Length < _fullText.Length)
        {
            _typedCharacters = _fullText;
            return;
        }

        ClearText();

        if (_dialogBoxID < JsonClass.Instance.Smalltalk.Length - 1)
        {
            _dialogBoxID++;
        }
        else
        {
            dialogBox.SetActive(false);
            return;
        }

        if (JsonClass.Instance.Smalltalk[_dialogBoxID].Contains("|"))
        {
            _fullTextOptions = JsonClass.Instance.Smalltalk[_dialogBoxID].Split('|');
            _fullText = _fullTextOptions[0];
            _questionAsked = true;
        }
        else
        {
            _fullText = JsonClass.Instance.Smalltalk[_dialogBoxID];
        }
    }

    void ShowOptions()
    {
        for (int i = 0; i < ammountOfOptions; i++)
        {
            GameObject option = optionsBox.transform.GetChild(i).gameObject;
            option.SetActive(true);
            option.GetComponentInChildren<Text>().text = _fullTextOptions[i + 1];
            int x = i;
            option.GetComponent<Button>().onClick.AddListener(() => _optionsAction[x]());
        }//Need to select one of the options so u can interact with them with arrows
        optionsBox.transform.GetChild(0).GetComponent<Button>().Select();

        optionsBox.SetActive(true);
        sliders.SetActive(true);

        _optionsOn = true;
    }

    void CloseOptions()
    {
        for (int i = 0; i < ammountOfOptions; i++)
        {
            GameObject option = optionsBox.transform.GetChild(i).gameObject;
            option.SetActive(false);
        }

        _optionsOn = false;
        _questionAsked = false;
        optionsBox.SetActive(false);
        sliders.SetActive(false);
    }

    void ClearText()
    {
        _dialog.text = "";
        _typedCharacters = "";
    }

    void CountDown()
    {
        _timeOnDecision += Time.deltaTime;

        sliders.transform.Find("Slider 1").GetComponent<Slider>().value = 1f - _timeOnDecision / givenTime;
        sliders.transform.Find("Slider 2").GetComponent<Slider>().value = 1f - _timeOnDecision / givenTime;
        if (_timeOnDecision > givenTime)
        {
            _optionsAction[UnityEngine.Random.Range(0, _optionsAction.Count)]();
            _timeOnDecision = 0;
        }
    }

    public void Option1()
    {
        DecisionTracker.acceptedSanta = true;
        ClearText();
        _fullText = JsonClass.Instance.SmalltalkAnswer1[0];

        CloseOptions();
    }

    public void Option2()
    {
        DecisionTracker.declineSanta = true;
        ClearText();
        _fullText = JsonClass.Instance.SmalltalkAnswer2[0];

        CloseOptions();
    }

    public void Option3()
    {
        ClearText();
        _fullText = JsonClass.Instance.SmalltalkAnswer3[0];

        CloseOptions();
    }
}
