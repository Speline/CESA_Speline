using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour {

	[SerializeField]
	private Vector3 StopPos;

	[SerializeField]
	private float fSpeed;

	private bool bQuestFlg = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		MoveQuest ();
	}

	private void MoveQuest()
	{
		if (!bQuestFlg)
			return;

		Vector3 pos = transform.localPosition;

		pos.z += fSpeed;

		if (pos.z >= StopPos.z) {
			pos.z = StopPos.z;
			bQuestFlg = false;
		}

		transform.localPosition = pos;
		
	}

	public void MoveFlg()
	{
		bQuestFlg = true;
	}

	public bool CheckFlg()
	{
		return bQuestFlg;
	}

	public void LastQuest()
	{
		transform.localPosition = new Vector3 (StopPos.x, StopPos.y, StopPos.z);
	}
}
