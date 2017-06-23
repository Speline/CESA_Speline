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
        START,                  // スタート時
        GAME_MAIN,              // ゲームメイン
        GAME_CLEAR,             // ゲーム終了(成功)
        GAME_OVER,              // ゲーム終了(失敗)
    };


    //--- メンバ変数 ------------------------------------------------------------------------------------------------------------
    //--- メンバ定数

    //--- 静的メンバ変数
    private static int m_StageNum = 0;

    //--- メンバ変数
    private GameState m_NowState;           // 現在のステート
    private GameState m_NextChangeState;    // 変更するステート
    private float m_NowStateElapsedTime;    // 現在のステートの経過時間
	private bool m_bEndFlg;					// シーン遷移の関数を1回しか呼ばないためのフラグ

    // SerializeField
    [SerializeField] private GameObject m_EnemyParent;      // 敵の親オブジェクト
    [SerializeField] private GameObject m_GameOverCanvas;   // ゲームオーバーキャンバス

    //--- メンバ関数 ------------------------------------------------------------------------------------------------------------
	void Awake ()
    {
        m_NowState = GameState.SETTING;
        m_NextChangeState = GameState.MAGIC_SQUARE_SETTING;
		m_bEndFlg = false;

        //--- 敵親設定
        FinisherAtackObj.EnemyParent = m_EnemyParent;

        BGMManager.Instance.Play("Game_Main");

        m_GameOverCanvas.SetActive(false);

        m_NowStateElapsedTime = 0.0f;
	}

    //--- 更新
    void Update()
    {
        m_NowStateElapsedTime += Time.deltaTime;

        switch (m_NowState)
        {
            case GameManager.GameState.SETTING:
                EnemyManager.StageEnemy(m_StageNum);    // ステージ情報設定
                ChangeState(GameState.MAGIC_SQUARE_SETTING);
                break;

            case GameManager.GameState.MAGIC_SQUARE_SETTING:
            case GameManager.GameState.PLAYER_SETTING:
                break;

            case GameManager.GameState.START:
                ChangeState(GameState.GAME_MAIN);
                break;

            case GameManager.GameState.GAME_MAIN:
                break;

            case GameManager.GameState.GAME_CLEAR:
                if (m_NowStateElapsedTime > 2.0f && !m_bEndFlg)
                {
					m_bEndFlg = true;
                    Scenemanager.Instance.LoadLevel("Result", 1.0f, 1.0f, 1.0f);
                }
                break;

            case GameManager.GameState.GAME_OVER:
                m_GameOverCanvas.SetActive(true);
                break;
        }
    }

    //--- 更新(ループ中最後)
    void LateUpdate()
    {
        //--- ゲームメインステート変更
        if (m_NowState != m_NextChangeState)
        {
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
    public GameState    NowState { get { return m_NowState; } }
    public static int   GetStage { get { return m_StageNum; } }

    //--- 情報設定
    public static int SetStage { set { m_StageNum = value; } }

    public void ChangeScence(string ScenceName)
    {
        Scenemanager.Instance.LoadLevel(ScenceName,1.0f,1.0f,1.0f);
    }
}
