using FPS3_GameBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetWood : MonoBehaviour
{
    private AudioSource audioS;
    public AudioClip up;
    public AudioClip down;
    private bool isDown;
    private float curT = 0;
    [Header("靶子复原时间")]
    public float ReLifeTime = 0.5f;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.GetInstance().Register("TargetWood", OnHit);
        EventCenter.GetInstance().Register("Explosion", OnHit);
        audioS = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (isDown)
        {
            curT += Time.fixedDeltaTime;
            if(curT >= ReLifeTime)
            {
                curT = 0;
                isDown = false;
                anim.SetTrigger("TUp");
                audioS.clip = up;
                audioS.Play();
            }
        }
    }

    /// <summary>
    /// 物体被击中
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    void OnHit(object obj, int param1, int param2)
    {
        
        if((GameObject)obj == gameObject && !isDown)
        {
            audioS.clip = down;
            audioS.Play();  
            anim.SetTrigger("TDown");
            isDown = true;
        }
    }
}
