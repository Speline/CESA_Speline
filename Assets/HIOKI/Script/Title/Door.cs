using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	[SerializeField]
	private GameObject _Camera;
	private TitleCamera _CameraObj;

	[SerializeField]
	private GameObject DoorR;
	[SerializeField]
	private GameObject DoorL;
	[SerializeField]
	private float fRotSpeed;
	[SerializeField]
	private float fRotMax;


	private bool bMoveFlg = false;

	// Use this for initialization
	void Start () {
		_CameraObj = _Camera.GetComponent<TitleCamera> ();
	}
	
	// Update is called once per frame
	void Update () {
		RotDoorR ();
		RotDoorL ();
	}

	private void RotDoorR()
	{
		#region 扉を開く処理Right
		if (!bMoveFlg)
			return;

		Vector3 RotR = DoorR.transform.localEulerAngles;

		RotR.y += fRotSpeed;
		//チェック
		if (RotR.y >= fRotMax) {
			RotR.y = fRotMax;
			bMoveFlg = false;
			_CameraObj.CameraMoveFlg ();
		}

		DoorR.transform.localEulerAngles = RotR;
		#endregion

	}

	private void RotDoorL()
	{
		#region 扉を開く処理Left
		if (!bMoveFlg)
			return;

		Vector3 RotL = DoorL.transform.localEulerAngles;

		RotL.y -= fRotSpeed;

		DoorL.transform.localEulerAngles = RotL;
		#endregion
	}

	public void ChangeMoveFlg()
	{
		bMoveFlg = true;
	}
}
