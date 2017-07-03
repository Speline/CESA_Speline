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
    //--- メンバ定数

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

    // ゲーム開始時用
    private bool m_MoveSetPos;
    private bool m_SetEnd;
    private float m_MoveTime;

    //--- メンバ関数 ------------------------------------------------------------------------------------------------------------
    TargetManager()
    {
        m_TimeLimitCounter = 10.0f;

        m_GameElapsedTime = 0.0f;

        m_NowTargetNumber = 0;

        m_UpdateTime = true;

        m_MoveSetPos = false;
        m_SetEnd = false;
        m_MoveTime = 0.0f;
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

        //StageOneData.ToList().ForEach(x =>m_TargetDataList.Add(new TargetData(int.Parse(x), 10f)));


        //--- ステージで時間変更
        if (GameManager.GetStage < 6)
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
                Setting();
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

    void Setting()
    {

        ////--- 新しいスクリプトに作り直す予定
        //if (!m_MoveSetPos)
        //{
        //    m_TimeImage.fillAmount += 0.5f * Time.deltaTime;

        //    if (m_TimeImage.fillAmount >= 1.0f)
        //    {
        //        m_MoveSetPos = true;
        //    }
        //}
        //else if (!m_SetEnd)
        //{
        //    Debug.Log("m_MoveTime:" + m_MoveTime);

        //    m_MoveTime += Time.deltaTime;
        //    m_ClockImage.transform.position = Vector3.Lerp(new Vector3(0.0f,0.0f,0.0f), new Vector3(-316, 648, 0.0f), m_MoveTime);

        //    if (m_MoveTime > 1.0f)
        //    {
        //        m_SetEnd = true;
        //        GameManager.Instance.SetTimeMoveEnd = true;
        //    }
        //}
    }

    void Game()
    {
        if (m_UpdateTime)
        {
            m_TimeLimitCounter  -= Time.deltaTime;
            m_GameElapsedTime   += Time.deltaTime;
        }

        //--- 制限時間表示更新
        m_TimeDrawScript.SetTimeData = m_TimeLimitCounter / m_TargetDataList[m_NowTargetNumber].TimeLimit;

        //--- 目標数達成しているか
        if (m_ScoreManagerScript.Score >= m_TargetDataList[m_NowTargetNumber].TargetScore)
        {// 達成している場合
            Debug.Log("目標" + (m_NowTargetNumber + 1) + "達成");

            //m_GameTime += m_TargetDataList[m_NowTargetNumber].TimeLimit * 0.4f;

            //--- ステージで時間変更
            if (GameManager.GetStage < 6)
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
            GameManager.Instance.ChangeState(GameManager.GameState.GAME_OVER);
        }

        //--- 全ての目標を達成しているか
        if (m_NowTargetNumber >= m_TargetDataList.Count)
        {
            GameManager.Instance.ChangeState(GameManager.GameState.GAME_CLEAR);

            ResultManager.TimeandScore(m_GameElapsedTime,m_ScoreManagerScript.Score);
        }
    }

    public bool UpdateTime { set { m_UpdateTime = value; } }

    public bool SetEnd { get { return m_SetEnd; } }
}
