﻿using System.Collections;
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

    private float fRo = 0.2f;


	// Use this for initialization
	void Start () {
        SetRotF = fRotF;
        m_AddScoreNum = 110;
	}
	
	private void Rot()
	{
		Vector3 Rot = transform.localEulerAngles;

		Rot.y -= fSpeedRot * SetRotF;

		transform.localEulerAngles = Rot;
	}

    protected override void Move()
	{
        transform.position -= transform.forward * fSpeedMove;

        Rot();
	}

    protected override void GameOverMove()
    {
        transform.position -= transform.forward * fSpeedMove;

        OverRot();
    }

    private void OverRot()
    {
        Vector3 Rot = transform.localEulerAngles;

        Rot.y -= fSpeedRot * SetRotF * fRo;


        transform.localEulerAngles = Rot;
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
