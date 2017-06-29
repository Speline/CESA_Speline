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
    [SerializeField] private Image m_TimeImage;                     // 残り時間情報画像

    //--- 小目標表示用
    [SerializeField] private NumberDraw m_TargetNumberDrawScript;

    //--- メンバ関数 ------------------------------------------------------------------------------------------------------------
    TargetManager()
    {
        m_TimeLimitCounter = 10.0f;

        m_GameElapsedTime = 0.0f;

        m_NowTargetNumber = 0;

        m_UpdateTime = true;

    }

    void Awake()
    {
        m_TimeImage.fillAmount = 0.0f;

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

        StageOneData.ToList().ForEach(x =>m_TargetDataList.Add(new TargetData(int.Parse(x), 10f)));

    }
	
	// Update is called once per frame
	void Update ()
    {
        //--- 時間表示色変更
        if (m_TimeImage.fillAmount >= 0.6f)
        {
            m_TimeImage.color = Color.green;
        }
        else if (m_TimeImage.fillAmount >= 0.3f)
        {
            m_TimeImage.color = Color.yellow;
        }
        else if (m_TimeImage.fillAmount >= 0.0f)
        {
            m_TimeImage.color = Color.red;
        }

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
        m_TimeImage.fillAmount += 0.5f * Time.deltaTime;

        if (m_TimeImage.fillAmount > 1.0f)
            m_TimeImage.fillAmount = 1.0f;
    }

    void Game()
    {
        if (m_UpdateTime)
        {
            m_TimeLimitCounter -= Time.deltaTime;
            m_GameElapsedTime += Time.deltaTime;
        }

        //--- 制限時間表示
        m_TimeImage.fillAmount = m_TimeLimitCounter / 10.0f;

        //--- 一定時間ごとに判定（変更できるようにしておくか）
        //--- 目標数達成しているか
        if (m_ScoreManagerScript.Score >= m_TargetDataList[m_NowTargetNumber].TargetScore)
        {// 達成している場合
            Debug.Log("目標" + (m_NowTargetNumber + 1) + "達成");

            //m_GameTime += m_TargetDataList[m_NowTargetNumber].TimeLimit * 0.4f;

            m_TimeLimitCounter = 10.0f;

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

            //Scenemanager.Instance.LoadLevel("GameOver", 1.0f, 1.0f, 1.0f);
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
}
