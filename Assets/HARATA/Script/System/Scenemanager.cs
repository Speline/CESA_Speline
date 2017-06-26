using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenemanager : MonoBehaviour
{
	private static Scenemanager instance;
	FadeImage fadeimage;

	private bool isFading = false;  // フェード中かどうか

	static Color FadeColor;			// フェードの色


	public static Scenemanager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = (Scenemanager)FindObjectOfType(typeof(Scenemanager));

				if (instance == null)
				{
					GameObject go = new GameObject("Scenemanager");
					instance = go.AddComponent<Scenemanager>();
				}
			}

			return instance;
		}
	}

	public void Awake()
	{
		if (this != Instance)
		{
			Destroy(this);
			return;
		}

		DontDestroyOnLoad(this.gameObject);
	}

	// 描画遷移
	public void LoadLevel(string scene, float fadeinTime, float waitTime, float fadeoutTime)
	{
		StartCoroutine( aaa(scene, fadeinTime, waitTime, fadeoutTime) );
	}

	IEnumerator aaa(string scene, float fadeinTime, float waitTime, float fadeoutTime)
	{
		bool bFadeIn = false;


		Fade.Instance.SetColor(FadeColor);				// 色変更

		yield return null;
		yield return null;

		Fade.Instance.SetColor(FadeColor);				// 色変更

		fadeimage.UpdateMaskTexture(SceneManager.GetActiveScene().name, scene, true);	// フェード画像切り替え

		// フェードイン
		Fade.Instance.FadeIn(fadeinTime, () =>
		{
			bFadeIn = true;
		});

		// フェードインが終わるまではここで処理ストップ
		while(!bFadeIn)
			yield return null;

		Fade.Instance.SetColor(FadeColor);		// 色変更
		fadeimage.UpdateMaskTexture(SceneManager.GetActiveScene().name, scene, false);	// フェード画像切り替え

		// シーン切り替え
		SceneManager.LoadScene(scene);
		
		// フェードアウト
		Fade.Instance.FadeOut(fadeoutTime, () =>
		{
			//FadeColor = Color.black;
		});
	}
	
	// 描画遷移(色指定あり)
	public void LoadLevel(string scene, float fadeinTime, float waitTime, float fadeoutTime, Color color)
	{
		// 色変更
		FadeColor = color;
		
		LoadLevel(scene, fadeinTime, waitTime, fadeoutTime);
	}

	// フェードしているか
	public bool GetisFading()
	{
		return isFading;
	}

	// FadeImage.csを設定する
	public void SetFadeImage(FadeImage cs)
	{
		fadeimage = cs;
	}
}
