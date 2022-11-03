using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class JsonConvert
{
    private class Wrapper<T>
    {
        public T[] items;
    }

    public static T[] ArrayFromJson<T> (string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>> (json);
        return wrapper.items;
    }

    public static T FromJson<T> (string json)
    {
        return JsonUtility.FromJson<T> (json);
    }

    public static string ToJson<T> (T item, bool prettyPrint = true)
    {
        return JsonUtility.ToJson (item, prettyPrint);
    }

    public static string ToJson<T> (T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T> ();
        wrapper.items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T> (T[] array, bool prettyPrint = true)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }
}
