using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriangleVertical : EnemyBase {

	[SerializeField]
	private Vector3[] LookPos;

	[SerializeField]
	private float fForwardSpeed;

	[SerializeField]
	private float POSX;

	// Use this for initialization
	void Start () {


        m_AddScoreNum = 140;
	}

    protected override void Move()
	{
        if (!m_Move)
            return;

		Vector3 Rot = transform.localEulerAngles;
		transform.position += transform.forward * fForwardSpeed;



		if (Rot.y <= 180.0f) {
			if (transform.localPosition.x >= POSX) {
				if (transform.localPosition.z > 0.0f) {
					transform.LookAt (LookPos[2]);
				} else {
					transform.LookAt (LookPos[0]);
				}
			}
		} else {
			if (transform.localPosition.x <= -POSX) {
				if (transform.localPosition.z < 0.0f) {
					transform.LookAt (LookPos[1]);
				} else {
					transform.LookAt (LookPos[3]);
				}
			}
		}
	}

	public GameObject CreateEnemyTriangleVertical()
	{
		return this.gameObject;
	}

	public Quaternion RotTriangleVertical(int Look)
	{
		transform.LookAt (LookPos[Look]);
		return transform.localRotation;
	}
}
