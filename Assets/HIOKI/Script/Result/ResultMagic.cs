using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultMagic : MonoBehaviour {

	[SerializeField]
	private SpriteRenderer _MagR;

	[SerializeField]
	private float fRotSpeed;

	private bool bFlg = false;

	// Use this for initialization
	void Start () {
		_MagR = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		Rot ();
	}

	private void Rot()
	{
		if (!bFlg)
			return;
		
		Vector3 rot = transform.localEulerAngles;

		rot.z += fRotSpeed;

		if (rot.z >= 360.0f)
			rot.z = 0.0f;

		transform.localEulerAngles = rot;
	}

	public void FlgChenge()
	{
		bFlg = true;
		_MagR.color = new Color (0.0f, 255.0f, 255.0f, 255.0f);
	}
}
