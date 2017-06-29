//========================================================
// ゲームメインに使うオブジェクトのベース
//========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainObjectBase : MonoBehaviour
{

	// Update is called once per frame
    void Update()
    {
        switch (GameManager.Instance.NowState)
        {
            case GameManager.GameState.SETTING:
                Setting();
                break;

            case GameManager.GameState.MAGIC_SQUARE_SETTING:
                MagicSquareSetting();
                break;

            case GameManager.GameState.PLAYER_SETTING:
                PlayerSetting();
                break;

            case GameManager.GameState.GAME_START:
                GameStart();
                break;

            case GameManager.GameState.GAME_MAIN:
                GameMain();
                break;

            case GameManager.GameState.PAUSE:
                Pause();
                break;

            case GameManager.GameState.GAME_CLEAR:
                GameClear();
                break;

            case GameManager.GameState.GAME_OVER:
                GameOver();
                break;
        }
	}

    //--- オーバーライドする関数
    protected virtual void Setting(){}              // 初期設定
    protected virtual void MagicSquareSetting(){}   // 魔方陣出現
    protected virtual void PlayerSetting(){}        // プレイヤー出現
    protected virtual void GameStart(){}            // ゲーム開始時
    protected virtual void GameMain(){}             // ゲームメイン
    protected virtual void Pause(){}                // ポーズ中
    protected virtual void GameClear(){}            // ゲームクリア時
    protected virtual void GameOver(){}             // ゲームオーバー時

}
