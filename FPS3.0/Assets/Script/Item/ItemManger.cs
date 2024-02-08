using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS3_GameBase
{
    public class ItemManger : MonoBehaviour
    {
        [Header("手持位置")]
        public Transform handPos;
        [Header("装备位置")]
        public Transform aimPos;
        [Header("场景摄像机")]
        public Camera scenesC;
        [Header("装备摄像机")]
        public Camera eyeC;

        private GameObject myGun;
        private List<GameObject> equipAttachList = new List<GameObject>();

        // Start is called before the first frame update
        void Start()
        {
            EventCenter.GetInstance().Register("PickUpItem", EquipItem);
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// 控制装备物品
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        void EquipItem(object obj, int param1, int param2)
        {
            GameObject itemGO = (GameObject)obj;
            ItemArr getItemArr = itemGO.GetComponent<ItemArr>();
            if(getItemArr != null)
            {
                switch (getItemArr.itemArr.itemType)
                {
                    case ItemData.ItemType.IT_Gun: EquipGun((GunData)getItemArr.itemArr); break;
                    case ItemData.ItemType.IT_Throw: EquipThrow((GrenadeData)getItemArr.itemArr); break;
                    case ItemData.ItemType.IT_Prop: EquipProp(); break;
                    case ItemData.ItemType.IT_Accessories: EquipAccessories((AttachmentData)getItemArr.itemArr); break;
                }
            }
        }

        /// <summary>
        /// 装备枪
        /// </summary>
        /// <param name="gd"></param>
        void EquipGun(GunData gd)
        {
            scenesC.transform.SetParent(eyeC.transform);

            CheckPanel();

            myGun = GameObject.Instantiate(gd.itemPrefeb, aimPos);
            myGun.GetComponent<Gun>().Init(gd.cartridgeClip, gd.maxBulletNum, gd, scenesC, eyeC, gameObject);
            EventCenter.GetInstance().Trigger("EquipGun", myGun, 0, 0);
        }

        /// <summary>
        /// 装备投掷物
        /// </summary>
        /// <param name="gd"></param>
        void EquipThrow(GrenadeData gd)
        {
            myGun = null;
            scenesC.transform.SetParent(eyeC.transform);
            CheckPanel();

            GameObject myOBJ = Instantiate(gd.itemPrefeb, handPos);
            Destroy(myOBJ.GetComponent<Rigidbody>());
            myOBJ.layer = LayerMask.NameToLayer("Player");
            myOBJ.transform.SetParent(handPos);
            myOBJ.AddComponent<GrenadeControl>();
            myOBJ.GetComponent<GrenadeControl>().Init(gd);
            EventCenter.GetInstance().Trigger("EquipGrenade", gd, 0, 0);
        }

        /// <summary>
        /// 装备道具
        /// </summary>
        void EquipProp()
        {
            
        }

        /// <summary>
        /// 装备配件
        /// </summary>
        void EquipAccessories(AttachmentData ad)
        {
            if (myGun != null)
            {
                GameObject tmpObj = Instantiate(ad.itemPrefeb);
                Attach ac = tmpObj.GetComponent<Attach>();
                if (ac != null)
                {
                    if (!ac.DoAttach(myGun, ad))
                    {
                        DestroyImmediate(tmpObj);
                    }
                    if (ad.itemID == 7004 && ac.DoAttach(myGun, ad))
                    {
                        equipAttachList.Add(tmpObj);
                    }
                }
            }
        }

        void CheckPanel()
        {
            if (equipAttachList.Count > 0)
            {
                for(int i = 0; i < equipAttachList.Count; ++i)
                {
                    equipAttachList[i].GetComponent<Attach>().Dettach();                    
                }
            }
            equipAttachList.Clear();

            if (handPos.childCount > 0)
            {
                while (handPos.childCount > 0)
                {
                    DestroyImmediate(handPos.GetChild(0).gameObject);
                }
            }
            if (aimPos.childCount > 0)
            {
                while (aimPos.childCount > 0)
                {                    
                    DestroyImmediate(aimPos.GetChild(0).gameObject);
                }
            }
        }
    }
}