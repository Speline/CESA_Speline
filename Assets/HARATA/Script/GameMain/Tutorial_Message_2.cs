using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Message_2 : MonoBehaviour
{
	[SerializeField]	float fStartPosX;			// 初期座標X
	[SerializeField]	float fEndPosX;				// 移動後座標X
	[SerializeField]	float fSlideInTime;			// スライドインにかける時間
	[SerializeField]	float fFadeOutTime;			// フェードアウトにかける時間

	RectTransform recttrans;						// 自身のRectTransform
	Text text;										// 自身のTex
	string[] szMessage1;							// ステージ1のメッセージ
	string[] szMessage2;							// ステージ2のメッセージ
	string[] DrawMessage;							// 表示するメッセージ
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
		recttrans = GetComponent<RectTransform>();
		text = GetComponent<Text>();

		SetMessage();

        if (GameManager.GetStage == 0)
        {
            DrawMessage = new string[szMessage1.GetLength(0)];
            for (int i = 0; i < DrawMessage.GetLength(0); i++)
                DrawMessage[i] = szMessage1[i];
        }
        else if (GameManager.GetStage == 1)
        {
            DrawMessage = new string[szMessage2.GetLength(0)];
            for (int i = 0; i < DrawMessage.GetLength(0); i++)
                DrawMessage[i] = szMessage2[i];
        }
        else
        {
            Destroy(this);
            return;
        }


		text.text = DrawMessage[nMessageNum];

		recttrans.anchoredPosition = new Vector2(fStartPosX, recttrans.anchoredPosition.y);

		// フォントサイズ変更
		if(GameManager.GetStage == 0)
			text.fontSize = 55;
		else if(GameManager.GetStage == 1)
			text.fontSize = 55;
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
			recttrans.anchoredPosition = new Vector2(fStartPosX, recttrans.anchoredPosition.y);

			bInitializ = false;
		}

		fParameter += Time.deltaTime / fSlideInTime;

		// 終了判定
		if (fParameter >= 1.0f)
		{
			bInitializ = true;

			recttrans.anchoredPosition = new Vector2(fEndPosX, recttrans.anchoredPosition.y);

			return true;
		}

		fPos = Mathf.Lerp(fStartPosX, fEndPosX , fParameter);
		recttrans.anchoredPosition = new Vector2(fPos, recttrans.anchoredPosition.y);

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

			recttrans.anchoredPosition = new Vector2(fEndPosX, recttrans.anchoredPosition.y);

			return true;
		}

		fPos = Mathf.Lerp(fStartPosX, fEndPosX, fParameter);
		recttrans.anchoredPosition = new Vector2(fPos, recttrans.anchoredPosition.y);

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

				recttrans.anchoredPosition = new Vector2(fStartPosX, recttrans.anchoredPosition.y);								// 位置を初期座標に戻す
				text.color = new Color(text.color.r, text.color.g, text.color.b, 1.0f);											// α値を元に戻す
				text.text = DrawMessage[++nMessageNum];																			// スプライト切り替え
	
				bFinFadeOut = true;
	
				yield break; 
			}

			text.color = new Color(text.color.r, text.color.g, text.color.b, fAlpha);
	
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

			text.color = new Color(text.color.r, text.color.g, text.color.b, 0.0f);
	
			return true;
		}

		text.color = new Color(text.color.r, text.color.g, text.color.b, fAlpha);
		return false;
	}

	// テキストで出すメッセージを設定する
	private void SetMessage()
	{
		// ステージ１
		szMessage1 = new string[6];
		szMessage1[0] = "キャラを1人タップ";
		szMessage1[1] = "対面キャラをタップ";
		szMessage1[2] = "敵を倒すとスコア上昇";
		szMessage1[3] = "目標達成でスコア更新";
		szMessage1[4] = "敵を倒してスコアを稼ぐ";
		szMessage1[5] = "最終目標達成でクリア";


		// ステージ２
		szMessage2 = new string[6];
		szMessage2[0] = "敵を連続で倒すとコンボ発生";
		szMessage2[1] = "コンボに応じてスコアボーナス";
		szMessage2[2] = "目標クリアでストック獲得";
		szMessage2[3] = "キャラを1人ダブルタップ";
		szMessage2[4] = "残り2人をタップ";
		szMessage2[5] = "最終目標を達成しましょう!";
	}
}
