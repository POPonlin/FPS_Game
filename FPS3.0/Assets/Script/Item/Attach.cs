using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attach : MonoBehaviour
{
    protected Gun m_Gun = null;
    protected AttachmentData m_Attr = null;

    protected virtual void OnDettach()
    {

    }

    protected virtual void OnAttach()
    {

    }

    public bool DoAttach(GameObject gun, AttachmentData ad)
    {
        Gun g = gun.GetComponent<Gun>();
        if (g != null)
        {
            m_Gun = g;
            m_Attr = ad;
            if (m_Gun.AttachPos[m_Attr.AttachPos - 1] == null)
            {
                return false;
            }

            OnAttach();

            transform.SetParent(m_Gun.AttachPos[m_Attr.AttachPos - 1]);

            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
            transform.localScale = Vector3.one;

            return true;
        }
        return false;
    }

    public void Dettach()
    {
        OnDettach();
        m_Gun = null;
    }
}
