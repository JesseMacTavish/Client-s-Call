using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SantaCharacter : MonoBehaviour
{
    public GameObject dialogBox;
    Text text;

    void Start()
    {
        text = dialogBox.GetComponentInChildren<Text>();
        text.text = JsonClass.Instance.Greetings;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            text.text = JsonClass.Instance.Followup;

        }
    }
}
