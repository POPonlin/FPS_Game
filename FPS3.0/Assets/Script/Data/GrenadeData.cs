using FPS3_GameBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FPS3.0/Grenade Data")]
public class GrenadeData : ItemData
{
    public enum GrenadeType
    {
        GT_HandGrenade,     //手雷
        GT_FlashGrenade,    //闪光弹 
        GT_SmkoeGrenade,    //烟雾弹
    }

    public GrenadeType subType;
    public float delayTime = 5f;
    public float explosionRange = 5f;
    public float explosionForce = 500f;
    public float throwForce = 500f;
    public AudioClip throwClip;
}
