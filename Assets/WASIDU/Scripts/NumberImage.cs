//========================================================
// 数字Image(一桁)
//========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberImage : MonoBehaviour
{
    //--- メンバ変数 ------------------------------------------------------------------------------------------------------------
    //--- メンバ定数
    const float NUM_DIGIT = 0.1f;

    //--- メンバ変数
    private int m_DrawNumber;
    static Sprite[] m_NumberSprites = null;

    //--- メンバ関数 ------------------------------------------------------------------------------------------------------------
    NumberImage()
    {
        m_DrawNumber = 0;
    }

    void Start()
    {
        if (m_NumberSprites == null)
        {
            m_NumberSprites = Resources.LoadAll<Sprite>("NumberW");
        }

        SetUV();
    }

    public void SetNumber(int SetNumber)
    {
        m_DrawNumber = SetNumber;
        SetUV();
    }

    private void SetUV()
    {
        gameObject.GetComponent<Image>().sprite = m_NumberSprites[m_DrawNumber];

    }
}
