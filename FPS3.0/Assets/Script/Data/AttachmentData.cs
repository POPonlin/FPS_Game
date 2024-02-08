using FPS3_GameBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FPS3.0/Attachment Data")]
public class AttachmentData : ItemData
{
    public enum AttachmentType
    {
        AT_Scope,
        AT_Silence,
    }

    public AttachmentType subType;
    public int AttachPos;
    public float AttachArgs;

}
