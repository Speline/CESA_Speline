using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

	#region 他のスクリプト呼びよう
	//[SerializeField]
	//private GameObject _EnemyB;
	//private EnemyBase _EnemyBase;

	[SerializeField]
	private GameObject _EnemyVe;
	private EnemyVerticalAdvance _EnemyVeObj;

	[SerializeField]
	private GameObject _EnemySi;
	private EnemySideAdvance _EnemySiObj;

	[SerializeField]
	private GameObject _EnemySq;
	private EnemySquareRotation _EnemySqObj;

	[SerializeField]
	private GameObject _EnemyRot;
	private EnemyRot _EnemyRotObj;

	[SerializeField]
	private GameObject _StageE;
	private StageEnemy _StageEObj;
	#endregion

	[SerializeField]
	private Vector3[] SpawPos;

	[SerializeField]
	private Vector3[] SpawPos_Sq;

	[SerializeField]
	private int[] EnemyMax;

	private int nMax = 3;

	private static int nNowEnemy = 0;

	private int nCnt = 0;

	private int nSt;

	private static int nStStageEnemy;

	enum EnemyMove
	{
		VerticalAdvance = 0,
		SideAdvance,
		SquareRotation,
		SqRot,

		MAX_ENEMYMOVE
	};

	public static List<GameObject> myList = new List<GameObject>();

	// Use this for initialization
	void Start () {
		#region 他のスクリプト設定
        //_EnemyBase = _EnemyB.GetComponent<EnemyBase> ();
		//_EnemyBase = _EnemyB.GetComponent<EnemyBase> ();
		_EnemyVeObj = _EnemyVe.GetComponent<EnemyVerticalAdvance> ();
		_EnemySiObj = _EnemySi.GetComponent<EnemySideAdvance>();
		_EnemySqObj = _EnemySq.GetComponent<EnemySquareRotation>();
		_EnemyRotObj = _EnemyRot.GetComponent<EnemyRot>();
		_StageEObj = _StageE.GetComponent<StageEnemy>();
		_EnemySiObj = _EnemySi.GetComponent<EnemySideAdvance> ();
		_EnemySqObj = _EnemySq.GetComponent<EnemySquareRotation> ();
		_EnemyRotObj = _EnemyRot.GetComponent<EnemyRot> ();
		_StageEObj = _StageE.GetComponent<StageEnemy> ();
		#endregion

		nSt = nStStageEnemy;
		//nSt = 8;
		nSt = nStStageEnemy;							//ステージセット
		nNowEnemy = 0;									//初期化

        myList.Clear();
	}
	
	// Update is called once per frame
    void Update()
    {
		Debug.Log (GameManager.GetStage);
        switch (GameManager.Instance.NowState)
        {
            case GameManager.GameState.SETTING:
            case GameManager.GameState.MAGIC_SQUARE_SETTING:
            case GameManager.GameState.PLAYER_SETTING:
            case GameManager.GameState.START:
                break;

            case GameManager.GameState.GAME_MAIN:
                Create();
                break;

            case GameManager.GameState.GAME_CLEAR:
                break;

            case GameManager.GameState.GAME_OVER:
                break;
        }

	}

	private void Create()
	{
		nCnt++;

		if (nCnt >= 30) {

			if (nNowEnemy >= EnemyMax[nSt] || nNowEnemy >= nMax) {
				nCnt = 0;
				return;
			}

			int nSpawPoint = Random.Range (0, SpawPos.Length);
			int nSpawPointSq = Random.Range (0, SpawPos_Sq.Length);

			int nMoveEnemies = _StageEObj.SetEnemyStage (GameManager.GetStage);

			int nNowNomber;
			Vector3 vecSpaw = SpawPos [nSpawPoint];
			Vector3 vecSpawSq = SpawPos_Sq [nSpawPointSq];

			switch (nMoveEnemies) {
			case (int)EnemyMove.VerticalAdvance:			//縦移動
				GameObject EnemyVertical = Instantiate (_EnemyVeObj.CreateEnemyVertical (), vecSpaw, _EnemyVeObj.RotVertical (Random.Range (0, 2)));	//生成
				myList.Add (EnemyVertical);													//追加
				nNowNomber = myList.Count - 1;												//セット
				EnemyVertical.GetComponent<EnemyBase> ().SetNomber = nNowNomber;				//番号セット
                EnemyVertical.transform.SetParent(this.gameObject.transform);
				break;

			case (int)EnemyMove.SideAdvance:				//横移動
				GameObject EnemySide = Instantiate (_EnemySiObj.CreateEnemySide (), vecSpaw, _EnemySiObj.RotSide (Random.Range (0, 2)));	//生成
				myList.Add (EnemySide);													//追加
				nNowNomber = myList.Count - 1;												//セット
				EnemySide.GetComponent<EnemyBase> ().SetNomber = nNowNomber;				//番号セット
                EnemySide.transform.SetParent(this.gameObject.transform);
				break;

			case (int)EnemyMove.SquareRotation:				//四角回転移動
				GameObject EnemySquare = Instantiate (_EnemySqObj.CreateEnemySquare (), SpawPos [1], _EnemySqObj.RotSquare (Random.Range (0, 4)));	//生成
				myList.Add (EnemySquare);													//追加
				nNowNomber = myList.Count - 1;												//セット
				EnemySquare.GetComponent<EnemyBase> ().SetNomber = nNowNomber;				//番号セット
                EnemySquare.transform.SetParent(this.gameObject.transform);
				break;

			case (int)EnemyMove.SqRot:
				GameObject EnemyRotation = Instantiate (_EnemyRotObj.CreateEnemyRot (), vecSpawSq, _EnemyRotObj.RotRot (Random.Range (0, 2),nSpawPointSq));
				myList.Add (EnemyRotation);
				nNowNomber = myList.Count - 1;												//セット
				EnemyRotation.GetComponent<EnemyBase> ().SetNomber = nNowNomber;
                EnemyRotation.transform.SetParent(this.gameObject.transform);
				break;
			}

			nNowEnemy++;
			nCnt = 0;
		}
	}
	public static void DestroyEnemy(int nNom)
    {
        myList.RemoveAt(nNom);

        if (myList.Count > 0)
        {
            myList.ForEach(x =>
            {
                EnemyBase Script = x.GetComponent<EnemyBase>();

                if (Script.GetNomber > 0)
                    Script.SetNomber = Script.GetNomber - 1;
            });
        }
		nNowEnemy--;
    }

	static public void StageEnemy(int i)
	{
		nStStageEnemy = i;
	}

	//static public void StageEnemy(int i)
	//{
	//	nStStageEnemy = i;
	//}

    public static void AllMoveStart()
    {
        if (myList.Count > 0)
            myList.ForEach(x => x.GetComponent<EnemyBase>().MoveStart());
    }

    public static void AllMoveStop()
    {
        if (myList.Count > 0)
            myList.ForEach(x => x.GetComponent<EnemyBase>().MoveStop());
    }

    public void AddEnemy()
    {
        nNowEnemy = 0;
    }
}


