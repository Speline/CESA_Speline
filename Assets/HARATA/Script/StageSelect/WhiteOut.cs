using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhiteOut : MonoBehaviour
{
	[SerializeField]	float fWaitTime;		// 白くなり始めるまでの待ち時間
	[SerializeField]	float fWhiteOutTime;	// 真っ白になるまでにかける時間

	Image img;
	float fAlpha = 0.0f;
	float fTime = 0.0f;


	// Use this for initialization
	void Start ()
	{
		img = GetComponent<Image>();
		img.color = new Color(img.color.r, img.color.g, img.color.b, 0.0f);
		img.enabled = false;
	}

	// ホワイトアウト画像を描画する
	public bool DrawWhiteOut()
	{
		// 待ち時間
		fTime += Time.deltaTime;
		if(fTime < fWaitTime)
		{
			img.enabled = true;
			return false;
		}

		fAlpha += Time.deltaTime / fWhiteOutTime;
		if(fAlpha >= 1.0f)
		{
			img.color = new Color(img.color.r, img.color.g, img.color.b, 1.0f);

			return true;
		}

		img.color = new Color(img.color.r, img.color.g, img.color.b, fAlpha);

		return false;
	}
}
