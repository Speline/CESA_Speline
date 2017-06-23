using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ステージセレクトシーンのカメラ
public class StageSelect_Camera : MonoBehaviour
{
	[SerializeField]	float fTop2QuarterTime;		// トップビューからクォータービューに移動するのにかける時間
	[SerializeField]	Vector3 vOffsetPos;			// 先頭キャラからのずれ
	[SerializeField]	float fLookAtLeadOffset;	// 先頭キャラからどれだけ離れたところを見るか
	[SerializeField]	float fLinearMoveWaitTime;	// 直線移動までの待ち時間
	[SerializeField]	float fLinearMoveTime;		// 直線移動にかける時間
	[SerializeField]	float fStaseleDistance;		// ステージセレクト時の、注視点からの距離	
	[SerializeField]	Vector3 vLookAtStaseleOffsetPos;	// ステージセレクトの時の注視点(手元あたり)

	[SerializeField]	Vector3 vCounterOffsetPos;	// カウンターに近づいていく時の、カウンターの中心からどれだけ離れているかの数値
	[SerializeField]	float fCounterMoveTime;		// カウンターに近づいていくのにかける時間

	[SerializeField]	float fMoveUpWaitTime;		// カメラ退きを開始するまでの時間(すぐカメラを退くと違和感があったから)
	[SerializeField]	Vector3 vMoveUpPos;			// カメラを退くときの座標(World座標)
	[SerializeField]	float fMoveUpTime;			// カメラを退くときの時間

	[SerializeField]	float fLookAtWarpTime;		// 真下を見ているところから、ワープのほうを向くのにかける時間


	Transform LeadChar;				// 先頭キャラ
	Transform Counter;				// カウンター

	float fParameter = 0.0f;		// Mathf.Lerpで使うパラメーター変数
	bool bInitializ = true;			// 初期化フラグ

	Vector3 vStartPos;				// 移動開始座標			
	Vector3 vLookAtPos;				// 最初の注視点

	float fTime;					// タイマー

	Vector3 vWarpVec;				// ワープへのベクトル


	// Use this for initialization
	void Start ()
	{
		LeadChar = GameObject.Find("LeadChar").GetComponent<Transform>();
		Counter = GameObject.Find("Counter").GetComponent<Transform>();

		vStartPos = transform.position;					// 移動開始座標を保存
		vLookAtPos = new Vector3(0.0f, 0.0f, 0.0f);		// 注視点は(0.0f, 0.0f, 0.0f)とする
		transform.LookAt(vLookAtPos);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	// トップビューからクォータービューに移動する
	public bool Top2Quarter()
	{
		fParameter += Time.deltaTime / fTop2QuarterTime;

		// 終了判定
		if(fParameter >= 1.0f)
		{
			fParameter = 0.0f;

			transform.position = new Vector3(transform.position.x, LeadChar.position.y + vOffsetPos.y, LeadChar.position.z + vOffsetPos.z);
			transform.LookAt(new Vector3(vLookAtPos.x, vLookAtPos.y, LeadChar.position.z + fLookAtLeadOffset));

			return true;
		}

		// 座標
		float fPosy = Mathf.Lerp(vStartPos.y, LeadChar.position.y + vOffsetPos.y, fParameter);
		float fPosz = Mathf.Lerp(vStartPos.z, LeadChar.position.z + vOffsetPos.z, fParameter);
		transform.position = new Vector3(transform.position.x, fPosy, fPosz);

		// 注視点
		float fAngle = Mathf.Lerp(vLookAtPos.z, LeadChar.position.z + fLookAtLeadOffset, fParameter);
		transform.LookAt(new Vector3(vLookAtPos.x, vLookAtPos.y, fAngle));

		return false;
	}

	// 先頭キャラの後ろから、弧を描いて前方上に移動して、手元を映すようにする
	public bool LinearMove()
	{
		Vector3 vPos = Vector3.zero;
		Vector3 vForward = Vector3.zero;

		// 初期化処理
		if(bInitializ)
		{
			fTime = 0.0f;
			fParameter = 0.0f;

			vStartPos = transform.position;			// 最初の位置を保存
			vLookAtPos = transform.forward;			// 最初の前方ベクトルを保存(Posって書いてあるけど、この関数では座標ではなくベクトル扱い)

			bInitializ = false;		// 初期化終了
		}

		// 待ち時間
		fTime += Time.deltaTime;
		if (fTime < fLinearMoveWaitTime)
			return false;

		fParameter += Time.deltaTime / fLinearMoveTime;

		// 終了判定
		if(fParameter >= 1.0f)
		{
			fParameter = 0.0f;

			vPos.x = 0.0f;
			vPos.y = LeadChar.position.y + vLookAtStaseleOffsetPos.y + fStaseleDistance;
			vPos.z = LeadChar.position.z + vLookAtStaseleOffsetPos.z;
			transform.position = vPos;

			vForward.x = 0.0f;
			vForward.y = (LeadChar.position.y + vLookAtStaseleOffsetPos.y) - transform.position.y;
			vForward.z = (LeadChar.position.z + vLookAtStaseleOffsetPos.z) - transform.position.z;
			transform.forward = vForward;

			bInitializ = true;

			return true;
		}

		// 移動
		vPos.y = Mathf.Lerp(vStartPos.y, LeadChar.position.y + vLookAtStaseleOffsetPos.y + fStaseleDistance, fParameter);
		vPos.z = Mathf.Lerp(vStartPos.z, LeadChar.position.z + vLookAtStaseleOffsetPos.z, fParameter);
		transform.position = vPos;

		// 注視点(前方ベクトル)
		vForward.y = Mathf.Lerp(vLookAtPos.y, (LeadChar.position.y + vLookAtStaseleOffsetPos.y) - transform.position.y, fParameter);
		vForward.z = Mathf.Lerp(vLookAtPos.z, (LeadChar.position.z + vLookAtStaseleOffsetPos.z) - transform.position.z, fParameter);
		transform.forward = vForward;

		return false;
	}

	// 依頼書を追ってカウンターに近づく
	public bool CounterMove()
	{
		Vector3 vPos = Vector3.zero;

		// 初期化処理
		if (bInitializ)
		{
			fParameter = 0.0f;					// パラメーター変数初期化
			vStartPos = transform.position;		// 移動開始座標を保存

			bInitializ = false;
		}

		fParameter += Time.deltaTime / fCounterMoveTime;

		// 終了判定
		if (fParameter >= 1.0f)
		{
			vPos.x = Counter.position.x + vCounterOffsetPos.x;
			vPos.y = Counter.position.y + vCounterOffsetPos.y;
			vPos.z = Counter.position.z + vCounterOffsetPos.z;
			transform.position = vPos;

			bInitializ = true;

			return true;
		}

		vPos.x = Mathf.Lerp(vStartPos.x, Counter.position.x + vCounterOffsetPos.x, fParameter);
		vPos.y = Mathf.Lerp(vStartPos.y, Counter.position.y + vCounterOffsetPos.y, fParameter);
		vPos.z = Mathf.Lerp(vStartPos.z, Counter.position.z + vCounterOffsetPos.z, fParameter);
		transform.position = vPos;

		return false;
	}

	// Y上方向にカメラを退く
	//public bool MoveUp()
	//{
	//	Vector3 vPos = Vector3.zero;
	//
	//	// 初期化処理
	//	if (bInitializ)
	//	{
	//		fTime = 0.0f;						// タイマー初期化
	//		fParameter = 0.0f;					// パラメーター変数初期化
	//		vStartPos = transform.position;		// 移動開始座標を保存
	//
	//		bInitializ = false;
	//	}
	//
	//	// 移動開始待ち
	//	fTime += Time.deltaTime;
	//	if (fTime <= fMoveUpWaitTime)
	//		return false;
	//
	//	fParameter += Time.deltaTime / fMoveUpTime;
	//
	//	// 終了判定
	//	if (fParameter >= 1.0f)
	//	{
	//		transform.position = vMoveUpPos;
	//
	//		bInitializ = true;
	//
	//		return true;
	//	}
	//
	//	vPos.x = Mathf.Lerp(vStartPos.x, vMoveUpPos.x, fParameter);
	//	vPos.y = Mathf.Lerp(vStartPos.y, vMoveUpPos.y, fParameter);
	//	vPos.z = Mathf.Lerp(vStartPos.z, vMoveUpPos.z, fParameter);
	//	transform.position = vPos;
	//
	//	return false;
	//}

	// 真下を見ているところから、ワープのほうを向くのにかける時間
	public bool LookAtWarp()
	{
		Vector3 vPos = Vector3.zero;
		Vector3 vForward = Vector3.zero;

		// 初期化処理
		if (bInitializ)
		{
			fTime = 0.0f;
			fParameter = 0.0f;

			vLookAtPos = transform.eulerAngles;
			vStartPos = transform.position;		// 移動開始座標を保存

			bInitializ = false;		// 初期化終了
		}

		// 待ち時間
		fTime += Time.deltaTime;
		if (fTime < fMoveUpWaitTime)
			return false;
		
		fParameter += Time.deltaTime / fLookAtWarpTime;

		// 終了判定
		if (fParameter >= 1.0f)
		{
			fParameter = 0.0f;

			transform.eulerAngles = new Vector3(65.0f, 65.0f, 0.0f);
			transform.position = vMoveUpPos;

			bInitializ = true;

			return true;
		}

		// 注視点(前方ベクトル)
		vForward.x = Mathf.Lerp(vLookAtPos.x, 65.0f, fParameter);
		vForward.y = Mathf.Lerp(vLookAtPos.y, 65.0f, fParameter);
		vForward.z = 0.0f;
		transform.eulerAngles = vForward;

		// 移動
		vPos.x = Mathf.Lerp(vStartPos.x, vMoveUpPos.x, fParameter);
		vPos.y = Mathf.Lerp(vStartPos.y, vMoveUpPos.y, fParameter);
		vPos.z = Mathf.Lerp(vStartPos.z, vMoveUpPos.z, fParameter);
		transform.position = vPos;

		return false;
	}
	// 真下を見ているところから、ワープのほうを向くのにかける時間
	//public bool LookAtWarp()
	//{
	//	Vector3 vForward = Vector3.zero;
	//
	//	// 初期化処理
	//	if (bInitializ)
	//	{
	//		fParameter = 0.0f;
	//
	//		vLookAtPos = transform.eulerAngles;
	//
	//		bInitializ = false;		// 初期化終了
	//	}
	//
	//	fParameter += Time.deltaTime / fLookAtWarpTime;
	//
	//	// 終了判定
	//	if (fParameter >= 1.0f)
	//	{
	//		fParameter = 0.0f;
	//
	//		transform.eulerAngles = new Vector3(65.0f, 65.0f, 0.0f);
	//
	//		bInitializ = true;
	//
	//		return true;
	//	}
	//
	//	// 注視点(前方ベクトル)
	//	vForward.x = Mathf.Lerp(vLookAtPos.x, 65.0f, fParameter);
	//	vForward.y = Mathf.Lerp(vLookAtPos.y, 65.0f, fParameter);
	//	vForward.z = 0.0f;
	//	transform.eulerAngles = vForward;
	//
	//	return false;
	//}
}
