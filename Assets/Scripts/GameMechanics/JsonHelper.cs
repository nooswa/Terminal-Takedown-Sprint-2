using System;
using UnityEngine;

// Helper for serializing and deserializing JSON arrays with Unity's JsonUtility.
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    public static string ToJson<T>(T[] array, bool prettyPrint = false)
    {
        Wrapper<T> wrapper = new Wrapper<T> { array = array };
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    public class Wrapper<T>
    {
        public T[] array;
    }
}