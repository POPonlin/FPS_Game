using FPS3_GameBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletHitType
{
    public string hitName;
    public ParticleSystem hitEffect;
}

public class Bullet : MonoBehaviour
{
    public BulletHitType[] effectList;
    public AudioClip[] audioList;

    private SphereCollider sc;
    private TrailRenderer trail;

    private float effectiveRange;
    private GameObject owener;

    private Vector3 startPos;

    // Start is called before the first frame update
    void Awake()
    {
        sc = GetComponent<SphereCollider>();
        trail = GetComponent<TrailRenderer>();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if((transform.position - startPos).magnitude > effectiveRange)
        {
            gameObject.SetActive(false);            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explosion explosion = GetComponent<Explosion>();
        if (explosion != null)
        {
            explosion.Explode();
        }
        else
        {
            Transform tmp;
            if(audioList.Length > 0)
            {
                AudioSource.PlayClipAtPoint(audioList[Random.Range(0, audioList.Length)], transform.position);            
            }

            foreach (BulletHitType item in effectList)
            {
                if(item.hitName == collision.gameObject.tag)
                {
                    ParticleSystem obj = GameObject.Instantiate(item.hitEffect, transform.position, Quaternion.LookRotation(collision.contacts[0].normal));
                    obj.transform.SetParent(collision.transform);

                    tmp = obj.transform;  
                    if(tmp.localPosition.z < 0)
                    { 
                        EventCenter.GetInstance().Trigger("TargetWood", collision.gameObject, 0, 0);
                    }

                    gameObject.SetActive(false);
                }
            }

            gameObject.SetActive(false);
        }

    }

    public void Init(float _effectiveRange, GameObject _owner)
    {
        owener = _owner;
        effectiveRange = _effectiveRange;
    }
}


