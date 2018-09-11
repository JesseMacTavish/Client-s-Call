﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonParser : MonoBehaviour
{
    [Tooltip("The name of the .json file")]
    public string FileName = "Character.json";

    private string _path;
    public static string JsonString;

    private void Awake()
    {
        _path = Application.streamingAssetsPath + "/" + FileName;
        JsonString = File.ReadAllText(_path);
        JsonClass jason = JsonClass.Instance;
    }
}

[System.Serializable]
public class JsonClass
{
    public string[] Dialog;
    public string[] DialogAnswer1;
    public string[] DialogAnswer2;
    public string[] DialogAnswer3;

    public static JsonClass Instance
    {
        get
        {
            return JsonUtility.FromJson<JsonClass>(JsonParser.JsonString);
        }
    }
}
