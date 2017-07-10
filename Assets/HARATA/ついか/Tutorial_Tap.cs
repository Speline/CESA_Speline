using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Tap : MonoBehaviour {

	[SerializeField]	float fFadeInTime;

	Image image;
	bool bInitializ = true;							// 初期化フラグ(trueの時に初期化する)
	float fAlpha;

	// Use this for initialization
	void Start ()
	{
		image = GetComponent<Image>();
		image.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool FadeIn()
	{
		if (bInitializ)
		{
			image.enabled = true;
			fAlpha = 0.0f;

			image.color = new Color(image.color.r, image.color.g, image.color.b, 0.0f);

			bInitializ = false;
		}

		fAlpha += Time.deltaTime / fFadeInTime;
		if (fAlpha >= 1.0f)
		{
			image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);

			bInitializ = true;
			return true;
		}

		image.color = new Color(image.color.r, image.color.g, image.color.b, fAlpha);

		return false;
	}

	public void UnDraw()
	{
		image.enabled = false;
	}
}
