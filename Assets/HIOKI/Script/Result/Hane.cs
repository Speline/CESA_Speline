using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hane : MonoBehaviour {

	[SerializeField]
	private float fMoveSpeed;				//移動スピード
	[SerializeField]
	private float[] fMaxPosY;				//上下移動Max
	[SerializeField]
	private float[] fMinPosY;				//上下移動Min

	[SerializeField]
	private float[] fMoveMax;					//移動最大値
	[SerializeField]
	private float fStartSpeedX;
	[SerializeField]
	private float fStartSpeedY;
	[SerializeField]
	private float[] fStartStop;				//ストップする場所

	[SerializeField]
	private Vector3 LastPos;

	private int nMove = 3;
	private int nSelect = 1;
	private bool bMoveFlg = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		switch(nMove){
		case 0:
			StartMove ();
			break;
		case 1:
			Move ();
			break;
		case 2:
			SecondStop ();
			break;
		case 3:
			MoveScore ();
			break;
		case 4:
			DisplayOut ();
			break;
		case 5:
			break;
		}
	}


	private void StartMove()
	{
		Vector3 pos = transform.localPosition;
		pos.x -= fStartSpeedX;
		pos.y -= fStartSpeedY;

		if (pos.y <= fStartStop[0]) {
			pos.y = fStartStop[0];
			nMove++;
			bMoveFlg = true;
		}

		transform.localPosition = pos;
	}

	private void Move()
	{
		if (!bMoveFlg)
			return;
		
		Vector3 pos = transform.localPosition;

		switch(nSelect){
		case 0:
			pos.x += fMoveSpeed * 0.6f;
			pos.y += fMoveSpeed;
			if (pos.y >= fMaxPosY[0]) {
				pos.y = fMaxPosY[0];
				nSelect++;
			}
			break;
		case 1:
			pos.x -= fMoveSpeed * 0.1f;
			pos.y -= fMoveSpeed;
			if (pos.y <= fMinPosY[0]) {
				pos.y = fMinPosY[0];
				nSelect = 0;
			}
			break;
		}

		if (pos.x >= fMoveMax[0]) {
			pos.x = fMoveMax[0];
			bMoveFlg = false;
			nMove++;
			nSelect = 1;
		}

		transform.localPosition = pos;
	}

	private void SecondStop()
	{
		Vector3 pos = transform.localPosition;
		pos.x -= 0.1f;
		pos.y -= 0.06f;

		if (pos.y <= fStartStop[1]) {
			pos.y = fStartStop[1];
			nMove++;
			bMoveFlg = true;
		}

		transform.localPosition = pos;
	}
	private void MoveScore()
	{
		if (!bMoveFlg)
			return;

		Vector3 pos = transform.localPosition;

		switch(nSelect){
		case 0:
			pos.x += fMoveSpeed * 0.7f;
			pos.y += fMoveSpeed;
			if (pos.y >= fMaxPosY[1]) {
				pos.y = fMaxPosY[1];
				nSelect++;
			}
			break;
		case 1:
			pos.x -= fMoveSpeed * 0.1f;
			pos.y -= fMoveSpeed;
			if (pos.y <= fMinPosY[1]) {
				pos.y = fMinPosY[1];
				nSelect = 0;
			}
			break;
		}

		if (pos.x >= fMoveMax[1]) {
			pos.x = fMoveMax[1];
			bMoveFlg = false;
			nMove++;
		}

		transform.localPosition = pos;
	}

	public void DisplayOut()
	{
		Vector3 pos = transform.localPosition;

		pos.x += fMoveSpeed;

		if (pos.x >= LastPos.x) {
			pos.x = LastPos.x;
			nMove = 5;
			bMoveFlg = true;
		}

		transform.localPosition = pos;
	}

	public bool HaneFlg()
	{
		return bMoveFlg;
	}

	public void HaneMove()
	{
		nMove = 0;
	}

	public void LastHane()
	{
		transform.localPosition = new Vector3 (LastPos.x, LastPos.y, LastPos.z);
		nMove = 5;
	}
}
