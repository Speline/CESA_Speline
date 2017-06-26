using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magic : MonoBehaviour {

	[SerializeField]
	private float fRotSpeed;
	[SerializeField]
	private float fMoveSpeed;
	[SerializeField]
	private float fMaxY;
	[SerializeField]
	private SpriteRenderer _Magic;

	private int nSelect = 0;
	private bool bStopFlg = false;
	private bool bStart = true;

	// Use this for initialization
	void Start () {
		_Magic = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		switch (nSelect) {
		case 0:
			FadeMagic ();
			break;
		case 1:
			MoveMagic ();
			break;
		case 2:
			RotMove ();
			break;
		case 3:
			MagicActive ();
			break;
		}
	}
	private void FadeMagic()
	{
		Color color = _Magic.color;

		color.a += 0.007f;

		if (color.a >= 1.0f) {
			color.a = 1.0f;
			nSelect++;
		}

		_Magic.color = color;
	}

	private void MoveMagic()
	{
		Vector3 pos = transform.localPosition;

		pos.y += fMoveSpeed;

		if (pos.y >= fMaxY) {
			pos.y = fMaxY;
			nSelect++;
		}

		transform.localPosition = pos;
	}

	private void RotMove()
	{
		Vector3 vecRot = transform.localEulerAngles;

		vecRot.z += fRotSpeed;

		if (vecRot.z >= 360.0f) {
			vecRot.z = 0.0f;
		}

		transform.localEulerAngles = vecRot;
	}

	private void MagicActive()
	{
		Color color = _Magic.color;

		color.a -= 0.01f;

		if (color.a <= 0.0f) {
			color.a = 0.0f;
		}

		_Magic.color = color;
	}

	public void StopMove()
	{
		nSelect = 3;
	}

	public bool CheckFlg()
	{
		if (nSelect >= 2) {
			return true;
		}
		return false;
	}
}
