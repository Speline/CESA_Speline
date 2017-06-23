using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestForm : MonoBehaviour {

	[SerializeField]
	private float fSpeed;
	[SerializeField]
	private Vector3 vecStopPos;

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
		if (bFlg)
			return;

		Vector3 pos = transform.localPosition;

		pos.y -= fSpeed;

		if (pos.y < vecStopPos.y) {
			pos.y = vecStopPos.y;
			bFlg = true;
		}

		transform.localPosition = pos;

	}

	public bool RequestFlg()
	{
		return bFlg;
	}
	public void SetStopPos()
	{
		transform.localPosition = vecStopPos;
	}
}
