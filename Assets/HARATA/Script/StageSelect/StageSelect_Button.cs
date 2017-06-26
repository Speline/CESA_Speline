using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect_Button : MonoBehaviour {

	StageManager SM;
	Image[] img = new Image[2];	// ボタンのImage(コンフィグが追加されたら、3個に直す)

	bool bCanPush = false;		// ボタンを押せる時はtrue
	[SerializeField]	float fFadeOutTime;		// 画像出現時間
	[SerializeField]	float fFadeInTime;		// 画像が消える時間
	bool bInitializ = true;		// 初期化フラグ
	float fAlpha = 0.0f;		// α値

	// Use this for initialization
	void Start ()
	{
		SM = GameObject.Find("StageManager").GetComponent<StageManager>();

		// 子のimage取得
		img[0] = transform.GetChild(0).GetComponent<Image>();
		img[1] = transform.GetChild(1).GetComponent<Image>();
		for (int i = 0; i < 2; i++)
		{
			img[i].color = new Color(img[i].color.r, img[i].color.g, img[i].color.b, 0.0f);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void OnTapLeftButton()
	{
		if (!bCanPush)
			return;

		SM.ButtonMoveSet(false);
	}

	public void OnTapRightButton()
	{
		if (!bCanPush)
			return;

		SM.ButtonMoveSet(true);
	}


	// ボタンの画像を出現させる
	public bool ButtonImageFadeOut()
	{
		// 初期化処理
		if(bInitializ)
		{
			fAlpha = 0.0f;

			bInitializ = false;
		}

		fAlpha += Time.deltaTime / fFadeOutTime;

		// 終了判定
		if(fAlpha >= 1.0f)
		{
			for (int i = 0; i < img.GetLength(0); i++)
				img[i].color = new Color(img[i].color.r, img[i].color.g, img[i].color.b, 1.0f);

			bInitializ = true;		// 初期化処理をできるようにしておく
			bCanPush = true;		// ボタンを押せるようにする

			return true;
		}

		// α値変更
		for (int i = 0; i < img.GetLength(0); i++)
			img[i].color = new Color(img[i].color.r, img[i].color.g, img[i].color.b, fAlpha);


		return false;
	}

	// ボタンの画像を消す
	public bool ButtonImageFadeIn()
	{
		// 初期化処理
		if (bInitializ)
		{
			fAlpha = 1.0f;
			bCanPush = false;		// ボタンを押せないようにする

			bInitializ = false;
		}

		fAlpha -= Time.deltaTime / fFadeInTime;

		// 終了判定
		if (fAlpha <= 0.0f)
		{
			for (int i = 0; i < img.GetLength(0); i++)
				img[i].color = new Color(img[i].color.r, img[i].color.g, img[i].color.b, 0.0f);

			bInitializ = true;		// 初期化処理をできるようにしておく

			return true;
		}

		// α値変更
		for (int i = 0; i < img.GetLength(0); i++)
			img[i].color = new Color(img[i].color.r, img[i].color.g, img[i].color.b, fAlpha);


		return false;
	}
}
