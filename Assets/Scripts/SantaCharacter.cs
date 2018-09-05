using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SantaCharacter : MonoBehaviour
{
    public GameObject dialogBox;
    Text dialog;
    int dialogBoxID;

    //Variables for slow type
    public float typeDelay = 0.1f;
    float time;
    string fullText;
    string typedCharacters = "";

    void Start()
    {
        dialog = dialogBox.GetComponentInChildren<Text>();
        fullText = JsonClass.Instance.Smalltalk[0];
    }

    void Update()
    {
        Type();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextDialogBox();
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
    }

    void NextDialogBox()
    {
        dialog.text = "";
        typedCharacters = "";

        if (dialogBoxID < JsonClass.Instance.Smalltalk.Length - 1)
        {
            dialogBoxID++;
        }

        fullText = JsonClass.Instance.Smalltalk[dialogBoxID];
    }
}
