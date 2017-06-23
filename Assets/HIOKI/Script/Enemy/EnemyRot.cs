using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRot : EnemyBase {

	[SerializeField]
	private float fSpeedRot;

	[SerializeField]
	private float fSpeedMove;

	[SerializeField]
	private Vector3[] RotE;

	private static float fRotF;
	private float SetRotF;


	// Use this for initialization
	void Start () {
        SetRotF = fRotF;
        m_AddScoreNum = 110;
	}
	
	// Update is called once per frame
	void Update () {
        if (m_bMove)
        {
            Rot();
            Move();
        }
		base.Update ();
	}

	private void Rot()
	{
		Vector3 Rot = transform.localEulerAngles;

		Rot.y -= fSpeedRot * SetRotF;

		transform.localEulerAngles = Rot;
	}

	private void Move()
	{
		transform.position -= transform.forward * fSpeedMove;
	}

	public GameObject CreateEnemyRot()
	{
		return this.gameObject;
	}

	public Quaternion RotRot(int nRand, int nSetRotNom)
	{
		switch (nSetRotNom) {
		case 0:
			if (nRand == 0) {
				transform.localEulerAngles = RotE [0];
				fRotF = 1.0f;
			} else {
				transform.localEulerAngles = RotE [1];
				fRotF = -1.0f;
			}
			break;
		case 1:
			if (nRand == 0) {
				transform.localEulerAngles = RotE [2];
				fRotF = 1.0f;
			} else {
				transform.localEulerAngles = RotE [3];
				fRotF = -1.0f;
			}
			break;
		case 2:
			if (nRand == 0) {
				transform.localEulerAngles = RotE [0];
				fRotF = -1.0f;
			} else {
				transform.localEulerAngles = RotE [1];
				fRotF = 1.0f;
			}
			break;
		case 3:
			if (nRand == 0) {
				transform.localEulerAngles = RotE [2];
				fRotF = -1.0f;
			} else {
				transform.localEulerAngles = RotE [3];
				fRotF = 1.0f;
			}
			break;

		}
		return transform.localRotation;
	}
}
