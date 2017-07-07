using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BockSwitchButton : MonoBehaviour
{
    //--- メンバ変数 ------------------------------------------------------------------------------------------------------------
    //--- メンバ定数

    //--- メンバ変数
    private GameObject m_ONObject;
    private GameObject m_OFFObject;

    //--- メンバ関数 ------------------------------------------------------------------------------------------------------------
	// Use this for initialization
    void Start()
    {
        m_ONObject = transform.FindChild("ON").gameObject;
        m_OFFObject = transform.FindChild("OFF").gameObject;

        m_ONObject.SetActive(true);
        m_OFFObject.SetActive(false);
	}


    public void ChangePosition()
    {
        if (m_ONObject.activeSelf)
        {
            m_ONObject.SetActive(false);
            m_OFFObject.SetActive(true);
        }
        else
        {
            m_ONObject.SetActive(true);
            m_OFFObject.SetActive(false);
        }
    }
}
