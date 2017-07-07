using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Waku : MonoBehaviour
{
	[SerializeField]	float fStartPosX;		// 初期座標X
	[SerializeField]	float fEndPosX;			// 移動後座標X
	[SerializeField]	float fSlideInTime;		// スライドインにかける時間
	[SerializeField]	float fFadeOutTime;

	bool bInitializ = true;
	float fParameter;
	SpriteRenderer sr;
	float fAlpha;

	// Use this for initialization
	void Start ()
	{
		transform.localPosition = new Vector3(fStartPosX, transform.localPosition.y, transform.localPosition.z);	// 初期座標設定
		transform.forward = Camera.main.transform.forward;															// 向き合わせる
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	// スライドイン
	public bool SlideIn()
	{
		float fPos;

		if (bInitializ)
		{
			fParameter = 0.0f;
			bInitializ = false;
		}

		fParameter += Time.deltaTime / fSlideInTime;
		if (fParameter >= 1.0f)
		{
			bInitializ = true;

			transform.localPosition = new Vector3(fEndPosX, transform.localPosition.y, transform.localPosition.z);

			return true;
		}

		fPos = Mathf.Lerp(fStartPosX, fEndPosX, fParameter);
		transform.localPosition = new Vector3(fPos, transform.localPosition.y, transform.localPosition.z);

		return false;
	}

	// フェードアウト
	public bool FadeOut()
	{
		if (bInitializ)
		{
			sr = GetComponent<SpriteRenderer>();
			fAlpha = 1.0f;
			bInitializ = false;
		}

		fAlpha -= Time.deltaTime / fFadeOutTime;
		if (fParameter <= 0.0f)
		{
			bInitializ = true;

			sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.0f);

			return true;
		}

		sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, fAlpha);
		return false;
	}
}
