//========================================================
// 目標管理
//========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TargetManager : MonoBehaviour
{
    //--- 目標情報データ
    class TargetData
    {
        float m_TimeLimit;    // 制限時間
        int m_TargetScore;    // 目標得点

        public TargetData(int SetTargetScore, float SetTimeLimit)
        {
            m_TimeLimit     = SetTimeLimit;
            m_TargetScore   = SetTargetScore;
        }

        public float SetTimeLimit { set { m_TimeLimit = value; } }
        public float TimeLimit { get { return m_TimeLimit;  } }
        public int TargetScore { get { return m_TargetScore; } }
    };

    //--- メンバ変数 ------------------------------------------------------------------------------------------------------------
    //--- メンバ変数
    private float   m_GameElapsedTime;  // 経過時間
    private float   m_TimeLimitCounter; // 制限時間
    private bool    m_UpdateTime;

    private List<TargetData> m_TargetDataList;  // 目標情報(ステージにより変更できるようにList)
    private int m_NowTargetNumber;              // 現在の目標

    [SerializeField] private FinisherAtack  m_FinisherAtackScript;  // 必殺技ストック追加用
    [SerializeField] private ScoreManager   m_ScoreManagerScript;   // 現在のスコア取得用
    [SerializeField] private TimeDraw       m_TimeDrawScript;       // 残り時間表示スクリプト

    // 小目標表示用
    [SerializeField] private NumberDraw m_TargetNumberDrawScript;

    // チュートリアル用
    bool bTimeOver = false;

    //--- メンバ関数 ------------------------------------------------------------------------------------------------------------
    TargetManager()
    {
        m_TimeLimitCounter = 10.0f;

        m_GameElapsedTime = 0.0f;

        m_NowTargetNumber = 0;

        m_UpdateTime = true;

    }

    void Start()
    {
        m_TargetDataList = new List<TargetData>();
        //--- ステージの情報読み込み
        TextAsset StageData = Resources.Load("StageData/StageData") as TextAsset;   // テキストデータ取得
        string StageTextData = StageData.text;                                      // テキストデータをstringに
        string[] StageDataArray = StageTextData.Split('\n');                        // 行ごとに分ける(一種類ずつに分ける)
        string[] StageOneData = StageDataArray[GameManager.GetStage].Split(',');    // 情報ごとに分ける
        StageOneData.ToList().ForEach(x => x = x.Trim());                           // 空白部分削除

        //--- ステージで時間変更
		if (GameManager.GetStage == 0)
		{
			StageOneData.ToList().ForEach(x => m_TargetDataList.Add(new TargetData(int.Parse(x), 60f)));
			m_TimeLimitCounter = 60f;
		}
		else if(GameManager.GetStage == 1)
		{
			StageOneData.ToList().ForEach(x => m_TargetDataList.Add(new TargetData(int.Parse(x), 30f)));
			m_TimeLimitCounter = 30f;
		}
        else if (GameManager.GetStage < 6)
        {
            StageOneData.ToList().ForEach(x => m_TargetDataList.Add(new TargetData(int.Parse(x), 15f)));
            m_TimeLimitCounter = 15f;
        }
        else
        {
            StageOneData.ToList().ForEach(x => m_TargetDataList.Add(new TargetData(int.Parse(x), 10f)));
            m_TimeLimitCounter = 10f;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        //--- ステートによる変更処理
        switch (GameManager.Instance.NowState)
        {
            case GameManager.GameState.SETTING:
                m_TargetNumberDrawScript.SetNumber(m_TargetDataList[m_NowTargetNumber].TargetScore);
                break;

            case GameManager.GameState.MAGIC_SQUARE_SETTING:
            case GameManager.GameState.PLAYER_SETTING:
            case GameManager.GameState.GAME_START:
                break;

            case GameManager.GameState.GAME_MAIN:
                Game();
                break;

            case GameManager.GameState.GAME_CLEAR:
                break;

            case GameManager.GameState.GAME_OVER:
                break;
        }

	}

    void Game()
    {
        if (m_UpdateTime)
        {
            m_TimeLimitCounter -= Time.deltaTime;
            m_GameElapsedTime   += Time.deltaTime;
        }

        //--- 全ての目標を達成しているか
        if (m_NowTargetNumber >= m_TargetDataList.Count)
        {
            if (m_FinisherAtackScript.GetUseFinisher)
                return;

            GameManager.Instance.ChangeState(GameManager.GameState.GAME_CLEAR);

            BGMManager.Instance.Play("ファンファーレは誰のために？");

            if (!bTimeOver)
                ResultManager.TimeandScore(m_GameElapsedTime, m_ScoreManagerScript.Score);      // 普通のクリア
            else
            {
                if (m_GameElapsedTime >= 599.0f)
                    m_GameElapsedTime = 599.0f;

                ResultManager.TimeandScore(m_GameElapsedTime, m_ScoreManagerScript.Score);      // 1回でもタイムオーバーになっていたら、記録は残さない
            }
        }
        else
        {

            //--- 制限時間表示更新
            m_TimeDrawScript.TimeData = m_TimeLimitCounter / m_TargetDataList[m_NowTargetNumber].TimeLimit;

            //--- 目標数達成しているか
            if (m_ScoreManagerScript.Score >= m_TargetDataList[m_NowTargetNumber].TargetScore)
            {// 達成している場合
                //m_GameTime += m_TargetDataList[m_NowTargetNumber].TimeLimit * 0.4f;

                //--- ステージで時間変更
                if (GameManager.GetStage == 0)
                {
                    m_TimeLimitCounter = 60f;
                }
                else if (GameManager.GetStage == 1)
                {
                    m_TimeLimitCounter = 30f;
                }
                else if (GameManager.GetStage < 6)
                    m_TimeLimitCounter = 15f;
                else
                    m_TimeLimitCounter = 10f;

                m_NowTargetNumber++;    // 次の目標に

                if (m_NowTargetNumber < m_TargetDataList.Count)
                    m_TargetDataList[m_NowTargetNumber].SetTimeLimit = m_TimeLimitCounter;

                if (m_NowTargetNumber % 2 == 0)
                    m_FinisherAtackScript.AddFinisherStock();

                if (m_NowTargetNumber < m_TargetDataList.Count)
                    m_TargetNumberDrawScript.SetNumber(m_TargetDataList[m_NowTargetNumber].TargetScore);
            }

            if (m_TimeLimitCounter <= 0.0f)
            {
                if (GameManager.GetStage != 0 && GameManager.GetStage != 1)
                    GameManager.Instance.ChangeState(GameManager.GameState.GAME_OVER);
                else
                {
                    m_TimeLimitCounter = 0.0f;      // ステージが0か1なら、タイムを0にしたままゲーム継続
                    GameObject.Find("hdbcjbdsvisCanvas").GetComponent<Tutorial_Gameover>().GAMEOVER();
                }

            }

        }

    }

    public bool UpdateTime { set { m_UpdateTime = value; } }

    public void TimeReborn()
    {
        if (GameManager.GetStage == 0)
        {
            m_TimeLimitCounter = 60f;
        }
        else if (GameManager.GetStage == 1)
        {
            m_TimeLimitCounter = 30f;
        }

    }

}
