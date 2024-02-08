using FPS3_GameBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AudioClip[] explosionClips;
    public GameObject explosionEffect;
    public float explosionAreaRadio = 5f;
    public float explosionForce = 500f;

    /// <summary>
    /// 爆炸模块
    /// </summary>
    public void Explode()
    {
        Vector3 pos = transform.position;
        if (explosionClips.Length > 0)
        {
            AudioSource.PlayClipAtPoint(explosionClips[Random.Range(0, explosionClips.Length)], pos);
        }
        if (explosionEffect != null)
        {
            GameObject obj = Instantiate(explosionEffect, pos, transform.localRotation);
            Destroy(obj, 1.5f);
        }
        if (explosionAreaRadio > 0f)
        {
            Collider[] colliders = Physics.OverlapSphere(pos, explosionAreaRadio);
            foreach (Collider collider in colliders) 
            {
                EventCenter.GetInstance().Trigger("Explosion", collider.gameObject, 0, 0);
                Rigidbody rig = collider.GetComponent<Rigidbody>();
                if (rig != null)
                {
                    rig.AddExplosionForce(explosionForce, pos, explosionAreaRadio);
                }
            }
        }
        gameObject.SetActive(false);
    }
}
