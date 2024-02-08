using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenda : Grenade
{
    public GameObject smokeEffect;
    protected override void Explosion()
    {
        if (smokeEffect != null)
        {
            GameObject obj = Instantiate(smokeEffect, transform);
            obj.transform.SetParent(null);
            obj.transform.localScale = Vector3.one * gd.explosionRange / 2f;

            Destroy(obj, gd.explosionForce);   
            Destroy(gameObject, gd.explosionForce);
        }
    }
}
