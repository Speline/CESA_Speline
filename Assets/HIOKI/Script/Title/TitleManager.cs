using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour {

	#region 他のスクリプト呼ぶよう
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

	[SerializeField]
	private GameObject _Log;
	private TitleLog _LogObj;

	#endregion

	private int nSelect = 0;				//イベント
	private Touch touch;					//Touch
	private int nMaCnt = 0;					//タッチ関係

	// Use this for initialization
	void Start () {
		
		#region 他のスクリプト呼ぶよう
		_TStartObj = _TStart.GetComponent<TitleStart> ();
		_MagicObj = _Magic.GetComponent<magic> ();
		_DoorObj = _Door.GetComponent<Door> ();
		_CameraObj = _Camera.GetComponent<TitleCamera> ();
		_LogObj = _Log.GetComponent<TitleLog> ();

		#endregion
		//ここにBGMを呼び込む設定を入れる
<<<<<<< HEAD
        BGMManager.Instance.Play("Title");
	}
	
	// Update is called once per frame
	void Update () {
		switch(nSelect){
		case 0:
			StartTitle ();	//最初
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

		if(Input.GetKeyDown(KeyCode.D))
			PlayerPrefs.DeleteAll();
	}

	private void StartTitle()
	{
		bool bFlg = _MagicObj.CheckFlg ();		//表示関係

		if (bFlg) {
			_TStart.gameObject.SetActive (true);	//表示
			nSelect++;							//変更
		}
	}

	private void Touch()
	{
		if (Input.GetKeyDown (KeyCode.Return)) {

			//ここにタッチした時の効果音をいれる

			_TStartObj.StopFade ();			//Startのフェードを止める
			_MagicObj.StopMove();			//魔法陣を止める
			_LogObj.ChangeFlg();			//タイトルを消す
			_DoorObj.ChangeMoveFlg();		//ドアを開く
			nSelect = 2;

		}

		// Mouseって書いてあるけど、タッチもこれで反応する。
		if (Input.GetMouseButtonDown (0)) {
			_TStartObj.StopFade ();			//Startのフェードを止める
			_MagicObj.StopMove();			//魔法陣を止める
			_LogObj.ChangeFlg();			//タイトルを消す
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
=======
        BGMManager.Instance.Play("Title");
	}
	
	// Update is called once per frame
	void Update () {
		switch(nSelect){
		case 0:
			StartTitle ();	//最初
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

		if(Input.GetKeyDown(KeyCode.D))
			PlayerPrefs.DeleteAll();
	}

	private void StartTitle()
	{
		bool bFlg = _MagicObj.CheckFlg ();		//表示関係

		if (bFlg) {
			_TStart.gameObject.SetActive (true);	//表示
			nSelect++;							//変更
		}
	}

	private void Touch()
	{
		if (Input.GetKeyDown (KeyCode.Return)) {

			//ここにタッチした時の効果音をいれる
            SEManager.Instance.Play("sei_ge_door_big_op01");
			_TStartObj.StopFade ();			//Startのフェードを止める
			_MagicObj.StopMove();			//魔法陣を止める
			_LogObj.ChangeFlg();			//タイトルを消す
			_DoorObj.ChangeMoveFlg();		//ドアを開く
			nSelect = 2;

		}

		// Mouseって書いてあるけど、タッチもこれで反応する。
        if (Input.GetMouseButtonDown(0))
        {
            SEManager.Instance.Play("sei_ge_door_big_op01");
			_TStartObj.StopFade ();			//Startのフェードを止める
			_MagicObj.StopMove();			//魔法陣を止める
			_LogObj.ChangeFlg();			//タイトルを消す
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
>>>>>>> f38db8d4c49e231478fbc3a797f71283eade1288
