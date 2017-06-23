using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class NestQuestButton : MonoBehaviour {

	[SerializeField]
	private Image NestBut;

	private bool bMoveFlg = false;
	private bool bCheckFlg = false;

	// Use this for initialization
	void Start () {
		NestBut = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
	}

	public void Move()
	{
		if (!bMoveFlg)
			return;

		Color MyColor = NestBut.color;

		MyColor.a += 0.05f;

		if (MyColor.a >= 1.0f) {
			MyColor.a = 1.0f;
			bMoveFlg = false;
		}

		NestBut.color = MyColor;
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
