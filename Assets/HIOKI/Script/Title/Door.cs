using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	#region 他のスクリプト呼ぶよう
	[SerializeField]
	private GameObject _Camera;
	private TitleCamera _CameraObj;

	#endregion

	[SerializeField]
	private GameObject DoorR;				//RightDoor
	[SerializeField]
	private GameObject DoorL;				//LeftDoor
	[SerializeField]
	private float fRotSpeed;				//回転スピード
	[SerializeField]
	private float fRotMax;					//回転最大値


	private bool bMoveFlg = false;			//回転フラグ

	// Use this for initialization
	void Start () {
		_CameraObj = _Camera.GetComponent<TitleCamera> ();		//設定
	}
	
	// Update is called once per frame
	void Update () {
		RotDoorR ();			//開く処理Right
		RotDoorL ();			//開く処理Left

	}

	private void RotDoorR()
	{
		#region 扉を開く処理Right
		if (!bMoveFlg)
			return;

		Vector3 RotR = DoorR.transform.localEulerAngles;	//値セット

		RotR.y += fRotSpeed;								//変更
		//チェック
		if (RotR.y >= fRotMax) {
			RotR.y = fRotMax;								//補正
			bMoveFlg = false;								//回転ストップ
			_CameraObj.CameraMoveFlg ();					//カメラ移動させる
		}

		DoorR.transform.localEulerAngles = RotR;			//値代入
		#endregion

	}

	private void RotDoorL()
	{
		#region 扉を開く処理Left
		if (!bMoveFlg)
			return;

		Vector3 RotL = DoorL.transform.localEulerAngles;	//値セット

		RotL.y -= fRotSpeed;								//変更

		DoorL.transform.localEulerAngles = RotL;			//値代入
		#endregion
	}

	public void ChangeMoveFlg()
	{
		bMoveFlg = true;
	}
}
