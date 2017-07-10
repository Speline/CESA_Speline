using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Gameover : MonoBehaviour
{
	enum State_Tutorial_GameOver
	{
		GAMEOVER_FADEIN,
		TAP_FADEIN,
		TAPWAIT,

		FIN
	};

	State_Tutorial_GameOver State = State_Tutorial_GameOver.GAMEOVER_FADEIN;	// ステート変数
	bool bInitializ = true;							// 初期化フラグ(trueの時に初期化する)
	List<bool> bFlgs = new List<bool>();			// 各処理が終わったかどうかのフラグ
	float fTime = 0.0f;

	// 状態を管理するスクリプト
	Image GrayImage;
	Tutorial_GameOverImage cs_GameOverImage;
	Tutorial_Tap	cs_Tap;

	#region  他のスクリプトから参照される値

	#endregion


	// Use this for initialization
	void Start ()
	{
		State = State_Tutorial_GameOver.FIN;

		GrayImage = GameObject.Find("GrayButton").GetComponent<Image>();
		cs_GameOverImage = GameObject.Find("GameOverImage").GetComponent<Tutorial_GameOverImage>();
		cs_Tap = GameObject.Find("TapImage").GetComponent<Tutorial_Tap>();

		GrayImage.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		switch (State)
		{
			// キャラと枠がスライドインしてくる
			case State_Tutorial_GameOver.GAMEOVER_FADEIN:
				GameOver_FadeIn();
				break;

			case State_Tutorial_GameOver.TAP_FADEIN:
				Tap_FadeIn();
				break;

			case State_Tutorial_GameOver.TAPWAIT:
				break;

			case State_Tutorial_GameOver.FIN:
				break;
		}
	}

	void GameOver_FadeIn()
	{
		if (bInitializ)
		{
			GameManager.Instance.ChangeState(GameManager.GameState.PAUSE);
			GrayImage.enabled = true;	// 画面を暗くする

			InitializFlgs(1);	//	このステートで使うフラグの数に初期化しておく

			bInitializ = false;	// 初期化終了
		}

		if (!bFlgs[0])
			bFlgs[0] = cs_GameOverImage.FadeIn();

		// 終了判定
		if (CheckFlgs())
		{
			bInitializ = true;						// 初期化可能状態にする
			State = State_Tutorial_GameOver.TAP_FADEIN;		// キャラを1人タップ、へ
		}
	}

	void Tap_FadeIn()
	{
		if (bInitializ)
		{
			InitializFlgs(1);	//	このステートで使うフラグの数に初期化しておく

			bInitializ = false;	// 初期化終了
		}

		if (!bFlgs[0])
			bFlgs[0] = cs_Tap.FadeIn();

		// 終了判定
		if (CheckFlgs())
		{
			bInitializ = true;						// 初期化可能状態にする
			State = State_Tutorial_GameOver.TAPWAIT;		// キャラを1人タップ、へ
		}
	}




	public void TAPSARETA()
	{
		if(State != State_Tutorial_GameOver.TAPWAIT)
			return;

		GameManager.Instance.ChangeState(GameManager.GameState.GAME_MAIN);
		GameObject.Find("ManagerScripts").GetComponent<TargetManager>().TimeReborn();
		cs_Tap.UnDraw();
		cs_GameOverImage.UnDraw();
		GrayImage.enabled = false;
	}




	// bFlgsの要素数をnum個にして、falseで初期化する
	private void InitializFlgs(int num)
	{
		bFlgs.Clear();		// 中身をすべて削除

		for (int i = 0; i < num; i++)
			bFlgs.Add(false);
	}

	// bFlgsの中身が全部tureかどうかを判定する
	private bool CheckFlgs()
	{
		for (int i = 0; i < bFlgs.Count; i++)
		{
			if (!bFlgs[i])
				return false;
		}

		return true;
	}



	public void GAMEOVER()
	{
		State = State_Tutorial_GameOver.GAMEOVER_FADEIN;
	}
}
