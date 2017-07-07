using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquareRotation : EnemyBase {

	[SerializeField]
	private float fEnemyMoveSpeed;
	[SerializeField]
	private float fSquare;
	[SerializeField]
	private Vector3[] PhaseRot;


	enum CHANGEDIR
	{
		Phase_0 = 0,
		Phase_1,
		Phase_2,
		Phase_3,

		Phase_Max,
	};

	private int ChangeDir = (int)CHANGEDIR.Phase_0;
	private Vector3 OriPos;
	private static int nStartPhase;
	private int nMoveSelect;

	// Use this for initialization
	void Start () {
        ChangeDir = nStartPhase;

        m_AddScoreNum = 120;
	}

    protected override void Move()
    {
        if (!m_Move)
            return;

		Vector3 EnemyPos = transform.localPosition;
		Vector3 EnemyRot = transform.localEulerAngles;

		//Debug.Log (ChangeDir);

		switch (ChangeDir) {
		case (int)CHANGEDIR.Phase_0:
			EnemyPos.x += fEnemyMoveSpeed * Time.deltaTime * 1.0f;
			if (OriPos.x + fSquare <= EnemyPos.x) {
				EnemyPos.x = OriPos.x + fSquare;
				OriPos = EnemyPos;
				EnemyRot.y = PhaseRot[(int)CHANGEDIR.Phase_1].y;
				ChangeDir++;
			}

			break;
		case (int)CHANGEDIR.Phase_1:
			EnemyPos.z -= fEnemyMoveSpeed * Time.deltaTime * 1.0f;
			if (EnemyPos.z <= OriPos.z - fSquare) {
				EnemyPos.z = OriPos.z - fSquare;
				OriPos = EnemyPos;
				EnemyRot.y = PhaseRot[(int)CHANGEDIR.Phase_2].y;
				ChangeDir++;
			}
			break;
		case (int)CHANGEDIR.Phase_2:
			EnemyPos.x -= fEnemyMoveSpeed * Time.deltaTime * 1.0f;
			if (EnemyPos.x <= OriPos.x - fSquare) {
				EnemyPos.x = OriPos.x - fSquare;
				OriPos = EnemyPos;
				EnemyRot.y = PhaseRot[(int)CHANGEDIR.Phase_3].y;
				ChangeDir++;
			}

			break;
		case (int)CHANGEDIR.Phase_3:
			EnemyPos.z += fEnemyMoveSpeed * Time.deltaTime * 1.0f;
			if (OriPos.z + fSquare <= EnemyPos.z) {
				EnemyPos.z = OriPos.z + fSquare;
				OriPos = EnemyPos;
				EnemyRot.y = PhaseRot[(int)CHANGEDIR.Phase_0].y;
				ChangeDir++;
			}
			break;
		case (int)CHANGEDIR.Phase_Max:
			ChangeDir = (int)CHANGEDIR.Phase_0;
			break;
		}

		transform.localEulerAngles = EnemyRot;
		transform.localPosition = EnemyPos;
	}

    protected override void GameOverMove()
    {

        Vector3 EnemyPos = transform.localPosition;
        Vector3 EnemyRot = transform.localEulerAngles;


        switch (ChangeDir)
        {
            case (int)CHANGEDIR.Phase_0:
                EnemyPos.x += fEnemyMoveSpeed * Time.deltaTime * 1.0f;
                if (OriPos.x + fSquare * 2.0f <= EnemyPos.x)
                {
                    EnemyPos.x = OriPos.x + fSquare * 2.0f;
                    OriPos = EnemyPos;
                    EnemyRot.y = PhaseRot[(int)CHANGEDIR.Phase_1].y;
                    ChangeDir++;
                }

                break;
            case (int)CHANGEDIR.Phase_1:
                EnemyPos.z -= fEnemyMoveSpeed * Time.deltaTime * 1.0f;
                if (EnemyPos.z <= OriPos.z - fSquare * 2.0f)
                {
                    EnemyPos.z = OriPos.z - fSquare * 2.0f;
                    OriPos = EnemyPos;
                    EnemyRot.y = PhaseRot[(int)CHANGEDIR.Phase_2].y;
                    ChangeDir++;
                }
                break;
            case (int)CHANGEDIR.Phase_2:
                EnemyPos.x -= fEnemyMoveSpeed * Time.deltaTime * 1.0f;
                if (EnemyPos.x <= OriPos.x - fSquare * 2.0f)
                {
                    EnemyPos.x = OriPos.x - fSquare * 2.0f;
                    OriPos = EnemyPos;
                    EnemyRot.y = PhaseRot[(int)CHANGEDIR.Phase_3].y;
                    ChangeDir++;
                }

                break;
            case (int)CHANGEDIR.Phase_3:
                EnemyPos.z += fEnemyMoveSpeed * Time.deltaTime * 1.0f;
                if (OriPos.z + fSquare * 2.0f <= EnemyPos.z)
                {
                    EnemyPos.z = OriPos.z + fSquare * 2.0f;
                    OriPos = EnemyPos;
                    EnemyRot.y = PhaseRot[(int)CHANGEDIR.Phase_0].y;
                    ChangeDir++;
                }
                break;
            case (int)CHANGEDIR.Phase_Max:
                ChangeDir = (int)CHANGEDIR.Phase_0;
                break;
        }

        transform.localEulerAngles = EnemyRot;
        transform.localPosition = EnemyPos;
    }

    #region 生成
    public GameObject CreateEnemySquare()
	{
		return this.gameObject;									//生成
	}
	#endregion

	#region 角度
	public Quaternion RotSquare(int nRand)
	{
		transform.localEulerAngles = PhaseRot[nRand];			//角度設定
		nStartPhase = nRand;									//設定
		return transform.localRotation;
	}
	#endregion
}
