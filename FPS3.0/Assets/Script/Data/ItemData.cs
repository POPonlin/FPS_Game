using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS3_GameBase
{
    public class ItemData : ScriptableObject
    {
        public enum ItemType
        {
            [Tooltip("枪械类")]
            IT_Gun,
            [Tooltip("投掷类")]
            IT_Throw,
            [Header("配件类")]
            IT_Accessories,
            [Header("道具类")]
            IT_Prop,
        }

        public string itemName;
        public int itemID;
        public GameObject itemPrefeb;
        public ItemType itemType;
        public Sprite itemSprite;
    }
}
