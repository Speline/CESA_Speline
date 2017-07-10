using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalScore : MonoBehaviour {

	[SerializeField]
	private Sprite[] NomberS;

	[SerializeField]
	private GameObject[] Score;

	[SerializeField]
	private SpriteRenderer[] ScoreRender;

	[SerializeField]
	private float fSpeed;

	private int nSelect = 0;
	private bool bFlg = false;
	private bool bMoveFlg = false;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < ScoreRender.Length; i++) {
			ScoreRender [i] = Score [i].GetComponent<SpriteRenderer> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		MoveAlpha ();
	}

	private void MoveAlpha()
	{
		if (!bMoveFlg)
			return;
		
		Color MyColor = ScoreRender [nSelect].color;

		MyColor.a += fSpeed;

		if (MyColor.a >= 1.0f) {
			MyColor.a = 1.0f;
			bFlg = true;
		}

		ScoreRender [nSelect].color = MyColor;

		if (bFlg) {
			nSelect++;
			bFlg = false;
			if (nSelect >= 5)
				bMoveFlg = false;
		}
	}

	public void ChageFlg()
	{
		bMoveFlg = true;
	}


	public void SetFinalScore(int nTenTh, int nTho, int nHun, int nTen, int nOne)
	{
		Score [0].GetComponent<SpriteRenderer> ().sprite = NomberS [nTenTh];
		Score [1].GetComponent<SpriteRenderer> ().sprite = NomberS [nTho];
		Score [2].GetComponent<SpriteRenderer> ().sprite = NomberS [nHun];
		Score [3].GetComponent<SpriteRenderer> ().sprite = NomberS [nTen];
		Score [4].GetComponent<SpriteRenderer> ().sprite = NomberS [nOne];	
	}

	public void LastDisplayScore()
	{
		for (int i = 0; i < ScoreRender.Length; i++) {
            ScoreRender[i].color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
		}
	}
}
