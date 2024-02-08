using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS3_GameBase
{
    public class MoveControl : MonoBehaviour
    {
        public float walkSpeed = 2f;
        public float runSpeed = 5f;

        public float jumpHeight = 1.2f;
        private float gravity = 9.8f;

        [HideInInspector]
        public static float fSpeed;
        private float fHorX;
        private float fVerY;
        [Tooltip("水平方向速度向量")]
        private Vector3 dir;
        [Tooltip("水平与竖直总速度（矢量）")]
        private Vector3 playVector3;
        private CharacterController character;

        [Tooltip("脚步声音数据，目前获取方式为拖拽获取。随后考虑更改为自动获取资源")]
        public FootSoundData footSoundData;
        private AudioSource audioSource;
        [Tooltip("单次脚步声播放时间")]
        private float soundPlayTime = 0.8f;
        private float originPlayPoint = 0.0f;

        private Animator anim;

        private float value = 0f;
        // Start is called before the first frame update
        void Start()
        {
            character = GetComponent<CharacterController>();
            audioSource = GetComponent<AudioSource>();
            anim = GetComponent<Animator>();
            EventCenter.GetInstance().Register("EquipGun", EquipGun);
            EventCenter.GetInstance().Register("EquipOther", EquipOther);
        }

        // Update is called once per frame
        void Update()
        {
            Sound_Update();
            Height_Update();
            Move_Update();
            character.Move(playVector3 * Time.deltaTime);
            if (anim != null && character.isGrounded)
            {
                anim.SetFloat("Speed", new Vector3(playVector3.x, 0, playVector3.z).magnitude);
            }
        }


        void Sound_Update()
        {
            if (character.isGrounded)
            {
                var tVec = playVector3;
                //此处使竖直方向速度为零，防止静止状态下magnitude > 0
                tVec.y = 0f;
                //移动幅度过小就不播放脚步声
                if (tVec.magnitude > 0.1f)
                {
                    originPlayPoint += Time.deltaTime;
                    
                    float tSPT = soundPlayTime * (walkSpeed / fSpeed);
                    tSPT = Mathf.Clamp(tSPT, 0.4f, soundPlayTime);

                    if (originPlayPoint >= tSPT)
                    {                        
                        string tTag = GetSoundTag();
                        if (tTag != null)
                        {
                            AudioClip clip = footSoundData.GetAudioClip(tTag);
                            audioSource.clip = clip;
                            audioSource.Play();
                        }

                        originPlayPoint = 0.0f;
                    }
                }
                else
                {
                    audioSource.Stop();
                }
            }
        }
        
        void Height_Update()
        {
            if (character.isGrounded)
            {
                if (playVector3.y < -1)
                {
                    playVector3.y = -1f;
                }

                if (Input.GetButtonDown("Jump"))
                {
                    playVector3.y = Mathf.Sqrt(2 * jumpHeight * gravity);
                }
            }

            playVector3.y += -gravity * Time.deltaTime;
        }

        void Move_Update()
        {
            if (character.isGrounded)
            {
                fHorX = Input.GetAxis("Horizontal");
                fVerY = Input.GetAxis("Vertical");
                //归一化处理
                Vector2 inputDirection = new Vector2(fHorX, fVerY).normalized;

                dir = inputDirection.x * transform.right + inputDirection.y * transform.forward;
                
                //fSpeed = Mathf.Lerp(fSpeed, (Input.GetKey(KeyCode.LeftShift)) ? runSpeed : walkSpeed, 0.2f);

                fSpeed = Mathf.SmoothDamp(fSpeed, (Input.GetKey(KeyCode.LeftShift)) ? runSpeed : walkSpeed, ref value, 0.25f);

                playVector3.x = dir.x * fSpeed;
                playVector3.z = dir.z * fSpeed;
            }
            else
            {
                if(anim != null && anim.GetFloat("Speed") != 0)
                {
                    anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), 0, 3.5f * Time.deltaTime));
                }
            }
        }

        string GetSoundTag()
        {
            float distance = 1f; // 射线的长度

            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, distance))
            {
                return hit.collider.tag;
            }

            return null;
        }



        void EquipGun(object obj, int param1, int param2)
        {
            GameObject item = (GameObject)obj;
            anim = item.GetComponent<Animator>();
        }

        void EquipOther(object obj, int param1, int param2)
        {
            anim = null;
        }
    }
}