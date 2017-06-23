using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEnemy : MonoBehaviour {

	[SerializeField]
	private int[] nEnemy_Vi_MAX;
	[SerializeField]
	private int[] nEnemy_Si_MAX;
	[SerializeField]
	private int[] nEnemy_Sq_MAX;
	[SerializeField]
	private int[] nEnemy_Ro_MAX;


	#region ステージナンバー
	enum STAGE
	{
		Stage_1 = 0,
		Stage_2,
		Stage_3,
		Stage_4,
		Stage_5,
		Stage_6,
		Stage_7,
		Stage_8,
		Stage_9,
		Stage_10,

		Stage_MAX
	};
	#endregion

	private int nEnemyVi = 0;
	private int nEnemySi = 0;
	private int nEnemySq = 0;
	private int nEnemyRo = 0;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int SetEnemyStage(int i)
	{
		int nRan = 0;						//変数　Stage

		switch (i) {
		case (int)STAGE.Stage_1:
			#region ステージ1
			nRan = 0;						//代入
			#endregion
			break;

		case (int)STAGE.Stage_2:
			#region ステージ2
			nRan = Random.Range (0, 2);		//設定

			if (nRan == 0) {
				nEnemyVi++;				//増やす
				//補正
				if (nEnemyVi > nEnemy_Vi_MAX [(int)STAGE.Stage_2]) {
					//最大より大きくなったら
					nEnemyVi--;				//減らす
					nEnemySi++;				//増やす
					nRan = 1;				//変更
				}
			} else {
				nEnemySi++;
				//補正
				if (nEnemySi > nEnemy_Si_MAX [(int)STAGE.Stage_2]) {
					//最大より大きくなったら
					nEnemySi--;				//減らす
					nEnemyVi++;				//増やす
					nRan = 0;				//変更
				}
			}
			#endregion
			break;

		case (int)STAGE.Stage_3:
			#region ステージ3
			nRan = Random.Range (0, 2);
			if (nRan == 0) {
				nEnemyVi++;				//増やす
				//補正
				if (nEnemyVi > nEnemy_Vi_MAX [(int)STAGE.Stage_2]) {
					//最大より大きくなったら
					nEnemyVi--;				//減らす
					nEnemySi++;				//増やす
					nRan = 1;				//変更
				}
			} else {
				nEnemySi++;
				//補正
				if (nEnemySi > nEnemy_Si_MAX [(int)STAGE.Stage_2]) {
					//最大より大きくなったら
					nEnemySi--;				//減らす
					nEnemyVi++;				//増やす
					nRan = 0;				//変更
				}
			}
			#endregion
			break;

		case (int)STAGE.Stage_4:
			#region ステージ4
			nRan = Random.Range (0, 2);

			if (nRan == 1)
				nRan = 2;

			if (nRan == 0) {
				nEnemyVi++;				//増やす
				//補正
				if (nEnemyVi > nEnemy_Vi_MAX [(int)STAGE.Stage_4]) {
					//最大より大きくなったら
					nEnemyVi--;				//減らす
					nEnemySq++;				//増やす
					nRan = 2;				//変更
				}
			} else {
				nEnemySq++;
				//補正
				if (nEnemySq > nEnemy_Sq_MAX [(int)STAGE.Stage_4]) {
					//最大より大きくなったら
					nEnemySq--;				//減らす
					nEnemyVi++;				//増やす
					nRan = 0;				//変更
				}
			}
			#endregion
			break;

		case (int)STAGE.Stage_5:
			#region ステージ5
			nRan = Random.Range (0, 3);

			if (nRan == 0) {
				nEnemyVi++;				//増やす
				//補正
				if (nEnemyVi > nEnemy_Vi_MAX [(int)STAGE.Stage_5]) {
					//最大より大きくなったら
					nEnemyVi--;				//減らす
					if(nEnemySi < nEnemy_Si_MAX [(int)STAGE.Stage_5]){
						nRan = 1;
						nEnemySi++;
					}else{
						nRan = 2;
						nEnemySq++;
					}
				} 
			} else if(nRan == 1){
				nEnemySi++;
				if (nEnemySi > nEnemy_Si_MAX [(int)STAGE.Stage_5]) {
					//最大より大きくなったら
					nEnemySi--;				//減らす
					if(nEnemyVi < nEnemy_Vi_MAX [(int)STAGE.Stage_5]){
						nRan = 0;
						nEnemyVi++;
					}else{
						nRan = 2;
						nEnemySq++;
					}
				}
			} else {
				nEnemySq++;
				if (nEnemySq > nEnemy_Sq_MAX [(int)STAGE.Stage_5]) {
					//最大より大きくなったら
					nEnemySq--;				//減らす

					if(nEnemyVi < nEnemy_Vi_MAX [(int)STAGE.Stage_5]){
						nRan = 0;
						nEnemyVi++;
					}else{
						nRan = 1;
						nEnemySi++;
					}
				}
			}

			#endregion

			break;

		case (int)STAGE.Stage_6:
			#region ステージ6
			nRan = Random.Range (0, 3);

			if (nRan == 0) {
				nEnemyVi++;				//増やす
				//補正
				if (nEnemyVi > nEnemy_Vi_MAX [(int)STAGE.Stage_6]) {
					//最大より大きくなったら
					nEnemyVi--;				//減らす
					if(nEnemySi < nEnemy_Si_MAX [(int)STAGE.Stage_6]){
						nRan = 1;
						nEnemySi++;
					}else{
						nRan = 2;
						nEnemySq++;
					}
				} 
			} else if(nRan == 1){
				nEnemySi++;
				if (nEnemySi > nEnemy_Si_MAX [(int)STAGE.Stage_6]) {
					//最大より大きくなったら
					nEnemySi--;				//減らす
					if(nEnemyVi < nEnemy_Vi_MAX [(int)STAGE.Stage_6]){
						nRan = 0;
						nEnemyVi++;
					}else{
						nRan = 2;
						nEnemySq++;
					}
				}
			} else {
				nEnemySq++;
				if (nEnemySq > nEnemy_Sq_MAX [(int)STAGE.Stage_6]) {
					//最大より大きくなったら
					nEnemySq--;				//減らす

					if(nEnemyVi < nEnemy_Vi_MAX [(int)STAGE.Stage_6]){
						nRan = 0;
						nEnemyVi++;
					}else{
						nRan = 1;
						nEnemySi++;
					}
				}
			}

			#endregion

			break;

		case (int)STAGE.Stage_7:
			#region ステージ7

			nRan = Random.Range (0, 3);

			if (nRan == 2)
				nRan = 3;

			if (nRan == 0) {
				nEnemyVi++;				//増やす
				//補正
				if (nEnemyVi > nEnemy_Vi_MAX [(int)STAGE.Stage_7]) {
					//最大より大きくなったら
					nEnemyVi--;				//減らす
					if(nEnemySi < nEnemy_Si_MAX [(int)STAGE.Stage_7]){
						nRan = 1;
						nEnemySi++;
					}else{
						nRan = 3;
						nEnemyRo++;
					}
				} 
			} else if(nRan == 1){
				nEnemySi++;
				if (nEnemySi > nEnemy_Si_MAX [(int)STAGE.Stage_7]) {
					//最大より大きくなったら
					nEnemySi--;				//減らす
					if(nEnemyVi < nEnemy_Vi_MAX [(int)STAGE.Stage_7]){
						nRan = 0;
						nEnemyVi++;
					}else{
						nRan = 3;
						nEnemyRo++;
					}
				}
			} else {
				nEnemyRo++;
				if (nEnemyRo > nEnemy_Ro_MAX [(int)STAGE.Stage_7]) {
					//最大より大きくなったら
					nEnemySq--;				//減らす

					if(nEnemyVi < nEnemy_Vi_MAX [(int)STAGE.Stage_7]){
						nRan = 0;
						nEnemyVi++;
					}else{
						nRan = 1;
						nEnemySi++;
					}
				}
			}
			#endregion
			break;

		case (int)STAGE.Stage_8:
			#region ステージ8
			nRan = Random.Range (0, 4);

			if(nRan == 0){
				nEnemyVi++;

				if(nEnemyVi > nEnemy_Vi_MAX[(int)STAGE.Stage_8]){
					nEnemyVi--;

					if(nEnemySi < nEnemy_Si_MAX[(int)STAGE.Stage_8]){
						nRan = 1;
						nEnemySi++;
					}else if(nEnemySq < nEnemy_Sq_MAX[(int)STAGE.Stage_8]){
						nRan = 2;
						nEnemySq++;
					}else{
						nRan = 3;
						nEnemyRo++;
					}
				}
			} else if(nRan == 1){
				nEnemySi++;

				if(nEnemySi > nEnemy_Si_MAX[(int)STAGE.Stage_8]){
					nEnemySi--;

					if(nEnemyVi < nEnemy_Vi_MAX[(int)STAGE.Stage_8]){
						nRan = 0;
						nEnemyVi++;
					}else if(nEnemySq < nEnemy_Sq_MAX[(int)STAGE.Stage_8]){
						nRan = 2;
						nEnemySq++;
					}else{
						nRan = 3;
						nEnemyRo++;
					}
				}
			} else if(nRan == 2){
				nEnemySq++;

				if(nEnemySq > nEnemy_Sq_MAX[(int)STAGE.Stage_8]){
					nEnemySq--;

					if(nEnemyVi < nEnemy_Vi_MAX[(int)STAGE.Stage_8]){
						nRan = 0;
						nEnemyVi++;
					}else if(nEnemySi < nEnemy_Si_MAX[(int)STAGE.Stage_8]){
						nRan = 1;
						nEnemyVi++;
					}else{
						nRan = 3;
						nEnemyRo++;
					}
				}
			} else {
				nEnemyRo++;

				if(nEnemyRo > nEnemy_Ro_MAX[(int)STAGE.Stage_8]){
					nEnemyRo--;

					if(nEnemyVi < nEnemy_Vi_MAX[(int)STAGE.Stage_8]){
						nRan = 0;
						nEnemyVi++;
					}else if(nEnemySi < nEnemy_Si_MAX[(int)STAGE.Stage_8]){
						nRan = 1;
						nEnemySi++;
					}else{
						nRan = 2;
						nEnemySq++;
					}
				}
			}

			#endregion
			break;

		case (int)STAGE.Stage_9:
			#region ステージ9
			nRan = Random.Range (0, 4);

			if(nRan == 0){
				nEnemyVi++;

				if(nEnemyVi > nEnemy_Vi_MAX[(int)STAGE.Stage_9]){
					nEnemyVi--;

					if(nEnemySi < nEnemy_Si_MAX[(int)STAGE.Stage_9]){
						nRan = 1;
						nEnemySi++;
					}else if(nEnemySq < nEnemy_Sq_MAX[(int)STAGE.Stage_9]){
						nRan = 2;
						nEnemySq++;
					}else{
						nRan = 3;
						nEnemyRo++;
					}
				}
			} else if(nRan == 1){
				nEnemySi++;

				if(nEnemyVi > nEnemy_Si_MAX[(int)STAGE.Stage_9]){
					nEnemySi--;

					if(nEnemyVi < nEnemy_Vi_MAX[(int)STAGE.Stage_9]){
						nRan = 0;
						nEnemyVi++;
					}else if(nEnemySq < nEnemy_Sq_MAX[(int)STAGE.Stage_9]){
						nRan = 2;
						nEnemySq++;
					}else{
						nRan = 3;
						nEnemyRo++;
					}
				}
			} else if(nRan == 2){
				nEnemySq++;

				if(nEnemySq > nEnemy_Sq_MAX[(int)STAGE.Stage_9]){
					nEnemySq--;

					if(nEnemyVi < nEnemy_Vi_MAX[(int)STAGE.Stage_9]){
						nRan = 0;
						nEnemyVi++;
					}else if(nEnemySi < nEnemy_Si_MAX[(int)STAGE.Stage_9]){
						nRan = 1;
						nEnemyVi++;
					}else{
						nRan = 3;
						nEnemyRo++;
					}
				}
			} else {
				nEnemyRo++;

				if(nEnemyRo > nEnemy_Ro_MAX[(int)STAGE.Stage_9]){
					nEnemyRo--;

					if(nEnemyVi < nEnemy_Vi_MAX[(int)STAGE.Stage_9]){
						nRan = 0;
						nEnemyVi++;
					}else if(nEnemySi < nEnemy_Si_MAX[(int)STAGE.Stage_9]){
						nRan = 1;
						nEnemySi++;
					}else{
						nRan = 2;
						nEnemySq++;
					}
				}
			}
			#endregion
			break;

		case (int)STAGE.Stage_10:
			#region ステージ10
			nRan = Random.Range (0, 4);

			if(nRan == 0){
				nEnemyVi++;

				if(nEnemyVi > nEnemy_Vi_MAX[(int)STAGE.Stage_10]){
					nEnemyVi--;

					if(nEnemySi < nEnemy_Si_MAX[(int)STAGE.Stage_10]){
						nRan = 1;
						nEnemySi++;
					}else if(nEnemySq < nEnemy_Sq_MAX[(int)STAGE.Stage_10]){
						nRan = 2;
						nEnemySq++;
					}else{
						nRan = 3;
						nEnemyRo++;
					}
				}
			} else if(nRan == 1){
				nEnemySi++;

				if(nEnemyVi > nEnemy_Si_MAX[(int)STAGE.Stage_10]){
					nEnemySi--;

					if(nEnemyVi < nEnemy_Vi_MAX[(int)STAGE.Stage_10]){
						nRan = 0;
						nEnemyVi++;
					}else if(nEnemySq < nEnemy_Sq_MAX[(int)STAGE.Stage_10]){
						nRan = 2;
						nEnemySq++;
					}else{
						nRan = 3;
						nEnemyRo++;
					}
				}
			} else if(nRan == 2){
				nEnemySq++;

				if(nEnemySq > nEnemy_Sq_MAX[(int)STAGE.Stage_10]){
					nEnemySq--;

					if(nEnemyVi < nEnemy_Vi_MAX[(int)STAGE.Stage_10]){
						nRan = 0;
						nEnemyVi++;
					}else if(nEnemySi < nEnemy_Si_MAX[(int)STAGE.Stage_10]){
						nRan = 1;
						nEnemyVi++;
					}else{
						nRan = 3;
						nEnemyRo++;
					}
				}
			} else {
				nEnemyRo++;

				if(nEnemyRo > nEnemy_Ro_MAX[(int)STAGE.Stage_10]){
					nEnemyRo--;

					if(nEnemyVi < nEnemy_Vi_MAX[(int)STAGE.Stage_10]){
						nRan = 0;
						nEnemyVi++;
					}else if(nEnemySi < nEnemy_Si_MAX[(int)STAGE.Stage_10]){
						nRan = 1;
						nEnemySi++;
					}else{
						nRan = 2;
						nEnemySq++;
					}
				}
			}

			#endregion
			break;
		}

		return nRan;
	}
}
