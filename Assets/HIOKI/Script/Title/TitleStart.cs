using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleStart : MonoBehaviour {

	[SerializeField]
	private SpriteRenderer _Tex;	//フェードさせるよう

	[SerializeField]
	private float fFadeSpeed;		//フェードのスピード

	private bool bFadeFlg = true;	//フェードフラグ

	// Use this for initialization
	void Start () {
		_Tex = GetComponent<SpriteRenderer> ();		//設定
	}
	
	// Update is called once per frame
	void Update () {
		FadeStart ();			//フェードインアウト
		FadeStop();				//フェードアウト
	}

	private void FadeStart()
	{
		if (!bFadeFlg)
			return;			//fasleだったら処理を行わない

		Color cFade = _Tex.color;	//値代入

		cFade.a += fFadeSpeed;		//変更

		#region 補正
		if (cFade.a <= 0.0f) {
			cFade.a = 0.0f;
			fFadeSpeed *= -1;
		} else if (1.0f < cFade.a) {
			cFade.a = 1.0f;
			fFadeSpeed *= -1;
		}
		#endregion

		_Tex.color = cFade;			//値セット

	}

	private void FadeStop()
	{
		if (bFadeFlg)
			return;			//trueだったら処理を行わない

		//値補正
		if (fFadeSpeed < 0) {
			fFadeSpeed *= -1;
		}

		Color cFade = _Tex.color;	//値代入

		cFade.a -= fFadeSpeed;		//変更

		//値補正
		if (cFade.a <= 0.0f) {
			cFade.a = 0.0f;
		}

		_Tex.color = cFade;			//値設定
	}

	public void StopFade()
	{
		bFadeFlg = false;			//変更
	}
		
}
