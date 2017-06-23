using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenemanager : MonoBehaviour
{
	private static Scenemanager instance;

	private bool isFading = false;  // フェード中かどうか

	static Color FadeColor;				// フェードの色


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
		if (this.isFading)
			return;

		Fade.Instance.SetColor(FadeColor);				// 色変更

		Fade.Instance.FadeIn(fadeinTime, () =>
		{
			// シーン切り替え
			SceneManager.LoadScene(scene);

			Fade.Instance.SetColor(FadeColor);			// 色変更
			
			Fade.Instance.FadeOut(fadeoutTime, () =>
			{
				FadeColor = Color.black;
			});
		});
	}
	
	// 描画遷移(色指定あり)
	public void LoadLevel(string scene, float fadeinTime, float waitTime, float fadeoutTime, Color color)
	{
		// 色変更
		FadeColor = color;
		
		LoadLevel(scene, fadeinTime, waitTime, fadeoutTime);
	}

	// フェードインなし
	public void LoadLevel_NoFadeIn(string scene, float waitTime, float fadeoutTime)
	{
		if (this.isFading)
			return;

		// シーン切り替え
		SceneManager.LoadScene(scene);

		Fade.Instance.FadeOut(fadeoutTime, () =>
		{

		});
	}

	// フェードしているか
	public bool GetisFading()
	{
		return isFading;
	}
}
