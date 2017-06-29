using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCamera : MonoBehaviour {

	[SerializeField]
	private float fMoveSpeed;				//カメラスピード
	[SerializeField]
	private float fMaxZ;					//ストップする座標

	private bool bMoveFlg = false;			//移動フラグ
	private bool bChange = false;			//チェンジフラグ

	// Use this for initialization
	//void Start () {
		
	//}
	
	// Update is called once per frame
	void Update () {
		MoveCamera ();				//移動
	}

	private void MoveCamera()
	{
		if (!bMoveFlg)
			return;
		
		Vector3 pos = transform.localPosition;		//値セット

		pos.z += fMoveSpeed;						//変更

		#region 補正
		if (pos.z >= fMaxZ)
		{
			pos.z = fMaxZ;							//補正
			bMoveFlg = false;						//移動ストップ
			bChange = true;							//チェンジon
		}
		#endregion

		transform.localPosition = pos;				//値代入

	}

	public void CameraMoveFlg()
	{
		bMoveFlg = true;
	}

	public bool ChangeCheck()
	{
		return bChange;
	}
}
