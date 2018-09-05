using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonParser : MonoBehaviour
{
    public static string _path;
    public static string jsonString;

    private void Awake()
    {
        _path = Application.streamingAssetsPath + "/Character.json";
        jsonString = File.ReadAllText(_path);
        JsonClass jason = JsonClass.Instance;
    }
}

[System.Serializable]
public class JsonClass
{
    public string[] Smalltalk;

    public static JsonClass Instance
    {
        get
        {
            return JsonUtility.FromJson<JsonClass>(JsonParser.jsonString);
        }
    }
}