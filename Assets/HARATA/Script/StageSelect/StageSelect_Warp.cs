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

	ParticleSystem ps;
	ParticleSystem ps1;

	// Use this for initialization
	void Start ()
	{
		sr = GetComponent<SpriteRenderer>();
		sr.enabled = false;		// 最初は見えないようにしておく

		//ps = transform.GetChild(0).GetComponent<ParticleSystem>();
		ps = GameObject.Find("Aura").GetComponent<ParticleSystem>();
		ps.Stop();
		ps1 = GameObject.Find("Aura").GetComponent<Transform>().GetChild(0).GetComponent<ParticleSystem>();
		ps1.Stop();
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
		ps.Play();
		ps1.Stop();
	}

	// 回転を開始する
	public void StartRotate()
	{
		bRotate = true;
		ps1.Play();
	}
}
