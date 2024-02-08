using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS3_GameBase
{
    public class CameraControl : MonoBehaviour
    {
        public float ViewSensitivity = 300f;
        public float maxView = 80f;
        public float minView = -80f;

        private float fMouseX;
        private float fMouseY;
        private float tMY;

        public Transform playerTra;

        [Header("射线检测物品的距离")]
        public float rayTestDistance = 10f;
        [Header("指定物品检测层")]
        public LayerMask rayTestMask;
        public Transform wordCamera;
        private GameObject shadowGO = null;

        private Vector2 m_Recoil;
        [Header("后坐力设置")]
        public Vector2 recoilFactor = Vector2.zero;
        public float recoilDamping;
        private bool flag_recoilC;
        // Start is called before the first frame update
        void Start()
        {
            playerTra = transform.parent.GetComponent<Transform>();
            wordCamera = GetComponent<Transform>();
            Cursor.lockState = CursorLockMode.Locked;
            EventCenter.GetInstance().Register("Fire", OnFireRecoil);
            EventCenter.GetInstance().Register("EndFire", ChangeRecoilFlag);
        }

        // Update is called once per frame
        void Update()
        {
            View_Update();
            LookItem_Update();
            PickUp_Update();
            m_Recoil = Vector2.Lerp(m_Recoil, Vector2.zero, recoilDamping * Time.deltaTime);
            if (flag_recoilC)
            {
                Debug.Log("停止射击");                
                //
                //TODO 视角回复，还需要做一个记录初始射击视角位置的函数，可以检测x,y轴数值。
                //
                if(m_Recoil == Vector2.zero)
                {
                    flag_recoilC = false;
                }
            }
        }

        void ChangeRecoilFlag(object obj, int param1, int param2)
        {
            flag_recoilC = true;
        }

        void OnFireRecoil(object obj, int param1, int param2)
        {
            GunData gd = (GunData)obj;
            m_Recoil.x = Random.Range(-gd.recoil * recoilFactor.x, gd.recoil * recoilFactor.x);
            m_Recoil.y = Random.Range(0, gd.recoil * recoilFactor.y);
        }

        void View_Update()
        {
            fMouseY = Input.GetAxisRaw("Mouse Y");
            fMouseX = Input.GetAxisRaw("Mouse X");

            fMouseX += m_Recoil.x;
            fMouseY += m_Recoil.y;

            tMY -= fMouseY * ViewSensitivity * Time.deltaTime;
            tMY = Mathf.Clamp(tMY, minView, maxView);

            transform.localRotation = Quaternion.Euler(tMY, 0, 0);
            //transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(tMY, 0, 0), 8f);
            if (playerTra != null)
            {
                playerTra.Rotate(playerTra.up, fMouseX * ViewSensitivity * Time.deltaTime);
            }
        }

        void LookItem_Update()
        {
            ///Debug.DrawRay(wordCamera.position, wordCamera.forward, Color.red, rayTestDistance);
            if (Physics.Raycast(wordCamera.position, wordCamera.forward, out RaycastHit hit, rayTestDistance, rayTestMask))
            {
                if (shadowGO != hit.collider.gameObject)
                {
                    shadowGO = hit.collider.gameObject;
                    EventCenter.GetInstance().Trigger("InSight", shadowGO, 0, 0);
                }
            }
            else
            {
                if (shadowGO != null)
                {
                    EventCenter.GetInstance().Trigger("OutSight", null, 0, 0);
                    shadowGO = null;
                }
            }

        }

        void PickUp_Update()
        {
            if(Input.GetKeyDown(KeyCode.F) && shadowGO!= null) 
            {
                EventCenter.GetInstance().Trigger("PickUpItem", shadowGO, 0, 0);                
            }
        }
    }
}
