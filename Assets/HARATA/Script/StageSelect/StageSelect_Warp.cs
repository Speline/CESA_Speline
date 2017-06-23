using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ステージセレクトで使う、ワープにアタッチするスクリプト
public class StageSelect_Warp : MonoBehaviour
{
	[SerializeField]	float fAddAngleSpeed;

	bool bRotate = false;		// 回転してもいいならtrue
	float fAngle = 0.0f;
	SpriteRenderer sr;

	// Use this for initialization
	void Start ()
	{
		sr = GetComponent<SpriteRenderer>();
		sr.enabled = false;		// 最初は見えないようにしておく
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (bRotate)
		{
			fAngle += fAddAngleSpeed;
			transform.Rotate(new Vector3(0.0f, 0.0f, fAngle));
		}
	}


	// ワープを表示する
	public void DrawWarp()
	{
		sr.enabled = true;
	}

	// 回転を開始する
	public void StartRotate()
	{
		bRotate = true;
	}
}
