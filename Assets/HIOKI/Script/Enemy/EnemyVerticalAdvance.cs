using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVerticalAdvance : EnemyBase {

	[SerializeField]
	private float fEnemyMoveSpeed;
	[SerializeField]
	private float fMinRange;
	[SerializeField]
	private float fMaxRange;
	[SerializeField]
	private Vector3[] SetRot;

	private static float fOriPos;
	private static int nStart;
	private float fSetPos;
	private bool bFlg = false;

	//private bool bDirFlg = false;

	// Use this for initialization
	void Start () {
        fEnemyMoveSpeed *= nStart;

        m_AddScoreNum = 100;
	}
	
	protected override void Move()
    {
        if (!m_Move)
            return;

		Vector3 EnemyPos = transform.position;					//値代入
		Vector3 Rot = transform.localEulerAngles;

		EnemyPos.z += fEnemyMoveSpeed * Time.deltaTime * 1.0f;		//移動

		#region 範囲補正
		if (EnemyPos.z <= fMinRange) {
			EnemyPos.z = fMinRange;									//補正
			fEnemyMoveSpeed *= -1;									//反転
			Rot.y = SetRot[0].y;											//回転
		} else if (fMaxRange <= EnemyPos.z) {
			EnemyPos.z = fMaxRange;									//補正
			fEnemyMoveSpeed *= -1;									//反転
			Rot.y = SetRot[1].y;									//回転
		}
		#endregion

		transform.position = EnemyPos;							//値セット
		transform.localEulerAngles = Rot;							//値セット
	}

	public GameObject CreateEnemyVertical()
	{
		return this.gameObject;									//生成
	}

	public Quaternion RotVertical(int nRand)
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
