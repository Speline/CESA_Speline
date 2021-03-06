﻿//========================================================
// ゲーム管理
//========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    //--- ゲームステート
    public enum GameState
    {
        SETTING,
        MAGIC_SQUARE_SETTING,   // 魔方陣
        PLAYER_SETTING,         // プレイヤーキャラ
        GAME_START,             // スタート時
        GAME_MAIN,              // ゲームメイン
        PAUSE,                  // ポーズ
        GAME_CLEAR,             // ゲーム終了(成功)
        GAME_OVER,              // ゲーム終了(失敗)
    };


    //--- メンバ変数 ------------------------------------------------------------------------------------------------------------
    //--- 静的メンバ変数
    private static int m_StageNum = 1;

    //--- メンバ変数
    private GameState   m_NowState;             // 現在のステート
    private GameState   m_NextChangeState;      // 変更するステート
    private float       m_NowStateElapsedTime;  // 現在のステートの経過時間
    private bool        m_ChangedGameState;     // シーンが変更された次のループかどうか
    private bool        m_ScenceLoadFlg;		// シーン遷移の関数を1回しか呼ばないためのフラグ

    private bool m_TimeMoveEnd; // 時間表示が移動し終わったか
    private bool m_VolumeZero;  // 音量0かどうか

    // SerializeField
    [SerializeField] private GameObject m_EnemyParent;      // 敵の親オブジェクト
    [SerializeField] private GameObject m_GameOverCanvas;   // ゲームオーバーキャンバス
    [SerializeField] private GameObject m_ConfigCanvas;     // コンフィグ表示オブジェクト

    //--- メンバ関数 ------------------------------------------------------------------------------------------------------------
	void Awake ()
    {
        m_NowState = GameState.SETTING;
        m_NextChangeState   = GameState.MAGIC_SQUARE_SETTING;
        m_ChangedGameState  = false;
		m_ScenceLoadFlg     = false;

        //--- 敵親設定
        FinisherAtackObj.EnemyParent = m_EnemyParent;

        BGMManager.Instance.Play("Game_Main");

        m_GameOverCanvas.SetActive(false);
        m_ConfigCanvas.SetActive(false);

        m_NowStateElapsedTime = 0.0f;

        m_TimeMoveEnd = false;

        m_VolumeZero = false;

	}

    //--- 更新
    void Update()
    {
        m_NowStateElapsedTime += Time.deltaTime;


        switch (m_NowState)
        {
            case GameManager.GameState.MAGIC_SQUARE_SETTING:
                if (m_NowStateElapsedTime > 1.0f)
                    GameManager.Instance.ChangeState(GameManager.GameState.PLAYER_SETTING);
                break;

            case GameManager.GameState.PLAYER_SETTING:
                break;

            case GameManager.GameState.GAME_START:
                if (m_TimeMoveEnd)
                    ChangeState(GameState.GAME_MAIN);
                break;

            case GameManager.GameState.GAME_MAIN:
                break;

            case GameManager.GameState.PAUSE:
                break;

            case GameManager.GameState.GAME_CLEAR:
                if (m_NowStateElapsedTime > 5.0f && !m_ScenceLoadFlg)
                {
					m_ScenceLoadFlg = true;
                    Scenemanager.Instance.LoadLevel("Result", 1.0f, 1.0f, 1.0f);
                }
                break;

            case GameManager.GameState.GAME_OVER:
                if (m_NowStateElapsedTime > 5.0f && !m_ScenceLoadFlg)
                {
                    m_ScenceLoadFlg = true;
                    m_GameOverCanvas.SetActive(true);

                    SEManager.Instance.Play("ゲームオーバーのジングル");
                }
                break;
        }
    }

    //--- 更新(ループ中最後)
    void LateUpdate()
    {
        m_ChangedGameState = false;
        //--- ゲームメインステート変更
        if (m_NowState != m_NextChangeState)
        {
            m_ChangedGameState = true;
            m_NowState = m_NextChangeState;
            m_NowStateElapsedTime = 0.0f;
        }
    }

    //--- ゲームメインステート変更予約
    public void ChangeState(GameState SetState)
    {
        m_NextChangeState = SetState;
    }

    //--- 情報取得
    public GameState    NowState                { get { return m_NowState;              } }
    public float        GetNowStateElapsedTime  { get { return m_NowStateElapsedTime;   } }
    public bool         GetChangedState         { get { return m_ChangedGameState;      } }
    public static int   GetStage                { get { return m_StageNum;              } }

    //--- 情報設定
    public static int SetStage { set { m_StageNum = value;      } }
    public bool SetTimeMoveEnd { set { m_TimeMoveEnd = value;   } }

    public void ConfigCamvas(bool Active)
    {
        if (m_NowState != GameState.GAME_MAIN &&
            m_NowState != GameState.PAUSE)
            return;

        m_ConfigCanvas.SetActive(Active);

        if (Active)
        {
            ChangeState(GameState.PAUSE);
        }
        else
        {
            ChangeState(GameState.GAME_MAIN);
            SEManager.Instance.Play("アイテム発見");
        }
    }

    public void ChangeScence(string ScenceName)
    {
        SEManager.Instance.Play("アイテム発見");
        Scenemanager.Instance.LoadLevel(ScenceName,1.0f,1.0f,1.0f);
    }

    public void SetSoundVolume()
    {
        if (m_VolumeZero)
        {
            SEManager.Instance.SetVolume(1.0f);
            BGMManager.Instance.SetVolume(1.0f);

            m_VolumeZero = false;
        }
        else
        {
            SEManager.Instance.SetVolume(0.0f);
            BGMManager.Instance.SetVolume(0.0f);

            m_VolumeZero = true;
        }
    }
}
