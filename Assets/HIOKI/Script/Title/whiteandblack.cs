using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whiteandblack : MonoBehaviour {

	[SerializeField]
	private SpriteRenderer _White;		//fade

	[SerializeField]
	private float fSpeed;				//fadeスピード

	private bool bEndFlg = false;		//終わったフラグ

	// Use this for initialization
	void Start () {
		_White = GetComponent<SpriteRenderer> ();	//設定
	}
	
	// Update is called once per frame
	void Update () {
		WhiteorBlack ();					//フェードインアウト
	}

	private void WhiteorBlack()
	{

		Color color = _White.color;			//値設定

		color.a -= fSpeed;					//変更

		//補正
		if (color.a <= 0.0f) {
			color.a = 0.0f;					//補正
			bEndFlg = true;					//ストップ
		}

		_White.color = color;				//値代入
	}

	public bool CheckFlg()
	{
		return bEndFlg;
	}
}
