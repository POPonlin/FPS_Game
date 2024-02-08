using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS3_GameBase
{
    [CreateAssetMenu(menuName = "FPS3.0/Gun Data")]
    public class GunData : ItemData
    {
        public enum GunType
        {
            [Tooltip("突击步枪")]
            GT_Rifle,
            [Tooltip("手枪")]
            GT_HandGun,
            [Tooltip("冲锋枪")]
            GT_Submachine,
            [Tooltip("霰弹枪")]
            GT_ShotGun,
            [Tooltip("狙击枪")]
            GT_Sniper,
            [Tooltip("发射器")]
            GT_Emitter,
        }

        public enum FireType
        {
            [Tooltip("全自动射击")]
            FT_Auto,
            [Tooltip("单发射击")]
            FT_Single,
            [Tooltip("全自动与单发射击")]
            FT_Max,
        }

        public GunType gunType;
        public FireType fireType;
        [Header("最大载弹量"), Tooltip("最大载弹量")]
        public int maxBulletNum;
        [Header("最大弹匣载弹量"), Tooltip("最大弹匣载弹量")]
        public int cartridgeClip;
        [Header("是否可以切换射击模式"), Tooltip("是否可以切换射击模式")]
        public bool changeFireTypeAble;
        [Header("有效射程"), Tooltip("有效射程")]
        public float effectiveRange;
        [Header("子弹速度（秒每发）"), Tooltip("子弹速度（秒每发）")]
        public float rateOfFire;
        [Header("发射子弹力量值(弃用)"), Tooltip("发射子弹力量值（弃用）")]
        public float bulletForce;
        [Header("发射子弹速度"), Tooltip("发射子弹速度")]
        public float bulletSpeed;
        [Header("后坐力"), Tooltip("后坐力")]
        public float recoil;
        [Header("腰射散布"), Tooltip("腰射散布")]
        public float SpreadAngle;
        [Header("子弹预制体"), Tooltip("子弹预制体")]
        public GameObject bulletPrefeb;
        [Header("子弹头类型"), Tooltip("子弹类型")]
        public BulletPool.BulletType bulletType;
        [Header("子弹壳类型"), Tooltip("子弹壳类型")]
        public BulletPool.BulletType bulletCasingType;
        [Header("基础瞄准视野"), Tooltip("基础瞄准视野")]
        public float fovForAim;
        [Header("常态视野"), Tooltip("常态视野")]
        public float fovForNormal;
        [Header("换弹基础等待时间"), Tooltip("换弹基础等待时间")]
        public float waitReload;
        [Header("空仓换弹基础等待时间"), Tooltip("空仓换弹基础等待时间")]
        public float waitReloadOut;
        [Header("发射音效"), Tooltip("发射音效")]
        public AudioClip FireSound;
        [Header("消音发射音效"), Tooltip("消音发射音效")]
        public AudioClip FireSoundSilencer;
        [Header("空仓激发音效"), Tooltip("空仓激发音效")]
        public AudioClip noBulletSound;
        [Header("瞄准音效"), Tooltip("瞄准音效")]
        public AudioClip AimSound;
        [Header("换弹音效"), Tooltip("换弹音效")]
        public AudioClip reLoadSound;
        [Header("空仓换弹音效"), Tooltip("空仓换弹音效")]
        public AudioClip outReloadSound;
        [Header("空仓换弹音效2"), Tooltip("空仓换弹音效2")]
        public AudioClip ReloadSoundIn;
        [Header("武器装备音效"), Tooltip("武器装备音效")]
        public AudioClip drawSound;
        [Header("武器切换音效"), Tooltip("武器切换音效")]
        public AudioClip changeSound;
        [Header("弹壳粒子特效"), Tooltip("弹壳粒子特效")]
        public ParticleSystem bulletParticle;
        [Header("弹壳"), Tooltip("弹壳")]
        public GameObject bulletShell;
    }
}
