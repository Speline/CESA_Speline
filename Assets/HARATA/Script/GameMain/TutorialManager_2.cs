using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager_2 : MonoBehaviour
{
	enum State_Tutorial_2
	{
		SLIDEIN,			// キャラと枠がスライドインしてくる
		MESSAGE1,			// 敵を連続で倒すとコンボ達成、メッセージ
		MESSAGE2,			// コンボに応じてスコアボーナス、メッセージ
		MESSAGE3,			// 目標クリアでストック獲得、メッセージ
		MESSAGE4,			// キャラ1人をダブルタップ、メッセージ
		DOUBLE_TAP,			// ダブルタップ待ち
		MESSAGE5,			// 残り2人をタップ、メッセージ
		WAIT_HISSATU,		// 他の2人がタップされ、必殺技が発動するまで待つ
		HISSATU,			// 必殺技発動中
		MESSAGE6,			// あとは適当にがんばって、メッセージ
		FADEOUT,			// フェードアウト

		FIN
	};

	State_Tutorial_2 State = State_Tutorial_2.SLIDEIN;	// ステート変数
	bool bInitializ = true;							// 初期化フラグ(trueの時に初期化する)
	List<bool> bFlgs = new List<bool>();			// 各処理が終わったかどうかのフラグ

	// 状態を管理するスクリプト
	Tutorial_Waku cs_Waku;			// 枠
	Tutorial_Char cs_Char;			// チュートリアルキャラクター
	Tutorial_Message_2 cs_Message;	// メッセージ
	PlayerManager cs_Player;		// プレイヤーマネージャー
	ScoreManager cs_Score;			// スコア

	#region  他のスクリプトから参照される値

	bool bCanDoubleTap = false;		// ダブルタップしてもいいかどうか
	public bool GetbCanDoubleTap { get { return bCanDoubleTap; } }
	bool bDoubleTap = false;		// ダブルタップしたかどうか
	public bool SetbDoubleTap { set { bDoubleTap = value; } }

	bool bCanHissatu = false;		// 必殺技を撃っていいかどうか
	public bool CanHissatu { set { bCanHissatu = value; } get { return bCanHissatu; } }
	bool bHissatuInvocation;		// 必殺技が発動したかどうか
	public bool HissatuInvocation { set { bHissatuInvocation = value; } get { return bHissatuInvocation; } }
	bool bFinHissatu = false;		// 必殺技が終了したかどうか
	public bool FinHissatu { set { bFinHissatu = value; } get { return bFinHissatu; } }

	#endregion

	float fTime = 0.0f;

	Vector3 pos_waku;
	Vector3 pos_char;
	Vector3 pos_messa;


	void Awake()
	{
		// ステージ1でなければ、このスクリプトを削除する
		if (GameManager.GetStage != 1)
		{
			Destroy(this);
		}
	}

	// Use this for initialization
	void Start()
	{
		cs_Waku = GameObject.Find("Waku").GetComponent<Tutorial_Waku>();
		cs_Char = GameObject.Find("chara_tutorial").GetComponent<Tutorial_Char>();
		cs_Message = GameObject.Find("Message").GetComponent<Tutorial_Message_2>();
		cs_Player = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
		cs_Score = GameObject.Find("ManagerScripts").GetComponent<ScoreManager>();
	}

	// Update is called once per frame
	void Update()
	{
		switch (State)
		{
			// キャラと枠がスライドインしてくる
			case State_Tutorial_2.SLIDEIN:
				SlideIn();
				break;

			case State_Tutorial_2.MESSAGE1:
				Message1();
				break;

			case State_Tutorial_2.MESSAGE2:
				Message2();
				break;

			case State_Tutorial_2.MESSAGE3:
				Message3();
				break;

			case State_Tutorial_2.MESSAGE4:
				Message4();
				break;

			case State_Tutorial_2.DOUBLE_TAP:
				Double_Tap();
				break;

			case State_Tutorial_2.MESSAGE5:
				Message5();
				break;

			case State_Tutorial_2.WAIT_HISSATU:
				Wait_Hissatu();
				break;

			case State_Tutorial_2.HISSATU:
				Hissatu();
				break;

			case State_Tutorial_2.MESSAGE6:
				Message6();
				break;

			case State_Tutorial_2.FADEOUT:
				FadeOut();
				break;

			case State_Tutorial_2.FIN:
				break;
		}
	}

	private void SlideIn()
	{
		if (bInitializ)
		{
			InitializFlgs(2);	//	このステートで使うフラグの数に初期化しておく

			bInitializ = false;	// 初期化終了
		}

		if (!bFlgs[0])
			bFlgs[0] = cs_Waku.SlideIn();			// キャラクタースライドイン
		if (!bFlgs[1])
			bFlgs[1] = cs_Char.SlideIn();			// キャラクタースライドイン

		// 終了判定
		if (CheckFlgs())
		{
			bInitializ = true;						// 初期化可能状態にする
			State = State_Tutorial_2.MESSAGE1;		// キャラを1人タップ、へ
		}
	}

	private void Message1()
	{
		if (bInitializ)
		{
			InitializFlgs(1);	//	このステートで使うフラグの数に初期化しておく

			bInitializ = false;	// 初期化終了
		}

		if (!bFlgs[0])
			bFlgs[0] = cs_Message.Message1();		// 敵を連続で倒すとコンボ達成、メッセージ

		// 終了判定
		if (CheckFlgs())
		{
			bInitializ = true;						// 初期化可能状態にする
			State = State_Tutorial_2.MESSAGE2;		// コンボに応じてスコアボーナス、メッセージ
		}
	}

	private void Message2()
	{
		if (bInitializ)
		{
			fTime = 0.0f;

			InitializFlgs(1);	//	このステートで使うフラグの数に初期化しておく

			bInitializ = false;	// 初期化終了
		}

		// 2秒待つ
		fTime += Time.deltaTime;
		if (fTime < 2.0f)
			return;

		if (!bFlgs[0])
			bFlgs[0] = cs_Message.NextMessage();	// コンボに応じてスコアボーナス、メッセージ

		// 終了判定
		if (CheckFlgs())
		{
			bInitializ = true;						// 初期化可能状態にする
			State = State_Tutorial_2.MESSAGE3;		// 目標クリアでストック獲得、メッセージ
		}
	}

	private void Message3()
	{
		if (bInitializ)
		{
			InitializFlgs(1);	//	このステートで使うフラグの数に初期化しておく

			bInitializ = false;	// 初期化終了
		}

		if(cs_Score.Score < 600)
			return;


		if (!bFlgs[0])
			bFlgs[0] = cs_Message.NextMessage();	// 目標クリアでストック獲得、メッセージ

		// 終了判定
		if (CheckFlgs())
		{
			bInitializ = true;						// 初期化可能状態にする
			State = State_Tutorial_2.MESSAGE4;		// キャラ1人をダブルタップ、メッセージ
		}
	}

	private void Message4()
	{
		if (bInitializ)
		{
			InitializFlgs(1);	//	このステートで使うフラグの数に初期化しておく

			bInitializ = false;	// 初期化終了
		}

		if (cs_Score.Score < 800)					// スコア800でメッセージをだす
			return;

		if (!bFlgs[0])
			bFlgs[0] = cs_Message.NextMessage();	// キャラ1人をダブルタップ、メッセージ

		// 終了判定
		if (CheckFlgs())
		{
			bInitializ = true;						// 初期化可能状態にする
			State = State_Tutorial_2.DOUBLE_TAP;	// ダブルタップ待ち
		}
	}

	private void Double_Tap()
	{
		if (bInitializ)
		{
			bCanDoubleTap = true;		// ダブルタップ可能状態にする

			InitializFlgs(1);	//	このステートで使うフラグの数に初期化しておく

			bInitializ = false;	// 初期化終了
		}

		if (!bFlgs[0])
			bFlgs[0] = bDoubleTap;	// ダブルタップ待ち

		// 終了判定
		if (CheckFlgs())
		{
			bCanHissatu = true;						// ダブルタップされ、必殺技を撃つことが確定したので、trueにしてスコア制限を解放できるようにする
			bInitializ = true;						// 初期化可能状態にする
			State = State_Tutorial_2.MESSAGE5;		// 残り2人をタップ、メッセージ
		}
	}

	private void Message5()
	{
		if (bInitializ)
		{
			InitializFlgs(1);	//	このステートで使うフラグの数に初期化しておく

			bInitializ = false;	// 初期化終了
		}


		if (!bFlgs[0])
			bFlgs[0] = cs_Message.NextMessage();	// 残り2人をタップ、メッセージ

		// 終了判定
		if (CheckFlgs())
		{
			bInitializ = true;							// 初期化可能状態にする
			State = State_Tutorial_2.WAIT_HISSATU;		// 必殺技発動待ちへ
		}
	}

	private void Wait_Hissatu()
	{
		if (bInitializ)
		{
			InitializFlgs(1);	//	このステートで使うフラグの数に初期化しておく

			bInitializ = false;	// 初期化終了
		}


		if (!bFlgs[0])
			bFlgs[0] = bHissatuInvocation;			// 必殺技発動待ち

		// 終了判定
		if (CheckFlgs())
		{
			bInitializ = true;						// 初期化可能状態にする
			State = State_Tutorial_2.HISSATU;		// 必殺技発動中へ
		}
	}

	private void Hissatu()
	{
		if (bInitializ)
		{
			// 必殺技発動中はカメラから見えないとこにいる。
			pos_waku = cs_Waku.gameObject.transform.localPosition;		// 座標保存
			pos_char = cs_Char.gameObject.transform.localPosition;
			pos_messa = cs_Message.gameObject.transform.localPosition;

			cs_Waku.gameObject.transform.position = new Vector3(100.0f, 100.0f, 100.0f);		// 適当にぶっとばす
			cs_Char.gameObject.transform.position = new Vector3(100.0f, 100.0f, 100.0f);		// 適当にぶっとばす
			cs_Message.gameObject.transform.position = new Vector3(100.0f, 100.0f, 100.0f);		// 適当にぶっとばす

			InitializFlgs(1);	//	このステートで使うフラグの数に初期化しておく

			bInitializ = false;	// 初期化終了
		}


		if (!bFlgs[0])
			bFlgs[0] = bFinHissatu;					// 必殺技終了待ち

		// 終了判定
		if (CheckFlgs())
		{
			GameObject.Find("Tutorial").GetComponent<Transform>().eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);

			cs_Waku.gameObject.transform.localPosition = pos_waku;		// 座標を元に戻す
			cs_Char.gameObject.transform.localPosition = pos_char;
			cs_Message.gameObject.transform.localPosition = pos_messa;


			bInitializ = true;						// 初期化可能状態にする
			State = State_Tutorial_2.MESSAGE6;		// あとは適当にがんばってへ
		}
	}

	private void Message6()
	{
		if (bInitializ)
		{
			InitializFlgs(1);	//	このステートで使うフラグの数に初期化しておく

			bInitializ = false;	// 初期化終了
		}


		if (!bFlgs[0])
			bFlgs[0] = cs_Message.NextMessage();	// あとは適当にがんばって

		// 終了判定
		if (CheckFlgs())
		{
			bInitializ = true;						// 初期化可能状態にする
			State = State_Tutorial_2.FADEOUT;		// フェードアウトへ
		}
	}

	private void FadeOut()
	{
		if (bInitializ)
		{
			fTime = 0.0f;
			InitializFlgs(3);	//	このステートで使うフラグの数に初期化しておく

			bInitializ = false;	// 初期化終了
		}

		// 少し待つ
		fTime += Time.deltaTime;
		if(fTime < 1.5f)
			return;

		if (!bFlgs[0])
			bFlgs[0] = cs_Waku.FadeOut();	// 枠のフェードアウト
		if (!bFlgs[1])
			bFlgs[1] = cs_Char.FadeOut();	// キャラのフェードアウト
		if (!bFlgs[2])
			bFlgs[2] = cs_Message.FinFadeOut();	// メッセージのフェードアウト
			
		// 終了判定
		if (CheckFlgs())
		{
			bInitializ = true;						// 初期化可能状態にする
			State = State_Tutorial_2.FADEOUT;		// フェードアウトへ
		}
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
}
