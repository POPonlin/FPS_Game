using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GrenadeControl : MonoBehaviour
{
    [Header("投掷物体")]
    public GameObject grenadeObj;
    [Header("投掷位置")]
    public Transform throwPos;
    
    private GrenadeData gd;
    private float timer = 0f;
    private float waitTime = 1.5f;
    private bool canThrow = true;
    private void Update()
    {
        OnTrigger_Update();
    }

    /// <summary>
    /// 初始化投掷物信息
    /// </summary>
    /// <param name="_gd"></param>
    public void Init(GrenadeData _gd)
    {
        gd = _gd;
        grenadeObj = gd.itemPrefeb;
        throwPos = transform.parent.parent.Find("ThrowPos");
    }

    public void OnTrigger_Update()
    {
        if (!canThrow)
        {
            timer += Time.deltaTime;
            if (timer >= waitTime)
            {
                timer = 0f;
                canThrow = true;
            }
        }

        if (canThrow && Input.GetKey(KeyCode.Mouse0))
        {
            AudioSource.PlayClipAtPoint(gd.throwClip, transform.position);
            GameObject obj = Instantiate(gd.itemPrefeb, throwPos);
            obj.transform.SetParent(null);
            Grenade tmp = obj.GetComponent<Grenade>();
            if (tmp != null)
            {
                tmp.Init(gameObject, gd);
                tmp.Throw();
            }
            obj.GetComponent<Rigidbody>().AddForce(gd.throwForce * transform.forward);
            canThrow = false;
        }
    }
}


