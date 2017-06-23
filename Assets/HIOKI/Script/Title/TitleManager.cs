using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour {

	#region 他のスプリクト呼ぶよう
	[SerializeField]
	private GameObject _TStart;
	private TitleStart　_TStartObj;

	[SerializeField]
	private GameObject _Magic;
	private magic _MagicObj;

	[SerializeField]
	private GameObject _Door;
	private Door _DoorObj;

	[SerializeField]
	private GameObject _Camera;
	private TitleCamera _CameraObj;

	#endregion

	private int nSelect = 0;				//Select
	private Touch touch;
	private int nMaCnt = 0;

	// Use this for initialization
	void Start () {
		//ここにBGMを呼び込む設定を入れる
		_TStartObj = _TStart.GetComponent<TitleStart> ();
		_MagicObj = _Magic.GetComponent<magic> ();
		_DoorObj = _Door.GetComponent<Door> ();
		_CameraObj = _Camera.GetComponent<TitleCamera> ();
	}
	
	// Update is called once per frame
	void Update () {
		switch(nSelect){
		case 0:
			StartTitle ();
			break;
		case 1:
			Touch ();		//画面タッチ
			break;
		case 2:
			//ここに飛ぶ処理
			Change();
			break;
		default:
			GameObject.Find("Back").GetComponent<SpriteRenderer>().color = new Color(0, 0, 255);
			break;
		}
	}

	private void StartTitle()
	{
		bool bFlg = _MagicObj.CheckFlg ();		//表示関係

		if (bFlg) {
			_TStart.gameObject.SetActive (true);	//表示
			nSelect++;
		}
	}

	private void Touch()
	{
		if (Input.GetKeyDown (KeyCode.Return)) {

			//ここにタッチした時の効果音をいれる

			_TStartObj.StopFade ();			//Startのフェードを止める
			_MagicObj.StopMove();			//魔法陣を止める
			_DoorObj.ChangeMoveFlg();		//ドアを開く
			nSelect = 2;
			//ここに画面遷移の設定をいれる

			Debug.Log ("押したよ");
		}

		// Mouseって書いてあるけど、タッチもこれで反応する。
		if (Input.GetMouseButtonDown (0)) {
			Debug.Log ("マウス");
			_TStartObj.StopFade ();			//Startのフェードを止める
			_MagicObj.StopMove();			//魔法陣を止める
			_DoorObj.ChangeMoveFlg();		//ドアを開く
			nSelect = 2;
		}
	}

	private void Change()
	{
		if (_CameraObj.ChangeCheck ()) {
			if(nMaCnt == 0){
				//ここに飛ぶ処理
				Scenemanager.Instance.LoadLevel("StageSelect", 0.5f, 0.5f, 0.5f);
				nMaCnt++;
			}
		}
	}
}
