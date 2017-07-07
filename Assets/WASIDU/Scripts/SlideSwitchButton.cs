using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideSwitchButton : MonoBehaviour
{
    //--- メンバ変数 ------------------------------------------------------------------------------------------------------------
    //--- メンバ定数

    //--- メンバ変数
    private RectTransform m_BackImageTransform;
    private GameObject m_ButtonImageObject;
    private bool m_ButtonRight;

    //--- メンバ関数 ------------------------------------------------------------------------------------------------------------
	// Use this for initialization
	void Start ()
    {
        m_BackImageTransform = gameObject.GetComponent<RectTransform>();
        m_ButtonImageObject = transform.FindChild("ButtonImage").gameObject;
        m_ButtonRight = false;
	}

    public void ChangePosition()
    {
        Vector3 ButtonPos = Vector3.zero;

        if (m_ButtonRight)
        {
            ButtonPos.x = -gameObject.GetComponent<RectTransform>().sizeDelta.x / 4f;

            m_ButtonImageObject.transform.localPosition = ButtonPos;
            m_ButtonRight = false;
        }
        else
        {
            ButtonPos.x = gameObject.GetComponent<RectTransform>().sizeDelta.x / 4f;

            m_ButtonImageObject.transform.localPosition = ButtonPos;
            m_ButtonRight = true;
        }
    }

}
