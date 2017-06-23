using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 常にカメラの正面に居続ける
public class CameraBillboard : MonoBehaviour
{
	[SerializeField]	float fDistance;		// カメラからの距離

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 vPos = Camera.main.transform.position;
		Vector3 vForward = Camera.main.transform.forward;

		// 座標
		transform.position = new Vector3(vPos.x + vForward.x * fDistance, vPos.y + vForward.y * fDistance, vPos.z + vForward.z * fDistance);

		// 向き
		transform.rotation = Camera.main.transform.rotation;
	}
}
