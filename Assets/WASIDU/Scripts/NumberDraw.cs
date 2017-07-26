//========================================================
// 数字描画用スクリプト
//========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NumberDraw : MonoBehaviour
{
    //--- メンバ変数 ------------------------------------------------------------------------------------------------------------
    //--- メンバ定数
    private const float NUMBER_DISPLAY_SPACING = 130.0f;

    //--- メンバ変数
    [SerializeField] private GameObject m_NumberObjectPrefub;   // 番号表示オブジェクトプレハブ

    private int m_DrawNumber;   // 表示数字
    private List<GameObject> m_NumberImageList;
    private bool m_DrawParentPosCenter;

    //--- メンバ関数 ------------------------------------------------------------------------------------------------------------
    NumberDraw()
    {
        m_NumberImageList = new List<GameObject>();
        m_DrawNumber = 0;
        m_DrawParentPosCenter = false;
    }

    void Awake ()
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
        int OneDigit = 0;               // 一桁の情報
        int NumberData = m_DrawNumber;  // 表示数字
        int DigitNum = 0;               // 桁数

        while (true)
        {
            OneDigit = NumberData % 10;

            if (m_NumberImageList.Count <= DigitNum)
            {
                AddNumberObject();
            }

            m_NumberImageList[DigitNum].GetComponent<NumberImage>().SetNumber(OneDigit);

            DigitNum++;
            NumberData = NumberData / 10;

            if (NumberData <= 0)
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

        //--- 桁数が二桁超えている場合
        if (m_NumberImageList.Count > 1)
        {
            SetPosition();
        }
    }

    //--- 位置設定
    private void SetPosition()
    {
        int Count = m_NumberImageList.Count;

        Vector3 pos = m_NumberImageList[Count - 2].transform.localPosition;

        pos.x -= NUMBER_DISPLAY_SPACING;

        m_NumberImageList[Count - 1].transform.localPosition = pos;

        if (m_DrawParentPosCenter)
        {
            m_NumberImageList.ForEach(Obj =>
            {
                pos = Obj.transform.localPosition;

                pos.x += NUMBER_DISPLAY_SPACING / 2f;
                Obj.transform.localPosition = pos;
            });
        }
    }

    public void Reset()
    {
        m_NumberImageList.ForEach(Obj =>
        {
            Destroy(Obj);
        });

        m_NumberImageList.Clear();

        m_DrawNumber = 0;

        SetDrawNumber();
    }

    public bool SetDrawCenter { set { m_DrawParentPosCenter = value; } }
}
