using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager_1 : MonoBehaviour
{
	enum State_Tutorial_1
	{
		SLIDEIN,			// キャラと枠がスライドインしてくる
		MESSAGE1,			// キャラを1人タップ、メッセージ
		CHAR_TAP,			// キャラタップ待ち
		MESSAGE2,			// 対面キャラをタップ、メッセージ
		OPPOSITE_TAP,		// 対面のキャラタップ待ち
		MESSAGE3,			// 敵を倒すとスコア上昇、メッセージ
		ENEMY_DEFEAT,		// 敵を1体撃破する
		MESSAGE4,			// 目標達成でスコアを更新、メッセージ
		MESSAGE5,			// 敵を倒してスコアを稼ぐ、メッセージ
		MESSAGE6,			// 最終目標を達成でクリア、メッセージ

		FIN
	};

	State_Tutorial_1 State = State_Tutorial_1.SLIDEIN;	// ステート変数
	bool bInitializ = true;							// 初期化フラグ(trueの時に初期化する)
	List<bool> bFlgs = new List<bool>();			// 各処理が終わったかどうかのフラグ

	// 状態を管理するスクリプト
	Tutorial_Waku cs_Waku;			// 枠
	Tutorial_Char cs_Char;			// チュートリアルキャラクター
	Tutorial_Message_2 cs_Message;	// メッセージ
	PlayerManager cs_Player;		// プレイヤーマネージャー
	ScoreManager cs_Score;			// スコア

	#region  他のスクリプトから参照される値

	bool bCanCharTap = false;		// キャラタップ判定をしてもいいのかどうか
	public bool GetbCanCharTap { get { return bCanCharTap; } }
	bool bCharTap = false;			// キャラタップ情報
	public bool SetbCharTap { set { bCharTap = value; } }

	bool bCanOppositeTap;			// 対面キャラタップ判定をしてもいいのかどうか
	public bool GetbCanOppositeTap { get { return bCanOppositeTap; } }
	bool bOppositeTap;				// 対面キャラタップ情報
	public bool SetbOppositeTap { set { bOppositeTap = value; } }

	bool bCanEnemyDefeat = false;	// 敵撃破判定をしてもいいのかどうか
	public bool GetbCanEnemyDefeat { get { return bCanEnemyDefeat; } }
	bool bEnemyDefeat = false;		// 敵撃破情報
	public bool SetbEnemyDefeat { set { bEnemyDefeat = value; } }

	bool bCanClear = false;			// クリアしてもいいかどうか
	public bool GetbCanClear { get {return bCanClear; } }

	#endregion

	float fTime = 0.0f;


	void Awake()
	{
		// ステージ1でも2でもなければオブジェクトごと削除する
		if(GameManager.GetStage != 0 && GameManager.GetStage != 1)
		{
			Destroy(this.gameObject);
			return;
		}

		// ステージ0でなければ、このスクリプトを削除する
		if( GameManager.GetStage != 0 )
		{
			Destroy(this);
		}
	}

	// Use this for initialization
	void Start ()
	{
		cs_Waku = GameObject.Find("Waku").GetComponent<Tutorial_Waku>();
		cs_Char = GameObject.Find("chara_tutorial").GetComponent<Tutorial_Char>();
		cs_Message = GameObject.Find("Message").GetComponent<Tutorial_Message_2>();
		cs_Player = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
		cs_Score = GameObject.Find("ManagerScripts").GetComponent<ScoreManager>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		switch(State)
		{
			// キャラと枠がスライドインしてくる
			case State_Tutorial_1.SLIDEIN:
				SlideIn();
				break;

			case State_Tutorial_1.MESSAGE1:
				Message1();
				break;

			case State_Tutorial_1.CHAR_TAP:
				Char_Tap();
				break;

			case State_Tutorial_1.MESSAGE2:
				Message2();
				break;

			case State_Tutorial_1.OPPOSITE_TAP:
				Opposite_Tap();
				break;

			case State_Tutorial_1.MESSAGE3:
				Message3();
				break;

			case State_Tutorial_1.ENEMY_DEFEAT:
				Enemy_Defeat();
				break;

			case State_Tutorial_1.MESSAGE4:
				Message4();
				break;

			case State_Tutorial_1.MESSAGE5:
				Message5();
				break;

			case State_Tutorial_1.MESSAGE6:
				Message6();
				break;

			case State_Tutorial_1.FIN:
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
			State = State_Tutorial_1.MESSAGE1;		// キャラを1人タップ、へ
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
			bFlgs[0] = cs_Message.Message1();		// キャラを1人タップ

		// 終了判定
		if (CheckFlgs())
		{
			bInitializ = true;						// 初期化可能状態にする
			State = State_Tutorial_1.CHAR_TAP;		// キャラタップ待ちへ
		}
	}

	private void Char_Tap()
	{
		if (bInitializ)
		{
			InitializFlgs(1);	//	このステートで使うフラグの数に初期化しておく

			bCanCharTap = true;			// キャラタップ待ち状態

			bInitializ = false;	// 初期化終了
		}

		if (!bFlgs[0])
			bFlgs[0] = bCharTap;			// キャラタップ待ち

		// 終了判定
		if (CheckFlgs())
		{
			bInitializ = true;						// 初期化可能状態にする
			bCanCharTap = false;					// キャラタップ待ち終了
			State = State_Tutorial_1.MESSAGE2;		// 対面キャラをタップ、メッセージ
			cs_Char.StartAttackAnime();				// 攻撃アニメーション終了
		}
	}

	private void Message2()
	{
		if (bInitializ)
		{
			InitializFlgs(1);	//	このステートで使うフラグの数に初期化しておく

			bInitializ = false;	// 初期化終了
		}

		if (!bFlgs[0])
			bFlgs[0] = cs_Message.NextMessage();	// 対面キャラをタップ、メッセージ
			
		// 終了判定
		if (CheckFlgs())
		{
			bInitializ = true;						// 初期化可能状態にする
			State = State_Tutorial_1.OPPOSITE_TAP;	// 対面キャラをタッチ
		}
	}

	private void Opposite_Tap()
	{
		if (bInitializ)
		{
			InitializFlgs(1);	//	このステートで使うフラグの数に初期化しておく

			bCanOppositeTap = true;		// 対面キャラタップ可能

			bInitializ = false;	// 初期化終了
		}

		if (!bFlgs[0])
			bFlgs[0] = bOppositeTap;		// 対面キャラタップ待ち

		// 終了判定
		if (CheckFlgs())
		{
			bInitializ = true;						// 初期化可能状態にする
			bCanOppositeTap = false;		// 対面キャラタップ不可能
			State = State_Tutorial_1.MESSAGE3;		// 敵を倒すとスコア上昇、メッセージ
		}
	}

	private void Message3()
	{
		if (bInitializ)
		{
			InitializFlgs(1);	//	このステートで使うフラグの数に初期化しておく

			bInitializ = false;	// 初期化終了
		}

		if (!bFlgs[0])
			bFlgs[0] = cs_Message.NextMessage();	// 敵を倒すとスコア上昇、メッセージ

		// 終了判定
		if (CheckFlgs())
		{
			bInitializ = true;						// 初期化可能状態にする
			State = State_Tutorial_1.ENEMY_DEFEAT;	// 敵を1体撃破する
		}
	}

	private void Enemy_Defeat()
	{
		if (bInitializ)
		{
			InitializFlgs(1);	//	このステートで使うフラグの数に初期化しておく

			bCanEnemyDefeat = true;		// 敵撃破判定をする

			bInitializ = false;	// 初期化終了
		}

		if (!bFlgs[0])
			bFlgs[0] = bEnemyDefeat;		// 敵撃破待ち

		// 終了判定
		if (CheckFlgs())
		{
			bInitializ = true;						// 初期化可能状態にする
			State = State_Tutorial_1.MESSAGE4;		// 目標達成でスコアを更新、メッセージ
		}
	}

	private void Message4()
	{
		if (bInitializ)
		{
			InitializFlgs(1);	//	このステートで使うフラグの数に初期化しておく

			bInitializ = false;	// 初期化終了
		}

		if (!bFlgs[0])
			bFlgs[0] = cs_Message.NextMessage();	// 目標達成でスコアを更新、メッセージ

		// 終了判定
		if (CheckFlgs())
		{
			bInitializ = true;						// 初期化可能状態にする
			State = State_Tutorial_1.MESSAGE5;		// 敵を倒してスコアを稼ぐ、メッセージ
		}
	}

	private void Message5()
	{
		if (bInitializ)
		{
			fTime = 0.0f;
			InitializFlgs(1);	//	このステートで使うフラグの数に初期化しておく

			bInitializ = false;	// 初期化終了
		}

		// 1秒待つ
		fTime += Time.deltaTime;
		if(fTime < 1.0f)
			return;

		if (!bFlgs[0])
			bFlgs[0] = cs_Message.NextMessage();	// 敵を倒してスコアを稼ぐ、メッセージ

		// 終了判定
		if (CheckFlgs())
		{
			bInitializ = true;						// 初期化可能状態にする
			State = State_Tutorial_1.MESSAGE6;		// 終了
		}
	}

	private void Message6()
	{
		if (bInitializ)
		{
			InitializFlgs(1);	//	このステートで使うフラグの数に初期化しておく

			bInitializ = false;	// 初期化終了
		}

		if (cs_Score.Score < 500)
			return;
		else if(cs_Score.Score == 500)
		{
			bCanClear = true;
		}

		if (!bFlgs[0])
			bFlgs[0] = cs_Message.NextMessage();	// キャラを1人タップ

		// 終了判定
		if (CheckFlgs())
		{
			bInitializ = true;						// 最終目標を達成でクリア、メッセージ
			State = State_Tutorial_1.FIN;			// 終了
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
