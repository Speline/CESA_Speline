using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hane : MonoBehaviour {

	[SerializeField]
	private float fMoveSpeed;
	[SerializeField]
	private float fMaxPosY;
	[SerializeField]
	private float fMinPosY;
	[SerializeField]
	private float fMoveMax;
	[SerializeField]
	private float fStartSpeedX;
	[SerializeField]
	private float fStartSpeedY;
	[SerializeField]
	private float fStartStop;
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
			DisplayOut ();
			break;
		case 3:
			break;
		}
	}
	private void StartMove()
	{
		Vector3 pos = transform.localPosition;
		pos.x -= fStartSpeedX;
		pos.y -= fStartSpeedY;

		if (pos.x <= fStartStop) {
			pos.x = fStartStop;
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
			pos.x += fMoveSpeed;
			pos.y += fMoveSpeed;
			if (pos.y >= fMaxPosY) {
				pos.y = fMaxPosY;
				nSelect++;
			}
			break;
		case 1:
			pos.x -= fMoveSpeed * 0.1f;
			pos.y -= fMoveSpeed;
			if (pos.y <= fMinPosY) {
				pos.y = fMinPosY;
				nSelect = 0;
			}
			break;
		}

		if (pos.x >= fMoveMax) {
			pos.x = fMoveMax;
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
			nMove = 3;
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
		nMove = 3;
	}
}
