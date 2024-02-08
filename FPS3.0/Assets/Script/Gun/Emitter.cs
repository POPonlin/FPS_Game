using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emitter : Gun
{
    public GameObject rocketGO;
    protected override void CreatBullet(Vector3 vec)
    {
        if (itemArr.bulletPrefeb != null && itemArr.bulletType != BulletPool.BulletType.BT_Max)
        {
            GameObject bullet = BulletPool.GetInstance().GetBullet(itemArr.bulletType, firePos.position, firePos.rotation);
            bullet.transform.SetParent(null);

            bullet.GetComponent<Rigidbody>().velocity = (vec + bullet.transform.forward) * itemArr.bulletSpeed;
            bullet.GetComponent<Bullet>().Init(itemArr.effectiveRange, owner);      
        }
        if (rocketGO != null)
        {
            rocketGO.SetActive(false);
        }
    }

    protected override void PlayReload()
    {        
        if (itemArr != null && !isReload && maxBullet > 0 && currentBulletNum < itemArr.cartridgeClip)
        {
  
            anim.SetTrigger("Reload");
            PlayAudioSource(itemArr.reLoadSound, 2);

            isReload = true;
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(0.5f);

        if (rocketGO != null)
        {
            rocketGO.SetActive(true);
        }

        while(AnimatorIsPlaying("Reload"))
        {
            yield return null;
        }

        currentBulletNum++;
        maxBullet--;
        SetBulletNum(currentBulletNum, maxBullet);
        isReload = false;
    }
}
