using FPS3_GameBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Gun : MonoBehaviour
{
    public List<Transform> AttachPos = new List<Transform>(5);

    [Header("检测层")]
    public LayerMask mask;

    [HideInInspector]
    public GameObject owner;

    [HideInInspector]
    public int currentBulletNum;
    [HideInInspector]
    public int maxBullet;
    public Transform firePos;
    public Transform casingPos;

    [HideInInspector]
    public GunData itemArr;
    protected Animator anim;
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    protected bool isReload = false;
    protected bool isAutoFire;

    protected float originV ;

    protected ParticleSystem particle;

    public Transform cameraBons;
    protected bool isAim;
    protected Camera sceneCamera;
    protected Camera eyeCamera;

    public AudioClip aimSound;
    protected bool isInspect;
    public float aimPos = 0;
    public bool isChange = false;
    // Start is called before the first frame update
    void Awake()
    {        
        anim = GetComponent<Animator>();        
    }

    // Update is called once per frame
    void Update()
    {
        InspectWeapon();
        OnTrigger_Update();
        ReLoad_Update();
        ChangeFireType_Update();
        Aim_Update();
    }

    public void Init(int n, int m, GunData gunData, Camera cc, Camera ec, GameObject _owner)
    {
        itemArr = gunData;
        SetBulletNum(n, m);
        sceneCamera = cc;
        eyeCamera = ec;
        originV = 1 / itemArr.rateOfFire;
        PlayAudioSource(itemArr.drawSound, 2);

        sceneCamera.transform.SetParent(cameraBons);

        owner = _owner;

        if (AttachPos.Count == 4)
        {
            AttachPos.Add(null);
        }
        

    }

    void InspectWeapon()
    {
        if(Input.GetKeyDown(KeyCode.I) && !isInspect && !isReload)
        {
            anim.Play("Inspect");
            isInspect = true;
            StartCoroutine(InInspcet());
        }
    }

    IEnumerator InInspcet()
    {
        yield return new WaitForSeconds(2f);
        while(AnimatorIsPlaying("Inspect"))
        {
            yield return null;
        }
        isInspect = false;
    }

    void Aim_Update()
    {
        if (isReload || isInspect)
        {
            return;
        }
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            isAim = true;
            anim.SetFloat("AimType", aimPos);
            anim.SetBool("IsAim", isAim);
            anim.Play("AimIn");
            //sceneCamera.fieldOfView = itemArr.fovForAim;
 
            PlayAudioSource(aimSound, 2);
            EventCenter.GetInstance().Trigger("Aim", null, 1, 0);
        }
        if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            AimOut();
        }
        if(isAim && sceneCamera.fieldOfView != itemArr.fovForAim)
        {
            sceneCamera.fieldOfView = Mathf.Lerp(sceneCamera.fieldOfView, itemArr.fovForAim, 0.3f);
        }
    }

    void AimOut()
    {
        isAim = false;
        anim.SetBool("IsAim", isAim);
        sceneCamera.fieldOfView = itemArr.fovForNormal;
        EventCenter.GetInstance().Trigger("Aim", null, 2, 0);
    }

    protected bool AnimatorIsPlaying(string name)
    {
        AnimatorStateInfo ainfo = anim.GetCurrentAnimatorStateInfo(0);
        return ainfo.IsName(name) && ainfo.normalizedTime < 1.0f;
    }

    void ReLoad_Update()
    {
        if (maxBullet <= 0 || isInspect)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.R) && maxBullet > 0 && isReload == false)
        {
            AimOut();
            PlayReload();
        }
    }

    protected virtual void PlayReload()
    {
        isReload = true;
        StartCoroutine(ReLoad());
    }

    protected IEnumerator ReLoad()
    {        
        if(currentBulletNum == 0)
        {
            anim.SetTrigger("ReloadOut");          
            PlayAudioSource(itemArr.outReloadSound, 2);

            yield return new WaitForSeconds(itemArr.waitReloadOut);  
            
            if (maxBullet > itemArr.cartridgeClip) 
            {
                currentBulletNum = itemArr.cartridgeClip;
                maxBullet -= itemArr.cartridgeClip;
            }
            else
            {
                currentBulletNum = maxBullet;
                maxBullet = 0;
            }            
        }
        else if(currentBulletNum > 0 && currentBulletNum< itemArr.cartridgeClip)
        {
            anim.SetTrigger("Reload");
            PlayAudioSource(itemArr.reLoadSound, 2);

            yield return new WaitForSeconds(itemArr.waitReload);

            if (maxBullet > itemArr.cartridgeClip - currentBulletNum)
            {
                maxBullet -= itemArr.cartridgeClip - currentBulletNum;
                currentBulletNum = itemArr.cartridgeClip;
            }
            else
            {
                currentBulletNum += maxBullet; 
                maxBullet = 0;
            }
        }
        isReload = false;
        anim.SetBool("CanFire", !isReload);
        EventCenter.GetInstance().Trigger("SetBullet", null, currentBulletNum, maxBullet);
    }

    public void SetBulletNum(int n, int m)
    {
        if(itemArr != null)
        {            
            currentBulletNum = n;
            maxBullet = m;            
        }
        EventCenter.GetInstance().Trigger("SetBullet", null, currentBulletNum, maxBullet);
    }

    protected virtual void OnTrigger_Update()
    {
        if(isInspect || itemArr is null || MoveControl.fSpeed >=4.5f || (isReload == true && currentBulletNum == 0 ))
        {
            return;
        }
        if(Input.GetKeyDown(KeyCode.Mouse0) && isAutoFire == false)
        {
            Fire();
        }
        else if(Input.GetKey(KeyCode.Mouse0) && isAutoFire == true)
        {
            originV += Time.deltaTime;
            if(originV >= 1 / itemArr.rateOfFire)
            {             
                Fire();
                originV = 0.0f;
            }
        }
    }

    protected void Fire()
    {        
        if(currentBulletNum > 0 && !isReload)
        {
            Vector3 vec = new Vector3();

            if(Physics.Raycast(eyeCamera.transform.position, eyeCamera.transform.forward, out RaycastHit hit, 1500f, mask))
            {
                vec = (hit.point - firePos.position).normalized;
            }

            anim.SetTrigger("Fire");
            ChangeFireSound();
            CreatBullet(vec);
            CreatCasing();

            --currentBulletNum;
            if(currentBulletNum == 0 )
            {
                anim.SetBool("CanFire", false);
            }
            EventCenter.GetInstance().Trigger("Fire", itemArr, currentBulletNum, maxBullet);
        }
    }

    public void ChangeFireSound()
    {
        if (!isChange)
        {
            PlayAudioSource(itemArr.FireSound, 1);
        }
        else
        {
            PlayAudioSource(itemArr.FireSoundSilencer, 1);
        }
    }

    protected virtual void CreatBullet(Vector3 vec)
    {
        if(itemArr.bulletPrefeb != null && itemArr.bulletType != BulletPool.BulletType.BT_Max)
        {
            //GameObject bullet = GameObject.Instantiate(itemArr.bulletPrefeb, firePos);
            GameObject bullet = BulletPool.GetInstance().GetBullet(itemArr.bulletType, firePos.position, firePos.rotation);
            bullet.transform.SetParent(null);

            if (bullet.TryGetComponent<TrailRenderer>(out TrailRenderer tmp))
            {
                bullet?.GetComponent<TrailRenderer>().Clear();   //清除顶点信息
                bullet?.GetComponent<TrailRenderer>().AddPosition(firePos.position);
            }

            if (isAim is true)
            {
                bullet.transform.eulerAngles += BulletScatteringRange(itemArr.SpreadAngle * 0.3f);                
            }
            else
            {
                bullet.transform.eulerAngles += BulletScatteringRange(itemArr.SpreadAngle);
            }
            //bullet.GetComponent<Rigidbody>().AddForce((vec + bullet.transform.forward) * itemArr.bulletForce);
            bullet.GetComponent<Rigidbody>().velocity = (vec + bullet.transform.forward) * itemArr.bulletSpeed;
            bullet.GetComponent<Bullet>().Init(itemArr.effectiveRange, owner);
            //Destroy(bullet, 2f);     //此行临时，做完对象池删       
        }
    }

    void CreatCasing()
    {
        if(itemArr != null && itemArr.bulletShell != null)
        {
            //GameObject ob = Instantiate(itemArr.bulletShell, casingPos);
            GameObject ob = BulletPool.GetInstance().GetBullet(itemArr.bulletCasingType, casingPos.position, casingPos.rotation); ;
            //ob.transform.SetParent(null);
            Rigidbody rb = ob.GetComponent<Rigidbody>();
            rb.AddForce((casingPos.up * Random.Range(0, 1.5f) + casingPos.right * Random.Range(0, 1.5f)) * 100);
            if(owner != null)
            {
                rb.velocity += owner.GetComponent<CharacterController>().velocity;
            }

            //Destroy(ob, 3f);
        }
    }

    void ChangeFireType_Update()
    {
        if(Input.GetKeyDown(KeyCode.B) && itemArr.changeFireTypeAble == true)
        {
            isAutoFire = !isAutoFire;
        }
    }

    Vector3 BulletScatteringRange(float angle)
    {
        return NormalDistribution.CalcN(Random.Range(0, angle)) * Random.insideUnitCircle ;
        //return Random.insideUnitCircle * itemArr.SpreadAngle;
    }

    protected void PlayAudioSource(AudioClip audioClip, int par)
    {
        if(par is 1)
        {
            audioSource1.clip = audioClip;
            audioSource1.Play();
        }
        else if(par is 2)
        {
            audioSource2.clip = audioClip;
            audioSource2.Play();
        }
    }
}
