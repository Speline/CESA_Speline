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
	[SerializeField]
	private GameObject _fR;
	[SerializeField]
	private GameObject _fL;
	[SerializeField]
	private SpriteRenderer fFadeDoorR;
	[SerializeField]
	private SpriteRenderer fFadeDoorL;


	private bool bMoveFlg = false;
	private int nSelect = 0;

	// Use this for initialization
	void Start () {
		_CameraObj = _Camera.GetComponent<TitleCamera> ();
		fFadeDoorR = _fR.GetComponent<SpriteRenderer> ();
		fFadeDoorL = _fL.GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {

		switch(nSelect){
		case 0:
			FadeDoor ();
			break;
		case 1:
			RotDoorR ();
			RotDoorL ();
			break;
		}

	}

	private void FadeDoor()
	{
		if (!bMoveFlg)
			return;
		Color FadeCoR = fFadeDoorR.color;
		Color FadeCoL = fFadeDoorL.color;

		FadeCoR.a += 0.01f;
		FadeCoL.a += 0.01f;

		if (FadeCoR.a >= 1.0f) {
			FadeCoR.a = 1.0f;
			FadeCoL.a = 1.0f;
			nSelect++;
		}

		fFadeDoorR.color = FadeCoR;
		fFadeDoorL.color = FadeCoL;

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
