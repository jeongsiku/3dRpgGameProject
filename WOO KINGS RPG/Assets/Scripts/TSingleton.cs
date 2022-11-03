using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// where T : TSingleton<T>
// TSingleton을 상속받은 클래스만 사용될 수 있다.
public class TSingleton<T> : MonoBehaviour where T : TSingleton<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject newObject = new GameObject(typeof(T).Name, typeof(T));
                instance = newObject.GetComponent<T>();

                DontDestroyOnLoad(newObject);
            }
            return instance;
        }
    }

    public virtual void Init()
    {

    }
}
