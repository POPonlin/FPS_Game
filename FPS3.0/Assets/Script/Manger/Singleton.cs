using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例模板
/// </summary>
/// <typeparam name="T">将要继承单例模板的类型</typeparam>
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = (T)this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(instance);
        }
    }

    public static T GetInstance()
    {
        return instance;
    }

}
