using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ģ��
/// </summary>
/// <typeparam name="T">��Ҫ�̳е���ģ�������</typeparam>
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
