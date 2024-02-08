using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS3_GameBase
{
    public class UIManger : MonoBehaviour
    {
        public Image flashImage;
        [Header("拾取提示")]
        public Text pickUpHint;

        public Text weaponName;
        public Text bulletInfo;
        public Image WeaponIcon;
        public Image sightBead;
        // Start is called before the first frame update
        void Start()
        {
            pickUpHint.enabled = false;
            EventCenter.GetInstance().Register("InSight", InSightUI);
            EventCenter.GetInstance().Register("OutSight", OutSightUI);
            EventCenter.GetInstance().Register("EquipGun", OnEquipmentGun);
            EventCenter.GetInstance().Register("SetBullet", OnSetBullet);
            EventCenter.GetInstance().Register("Fire", OnSetBullet);
            EventCenter.GetInstance().Register("FlashBoom", OnFlashBoom);
            EventCenter.GetInstance().Register("EquipGrenade", OnEquipmentGrenade);
            EventCenter.GetInstance().Register("Aim", SightBead);
        }

        void SightBead(object obj, int param1, int param2)
        {
            if (param1 == 1)
            {
                sightBead.gameObject.SetActive(false);
            }
            else
            {
                sightBead.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 在闪光弹范围内效果
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        void OnFlashBoom(object obj, int param1, int param2)
        {
            Transform pos = (Transform)obj;
            float fRange = param1 / 100;
            float fTime = param2 / 100;

            if ((transform.position - pos.position).magnitude <= fRange)
            {
                StartCoroutine(FlashFadeOut(fTime));
            }
        }

        IEnumerator FlashFadeOut(float ftime)
        {
            Color cc = Color.white;
            flashImage.color = cc;
            flashImage.gameObject.SetActive(true);

            float factor = 1 / ftime;
            while (ftime > 0)
            {
                cc.a -= Time.deltaTime * factor;
                flashImage.color = cc;
                yield return new WaitForEndOfFrame();
                //每帧执行
                ftime -= Time.deltaTime;
            }

            flashImage.gameObject.SetActive(false);
        }

        /// <summary>
        /// 设置子弹显示
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        void OnSetBullet(object obj, int param1, int param2)
        {
            bulletInfo.text = string.Format("{0}/{1}", param1, param2);
        }

        void OnEquipmentGun(object obj, int param1, int param2)
        {
            WeaponIcon.gameObject.SetActive(obj != null);
            GunData gunData = ((GameObject)obj).GetComponent<Gun>().itemArr;
            WeaponIcon.sprite = gunData.itemSprite;
            weaponName.text = gunData.itemName;
        }

        void OnEquipmentGrenade(object obj, int param1, int param2)
        {
            WeaponIcon.gameObject.SetActive(obj != null);
            bulletInfo.text = string.Format("∞");
            GrenadeData grenadeData = (GrenadeData)obj;
            WeaponIcon.sprite = grenadeData.itemSprite;
            weaponName.text = grenadeData.itemName;
        }

        /// <summary>
        /// 在视野内
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        void InSightUI(object obj, int param1, int param2)
        {
            pickUpHint.enabled = true;
        }

        /// <summary>
        /// 在视野外
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        void OutSightUI(object obj, int param1, int param2)
        {
            pickUpHint.enabled = ! pickUpHint.enabled;
        }

    }
}