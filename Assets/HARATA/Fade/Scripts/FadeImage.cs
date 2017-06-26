/*
 The MIT License (MIT)

Copyright (c) 2013 yamamura tatsuhiko

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

using UnityEditor;

public class FadeImage : UnityEngine.UI.Graphic , IFade
{
	// フェード画像の組み合わせ
	class ImageData
	{
		string m_szNowScene;
		string m_szNextScene;
		int m_nFadeIn;
		int m_nFadeOut;

		public ImageData(string SetNowScene, string SetNextScene, int SetnFadeIn, int SetnFadeOut)
		{
			m_szNowScene  = SetNowScene;
			m_szNextScene = SetNextScene;
			m_nFadeIn     = SetnFadeIn;
			m_nFadeOut    = SetnFadeOut;
		}

		public string szNowScene { get { return m_szNowScene; } }
		public string szNextScene { get { return m_szNextScene; } }
		public int nFadeIn { get { return m_nFadeIn; } }
		public int nFadeOut { get { return m_nFadeOut; } }
	}

	List<ImageData> imagedata = new List<ImageData>();

	[SerializeField]	Texture[] maskTexture;		// マスク画像


	[SerializeField, Range (0, 1)]
	private float cutoutRange;

	public float Range {
		get {
			return cutoutRange;
		}
		set {
			cutoutRange = value;
			UpdateMaskCutout (cutoutRange);
		}
	}

	// フェードの色変更用
	private Color color;
	public Color FadeColor
	{
		get
		{
			return color;
		}
		set
		{
			color = value;
			material.SetColor("_Color", value);
		}
	}

	void Awake()
	{
		SetFadeImage();

		StartCoroutine("bbb");
	}

	IEnumerator bbb()
	{
		yield return null;

		// スクリプト設定
		GameObject.Find("Scenemanager").GetComponent<Scenemanager>().SetFadeImage(this);
	}

	protected override void Start ()
	{
		base.Start ();
	}

	private void UpdateMaskCutout (float range)
	{
		enabled = true;
		material.SetFloat ("_Range", 1 - range);
	}

	// 今のシーンと次のシーンから、フェード用画像を切り替える
	public void UpdateMaskTexture(string now, string next, bool bFadeIn)
	{
		for(int i = 0 ; i < imagedata.Count ; i ++)
		{
			if ((imagedata[i].szNowScene == now) && (imagedata[i].szNextScene == next))
			{
				if(bFadeIn)
				{
					material.SetTexture("_MaskTex", maskTexture[imagedata[i].nFadeIn]);
				}
				else
				{
					material.SetTexture("_MaskTex", maskTexture[imagedata[i].nFadeOut]);
				}

				material.SetColor("_Color", FadeColor);
				break;
			}
		}
	}

	#if UNITY_EDITOR
	protected override void OnValidate ()
	{
		base.OnValidate ();
		UpdateMaskCutout (Range);
	}
	#endif

	// シーンとフェード画像の関係を設定する
	// 長いので関数にしただけ
	private void SetFadeImage()
	{
		imagedata.Add(new ImageData("Title"       , "StageSelect", 0, 1));	// ←タイトルからステージセレクトに行く時は、フェードイン0番の画像、フェードアウト1番の画像
		imagedata.Add(new ImageData("StageSelect" , "GameMain"   , 1, 2));
		imagedata.Add(new ImageData("GameMain"    , "Result"     , 0, 0));
		imagedata.Add(new ImageData("GameMain"    , "GameMain"   , 0, 0));
		imagedata.Add(new ImageData("GameMain"    , "StageSelect", 0, 0));
		imagedata.Add(new ImageData("Result"      , "GameMain"   , 0, 0));
		imagedata.Add(new ImageData("Result"      , "StageSelect", 0, 0));
		imagedata.Add(new ImageData("GameOver"    , "GameMain"   , 0, 0));
		imagedata.Add(new ImageData("GameOver"    , "StageSelect", 0, 0));
	}
}