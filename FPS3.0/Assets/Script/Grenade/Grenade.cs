using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    protected GrenadeData gd;
    protected GameObject owner;
    private float timer = 0f;
    protected bool isTrow;

    public void Init(GameObject _owner, GrenadeData _gd)
    {
        owner = _owner;
        gd = _gd;
    }

    public void Throw()
    {
        isTrow = true;
    }

    virtual protected void Explosion()
    {
        Explosion ep = GetComponent<Explosion>();
        ep.explosionAreaRadio = gd.explosionRange;
        ep.explosionForce = gd.explosionForce;
        ep.Explode();
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        Destroy(gameObject, 0.2f);
    }

    private void Update()
    {
        if (isTrow && gd != null)
        {
            timer += Time.deltaTime;
            if (timer > gd.delayTime)
            {
                timer = 0f;
                Explosion();
                isTrow = false;
            }
        }
    }
}
