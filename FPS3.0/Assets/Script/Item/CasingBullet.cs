using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasingBullet : MonoBehaviour
{
    public AudioClip[] audioClips;
    private float liveTime = 1.2f;
    private float tmp = 0f;
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if(collision != null && audioClips.Length != 0)
        {
            AudioSource.PlayClipAtPoint(audioClips[Random.Range(0, audioClips.Length)], collision.transform.position);
        }
    }

    private void Update()
    {
        tmp += Time.deltaTime;
        if(tmp >= liveTime)
        {
            tmp = 0;
            gameObject.SetActive(false);
        }
    }
}
