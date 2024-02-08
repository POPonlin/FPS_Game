using FPS3_GameBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBang : Grenade
{
    public AudioClip[] clips;
    public AudioClip tinnitsClip;
    protected override void Explosion()
    {
        if (clips.Length > 0)
        {
            AudioSource.PlayClipAtPoint(clips[Random.Range(0, clips.Length)], transform.position);
        }
        EventCenter.GetInstance().Trigger("FlashBoom", transform, (int)(gd.explosionRange * 100), (int)(gd.explosionForce * 100));
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        Destroy(gameObject, 1f);
    }
}
