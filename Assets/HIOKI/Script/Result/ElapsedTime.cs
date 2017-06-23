using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElapsedTime : MonoBehaviour {

	[SerializeField]
	private float fAlphaSpeed;

	[SerializeField]
	private GameObject[] Time;

	[SerializeField]
	private Sprite[] SpriteNomber;

	[SerializeField]
	private SpriteRenderer _TimeNomber_0;
	[SerializeField]
	private SpriteRenderer _TimeNomber_1;
	[SerializeField]
	private SpriteRenderer _TimeNomber_2;
	[SerializeField]
	private SpriteRenderer _TimeNomber_3;
	[SerializeField]
	private SpriteRenderer _TimeColon;

	private int nTime = 0;
	private int nSelect = 5;
	private bool bChengeFlg = false;

	// Use this for initialization
	void Start () {
		#region タイムの設定
		_TimeNomber_0 = Time[0].GetComponent<SpriteRenderer> ();
		_TimeNomber_1 = Time[1].GetComponent<SpriteRenderer> ();
		_TimeNomber_2 = Time[2].GetComponent<SpriteRenderer> ();
		_TimeNomber_3 = Time[3].GetComponent<SpriteRenderer> ();
		_TimeColon = Time[4].GetComponent<SpriteRenderer> ();
		#endregion
	}

	// Update is called once per frame
	void Update () {

		switch(nSelect){
		case 0:
			TimeDisplay_0 ();
			break;
		case 1:
			TimeDisplay_1 ();
			break;
		case 2:
			TimeDisplay_Colon ();
			break;
		case 3:
			TimeDisplay_2 ();
			break;
		case 4:
			TimeDisplay_3 ();
			break;
		case 5:
			break;
		}
	}

	#region タイムのAlpha0
	private void TimeDisplay_0()			
	{
		Color myColor = _TimeNomber_0.color;

		myColor.a += fAlphaSpeed;

		if (myColor.a >= 1.0f) {
			myColor.a = 1.0f;
			nSelect++;
		}

		_TimeNomber_0.color = myColor;
	}
	#endregion

	#region タイムのAlpha1
	private void TimeDisplay_1()
	{
		Color myColor = _TimeNomber_1.color;

		myColor.a += fAlphaSpeed;

		if (myColor.a >= 1.0f) {
			myColor.a = 1.0f;
			nSelect++;
		}

		_TimeNomber_1.color = myColor;
	}
	#endregion

	#region タイムのAlpha2
	private void TimeDisplay_2()
	{
		Color myColor = _TimeNomber_2.color;

		myColor.a += fAlphaSpeed;

		if (myColor.a >= 1.0f) {
			myColor.a = 1.0f;
			nSelect++;
		}

		_TimeNomber_2.color = myColor;
	}
	#endregion

	#region タイムのAlpha3
	private void TimeDisplay_3()
	{
		Color myColor = _TimeNomber_3.color;

		myColor.a += fAlphaSpeed;

		if (myColor.a >= 1.0f) {
			myColor.a = 1.0f;
			nSelect++;
		}

		_TimeNomber_3.color = myColor;
	}
	#endregion

	#region タイムのAlpha Colon
	private void TimeDisplay_Colon()
	{
		Color myColor = _TimeColon.color;

		myColor.a += fAlphaSpeed;

		if (myColor.a >= 1.0f) {
			myColor.a = 1.0f;
			nSelect++;
		}

		_TimeColon.color = myColor;
	}

	#endregion

	#region タイムの数字設定
	public void SetElapsedTime(int nTime_0, int nTime_1, int nTime_2, int nTime_3)
	{
		Time [0].GetComponent<SpriteRenderer> ().sprite = SpriteNomber [nTime_0];
		Time [1].GetComponent<SpriteRenderer> ().sprite = SpriteNomber [nTime_1];
		Time [2].GetComponent<SpriteRenderer> ().sprite = SpriteNomber [nTime_2];
		Time [3].GetComponent<SpriteRenderer> ().sprite = SpriteNomber [nTime_3];		
	}
	#endregion

	#region タイムのアルファの移動
	public void MoveTime()
	{
		nSelect = 0;
	}

	#endregion

	#region スキップ用
	public void LastDisplay()
	{
		_TimeNomber_0.color = new Color(255.0f,255.0f,255.0f,1.0f);
		_TimeNomber_1.color = new Color(255.0f,255.0f,255.0f,1.0f);
		_TimeColon.color = new Color(255.0f,255.0f,255.0f,1.0f);
		_TimeNomber_2.color = new Color(255.0f,255.0f,255.0f,1.0f);
		_TimeNomber_3.color = new Color(255.0f,255.0f,255.0f,1.0f);
		nSelect = 5;
	}

	#endregion
}
