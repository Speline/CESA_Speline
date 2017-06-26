using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RequestButton : MonoBehaviour {

	[SerializeField]
	private Image ReqBut;

	private bool bMoveFlg = false;
	private bool bCheckFlg = false;

	// Use this for initialization
	void Start () {
		ReqBut = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
	}

	public void Move()
	{
		if (!bMoveFlg)
			return;

		Color MyColor = ReqBut.color;

		MyColor.a += 0.05f;

		if (MyColor.a >= 1.0f) {
			MyColor.a = 1.0f;
			bMoveFlg = false;
		}

		ReqBut.color = MyColor;
	}

	public void ButtonPush()
	{
		if (!bMoveFlg) {
			bCheckFlg = true;
		}
	}

	public void DisplayActive()
	{
		bMoveFlg = true;
	}

	public bool LookCheckFlg()
	{
		return bCheckFlg;
	}
}
