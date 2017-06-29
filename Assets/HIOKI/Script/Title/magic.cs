using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magic : MonoBehaviour {

	[SerializeField]
	private float fRotSpeed;			//回転スピード
	[SerializeField]
	private float fMoveSpeed;			//移動スピード
	[SerializeField]
	private float fMaxY;				//移動最大値
	[SerializeField]
	private SpriteRenderer _Magic;		//フェード

	private int nSelect = 0;			//順番
	private bool bStopFlg = false;		//ストップフラグ
	private bool bStart = true;			//スタートフラグ

	// Use this for initialization
	void Start () {
		_Magic = GetComponent<SpriteRenderer> ();		//設定
	}
	
	// Update is called once per frame
	void Update () {
		switch (nSelect) {
		case 0:
			FadeMagic ();					//フェードイン
			break;
		case 1:
			MoveMagic ();					//移動
			break;
		case 2:
			RotMove ();						//回転
			break;
		case 3:
			MagicActive ();					//フェードアウト
			break;
		}
	}
	private void FadeMagic()
	{
		Color color = _Magic.color;			//値セット

		color.a += 0.007f;					//変更


		if (color.a >= 1.0f) {
			color.a = 1.0f;					//補正
			nSelect++;						//変更
		}

		_Magic.color = color;				//値代入
	}

	private void MoveMagic()
	{
		Vector3 pos = transform.localPosition;	//値セット

		pos.y += fMoveSpeed;					//変更

		if (pos.y >= fMaxY) {
			pos.y = fMaxY;						//補正
			nSelect++;							//変更
		}

		transform.localPosition = pos;			//値代入
	}

	private void RotMove()
	{
		Vector3 vecRot = transform.localEulerAngles;	//値セット

		vecRot.z += fRotSpeed;							//変更

		if (vecRot.z >= 360.0f) {
			vecRot.z = 0.0f;							//補正
		}

		transform.localEulerAngles = vecRot;			//値代入
	}

	private void MagicActive()
	{
		Color color = _Magic.color;					//値セット

		color.a -= 0.01f;							//変更

		if (color.a <= 0.0f) {
			color.a = 0.0f;							//補正
		}

		_Magic.color = color;						//値代入
	}

	public void StopMove()
	{
		nSelect = 3;
	}

	public bool CheckFlg()
	{
		if (nSelect >= 2) {
			return true;
		}
		return false;
	}
}
