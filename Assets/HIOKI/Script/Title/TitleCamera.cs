using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCamera : MonoBehaviour {

	[SerializeField]
	private float fMoveSpeed;
	[SerializeField]
	private float fMaxZ;

	private bool bMoveFlg = false;
	private bool bChange = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		MoveCamera ();
	}

	private void MoveCamera()
	{
		if (!bMoveFlg)
			return;
		Vector3 pos = transform.localPosition;

		pos.z += fMoveSpeed;

		if (pos.z >= fMaxZ)
		{
			pos.z = fMaxZ;
			bMoveFlg = false;
			bChange = true;
		}

		transform.localPosition = pos;

	}

	public void CameraMoveFlg()
	{
		bMoveFlg = true;
	}

	public bool ChangeCheck()
	{
		return bChange;
	}
}
