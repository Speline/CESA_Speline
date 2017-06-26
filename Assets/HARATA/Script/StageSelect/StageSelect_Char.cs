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

		for(int i = 0 ; i < transform.childCount ; i ++)			// 子のTransform取得
			Char[i] = transform.GetChild(i);

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

		// 狩猟判定
		if (fParameter >= 1.0f)
		{
			fParameter = 0.0f;		// パラメーター初期化

			Char[0].localPosition = new Vector3(transform.position.x, transform.position.y, fLeadStopPos);
			// ここで、カメラを手元に

			return true;
		}

		// 移動
		pos = Mathf.Lerp(0.0f, fLeadStopPos, fParameter);		// 先頭キャラはどうせlocal(Z)0.0fにいるので、移動開始位置はマジックナンバー
		Char[0].localPosition = new Vector3(transform.position.x, transform.position.y, pos);

		return false;
	}

	// 先頭キャラを透明にする
	public bool LeadCharFadeIn()
	{
		// 初期化処理
		if(bInitializ)
		{
			fTime = 0.0f;

			fParameter = 0.0f;												// パラメーター初期化
			LeadCharMat = Char[0].GetComponent<MeshRenderer>().material;	// 先頭キャラのマテリアル取得
			LeadCharMat.EnableKeyword("_Color");							// "_Color"を変更可能にしておく

			bInitializ = false;		// 初期化終了
		}

		// 待ち時間(StageSelect_Camera.csのfLinearMoveWaitTimeと同じ)
		fTime += Time.deltaTime;
		if (fTime < 0.5f)
			return false;

		fParameter += Time.deltaTime / fFadeInTime;
		if(fParameter >= 1.0f)
		{
			//LeadCharMat.color = new Color(LeadCharMat.color.r, LeadCharMat.color.b, LeadCharMat.color.b, 0.0f);
			LeadCharMat.SetColor("_Color", new Color(LeadCharMat.color.r, LeadCharMat.color.b, LeadCharMat.color.b, 0.0f));
			LeadCharMat.DisableKeyword("_Color");	// "_Color"を変更不可能にしておく

			fParameter = 0.0f;
			bInitializ = true;

			return true;
		}

		float fAlpha;
		fAlpha = Mathf.Lerp(1.0f, 0.0f, fParameter);

		LeadCharMat.SetColor("_Color", new Color(LeadCharMat.color.r, LeadCharMat.color.b, LeadCharMat.color.b, fAlpha));

		return false;
	}

	// 先頭キャラを表示する
	public void LeadCharDraw()
	{
		LeadCharMat.EnableKeyword("_Color");	// "_Color"を変更可能にしておく

		LeadCharMat.SetColor("_Color", new Color(LeadCharMat.color.r, LeadCharMat.color.b, LeadCharMat.color.b, 1.0f));

		LeadCharMat.DisableKeyword("_Color");	// "_Color"を変更不可能にしておく
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
				vStartPos[i] = Char[i].position;

			bInitializ = false;		// 初期化終了
		}

		fParameter += Time.deltaTime / fWarpMoveTime;
		if (fParameter >= 1.0f)
		{
			fParameter = 0.0f;

			for (int i = 0; i < 6; i++)
				Char[i].position = vWarpPos[i];

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
}
