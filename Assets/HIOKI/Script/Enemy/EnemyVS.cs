using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVS : EnemyBase {

	[SerializeField]
	private float fEnemyMoveSpeed;
	[SerializeField]
	private Vector3[] PhaseRot;

	[SerializeField]
	private float Vertical;
	[SerializeField]
	private float Range;

	enum CHANGEDIR
	{
		Phase_0 = 0,
		Phase_1,
		Phase_2,
		Phase_3,

		Phase_Max,
	};

	private int ChangeDir = (int)CHANGEDIR.Phase_0;

	private static int nStart;
	private float fSetVRange;
	private float fSetSRange;
	private int nDir;
	private static int nDirr;

	// Use this for initialization
	void Start () {
		ChangeDir = nStart;
        nDir = nDirr;

        m_AddScoreNum = 160;
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
		Move ();

	}

	private void Move()
    {
        if (!m_bMove)
            return;

		Vector3 EnemyPos = transform.localPosition;
		Vector3 EnemyRot = transform.localEulerAngles;

		Debug.Log (ChangeDir);
		switch (ChangeDir) {
		case (int)CHANGEDIR.Phase_0:
			EnemyPos.x += fEnemyMoveSpeed * Time.deltaTime * 1.0f;
			if (Range <= EnemyPos.x) {
				EnemyPos.x = Range;
				if (nDir == 0) {
					EnemyRot.y = PhaseRot [(int)CHANGEDIR.Phase_1].y;
					ChangeDir++;
				} else {
					EnemyRot.y = PhaseRot [(int)CHANGEDIR.Phase_3].y;
					ChangeDir = (int)CHANGEDIR.Phase_3;
				}
			}
			break;
		case (int)CHANGEDIR.Phase_1:
			EnemyPos.z -= fEnemyMoveSpeed * Time.deltaTime * 1.0f;
			if (EnemyPos.z <= -Vertical) {
				EnemyPos.z = -Vertical;
				if (nDir == 0) {
					EnemyRot.y = PhaseRot [(int)CHANGEDIR.Phase_2].y;
					ChangeDir++;
				} else {
					EnemyRot.y = PhaseRot [(int)CHANGEDIR.Phase_0].y;
					ChangeDir--;
				}
			}
			break;
		case (int)CHANGEDIR.Phase_2:
			EnemyPos.x -= fEnemyMoveSpeed * Time.deltaTime * 1.0f;
			if (EnemyPos.x <= -Range) {
				EnemyPos.x = -Range;
				if (nDir == 0) {
					EnemyRot.y = PhaseRot [(int)CHANGEDIR.Phase_3].y;
					ChangeDir++;
				} else {
					EnemyRot.y = PhaseRot [(int)CHANGEDIR.Phase_1].y;
					ChangeDir--;
				}
			}
			break;
		case (int)CHANGEDIR.Phase_3:
			EnemyPos.z += fEnemyMoveSpeed * Time.deltaTime * 1.0f;
			if (Vertical <= EnemyPos.z) {
				EnemyPos.z = Vertical;

				if (nDir == 0) {
					EnemyRot.y = PhaseRot [(int)CHANGEDIR.Phase_0].y;
					ChangeDir = (int)CHANGEDIR.Phase_0;
				} else {
					EnemyRot.y = PhaseRot [(int)CHANGEDIR.Phase_2].y;
					ChangeDir--;
				}
			}
			break;
		}

		transform.localEulerAngles = EnemyRot;
		transform.localPosition = EnemyPos;

	}

	public GameObject CreateEnemyVS()
	{
		return this.gameObject;
	}

	public Quaternion VSRot(int nSpawPoint)
	{
		int nDiror = Random.Range (0, 2);
		int nSetPoint = nSpawPoint % 3;
		if (nSetPoint == 0) {
			if (nDiror == 0) {
				transform.localEulerAngles = PhaseRot [3];
				nStart = 3;
				nDirr = 0;
			} else {
				transform.localEulerAngles = PhaseRot [1];
				nStart = 1;
				nDirr = 1;
			}
		} else {
			if (nDiror == 0) {
				transform.localEulerAngles = PhaseRot [1];
				nStart = 1;
				nDirr = 0;
			} else {
				transform.localEulerAngles = PhaseRot [3];
				nStart = 3;	
				nDirr = 1;
			}
		}
		return transform.localRotation;
	}
}
