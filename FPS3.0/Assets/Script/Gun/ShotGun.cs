using FPS3_GameBase;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ShotGun : Gun
{
    private float timer = 0f;
    private float inerval = 0.8f;
    private bool canShot;
    protected override void PlayReload()
    {
        if (currentBulletNum < itemArr.cartridgeClip && !isReload)
        {
            anim.SetTrigger("Reload");
            PlayAudioSource(itemArr.reLoadSound, 2);
            isReload = true;

            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(0.3f);

        while (AnimatorIsPlaying("ReloadOpen"))
        {
            yield return null;
        }

        int tmp = currentBulletNum;
        for(int i = 0; i < itemArr.cartridgeClip - tmp && maxBullet > 0; ++i)
        {
            anim.SetTrigger("ReloadInsert");
            PlayAudioSource(itemArr.outReloadSound, 2);
            yield return new WaitForSeconds(0.3f);

            while(AnimatorIsPlaying("ReloadInsert"))
            {
                yield return null;
            }
            currentBulletNum++;
            maxBullet--;
            EventCenter.GetInstance().Trigger("SetBullet", null, currentBulletNum, maxBullet);
        }

        anim.SetTrigger("ReloadOut");
        PlayAudioSource(itemArr.ReloadSoundIn, 2);
        yield return new WaitForSeconds(0.3f);
        while(AnimatorIsPlaying("ReloadClose"))
        {
            yield return null;
        }
        isReload = false;
    }

    protected override void CreatBullet(Vector3 vec)
    {
        for (int i = 0; i < 8; ++i)
        {
            base.CreatBullet(vec);
        }
    }

    protected override void OnTrigger_Update()
    {
        if (isInspect || itemArr is null || MoveControl.fSpeed >= 4.5f || (isReload == true && currentBulletNum == 0))
        {
            return;
        }

        if (!canShot)
        {
            timer += Time.deltaTime;
            if (timer >= inerval)
            {
                timer = 0f;
                canShot = true;
            }
        }

        
        if (Input.GetKeyDown(KeyCode.Mouse0) && canShot)
        {
            Fire();
            canShot = false;
        }
    }
}
