using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachScope : Attach
{
    public Camera scopeCamera = null;

    protected override void OnAttach()
    {
        for (int i = 0; i < 4; ++i)
        {
            while (m_Gun.AttachPos[i] != null && m_Gun.AttachPos[i].childCount > 0)
            {
                Attach ac = m_Gun.AttachPos[i].GetChild(0).gameObject.GetComponent<Attach>();
                GameObject go = m_Gun.AttachPos[i].GetChild(0).gameObject;
                if (ac != null)
                {
                    ac.Dettach();
                }
                DestroyImmediate(go);
            }
        }

        if (scopeCamera != null)
        {
            scopeCamera.fieldOfView = m_Attr.AttachArgs + 2;
        }

        m_Gun.aimPos = m_Attr.AttachPos;
    }

    protected override void OnDettach()
    {
        m_Gun.aimPos = 0;
    }
}
