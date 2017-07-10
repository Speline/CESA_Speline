using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// 各ステージの記録を管理する
// ついでにステージ画像にステージ番号をつける
public class StageSelect_Record : MonoBehaviour
{
	//[SerializeField]	Sprite TimeTexture;
	//[SerializeField]	Sprite ScoreTexture;
	[SerializeField]	Sprite[] NumberTexture = new Sprite[11];

	[SerializeField]	Vector3 vTimePos;		// ローカルポジション
	[SerializeField]	Vector3 vScorePos;		// スコアポジション
	[SerializeField]	float fCharSpace;		// 文字の間隔
	[SerializeField]	float fColonSpace;		// コロンの間隔

	[SerializeField]	Vector3 vStageNoPos;	// ステージ番号の場所

	Transform[] Stage = new Transform[10];
	//SpriteRenderer[] sr = new SpriteRenderer[100];
	SpriteRenderer[] sr = new SpriteRenderer[91];		// タイム3桁*10　＋　スコア5桁*10　＋　ステージ番号(10だけ2個使う)
	int nsr = 0;
	
	float[] fTime = new float[10];
	int[] nDrawTime = new int[3];
	int[] nScore = new int[10];
	int[] nDrawScore = new int[5];

	float fAlpha = 0.0f;



	// Use this for initialization
	void Start ()
	{
		Vector3 pos;
		Vector3 vtimepos;

		
		//--- ステージの情報読み込み
		TextAsset StageData = Resources.Load("StageData/StageData") as TextAsset;   // テキストデータ取得
		string StageTextData = StageData.text;                                      // テキストデータをstringに
		string[] StageDataArray = StageTextData.Split('\n');                        // 行ごとに分ける(一種類ずつに分ける)
		string[] StageOneData;

		for (int j = 0; j < StageDataArray.GetLength(0); j++)
		{
			StageOneData = StageDataArray[j].Split(',');
			StageOneData.ToList().ForEach(x => x = x.Trim());
			nScore[j] = int.Parse(StageOneData[StageOneData.GetLength(0) - 1]);
		}

		// データの読み込み
		for(int i = 0 ; i < 10 ; i ++)
		{
			Stage[i] = transform.GetChild(i);

			//if (PlayerPrefs.HasKey("ScoreStage" + (i + 1)))
			//	nScore[i] = PlayerPrefs.GetInt("ScoreStage" + (i + 1));
			//else
			//	nScore[i] = 0;

			if (PlayerPrefs.HasKey("TimeStage" + (i + 1)))
				fTime[i] = PlayerPrefs.GetFloat("TimeStage" + (i + 1));
			else
				fTime[i] = -1.0f;

			#region タイム生成
			if (fTime[i] >= 0.0f)
			{// 1度クリアされてて、タイムがあるとき

				nDrawTime[0] = (int)(fTime[i] / 60.0f);
				nDrawTime[1] = ((int)fTime[i] % 60) / 10;
				nDrawTime[2] = ((int)fTime[i] % 60) - nDrawTime[1] * 10;

				vtimepos = vTimePos;
			}
			else
			{// まだ一回もクリアしていないとき
				for (int j = 0; j < nDrawTime.GetLength(0); j++)
					nDrawTime[j] = 10;		// ハイフン

				vtimepos = new Vector3(vTimePos.x, vTimePos.y, vTimePos.z);
			}

			//GameObject obj = new GameObject("time");
			//sr[nsr] = obj.AddComponent<SpriteRenderer>();
			//obj.AddComponent<SortingLayer>().OrderInLayer = 1;
			//sr[nsr].sprite = NumberTexture[nDrawTime[0]];
			//sr[nsr].color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
			//nsr ++;
			//obj.transform.parent = Stage[i];
			//obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
			//pos = vtimepos;
			//obj.transform.localPosition = pos;

			GameObject obj = new GameObject("time");
			sr[nsr] = obj.AddComponent<SpriteRenderer>();
			obj.AddComponent<SortingLayer>().OrderInLayer = 1;
			sr[nsr].sprite = NumberTexture[nDrawTime[0]];
			sr[nsr].color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
			nsr++;
			obj.transform.parent = Stage[i];
			obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
			//pos = new Vector3(pos.x + fCharSpace, pos.y, pos.z);
			pos = vtimepos;
			obj.transform.localPosition = pos;

			obj = new GameObject("time");
			sr[nsr] = obj.AddComponent<SpriteRenderer>();
			obj.AddComponent<SortingLayer>().OrderInLayer = 1;
			sr[nsr].sprite = NumberTexture[nDrawTime[1]];
			sr[nsr].color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
			nsr++;
			obj.transform.parent = Stage[i];
			obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
			pos = new Vector3(pos.x + fCharSpace + fColonSpace, pos.y, pos.z);
			obj.transform.localPosition = pos;

			obj = new GameObject("time");
			sr[nsr] = obj.AddComponent<SpriteRenderer>();
			obj.AddComponent<SortingLayer>().OrderInLayer = 1;
			sr[nsr].sprite = NumberTexture[nDrawTime[2]];
			sr[nsr].color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
			nsr++;
			obj.transform.parent = Stage[i];
			obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
			pos = new Vector3(pos.x + fCharSpace, pos.y, pos.z);
			obj.transform.localPosition = pos;
			#endregion

			#region スコア生成
			nDrawScore[0] = nScore[i] / 10000;
			nDrawScore[1] = nScore[i] / 1000 - nDrawScore[0] * 10;
			nDrawScore[2] = nScore[i] / 100 - nDrawScore[0] * 100 - nDrawScore[1] * 10;
			nDrawScore[3] = nScore[i] / 10 - nDrawScore[0] * 1000 - nDrawScore[1] * 100 - nDrawScore[2] * 10;
			nDrawScore[4] = nScore[i] - nDrawScore[0] * 10000 - nDrawScore[1] * 1000 - nDrawScore[2] * 100 - nDrawScore[3] * 10;

			obj = new GameObject("score");
			sr[nsr] = obj.AddComponent<SpriteRenderer>();
			obj.AddComponent<SortingLayer>().OrderInLayer = 1;
			sr[nsr].sprite = NumberTexture[nDrawScore[0]];
			sr[nsr].color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
			nsr++;
			obj.transform.parent = Stage[i];
			obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
			pos = vScorePos;
			obj.transform.localPosition = pos;

			obj = new GameObject("score");
			sr[nsr] = obj.AddComponent<SpriteRenderer>();
			obj.AddComponent<SortingLayer>().OrderInLayer = 1;
			sr[nsr].sprite = NumberTexture[nDrawScore[1]];
			sr[nsr].color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
			nsr++;
			obj.transform.parent = Stage[i];
			obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
			pos = new Vector3(pos.x + fCharSpace, pos.y, pos.z);
			obj.transform.localPosition = pos;

			obj = new GameObject("score");
			sr[nsr] = obj.AddComponent<SpriteRenderer>();
			obj.AddComponent<SortingLayer>().OrderInLayer = 1;
			sr[nsr].sprite = NumberTexture[nDrawScore[2]];
			sr[nsr].color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
			nsr++;
			obj.transform.parent = Stage[i];
			obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
			pos = new Vector3(pos.x + fCharSpace, pos.y, pos.z);
			obj.transform.localPosition = pos;

			obj = new GameObject("score");
			sr[nsr] = obj.AddComponent<SpriteRenderer>();
			obj.AddComponent<SortingLayer>().OrderInLayer = 1;
			sr[nsr].sprite = NumberTexture[nDrawScore[3]];
			sr[nsr].color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
			nsr++;
			obj.transform.parent = Stage[i];
			obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
			pos = new Vector3(pos.x + fCharSpace, pos.y, pos.z);
			obj.transform.localPosition = pos;

			obj = new GameObject("score");
			sr[nsr] = obj.AddComponent<SpriteRenderer>();
			obj.AddComponent<SortingLayer>().OrderInLayer = 1;
			sr[nsr].sprite = NumberTexture[nDrawScore[4]];
			sr[nsr].color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
			nsr++;
			obj.transform.parent = Stage[i];
			obj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
			pos = new Vector3(pos.x + fCharSpace, pos.y, pos.z);
			obj.transform.localPosition = pos;
			#endregion

			#region ステージ番号をつける
			if(i != 9)
			{
				obj = new GameObject("StageNo");
				sr[nsr] = obj.AddComponent<SpriteRenderer>();
				obj.AddComponent<SortingLayer>().OrderInLayer = 1;
				sr[nsr].sprite = NumberTexture[i + 1];
				sr[nsr].color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
				nsr++;
				obj.transform.parent = Stage[i];
				obj.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
				pos = vStageNoPos;
				obj.transform.localPosition = pos;
			}
			else
			{
				obj = new GameObject("StageNo");
				sr[nsr] = obj.AddComponent<SpriteRenderer>();
				obj.AddComponent<SortingLayer>().OrderInLayer = 1;
				sr[nsr].sprite = NumberTexture[1];
				sr[nsr].color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
				nsr++;
				obj.transform.parent = Stage[i];
				obj.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
				pos = new Vector3(vStageNoPos.x - 0.3f, vStageNoPos.y, vStageNoPos.z);
				obj.transform.localPosition = pos;

				obj = new GameObject("StageNo");
				sr[nsr] = obj.AddComponent<SpriteRenderer>();
				obj.AddComponent<SortingLayer>().OrderInLayer = 1;
				sr[nsr].sprite = NumberTexture[0];
				sr[nsr].color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
				nsr++;
				obj.transform.parent = Stage[i];
				obj.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
				pos = new Vector3(pos.x + 0.6f, pos.y, pos.z);
				obj.transform.localPosition = pos;
			}
			#endregion
		}
	}


	// フェードインする
	public void FadeInRecord(float fFadeInTime)
	{
		StartCoroutine("FadeInCoroutine", fFadeInTime);
	}

	IEnumerator FadeInCoroutine(float fFadeInTime)
	{
		while(true)
		{
			fAlpha += Time.deltaTime / fFadeInTime;

			if (fAlpha >= 1.0f)
			{
				for (int i = 0; i < sr.GetLength(0); i++)
					sr[i].color = new Color(sr[i].color.r, sr[i].color.g, sr[i].color.b, 1.0f);

				yield break;
			}

			for (int i = 0; i < sr.GetLength(0); i++)
				sr[i].color = new Color(sr[i].color.r, sr[i].color.g, sr[i].color.b, fAlpha);

			yield return null;
		}
	}

	// スキップ処理
	public void Skip()
	{
		for (int i = 0; i < sr.GetLength(0); i++)
			sr[i].color = new Color(sr[i].color.r, sr[i].color.g, sr[i].color.b, 1.0f);
	}
}
