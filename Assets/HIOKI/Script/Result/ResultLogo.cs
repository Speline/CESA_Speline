using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultLogo : MonoBehaviour {


	private float nHosei = 1.0f;
	private bool bFlg = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
	}

	private void Move()
	{

		if (!bFlg)
			return;
		
		Vector3 pos = transform.localScale;

		pos.x += 0.005f * nHosei;
		pos.y += 0.005f * nHosei;

		if (pos.x >= 1.38f && pos.y >= 1.38f) {
			pos.x = 1.38f;
			pos.y = 1.38f;
			nHosei *= -1.0f;
		}

		if (pos.x <= 1.2f && pos.y <= 1.2f) {
			pos.x = 1.2f;
			pos.y = 1.2f;
			nHosei *= -1.0f;
		}

		transform.localScale = pos;
	}

	public void ChangeFlgLogo()
	{
		bFlg = true;
	}
}
