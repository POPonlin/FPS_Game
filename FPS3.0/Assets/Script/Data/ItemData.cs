using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS3_GameBase
{
    public class ItemData : ScriptableObject
    {
        public enum ItemType
        {
            [Tooltip("ǹе��")]
            IT_Gun,
            [Tooltip("Ͷ����")]
            IT_Throw,
            [Header("�����")]
            IT_Accessories,
            [Header("������")]
            IT_Prop,
        }

        public string itemName;
        public int itemID;
        public GameObject itemPrefeb;
        public ItemType itemType;
        public Sprite itemSprite;
    }
}
