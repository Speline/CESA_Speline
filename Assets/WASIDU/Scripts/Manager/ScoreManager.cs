//========================================================
// スコア管理
//========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //--- メンバ変数 ------------------------------------------------------------------------------------------------------------
    //--- メンバ定数

    //--- メンバ変数
    [SerializeField] private GameObject m_NumberObjectPrefub;   // 番号表示オブジェクトプレハブ
    [SerializeField] private GameObject m_ScoreParent;          // スコアの親オブジェクト
    [SerializeField] private GameObject m_ComboParent;          // コンボの親オブジェクト

    private int m_Score;    // スコア
    private int m_ComboCnt;   // コンボ数

    private List<GameObject> m_ScoreNumImageList;
    private List<GameObject> m_ComboNumImageList;


    //--- メンバ関数 ------------------------------------------------------------------------------------------------------------
    ScoreManager()
    {
        m_Score = 0;
        m_ComboCnt = 0;

        m_ScoreNumImageList = new List<GameObject>();
        m_ComboNumImageList = new List<GameObject>();
    }

    void Start()
    {
        //--- スコアマネージャー設定
        EnemyBase.ScoreManager = this;
        PlayerManager.ScoreManagerScript = this;

        AddNumberObject(m_ScoreParent, m_ScoreNumImageList);
        AddNumberObject(m_ComboParent, m_ComboNumImageList);
    }

    //--- コンボ設定
    public void SetCombo(bool bHit)
    {
        // 攻撃が消える時敵に当たっていたらコンボ加算
        if (bHit)
        {
            ////--- コンボ加算
            //m_nComboCnt++;

            SetDrawCombo();
        }
        else
        {
            //--- コンボ初期値に
            m_ComboCnt = 0;
            SetDrawCombo();
        }
    }

    //--- コンボ加算
    public void AddCombo()
    {
        m_ComboCnt++;
        SetDrawCombo();
    }

    //--- 得点加算
    public void AddScore(int AddScore)
    {
        //m_Score += AddScore;

        //--- コンボが1以上の場合コンボボーナス付き加算
        m_Score += m_ComboCnt >= 1 ? AddScore + (m_ComboCnt / 10 + 1) * 10 : AddScore;

        SetDrawScore();
    }

    //--- スコア初期化
    public void ReSetScore()
    {
        m_Score = 0;
        SetDrawScore();
    }

    //--- スコア表示設定
    private void SetDrawScore()
    {
        int OneDigit = 0;           // 一桁の情報
        int ScoreData = m_Score;    // スコア
        int DigitNum = 0;           // 桁数

        while (true)
        {
            OneDigit = ScoreData % 10;

            if (m_ScoreNumImageList.Count <= DigitNum)
            {
                AddNumberObject(m_ScoreParent, m_ScoreNumImageList);
            }

            m_ScoreNumImageList[DigitNum].GetComponent<NumberImage>().SetNumber(OneDigit);

            DigitNum++;
            ScoreData = ScoreData / 10;

            if (ScoreData <= 0)
                break;
        }
    }

    //--- コンボ表示設定
    private void SetDrawCombo()
    {
        // スコア設定と処理がほぼ同じなので統合予定

        int OneDigit = 0;           // 一桁の情報
        int ComboData = m_ComboCnt;    // コンボ数
        int DigitNum = 0;           // 桁数

        while (true)
        {
            OneDigit = ComboData % 10;

            if (m_ComboNumImageList.Count <= DigitNum)
            {
                AddNumberObject(m_ComboParent, m_ComboNumImageList);
            }

            m_ComboNumImageList[DigitNum].GetComponent<NumberImage>().SetNumber(OneDigit);

            DigitNum++;
            ComboData = ComboData / 10;

            if (ComboData <= 0)
                break;
        }
    }

    //--- 番号表示オブジェクト追加
    // 引数：親オブジェクト,追加したいリスト
    private void AddNumberObject(GameObject ParentObj,List<GameObject> AddImageList)
    {
        GameObject NumberObj = Instantiate(m_NumberObjectPrefub);
        NumberObj.transform.SetParent(ParentObj.transform);
        NumberObj.transform.position = ParentObj.transform.position;
        NumberObj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        AddImageList.Add(NumberObj);

        int Count = AddImageList.Count;

        if (Count > 1)
        {
            Vector3 pos = AddImageList[Count - 2].transform.position;
            pos.x -= 70.0f;
            AddImageList[Count - 1].transform.position = pos;
        }
    }

    //--- 情報取得
    public int Score { get { return m_Score; } }
    public int Combo { get { return m_ComboCnt; } }

}
