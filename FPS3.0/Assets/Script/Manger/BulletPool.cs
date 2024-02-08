using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : Singleton<BulletPool>
{
    public enum BulletType
    {
        BT_Normal,  //普通子弹头
        BT_NormalCasing,    //普通子弹壳
        BT_ShotgunCasings,  //霰弹壳
        BT_Rocket,  //火箭弹
        BT_Grenade, //榴弹
        BT_Max,
    }
    [Header("预制体数组")]
    public GameObject[] bulletPrefabs = new GameObject[(int)BulletType.BT_Max];
    [Header("对象池最大容量")]
    public int maxSize = 50;
    private Dictionary<BulletType, ArrayList> usingPool = new Dictionary<BulletType, ArrayList>();
    private Dictionary<BulletType, ArrayList> freePool = new Dictionary<BulletType, ArrayList>();
    
    public GameObject GetBullet(BulletType bt, Vector3 vc3, Quaternion rotation)
    {
        GameObject obj = null;
        if(!freePool.ContainsKey(bt)) 
        {
            freePool[bt] = new ArrayList();
            usingPool[bt] = new ArrayList();
        }
        ArrayList al = freePool[bt];
        if(al.Count > 0)
        {
            obj = (GameObject)al[0];
            al.RemoveAt(0);
        }
        else
        {
            if (usingPool[bt].Count < maxSize)
            {
                obj = Instantiate(bulletPrefabs[(int)bt]);
            }
        }
        if(obj != null)
        {
            obj.transform.localPosition = vc3;
            obj.transform.localRotation = rotation;
            usingPool[bt].Add(obj);

            obj.SetActive(true);
        }
        return obj;  
    }

    private void Update()
    {
        foreach (BulletType bt in usingPool.Keys)
        {
            ArrayList al = usingPool[bt];
            for(int i = 0; i < al.Count; i++)
            {
                GameObject obj = (GameObject)al[i];
                if(obj.activeSelf == false)
                {
                    freePool[bt].Add(obj);
                    usingPool[bt].RemoveAt(i--);
                }
            }
        }
    }

}
