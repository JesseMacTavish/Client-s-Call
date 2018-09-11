using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Socializer : DialogHandler
{
    public List<Action> options = new List<Action>();    

    override public void Option1()
    {
        DecisionTracker.killedHitler = true;
        ClearText();
        _fullText = answersScript[0];

        CloseOptions();
    }

    override public void Option2()
    {
        DecisionTracker.talkedHitler = true;
        ClearText();
        _fullText = answersScript[1];

        CloseOptions();
    }

    override public void Option3()
    {
        DecisionTracker.leftHitler = true;
        ClearText();
        _fullText = answersScript[2];

        CloseOptions();
    }
}
