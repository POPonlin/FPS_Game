using FPS3_GameBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS3_GameBase
{
    public class ItemShow : MonoBehaviour
    {
        public Vector3 rotationSpeed = new Vector3(0, 20, 0); // 旋转速度


        // Start is called before the first frame update
        void Start()
        {
            EventCenter.GetInstance().Register("InSight", InSight);
            EventCenter.GetInstance().Register("OutSight", OutSight);
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(5 * rotationSpeed * Time.deltaTime);
        }

        void InSight(object obj, int param1, int param2)
        {

        }

        void OutSight(object obj, int param1, int param2)
        {

        }

        /// <summary>
        /// 物体描边函数
        /// </summary>
        void OutLine()
        {
            //TODO:
        }
    }

}