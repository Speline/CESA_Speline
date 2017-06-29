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
    [SerializeField]
    private NumberDraw m_ScoreNumberDrawScript;
    [SerializeField]
    private NumberDraw m_ComboNumberDrawScript;

    private int m_Score;    // スコア
    private int m_ComboCnt;   // コンボ数

    //--- メンバ関数 ------------------------------------------------------------------------------------------------------------
    ScoreManager()
    {
        m_Score = 0;
        m_ComboCnt = 0;
    }

    void Start()
    {
        //--- スコアマネージャー設定
        EnemyBase.ScoreManager = this;
        PlayerManager.ScoreManagerScript = this;

    }

    //--- コンボ設定
    public void SetCombo(bool bHit)
    {
        // 攻撃が消える時敵に当たっていたらコンボ加算
        if (!bHit)
        {
            //--- コンボ初期値に
            m_ComboCnt = 0;
            m_ComboNumberDrawScript.SetNumber(m_ComboCnt);
        }
    }

    //--- コンボ加算
    public void AddCombo()
    {
        m_ComboCnt++;
        m_ComboNumberDrawScript.SetNumber(m_ComboCnt);
    }

    //--- 得点加算
    public void AddScore(int AddScore)
    {
        //m_Score += AddScore;

        //--- コンボが1以上の場合コンボボーナス付き加算
        m_Score += m_ComboCnt >= 1 ? AddScore + (m_ComboCnt / 10 + 1) * 10 : AddScore;

        m_ScoreNumberDrawScript.SetNumber(m_Score);
    }

    //--- スコア初期化
    public void ReSetScore()
    {
        m_Score = 0;
        m_ScoreNumberDrawScript.SetNumber(m_Score);
    }

    //--- 情報取得
    public int Score { get { return m_Score; } }
    public int Combo { get { return m_ComboCnt; } }

}
