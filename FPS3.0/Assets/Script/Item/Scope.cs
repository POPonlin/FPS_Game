using FPS3_GameBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour
{
    public Camera camera1;
    public GameObject go;
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.GetInstance().Register("Aim", OnAim);   
        go.SetActive(false);
        camera1.gameObject.SetActive(true);
    }

    void OnAim(object obj, int param1, int param2)
    {        
        if(camera1 != null)
        {
            camera1.gameObject.SetActive(param1 == 1);
        }
        if(go != null)
        {
            go.SetActive(param1 == 1);
        }
    }


}
