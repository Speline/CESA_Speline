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
		//Debug.Log (Rot);
		transform.position += transform.forward * fForwardSpeed;



		if (Rot.y <= 180.0f) {
			if (transform.localPosition.x >= POSX) {
				if (transform.localPosition.z > 0.0f) {
					transform.LookAt (LookPos[2]);
					Debug.Log ("utano");
					Debug.Log (transform.localPosition);
				} else {
					transform.LookAt (LookPos[0]);
					Debug.Log ("chihaya");
					Debug.Log (transform.localPosition);
				}
			}
		} else {
			if (transform.localPosition.x <= -POSX) {
				if (transform.localPosition.z < 0.0f) {
					transform.LookAt (LookPos[1]);
					Debug.Log ("yuki");
					Debug.Log (transform.localPosition);
				} else {
					transform.LookAt (LookPos[3]);
					Debug.Log ("keiri");
					Debug.Log (transform.localPosition);
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
