using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Message_1 : MonoBehaviour
{
	[SerializeField]	Sprite[] MessageSprite1;	// ステージ1のメッセージ
	[SerializeField]	Sprite[] MessageSprite2;	// ステージ2のメッセージ
	[SerializeField]	float fStartPosX;			// 初期座標X
	[SerializeField]	float fEndPosX;				// 移動後座標X
	[SerializeField]	float fSlideInTime;			// スライドインにかける時間
	[SerializeField]	float fFadeOutTime;			// フェードアウトにかける時間

	Sprite[] DrawMessage;							// 実際に描画するSprite
	SpriteRenderer sr;								// 自身のSpriteRenderer
	int nMessageNum = 0;							// 今表示しているのメッセージの添え字
	bool bFinFadeOut = false;						// メッセージのフェードアウトが終わったのかどうか

	bool bInitializ = true;				// 初期化フラグ
	bool bInitializ_FadeOut = true;		// フェードアウト用の初期化フラグ
	float fParameter;					// スライドイン用のパラメーター変数
	float fAlpha;						// フェードアウト用のα値
	float fPos;							// 計算用

	// Use this for initialization
	void Start ()
	{
		sr = GetComponent<SpriteRenderer>();

		if(GameManager.GetStage == 0)
		{
			DrawMessage = new Sprite[MessageSprite1.GetLength(0)];
			for(int i = 0 ; i < DrawMessage.GetLength(0) ; i ++)
				DrawMessage[i] = MessageSprite1[i];
		}
		else if (GameManager.GetStage == 1)
		{
			DrawMessage = new Sprite[MessageSprite2.GetLength(0)];
			for (int i = 0; i < DrawMessage.GetLength(0); i++)
				DrawMessage[i] = MessageSprite2[i];
		}

		sr.sprite = DrawMessage[nMessageNum];

		transform.localPosition = new Vector3(fStartPosX, transform.localPosition.y, transform.localPosition.z);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public bool Message1()
	{
		// 初期化処理
		if (bInitializ)
		{
			transform.localPosition = new Vector3(fStartPosX, transform.localPosition.y, transform.localPosition.z);

			bInitializ = false;
		}

		fParameter += Time.deltaTime / fSlideInTime;

		// 終了判定
		if (fParameter >= 1.0f)
		{
			bInitializ = true;

			transform.localPosition = new Vector3(fEndPosX, transform.localPosition.y, transform.localPosition.z);

			return true;
		}

		fPos = Mathf.Lerp(fStartPosX, fEndPosX , fParameter);
		transform.localPosition = new Vector3(fPos, transform.localPosition.y, transform.localPosition.z);

		return false;
	}

	public bool NextMessage()
	{
		// 初期化処理
		if (bInitializ)
		{
			fParameter = 0.0f;

			StartCoroutine("FadeOut");

			bInitializ = false;
		}

		// フェードアウト終了待ち
		if (!bFinFadeOut)
			return false;

		fParameter += Time.deltaTime / fSlideInTime;

		// 終了判定
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
	private IEnumerator FadeOut()
	{
		// 初期化処理
		fAlpha = 1.0f;
		bInitializ_FadeOut = false;
		bFinFadeOut = false;

		while(true)
		{
			fAlpha -= Time.deltaTime / fFadeOutTime;

			// 終了判定
			if (fAlpha <= 0.0f)
			{
				bInitializ_FadeOut = true;

				transform.localPosition = new Vector3(fStartPosX, transform.localPosition.y, transform.localPosition.z);		// 位置を初期座標に戻す
				sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1.0f);													// α値を元に戻す
				sr.sprite = DrawMessage[++nMessageNum];																			// スプライト切り替え

				bFinFadeOut = true;

				yield break; 
			}

			sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, fAlpha);

			yield return null;
		}
	}

	// 完全に終わるときのフェードアウト
	public bool FinFadeOut()
	{
		if (bInitializ)
		{
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
