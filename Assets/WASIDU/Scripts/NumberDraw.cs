using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberDraw : MonoBehaviour
{
    //--- メンバ変数 ------------------------------------------------------------------------------------------------------------
    //--- メンバ定数
    private const float NUMBER_DISPLAY_SPACING = 70.0f;
    //private const float NUMBER_DISPLAY_SPACING = 15.0f;
    //private const float NUMBER_DISPLAY_SPACING = 45.0f;

    //--- メンバ変数
    [SerializeField] private GameObject m_NumberObjectPrefub;   // 番号表示オブジェクトプレハブ

    private int m_DrawNumber;   // 表示数字
    private List<GameObject> m_NumberImageList;

    //--- メンバ関数 ------------------------------------------------------------------------------------------------------------
    NumberDraw()
    {
        m_NumberImageList = new List<GameObject>();
        m_DrawNumber = 0;
    }

    void Start ()
    {
        AddNumberObject();
	}
	
    //--- 値設定
    public void SetNumber(int SetNum)
    {
        m_DrawNumber = SetNum;
        SetDrawNumber();
    }

    //--- 表示設定
    private void SetDrawNumber()
    {
        // スコア設定と処理がほぼ同じなので統合予定

        int OneDigit = 0;               // 一桁の情報
        int ComboData = m_DrawNumber;   // 表示数字
        int DigitNum = 0;               // 桁数

        while (true)
        {
            OneDigit = ComboData % 10;

            if (m_NumberImageList.Count <= DigitNum)
            {
                AddNumberObject();
            }

            m_NumberImageList[DigitNum].GetComponent<NumberImage>().SetNumber(OneDigit);

            DigitNum++;
            ComboData = ComboData / 10;

            if (ComboData <= 0)
                break;
        }
    }

    //--- 番号表示オブジェクト追加
    private void AddNumberObject()
    {
        GameObject NumberObj = Instantiate(m_NumberObjectPrefub);
        NumberObj.transform.SetParent(transform);
        NumberObj.transform.position = transform.position;
        NumberObj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        m_NumberImageList.Add(NumberObj);

        int Count = m_NumberImageList.Count;

        if (Count > 1)
        {
            Vector3 pos = m_NumberImageList[Count - 2].transform.position;
            pos.x -= NUMBER_DISPLAY_SPACING;
            m_NumberImageList[Count - 1].transform.position = pos;
        }
    }
}
