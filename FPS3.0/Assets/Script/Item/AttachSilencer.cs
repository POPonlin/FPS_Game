using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachSilencer : Attach
{
    protected override void OnAttach()
    {     
        m_Gun.GetComponent<Gun>().isChange = true;
    }

    protected override void OnDettach()
    {
        m_Gun.GetComponent<Gun>().isChange = false;
    }
}
