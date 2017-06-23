using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ステージセレクトの状態遷移を管理する
public class StageSelect_Manager : MonoBehaviour
{
	public enum State_StageSelect
	{
		GROUP_MOVE = 0,		// 集団移動中
		WAITING,			// 集団移動が完了して何もしていない時間(先頭キャラが動き出すまでの時間)
		LEADCHAR_MOVE,		// 先頭キャラ移動中
		CAMERA_LINEAR_MOVE,	// カメラが直線軌道で移動して、手元を映す
		STAGE_FADE_OUT,		// ステージ画像とボタン出現
		STAGE_SELECT,		// ステージ選択中で何もしない
		STAGE_FADE_IN,		// ステージ画像とボタンが消える
		REQUEST_MOVE,		// 選択された依頼書がカウンターの方に移動
		CAMERA_MOVE_UP,		// カメラを退く
		CHAR_WARP_MOVE,		// キャラクターがワープ上に移動する
		SCENE_TRANSITION,	// シーン遷移

		FIN					// 終了
	};

	public State_StageSelect State = State_StageSelect.GROUP_MOVE;		// ステート変数
	bool bInitializ = true;										// 初期化フラグ(trueの時に初期化する)
	List<bool> bFlgs = new List<bool>();						// 各処理が終わったかどうかのフラグ

	// 状態を管理するスクリプト
	StageSelect_Char cs_Char;		// キャラクター
	StageSelect_Camera cs_Camera;	// カメラ
	StageManager cs_Stage;			// ステージ画像
	StageSelect_Button cs_Button;	// ボタン
	WhiteOut cs_WhiteOut;			// ホワイトアウト画像


	// Use this for initialization
	void Start ()
	{
		cs_Char = GameObject.Find("Characters").GetComponent<StageSelect_Char>();
		cs_Camera = GameObject.Find("Main Camera").GetComponent<StageSelect_Camera>();
		cs_Stage = GameObject.Find("StageManager").GetComponent<StageManager>();
		cs_Button = GameObject.Find("Canvas").GetComponent<StageSelect_Button>();
		cs_WhiteOut = GameObject.Find("WhiteOut").GetComponent<WhiteOut>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		switch(State)
		{
			// 集団移動
			case State_StageSelect.GROUP_MOVE:
				GroupMove();
				break;

			// 先頭キャラ移動待ち状態
			case State_StageSelect.WAITING:
				CharWaiting();
				break;

			// 先頭キャラ移動
			case State_StageSelect.LEADCHAR_MOVE:
				LEADCHAR_MOVE();
				break;

			// カメラ弧移動
			case State_StageSelect.CAMERA_LINEAR_MOVE:
				CAMERA_LINEAR_MOVE();
				break;

			// ステージ画像とボタン出現
			case State_StageSelect.STAGE_FADE_OUT:
				STAGE_FADE_OUT();
				break;

			// ステージ選択中
			case State_StageSelect.STAGE_SELECT:
				StageSelect();
				break;

			// ステージ画像とボタンが消える
			case State_StageSelect.STAGE_FADE_IN:
				StageFadeIn();
				break;

			// 選択された依頼書がカウンターの方に移動
			case State_StageSelect.REQUEST_MOVE:
				RequestMove();
				break;

			// カメラを退く
			case State_StageSelect.CAMERA_MOVE_UP:
				CameraMoveUp();
				break;

			// キャラクターがワープ上に移動する
			case State_StageSelect.CHAR_WARP_MOVE:
				CharWarpMove();
				break;

			// シーン遷移
			case State_StageSelect.SCENE_TRANSITION:
				SceneTransition();
				break;

			// 終わり
			case State_StageSelect.FIN:
				break;
		}
	}

	// 集団移動
	private void GroupMove()
	{
		if(bInitializ)
		{
			InitializFlgs(1);	//	このステートで使うフラグの数に初期化しておく

			bInitializ = false;	// 初期化終了
		}

		if (!bFlgs[0])
			bFlgs[0] = cs_Char.GroupMove();			// キャラクター集団移動

		// 終了判定
		if (CheckFlgs())
		{
			bInitializ = true;						// 初期化可能状態にする
			State = State_StageSelect.WAITING;		// 先頭キャラ移動待ち状態へ
		}
	}

	// 先頭キャラ移動待ち
	private void CharWaiting()
	{
		if (bInitializ)
		{
			InitializFlgs(1);	//	このステートで使うフラグの数に初期化しておく

			bInitializ = false;	// 初期化終了
		}

		if (!bFlgs[0])
			bFlgs[0] = cs_Char.CharWaiting();		// 先頭キャラ移動待ち

		if (CheckFlgs())
		{
			bInitializ = true;						// 初期化可能状態にする
			State = State_StageSelect.LEADCHAR_MOVE;	// 先頭キャラ移動状態へ
		}
	}

	// 先頭キャラ移動
	private void LEADCHAR_MOVE()
	{
		if (bInitializ)
		{
			InitializFlgs(2);	// このステートでは2個の関数を使う。

			bInitializ = false;	// 初期化終了
		}

		if (!bFlgs[0])
			bFlgs[0] = cs_Char.LeadCharMove();			// 先頭キャラのみ移動
		if (!bFlgs[1])
			bFlgs[1] = cs_Camera.Top2Quarter();			// カメラをトップビューから、クォータービューに

		if (CheckFlgs())
		{
			InitializFlgs(1);							// 次のステートでは1個の関数を使う。
			bInitializ = true;							// 初期化可能状態にする
			State = State_StageSelect.CAMERA_LINEAR_MOVE;	// カメラ弧移動へ
		}
	}

	// カメラが弧を描いて移動して、手元を映す
	private void CAMERA_LINEAR_MOVE()
	{
		if (bInitializ)
		{
			InitializFlgs(2);	// このステートでは2個の関数を使う。

			bInitializ = false;	// 初期化終了
		}

		if (!bFlgs[0])
			bFlgs[0] = cs_Camera.LinearMove();			// 手元を映すように、座標と注視点を変更する
		if (!bFlgs[1])
			bFlgs[1] = cs_Char.LeadCharFadeIn();		// 先頭キャラを透明にする

		if(CheckFlgs())
		{
			bInitializ = true;							// 初期化可能状態にする
			State = State_StageSelect.STAGE_FADE_OUT;		// ステージ画像出現へ
		}
	}

	// ステージ画像とボタン出現
	private void STAGE_FADE_OUT()
	{
		if (bInitializ)
		{
			InitializFlgs(2);	// このステートでは2個の関数を使う。

			bInitializ = false;	// 初期化終了
		}

		if (!bFlgs[0])
			bFlgs[0] = cs_Stage.StageSpriteFadeOut();		// ステージ画像を出現させる
		if (!bFlgs[1])
			bFlgs[1] = cs_Button.ButtonImageFadeOut();		// ボタンを出現させる

		if (CheckFlgs())
		{
			bInitializ = true;						// 初期化可能状態にする
			State = State_StageSelect.STAGE_SELECT;	// ステージセレクトへ
		}
	}

	// ステージ選択中
	private void StageSelect()
	{
		if (bInitializ)
		{
			InitializFlgs(1);	// このステートでは1個の関数を使う。

			bInitializ = false;	// 初期化終了
		}

		if (!bFlgs[0])
			bFlgs[0] = cs_Stage.StageSelect();		// ステージ選択中

		if (CheckFlgs())
		{
			bInitializ = true;						// 初期化可能状態にする
			State = State_StageSelect.STAGE_FADE_IN;	// ステージ画像とボタンが消える処理へ
		}
	}

	// ステージ画像とボタンが消える
	private void StageFadeIn()
	{
		if (bInitializ)
		{
			InitializFlgs(2);	// このステートでは2個の関数を使う。

			bInitializ = false;	// 初期化終了
		}

		if (!bFlgs[0])
			bFlgs[0] = cs_Stage.StageSpriteSlideOut();		// 左右のステージ画像が画面外に消える
		if (!bFlgs[1])
			bFlgs[1] = cs_Button.ButtonImageFadeIn();		// ボタンを消す

		if (CheckFlgs())
		{
			bInitializ = true;								// 初期化可能状態にする
			State = State_StageSelect.REQUEST_MOVE;			// ステージセレクトへ
		}
	}

	// 選択された依頼書がカウンターの方に移動
	private void RequestMove()
	{
		if (bInitializ)
		{
			InitializFlgs(2);	// このステートでは2個の関数を使う。

			bInitializ = false;	// 初期化終了
		}

		if (!bFlgs[0])
			bFlgs[0] = cs_Camera.CounterMove();		// 依頼書を追ってカウンターに近づく
		if (!bFlgs[1])
			bFlgs[1] = cs_Stage.CounterMove();		// 依頼書をカウンターの上に移動させる

		if (CheckFlgs())
		{
			bInitializ = true;						// 初期化可能状態にする
			State = State_StageSelect.CAMERA_MOVE_UP;	// カメラを退くへ
		}
	}

	// カメラを退く
	private void CameraMoveUp()
	{
		if (bInitializ)
		{
			InitializFlgs(1);	// このステートでは1個の関数を使う。

			// 1回呼ぶだけでいいので、初期化処理で実行
			GameObject.Find("Warp").GetComponent<StageSelect_Warp>().DrawWarp();		// ワープ表示
			cs_Char.LeadCharDraw();														// 先頭キャラ表示

			bInitializ = false;	// 初期化終了
		}

		if (!bFlgs[0])
			bFlgs[0] = cs_Camera.LookAtWarp();				// カメラを退く
			//bFlgs[0] = cs_Camera.MoveUp();				// カメラを退く

		if (CheckFlgs())
		{
			bInitializ = true;							// 初期化可能状態にする
			State = State_StageSelect.CHAR_WARP_MOVE;	// ワープ上に移動へ
		}
	}

	// キャラクターがワープ上に移動する
	private void CharWarpMove()
	{
		if (bInitializ)
		{
			InitializFlgs(1);	// このステートでは2個の関数を使う。

			bInitializ = false;	// 初期化終了
		}

		if (!bFlgs[0])
			bFlgs[0] = cs_Char.WarpMove();				// キャラクターをワープ魔法陣へ移動させる
		//if (!bFlgs[1])
		//	bFlgs[1] = cs_Camera.LookAtWarp();			// ワープ魔法陣の方を向く

		if (CheckFlgs())
		{
			bInitializ = true;							// 初期化可能状態にする
			State = State_StageSelect.SCENE_TRANSITION;	// シーン遷移へ
		}
	}

	// シーン遷移
	private void SceneTransition()
	{
		if (bInitializ)
		{
			GameObject.Find("Warp").GetComponent<StageSelect_Warp>().StartRotate();		// ワープ回転かいし

			InitializFlgs(1);	// このステートでは1個の関数を使う。

			bInitializ = false;	// 初期化終了
		}

		if (!bFlgs[0])
			bFlgs[0] = cs_WhiteOut.DrawWhiteOut();	// ホワイトアウト画像を描画する

		if (CheckFlgs())
		{
			// シーン遷移
			//Scenemanager.Instance.LoadLevel_NoFadeIn("GameMain", 0.5f, 0.5f);
			Scenemanager.Instance.LoadLevel("GameMain", 0.5f, 0.5f, 0.5f, Color.white);
			//SceneManager.LoadScene("GameMain");

			bInitializ = true;						// 初期化可能状態にする
			State = State_StageSelect.FIN;			// 終了
		}
	}





	// bFlgsの要素数をnum個にして、falseで初期化する
	private void InitializFlgs(int num)
	{
		bFlgs.Clear();		// 中身をすべて削除

		for(int i = 0 ; i < num ; i ++)
			bFlgs.Add(false);
	}

	// bFlgsの中身が全部tureかどうかを判定する
	private bool CheckFlgs()
	{
		for(int i = 0 ; i < bFlgs.Count ; i ++)
		{
			if(!bFlgs[i])
				return false;
		}

		return true;
	}
}
