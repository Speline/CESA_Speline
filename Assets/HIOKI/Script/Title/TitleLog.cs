using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLog : MonoBehaviour {

	[SerializeField]
	private SpriteRenderer _Log;		//フェード
	[SerializeField]
	private float fMoveSpeed;			//フェードスピード
	[SerializeField]
	private float fMaxY;				//

	private int nSelect = 0;
	private bool bTodi = false;

	// Use this for initialization
	void Start () {
		_Log = GetComponent<SpriteRenderer> ();
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
			ToDisappear ();
			break;
		case 3:
			break;
		}
	}

	private void FadeMagic()
	{
		Color color = _Log.color;

		color.a += 0.007f;

		if (color.a >= 1.0f) {
			color.a = 1.0f;
			nSelect++;
		}

		_Log.color = color;
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

	private void ToDisappear()
	{
		if (!bTodi)
			return;
		
		Color color = _Log.color;

		color.a -= 0.01f;

		if (color.a <= 0.0f) {
			color.a = 0.0f;
			bTodi = false;
			nSelect++;
		}

		_Log.color = color;
	}

	public void ChangeFlg()
	{
		bTodi = true;
	}
}
