using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whiteandblack : MonoBehaviour {

	[SerializeField]
	private SpriteRenderer _White;

	[SerializeField]
	private float fSpeed;

	private bool bEndFlg = false;

	// Use this for initialization
	void Start () {
		_White = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		WhiteorBlack ();
	}

	private void WhiteorBlack()
	{

		Color color = _White.color;

		color.a -= fSpeed;

		if (color.a <= 0.0f) {
			color.a = 0.0f;
			bEndFlg = true;
		}

		_White.color = color;
	}

	public bool CheckFlg()
	{
		return bEndFlg;
	}
}
