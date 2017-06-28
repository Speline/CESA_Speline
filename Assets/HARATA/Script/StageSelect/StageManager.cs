using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
	[SerializeField]	GameObject[] StageObj;
	Transform[] Trans;
	[SerializeField]	float fSpace;		// スプライトの間隔
	int nMinus;								// 要素数からこの数字を引いた数字以上の添え字の座標をマイナスする
	float[] StartPos;						// スタート地点

	// フラグ
	bool bMoveTouch = false;				// タッチに追従フラグ
	bool bInertia = false;					// 慣性移動中フラグ
	bool bMovesCloser = false;				// 近いところに移動中フラグ
	bool bButtonMove = false;				// ボタンを押されて、隣に移動フラグ
	
	// 慣性移動
	float fMove;							// 移動量
	float fInertia = 0;						// 慣性移動で使うスピード
	[SerializeField, Range(0, 1)]	float fDeceleration;		// 減速の割合
	[SerializeField]	float fFinishInertia;					// 慣性移動終了の基準

	// 近いところに移動
	float fHeni;							// 変位
	float fTime = 0.0f;						// 移動が始まってからの時間
	[SerializeField]	float fMoveTime;	// 近いところに移動するのにかける時間
	float fStartSpeed;						// 初速度
	float fAccele;							// 加速度
	float[] fKijunPos;						// 基準座標

	// ボタン移動
	[SerializeField]	float fBottonMoveTime;	// ボタンを押された時に、何秒で隣に移動するか
	int nRightCnt = 0, nLeftCnt = 0;			// ボタン移動の連続で押されたカウンタ
	float fMoveDistance = 0;					// ボタン移動での進んだ距離
	float fPreMoveDistance = 0;					// 前回のボタン移動での進んだ距離
	float fTotalHeni = 0;						// ボタンが2回以上押された時用の、初期位置からの総変位量
	float fBugResolution;						// バグ回避用(なぜか1回代入するとバグが解決したので)
	float fSearchNearDistance = 0;

	// ステージ画像出現
	SpriteRenderer[] sr;
	float fParameter;							// パラメーター変数
	[SerializeField]	float SpriteFadeOutTime;// 画像出現時間
	bool bInitializ = true;						// 初期化フラグ
	GameObject DecisionObj = null;				// ステージ決定したオブジェクト

	// 選択したステージの左右の画像が左右にスライドアウトする
	[SerializeField]	float fSlideOutTime;	// スライドアウトにかかる時間
	[SerializeField]	float fSlideOutDistance;// スライドアウトする距離
	int nStageNo;								// 選択したステージ番号
	int[] nMoveStageNo = new int[2];			// 左右に動かすオブジェクトの添え字

	// 縮小しながらカウンター上に移動
	[SerializeField]	Vector3 vCounterOffsetPos;	// カウンターに近づいていく時の、カウンターの中心からどれだけ離れているかの数値
	[SerializeField]	float fCounterMoveTime;		// カウンターに近づいていくのにかける時間
	Transform Trans_Counter;						// カウンター
	Vector3 vStartPos;								// 移動開始座標
	[SerializeField]	float fCounterScale;		// 最終的な拡大率
	float fStartScale;								// 移動開始時の拡大率


	// Use this for initialization
	void Start()
	{
		// 可変配列作成(ここからは変わらない)、必要コンポーネント取得
		Trans = new Transform[StageObj.GetLength(0)];
		StartPos = new float[StageObj.GetLength(0)];
		fKijunPos = new float[StageObj.GetLength(0)];
		sr = new SpriteRenderer[StageObj.GetLength(0)];
		Trans_Counter = GameObject.Find("Counter").GetComponent<Transform>();

		// 座標をマイナスにする添え字計算
		nMinus = StageObj.GetLength(0);
		if(StageObj.GetLength(0) % 2 == 0)
			nMinus /= 2;
		else
			nMinus --;
		nMinus --;

		float fPos;
		for (int i = 0; i < StageObj.GetLength(0); i++)
		{
			Trans[i] = StageObj[i].GetComponent<Transform>();	// 子オブジェクト取得
			sr[i] = StageObj[i].GetComponent<SpriteRenderer>();	// SpriteRenderer取得
			sr[i].color = new Color(1.0f, 1.0f, 1.0f, 0.0f);	// 透明にしておく

			// 初期位置セット
			fPos = fSpace * i;
			if(i >= StageObj.GetLength(0) - nMinus)
				fPos -= fSpace * StageObj.GetLength(0);

			StartPos[i] = fPos;
			Trans[i].localPosition = new Vector3(fPos, Trans[i].localPosition.y, Trans[i].localPosition.z);
        }
        BGMManager.Instance.Play("Stage_select");
	}

	// Update is called once per frame
	void Update()
	{

	}


	// 慣性移動関係
	private void Inertia()
	{
		fMove = InputManager.Instance.GetDeltaPosition().x * 10;	// *10はタッチ用(x90度にしたら値がすごく小さくなって、フリックできなくなったので、適当に大きくして調整)
		//fMove = Input.GetAxis("Mouse X") * 0.05f;	// 0609こっちに切り替えた

		// タッチに追従中に指を放したら、慣性移動開始
		if (Input.GetButtonUp("Fire1") && bMoveTouch)
		{
			fInertia = fMove;				// 移動量保存
			bMoveTouch = false;				// タッチ追従終了
			bInertia = true;				// 慣性移動開始
		}
		fInertia *= fDeceleration;			// 減速
		if (Mathf.Abs(fInertia) < fFinishInertia && bInertia)
		{
			fInertia = 0.0f;				// 停止
			bInertia = false;				// 慣性移動終了
			bMovesCloser = true;			// 近いところに移動開始

			// 真ん中に一番近いオブジェクトを探す
			float fDistance = 100;
			for (int i = 0; i < StageObj.GetLength(0); i++)
			{
				fKijunPos[i] = Trans[i].localPosition.x;	// 基準座標
				if (Mathf.Abs(fDistance) > Mathf.Abs(Trans[i].localPosition.x))
					fDistance = Trans[i].localPosition.x;
			}
			fStartSpeed = -fDistance * 2 / fMoveTime;		// 初速度の計算
			fAccele = -fStartSpeed / fMoveTime;		// 加速度の計算
			fTime = 0.0f;							// タイマーの初期化
		}
	}

	// 近いところに移動関係
	private void MovesCloser()
	{
		if(bMovesCloser)
		{
			fTime += Time.deltaTime;
			fHeni = (fStartSpeed * fTime) + (0.5f * fAccele * fTime * fTime);	// 変位の計算

			if (fTime >= fMoveTime)
			{
				bMovesCloser = false;

				// 一番近い基準座標に移動する
				MoveNear();

				fHeni = 0;
			}
		}
	}

	// 移動
	private void Move()
	{
		// クリックされた時に、場所がボタンだったらタッチに追従しない
		if (Input.GetButtonDown("Fire1") && !bMoveTouch)
		{
			Vector3 center = Vector3.zero;
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
				center = hit.point;

			Vector3 halfExtents = new Vector3(0.1f, 0.1f, 5.0f);
			Quaternion orientation = Camera.main.transform.rotation;
			Collider[] col = Physics.OverlapBox(center, halfExtents, orientation);

			if (col.GetLength(0) >= 1)
			{// タップ（クリック）した先にオブジェクトがあった場合
				if(col[0].tag != "StageSelect_Button")
				{
					bMoveTouch = true;
					bInertia = false;
					bMovesCloser = false;
					bButtonMove = false;

					// タッチされたので、カウンターのリセット
					nRightCnt = 0;
					nLeftCnt = 0;
					fMoveDistance = 0;
					fTotalHeni = 0;
					fHeni = 0;
				}
			}

			else
			{// タップ（クリック）した先にオブジェクトが無かった場合
				bMoveTouch = true;
				bInertia = false;
				bMovesCloser = false;
				bButtonMove = false;

				// タッチされたので、カウンターのリセット
				nRightCnt = 0;
				nLeftCnt = 0;
				fMoveDistance = 0;
				fTotalHeni = 0;
				fHeni = 0;
			}
		}

		for (int i = 0; i < StageObj.GetLength(0); i++)
		{
			if (bMoveTouch)
			{// タッチに追従
				Trans[i].localPosition = new Vector3(Trans[i].localPosition.x + fMove, Trans[i].localPosition.y, Trans[i].localPosition.z);
			}
			else if (bInertia)
			{// 慣性移動
				Trans[i].localPosition = new Vector3(Trans[i].localPosition.x + fInertia, Trans[i].localPosition.y, Trans[i].localPosition.z);
			}
			else if (bMovesCloser)
			{// 近いところに移動
				Trans[i].localPosition = new Vector3(fKijunPos[i] + fHeni, Trans[i].localPosition.y, Trans[i].localPosition.z);
			}
			else if(bButtonMove)
			{// ボタンを押されて隣に移動
				Trans[i].localPosition = new Vector3(fKijunPos[i] + fHeni, Trans[i].localPosition.y, Trans[i].localPosition.z);
			}

			// 左右ワープ
			if (Trans[i].localPosition.x > ((StageObj.GetLength(0) - nMinus - 1 + 0.5f) * fSpace))
			{
				Trans[i].localPosition = new Vector3(Trans[i].localPosition.x - (fSpace * StageObj.GetLength(0)), Trans[i].localPosition.y, Trans[i].localPosition.z);
				fKijunPos[i] -= fSpace * StageObj.GetLength(0);
			}
			else if (Trans[i].localPosition.x < ((-nMinus - 0.5f) * fSpace))
			{
				Trans[i].localPosition = new Vector3(Trans[i].localPosition.x + (fSpace * StageObj.GetLength(0)), Trans[i].localPosition.y, Trans[i].localPosition.z);
				fKijunPos[i] += fSpace * StageObj.GetLength(0);
			}
		}
	}

	// ステージ選択
	private bool StaSele()
	{
		// 1クリック目で仮選択
		if (InputManager.Instance.GetClick() == InputManager.CLICK_STATE.ONECLICK)
		{
			Vector3 center = Vector3.zero;
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
				center = hit.point;

			Vector3 halfExtents = new Vector3(0.1f, 0.1f, 5.0f);
			Quaternion orientation = Camera.main.transform.rotation;
			Collider[] col = Physics.OverlapBox(center, halfExtents, orientation);	// レイキャストではUIと当らないので、Physics.OverlapBox()でUIとの判定も取れるようにする

			if (col.GetLength(0) >= 1 && col[0].tag == "StageSelect_Stage")
			{
				DecisionObj = col[0].gameObject;	// オブジェクトを取得
			}
		}

		// 移動させたら選択解除
		if (InputManager.Instance.GetMove() && DecisionObj != null)
		{
			DecisionObj = null;
		}

		// 指を話した時にDecisionObjがあれば選択
		if (Input.GetButtonUp("Fire1") && DecisionObj != null)
		{
			// ステージ決定
			for(int i = 0 ; i < StageObj.GetLength(0) ; i ++)
			{
				if(DecisionObj == StageObj[i])
				{
					nStageNo = i;		// StageSpriteSlideOut()で使うので保存しておく

					GameManager.SetStage = nStageNo;		// ゲームメインにステージ番号を渡す

					return true;
				}
			}
		}

		return false;
	}

	// ボタンで隣に移動
	public void ButtonMoveSet(bool bLeft)
	{
		// 初速度の計算
		float fDistance;
		if(bLeft)
		{
			nLeftCnt++;
			fDistance = (fSpace * nLeftCnt) + fMoveDistance;
			if(nLeftCnt == 1)
			{
				fSearchNearDistance = SearchNearPos(false);				// 1タッチ目だったら距離を直す
				fDistance -= fSearchNearDistance;
			}
			fStartSpeed = -fDistance * 2 / fBottonMoveTime;
		}
		else
		{
			nRightCnt ++;

			fDistance = (fSpace * nRightCnt) - fMoveDistance;
			if (nRightCnt == 1)
			{
				fSearchNearDistance = SearchNearPos(true);				// 1タッチ目だったら距離を直す
				fDistance += fSearchNearDistance;
			}
			fStartSpeed = fDistance * 2 / fBottonMoveTime;
		}
		fAccele = -fStartSpeed / fBottonMoveTime;	// 加速度の計算
		fTime = 0.0f;								// タイマーの初期化
		fTotalHeni += fHeni;						// 今までの変位量を保存

		// 基準座標
		for (int i = 0; i < StageObj.GetLength(0); i++)
		{
			fKijunPos[i] = Trans[i].localPosition.x;
		}

		bMoveTouch = false;
		bInertia = false;
		bMovesCloser = false;
		bButtonMove = true;
	}

	// ボタンで隣に移動
	private void ButtonMove()
	{
		if(bButtonMove)
		{
			fTime += Time.deltaTime;
			if(fTime >= fBottonMoveTime)		// 時間補正(ぴったり移動してくれないので)
				fTime = fBottonMoveTime;

			fHeni = (fStartSpeed * fTime) + (0.5f * fAccele * fTime * fTime);	// 変位の計算
			fMoveDistance += fHeni - fPreMoveDistance;
			fPreMoveDistance = fMoveDistance;

			fBugResolution = fTotalHeni + fHeni;	// なぜか一度代入を挟むとバグが解決するっぽい

			if ((nRightCnt >= 1 && fBugResolution >= (nRightCnt * fSpace)) ||
				(nLeftCnt >= 1 && -fBugResolution >= (nLeftCnt * fSpace)))
			{
				bButtonMove = false;

				// 止まったので、カウンターのリセット
				nRightCnt = 0;
				nLeftCnt = 0;
				fMoveDistance = 0;
				fTotalHeni = 0;
				fSearchNearDistance = 0;

				// 一番近い基準座標に移動する
				MoveNear();

				fHeni = 0;		// MoveNear()で使っているので、これだけ後で初期化
			}
		}
	}

	// 一番近い基準座標に移動する
	private void MoveNear()
	{
		int nNear = 0;				// 1番近い座標の添え字
		float fDistance = 100;		// 距離(適当に大きな数字)

		for (int i = 0; i < StageObj.GetLength(0); i++)
		{
			Trans[i].localPosition = new Vector3(fKijunPos[i] + fHeni, Trans[i].localPosition.y, Trans[i].localPosition.z);

			// ステージ1の位置を決めて、他はそこから1つづつずらす
			if (i == 0)
			{
				for (int j = 0; j < StartPos.GetLength(0); j++)
				{
					if (Mathf.Abs(fDistance) > Mathf.Abs(Trans[i].localPosition.x - StartPos[j]))
					{
						fDistance = Trans[i].localPosition.x - StartPos[j];
						nNear = j;
					}
				}
			}
			else
			{
				nNear++;
				if (nNear == StageObj.GetLength(0))
					nNear -= StageObj.GetLength(0);
			}

			Trans[i].localPosition = new Vector3(StartPos[nNear], Trans[i].localPosition.y, Trans[i].localPosition.z);
		}
	}

	// 一番近い座標までの距離を計算する
	// 引数 : プラス方向に探すか、マイナス方向に探すか
	private float SearchNearPos(bool bPlus)
	{
		float fDistance = 0;
		float fPos = 0;

		// -fSpace~fSpaceの範囲にある座標を探す。(右端にあるオブジェクトからプラス方向に探すのが大変なため、どっちでもいける真ん中を探す)
		for(int i = 0 ; i < StageObj.GetLength(0) ; i ++)
		{
			fPos = StageObj[i].transform.localPosition.x + fHeni;

			if (-fSpace < fPos && fPos < fSpace)
				break;
		}

		if(bPlus)
		{// プラス方向に探索
			fDistance = fSpace - fPos;
		}
		else
		{// マイナス方向に探索
			fDistance = -fSpace - fPos;
		}

		//Debug.Log("fPos : " + fPos);
		//Debug.Log("fDistance : " + fDistance);

		return fDistance;
	}



	// ステージセレクトの画像を出現させる
	public bool StageSpriteFadeOut()
	{
		if(bInitializ)
		{
			fParameter = 0.0f;

			bInitializ = false;
		}

		fParameter += Time.deltaTime / SpriteFadeOutTime;

		// 終了判定
		if(fParameter >= 1.0f)
		{
			for (int i = 0; i < sr.GetLength(0); i++)
				sr[i].color = new Color(sr[i].color.r, sr[i].color.g, sr[i].color.b, 1.0f);

			bInitializ = true;		// 初期化処理をできるようにしておく
			return true;
		}

		// α値変更
		for(int i = 0 ; i < sr.GetLength(0) ; i ++)
			sr[i].color = new Color(sr[i].color.r, sr[i].color.g, sr[i].color.b, fParameter);

		return false;
	}

	// ステージを選択する
	public bool StageSelect()
	{
		// 慣性移動
		Inertia();

		// 近いところに移動
		MovesCloser();

		// ボタンで隣に移動
		ButtonMove();

		// 移動
		Move();

		// ステージ選択
		return StaSele();
	}

	// 選択した画像の左右の画像が左右に消える
	public bool StageSpriteSlideOut()
	{
		if (bInitializ)
		{
			fTime = 0.0f;									// もう変位の計算はしないので、こっちで使う

			// 移動量の計算
			fMove = fSlideOutDistance / fSlideOutTime;		// もう慣性移動では使わないので、こっちで使う

			// 左右に動かすオブジェクトの添え字の計算
			nMoveStageNo[0] = nStageNo - 1;
			nMoveStageNo[1] = nStageNo + 1;
			for (int i = 0; i < nMoveStageNo.GetLength(0); i++)
			{
				if(nMoveStageNo[i] < 0)	nMoveStageNo[i] = 9;
				else if (nMoveStageNo[i] > 9) nMoveStageNo[i] = 0;
			}

			bInitializ = false;
		}

		fTime += Time.deltaTime;

		// 終了判定
		if (fTime >= fSlideOutTime)
		{
			// 左右の2枚のスライドアウトが完了したのに合わせて、選択した画像以外を透明にする
			for (int i = 0; i < sr.GetLength(0); i++)
			{
				if(i != nStageNo)
					sr[i].color = new Color(sr[i].color.r, sr[i].color.g, sr[i].color.b, 0.0f);
			}

			this.gameObject.GetComponent<CameraBillboard>().enabled = false;	// カメラに追従するのをやめる。

			bInitializ = true;		// 初期化処理をできるようにしておく
			return true;
		}

		// 移動
		Vector3 pos;
		pos = new Vector3(Trans[nMoveStageNo[0]].localPosition.x - fMove * Time.deltaTime, Trans[nMoveStageNo[0]].localPosition.y, Trans[nMoveStageNo[0]].localPosition.z);
		Trans[nMoveStageNo[0]].localPosition = pos;
		pos = new Vector3(Trans[nMoveStageNo[1]].localPosition.x + fMove * Time.deltaTime, Trans[nMoveStageNo[1]].localPosition.y, Trans[nMoveStageNo[1]].localPosition.z);
		Trans[nMoveStageNo[1]].localPosition = pos;

		return false;
	}

	// 縮小しながらカウンターの上へ移動
	public bool CounterMove()
	{
		Vector3 vPos = Vector3.zero;

		// 初期化処理
		if (bInitializ)
		{
			fParameter = 0.0f;						// パラメーター変数初期化
			vStartPos = transform.position;			// 移動開始座標を保存
			fStartScale = transform.localScale.x;	// 移動開始時の拡大率を保存

			bInitializ = false;
		}

		fParameter += Time.deltaTime / fCounterMoveTime;

		// 終了判定
		if (fParameter >= 1.0f)
		{
			// 移動
			vPos.x = Trans_Counter.position.x + vCounterOffsetPos.x;
			vPos.y = Trans_Counter.position.y + vCounterOffsetPos.y;
			vPos.z = Trans_Counter.position.z + vCounterOffsetPos.z;
			transform.position = vPos;

			// 拡大率
			transform.localScale = new Vector3(fCounterScale, fCounterScale, fCounterScale);

			bInitializ = true;

			return true;
		}

		// 移動
		vPos.x = Mathf.Lerp(vStartPos.x, Trans_Counter.position.x + vCounterOffsetPos.x, fParameter);
		vPos.y = Mathf.Lerp(vStartPos.y, Trans_Counter.position.y + vCounterOffsetPos.y, fParameter);
		vPos.z = Mathf.Lerp(vStartPos.z, Trans_Counter.position.z + vCounterOffsetPos.z, fParameter);
		transform.position = vPos;

		// 拡大率
		float fScale = Mathf.Lerp(fStartScale, fCounterScale, fParameter);
		transform.localScale = new Vector3(fScale, fScale, fScale);

		return false;
	}
}


// あと、中心に来ている画像の左右の画像を押されても反応しない処理を追加する。
// もし押されたら、決定ではなく左右ボタンが押された時みたいに、１つ分スライドさせてやればいいと思う。