//========================================================
// スコア管理
//========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //--- メンバ変数 ------------------------------------------------------------------------------------------------------------
    //--- メンバ変数
    [SerializeField] private NumberDraw m_ScoreNumberDrawScript; // スコア数字表示用スクリプト
    [SerializeField] private NumberDraw m_ComboNumberDrawScript; // コンボ数字表示用スクリプト
    [SerializeField] private GameObject m_ComboDraw;

    private int m_Score;    // スコア
    private int m_ComboCnt; // コンボ数

    // チュートリアル用
    TutorialManager_1 Tutorial_1;
    TutorialManager_2 Tutorial_2;

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

        m_ComboNumberDrawScript.SetDrawCenter = true;

        Tutorial_1 = GameObject.Find("Tutorial").GetComponent<TutorialManager_1>();
        Tutorial_2 = GameObject.Find("Tutorial").GetComponent<TutorialManager_2>();
    }

    // Update is called once per frame
    void Update()
    {
        //--- ステートによる変更処理
        switch (GameManager.Instance.NowState)
        {
            case GameManager.GameState.SETTING:
                m_ComboDraw.SetActive(false);
                break;

            case GameManager.GameState.MAGIC_SQUARE_SETTING:
            case GameManager.GameState.PLAYER_SETTING:
            case GameManager.GameState.GAME_START:
            case GameManager.GameState.GAME_MAIN:
                break;

            case GameManager.GameState.GAME_CLEAR:
                m_ComboDraw.SetActive(false);
                break;

            case GameManager.GameState.GAME_OVER:
                m_ComboDraw.SetActive(false);
                break;
        }
    }

    //--- コンボ設定
    public void SetCombo(bool bHit)
    {
        // 攻撃が消える時敵に当たっていたらコンボ加算
        if (!bHit)
        {
            //--- コンボ初期値に
            m_ComboCnt = 0;
            m_ComboNumberDrawScript.Reset();

            m_ComboDraw.SetActive(false);
        }
        else
        {
            m_ComboDraw.SetActive(true);
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

		// チュートリアル用
		if (Tutorial_1 != null && !Tutorial_1.GetbCanClear)			// ステージ1で、まだクリアしちゃダメな時は500のスコア制限
		{
			if (m_Score >= 500)
				m_Score = 500;
		}
		else if (Tutorial_2 != null && !Tutorial_2.HissatuInvocation)		// ステージ2で、必殺技を撃つまでは800のスコア制限
		{
			if (m_Score > 800)
				m_Score = 800;
		}

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
