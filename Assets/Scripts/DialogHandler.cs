using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogHandler : MonoBehaviour
{
    public GameObject dialogBox;
    public GameObject optionsBox;
    public GameObject sliders;
    [Space]
    [Range(2, 3)]
    public int ammountOfOptions;
    public bool timedDecision;
    public float givenTime;
    public bool goToOtherScene;

    [Header("Scenes to load")]
    public UnityEngine.Object sceneToLoad1;
    public UnityEngine.Object sceneToLoad2;
    public UnityEngine.Object sceneToLoad3;

    float _timeOnDecision;
    Text _dialog;
    Text _name;
    string[] _fullTextOptions;
    int _dialogBoxID;
    bool _optionsOn;
    bool _questionAsked;

    [Space]
    [TextArea]
    public List<string> dialogScript = new List<string>();
    [TextArea]
    public List<string> answersScript = new List<string>();
    public List<Action> optionsAction = new List<Action>();

    public SetBool Op1;
    public SetBool Op2;
    public SetBool Op3;

    private bool _op1;
    private bool _op2;
    private bool _op3;

    //Variables for slow type
    public float typeDelay = 0.1f;
    float _timeType;
    protected string _fullText = "";
    string _typedCharacters = "";

    void Start()
    {
        //TODO : Make the first line react to decisions player made
        _dialog = dialogBox.GetComponentInChildren<Text>();
        _name = dialogBox.transform.Find("Name").GetComponent<Text>();
        string[] script = dialogScript[0].Split('|');
        _name.text = script[0];
        _fullText = script[1];

        optionsAction.Add(Option1);
        optionsAction.Add(Option2);
        optionsAction.Add(Option3);
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

        if (_typedCharacters.Length == _fullText.Length && _questionAsked)
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

        if (_dialogBoxID < dialogScript.Count - 1)
        {
            _dialogBoxID++;
        }
        else
        {
            dialogBox.SetActive(false);

            if (goToOtherScene)
            {
                if (_op1)
                {
                    SceneManager.LoadScene(sceneToLoad1.name);
                }
                else if (_op2)
                {
                    SceneManager.LoadScene(sceneToLoad2.name);
                }

                else if (_op3)
                {
                    SceneManager.LoadScene(sceneToLoad3.name);
                }
            }
            return;
        }

        //TODO : Somewhere here should determine what kind of dialog to show (like depending on decision maybe)

        string[] _script = dialogScript[_dialogBoxID].Split('|');
        _name.text = _script[0];

        if (_script[1].Contains("%"))
        {
            _fullTextOptions = _script[1].Split('%');
            _fullText = _fullTextOptions[0];
            _questionAsked = true;
        }
        else
        {
            _fullText = _script[1];
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
            option.GetComponent<Button>().onClick.AddListener(() => optionsAction[x]());
        }//Need to select one of the options so u can interact with them with arrows
        optionsBox.transform.GetChild(0).GetComponent<Button>().Select();

        optionsBox.SetActive(true);
        if (timedDecision)
        {
            sliders.SetActive(true);
        }

        _optionsOn = true;
        _questionAsked = false;
    }

    protected void CloseOptions()
    {
        for (int i = 0; i < ammountOfOptions; i++)
        {
            GameObject option = optionsBox.transform.GetChild(i).gameObject;
            option.SetActive(false);
        }

        _optionsOn = false;
        optionsBox.SetActive(false);
        sliders.SetActive(false);
    }

    protected void ClearText()
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
            optionsAction[UnityEngine.Random.Range(0, optionsAction.Count)]();
            _timeOnDecision = 0;
        }
    }

    public void Option1()
    {
        DecisionTracker.ToggleBool(Op1);
        ClearText();
        _fullText = answersScript[0];

        _op1 = true;
        CloseOptions();
    }

    public void Option2()
    {
        DecisionTracker.ToggleBool(Op2);
        ClearText();
        _fullText = answersScript[1];

        _op2 = true;
        CloseOptions();
    }

    public void Option3()
    {
        DecisionTracker.ToggleBool(Op3);
        ClearText();
        _fullText = answersScript[2];

        _op3 = true;
        CloseOptions();
    }
}
