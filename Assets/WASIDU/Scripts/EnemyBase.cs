using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    //--- メンバ変数
    // メンバ定数

    // 静的メンバ変数
    private static ScoreManager m_ScoreManagerScript;  // 目標管理スクリプト

    // メンバ変数
    protected Dictionary<string, bool> m_HitFlgDictionary;  // 当たっているフラグ
    protected bool m_bMove;
    protected int m_Number;
    protected int m_AddScoreNum = 100;

    //--- メンバ関数
    // コンストラクタ
    protected EnemyBase()
    {
        //--- 初期化
        m_HitFlgDictionary = new Dictionary<string, bool> {
            { "+X,0" , false }, // 右方向
            { "-X,0" , false }, // 左方向
            { "0,+Z" , false }, // 奥方向
            { "0,-Z" , false }, // 手前方向
            { "+X,+Z", false }, // 右奥方向
            { "+X,-Z", false }, // 右手前方向
            { "-X,+Z", false }, // 左奥方向
            { "-X,-Z", false }, // 左手前方向
        };

        m_bMove     = true;
        m_Number    = 0;
    }

    // Update is called once per frame
    protected void Update()
    {
        switch (GameManager.Instance.NowState)
        {
            case GameManager.GameState.SETTING:
            case GameManager.GameState.MAGIC_SQUARE_SETTING:
            case GameManager.GameState.PLAYER_SETTING:
            case GameManager.GameState.START:
            case GameManager.GameState.GAME_MAIN:
                break;

            case GameManager.GameState.GAME_CLEAR:
                break;

            case GameManager.GameState.GAME_OVER:
                if (transform.parent != null &&
                    transform.parent.tag == "SummonsBeast")
                {
                    transform.parent = null;
                    m_bMove = true;
                }
                break;
        }
	}

    //--- 更新(ループ中最後)
    void LateUpdate()
    {
        switch (GameManager.Instance.NowState)
        {
            case GameManager.GameState.SETTING:
            case GameManager.GameState.MAGIC_SQUARE_SETTING:
            case GameManager.GameState.PLAYER_SETTING:
            case GameManager.GameState.START:
            case GameManager.GameState.GAME_MAIN:
                break;

            case GameManager.GameState.GAME_CLEAR:
                DestryEnemy(true);
                break;

            case GameManager.GameState.GAME_OVER:
                break;
        }
    }

    // 当たり判定
    protected void OnTriggerEnter(Collider col)
    {
        //障害物との判定
        if (col.gameObject.tag == "SummonsBeast")
        {
            //--- フラグ設定
            SummonsBeast BeastData = col.gameObject.GetComponent<SummonsBeast>();

            float X = BeastData.MoveVec.x;
            float Z = BeastData.MoveVec.z;

            string HitVec = ""; // フラグ名

            //--- X軸の判定
            if (0.1f < X) HitVec += "+X";
            else if (-0.1f > X) HitVec += "-X";
            else HitVec += "0";

            //--- 「,」設定
            HitVec += ",";

            //--- Y軸の判定
            if (0.1f < Z) HitVec += "+Z";
            else if (-0.1f > Z) HitVec += "-Z";
            else HitVec += "0";

            //--- フラグTrue
            m_HitFlgDictionary[HitVec] = true;

            transform.SetParent(col.gameObject.transform);
            m_bMove = false;

            CheckDestry();
        }
    }

    //--- 消滅判定
    void CheckDestry()
    {
        if (m_HitFlgDictionary["+X,0" ] && m_HitFlgDictionary["-X,0" ] ||
            m_HitFlgDictionary["0,+Z" ] && m_HitFlgDictionary["0,-Z" ] ||
            m_HitFlgDictionary["+X,+Z"] && m_HitFlgDictionary["-X,-Z"] ||
            m_HitFlgDictionary["+X,-Z"] && m_HitFlgDictionary["-X,+Z"])
        {
            // 挟まれた時のエフェクト
            DestryEnemy();
        }
    }

    //--- 消滅
    public void DestryEnemy(bool GameEnd = false)
    {
        if (!GameEnd)
        {
            m_ScoreManagerScript.AddScore(m_AddScoreNum); // 得点加算

            //--- コンボ加算
            m_ScoreManagerScript.AddCombo();
        }

        //EnemyManager.DestroyEnemy(m_Number);
        EnemyManager.DestroyEnemy(this.gameObject);

        ParticleManager.Instance.NormalDeath.Play();
        ParticleManager.Instance.NormalDeathObj.transform.position = transform.position;

        Destroy(this.gameObject);
    }

    //--- 移動開始(アニメーション関係の設定がされてないので追加あるか)
    public void MoveStart()
    {
        m_bMove = true;
        //gameObject.transform.FindChild("EnemyA").GetComponent<Animator>().speed = 1.0f;
    }

    // 移動停止(アニメーション関係の設定がされてないので追加あるか)
    public void MoveStop()
    {
        m_bMove = false;
        //gameObject.transform.FindChild("EnemyA").GetComponent<Animator>().speed = 0.0f;
    }

    //--- 情報設定
    public int SetNomber { set { m_Number = value; } }
    public static ScoreManager ScoreManager { set { m_ScoreManagerScript = value; } }

    public int GetNomber { get { return m_Number; } }

}
