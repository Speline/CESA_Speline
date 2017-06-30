using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelect_Char : MonoBehaviour
{
	[SerializeField]	float fStartPos;		// スタート位置(z)
	[SerializeField]	float fGroupStopPos;	// 集団移動の停止位置(z)
	[SerializeField]	float fGroupMoveTime;	// 集団移動時間
	[SerializeField]	float fWaitingTime;		// 集団移動が完了して何もしていない時間(先頭キャラが動き出すまでの時間)
	[SerializeField]	float fLeadStopPos;		// 先頭キャラの停止位置
	[SerializeField]	float fLeadMoveTime;	// 先頭キャラの移動時間
	[SerializeField]	float fFadeInTime;		// 先頭キャラを透明にする時間
	[SerializeField]	float fWarpMoveTime;	// ワープ上に移動するのにかける時間

	Transform[] Char = new Transform[6];		// キャラ6人分のTransform
	Animator[] animator = new Animator[6];		// キャラのAnimator
	float fTime = 0.0f;

	float fParameter = 0.0f;					// 移動に使うパラメーター
	float pos;									// 計算用
	float fLeadCharStartPos;					// 先頭キャラが移動し始めるときの、ローカル座標(z)

	bool bInitializ = true;						// 初期化フラグ
	Material LeadCharMat;						// 先頭キャラのマテリアル

	Vector3[] vStartPos = new Vector3[6];		// ワープ上に移動するときの、移動開始座標
	Vector3[] vWarpPos = new Vector3[6];		// ワープ上に移動するときの、移動先座標

	// Use this for initialization
	void Start ()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, fStartPos);	// 座標の初期化

		for(int i = 0 ; i < transform.childCount ; i ++)
		{
			Char[i] = transform.GetChild(i);								// 子のTransform取得
			animator[i] = Char[i].gameObject.GetComponent<Animator>();		// 子のAnimator取得
		}

		// ワープ魔法陣へ移動するときの座標取得
		vWarpPos[0] = GameObject.Find("AdjustWarpPos1").GetComponent<Transform>().position;
		vWarpPos[1] = GameObject.Find("AdjustWarpPos2").GetComponent<Transform>().position;
		vWarpPos[2] = GameObject.Find("AdjustWarpPos3").GetComponent<Transform>().position;
		vWarpPos[3] = GameObject.Find("AdjustWarpPos4").GetComponent<Transform>().position;
		vWarpPos[4] = GameObject.Find("AdjustWarpPos5").GetComponent<Transform>().position;
		vWarpPos[5] = GameObject.Find("AdjustWarpPos6").GetComponent<Transform>().position;
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	// 集団移動
	public bool GroupMove()
	{
		fParameter += Time.deltaTime / fGroupMoveTime;

		// 終了判定
		if (fParameter >= 1.0f)
		{
			fParameter = 0.0f;		// パラメーター初期化

			transform.position = new Vector3(transform.position.x, transform.position.y, fGroupStopPos);

			for (int i = 0; i < transform.childCount; i++)
				animator[i].SetBool("bWalk", false);		// キャラクターを待機モーションにする

			return true;
		}
		
		// 移動
		pos = Mathf.Lerp(fStartPos, fGroupStopPos, fParameter);
		transform.position = new Vector3(transform.position.x, transform.position.y, pos);

		return false;
	}

	// 移動待ち
	public bool CharWaiting()
	{
		fTime += Time.deltaTime;

		// 終了判定
		if (fTime >= fWaitingTime)
		{
			fTime = 0.0f;
			//fLeadCharStartPos = Char[0].localPosition.z;	// 先頭キャラの移動開始位置を保存

			// ここでカメラを先頭キャラの三人称な感じにする関数を呼ぶ

			return true;
		}

		return false;
	}

	// 先頭キャラ移動
	public bool LeadCharMove()
	{
		fParameter += Time.deltaTime / fLeadMoveTime;

		// 初期化処理
		if(bInitializ)
		{
			animator[0].SetBool("bWalk", true);		// 先頭キャラだけ歩きモーションに

			bInitializ = false;
		}

		// 終了判定
		if (fParameter >= 1.0f)
		{
			fParameter = 0.0f;		// パラメーター初期化

			Char[0].localPosition = new Vector3(transform.position.x, transform.position.y, fLeadStopPos);

			animator[0].SetBool("bWalk", false);		// 先頭キャラだけ待機モーションに
			
			bInitializ = true; 

			return true;
		}

		// 移動
		pos = Mathf.Lerp(0.0f, fLeadStopPos, fParameter);		// 先頭キャラはどうせlocal(Z)0.0fにいるので、移動開始位置はマジックナンバー
		Char[0].localPosition = new Vector3(transform.position.x, transform.position.y, pos);

		return false;
	}

	public void CharMoveMotion()
	{
		for (int i = 0; i < 6; i++)
			animator[i].SetBool("bWalk", true);		// キャラクターを歩きモーションにする
	}

	// ワープ上に移動する
	public bool WarpMove()
	{
		Vector3 vPos;

		// 初期化処理
		if (bInitializ)
		{
			fParameter = 0.0f;		// パラメーター初期化

			// 移動開始座標保存
			for(int i = 0 ; i < 6 ; i ++)
			{
				vStartPos[i] = Char[i].position;
			}

			bInitializ = false;		// 初期化終了
		}

		fParameter += Time.deltaTime / fWarpMoveTime;
		if (fParameter >= 1.0f)
		{
			fParameter = 0.0f;

			for (int i = 0; i < 6; i++)
			{
				Char[i].position = vWarpPos[i];
				animator[i].SetBool("bWalk", false);		// キャラクターを待機モーションにする
			}

			bInitializ = true;

			return true;
		}

		// 移動
		for(int i = 0 ; i < 6 ; i ++)
		{
			vPos.x = Mathf.Lerp(vStartPos[i].x, vWarpPos[i].x, fParameter);
			vPos.y = Mathf.Lerp(vStartPos[i].y, vWarpPos[i].y, fParameter);
			vPos.z = Mathf.Lerp(vStartPos[i].z, vWarpPos[i].z, fParameter);
			Char[i].position = vPos;
		}

		return false;
	}

	// スキップ処理
	public void Skip()
	{
		fParameter = 0.0f;
		fTime = 0.0f;
		bInitializ = true;

		// 集団移動を完了させる
		transform.position = new Vector3(transform.position.x, transform.position.y, fGroupStopPos);

		// キャラクターを待機モーションにする
		for(int i = 0 ; i < 6 ; i ++)
			animator[i].SetBool("bWalk", false);

		// 先頭キャラの移動を完了させる
		Char[0].localPosition = new Vector3(transform.position.x, transform.position.y, fLeadStopPos);
	}
}
