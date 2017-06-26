using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySideAdvance : EnemyBase {

	[SerializeField]
	private float fEnemyMoveSpeed;
	[SerializeField]
	private float fMinRange;
	[SerializeField]
	private float fMaxRange;
	[SerializeField]
	private Vector3[] SetRot;

	private static int nStart;

	// Use this for initialization
	void Start () {
		fEnemyMoveSpeed *= nStart;

        m_AddScoreNum = 150;
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

		Vector3 EnemyPos = transform.localPosition;					//値代入
		Vector3 Rot = transform.localEulerAngles;					//値代入

		EnemyPos.x += fEnemyMoveSpeed * Time.deltaTime * 1.0f;		//移動

		#region 範囲補正
		if (EnemyPos.x <= fMinRange) {
			EnemyPos.x = fMinRange;									//補正
			fEnemyMoveSpeed *= -1;									//反転
			Rot.y = SetRot[0].y;											//回転
		} else if (fMaxRange <= EnemyPos.x) {
			EnemyPos.x = fMaxRange;									//補正
			fEnemyMoveSpeed *= -1;									//反転
			Rot.y = SetRot[1].y;											//回転
		}
		#endregion

		transform.localPosition = EnemyPos;							//値セット
		transform.localEulerAngles = Rot;							//値セット
	}

	public GameObject CreateEnemySide()
	{
		return this.gameObject;									//生成
	}

	public Quaternion RotSide(int nRand)
	{
		if (nRand == 0) {
			transform.localEulerAngles = SetRot[nRand];
			nStart = 1;
		} else {
			transform.localEulerAngles = SetRot [nRand];
			nStart = -1;
		}
		return transform.localRotation;
	}
}
