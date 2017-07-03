using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

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
    private Vector3[] SpawPos_V;

	[SerializeField]
	private Vector3[] SpawPos_Sq;

	[SerializeField]
	private int[] EnemyMax;

	private int nMax = 3;

	private static int nNowEnemy = 0;
    private int nEyMax = 0;

	private int nCnt = 0;

	private int nSt;

	private static int nStStageEnemy;

    private float m_GameOverTime;
    private int nSSS = 0;

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
		_EnemyVeObj = _EnemyVe.GetComponent<EnemyVerticalAdvance> ();
		_EnemySiObj = _EnemySi.GetComponent<EnemySideAdvance> ();
		_EnemySqObj = _EnemySq.GetComponent<EnemySquareRotation> ();
		_EnemyRotObj = _EnemyRot.GetComponent<EnemyRot> ();
		_StageEObj = _StageE.GetComponent<StageEnemy> ();
		#endregion

		nSt = nStStageEnemy;							//ステージセット
		nNowEnemy = 0;									//初期化
        nEyMax = 0;
        nSSS = 0;

    myList.Clear();
	}
	
	// Update is called once per frame
    void Update()
    {
        switch (GameManager.Instance.NowState)
        {
            case GameManager.GameState.SETTING:
            case GameManager.GameState.MAGIC_SQUARE_SETTING:
            case GameManager.GameState.PLAYER_SETTING:
            case GameManager.GameState.GAME_START:
                break;

            case GameManager.GameState.GAME_MAIN:
                Create();
                break;

            case GameManager.GameState.GAME_CLEAR:
                break;

            case GameManager.GameState.GAME_OVER:
                m_GameOverTime += Time.deltaTime;
                if (m_GameOverTime < 3.0f)
                {
                    AddEnemy();
                    Create();
                }
                break;
        }

	}



	private void Create()
	{
		nCnt++;

		if (nCnt >= 30) {

			if (nEyMax >= EnemyMax[GameManager.GetStage] || nNowEnemy >= nMax) {
				nCnt = 0;
				return;
			}

            //Debug.Log(nNowEnemy);
            ///nSSS++;
            //Debug.Log(nSSS+"回目");

            //int nSpawPoint = Random.Range (0, SpawPos.Length);
            //int nSpawPointSq = Random.Range (0, SpawPos_Sq.Length);

            int nMoveEnemies = _StageEObj.SetEnemyStage (GameManager.GetStage);

			int nNowNomber = 0;
            int nSpawPoint = 0;
            Vector3 vecSpaw = Vector3.zero;
            
			//Vector3 vecSpaw = SpawPos [nSpawPoint];
			//Vector3 vecSpawSq = SpawPos_Sq [nSpawPointSq];

			switch (nMoveEnemies) {
			case (int)EnemyMove.VerticalAdvance:			//縦移動
                    nSpawPoint = Random.Range(0, SpawPos_V.Length);
                    vecSpaw = SpawPos_V[nSpawPoint];
				GameObject EnemyVertical = Instantiate (_EnemyVeObj.CreateEnemyVertical (), vecSpaw, _EnemyVeObj.RotVertical (Random.Range (0, 2)));	//生成
				myList.Add (EnemyVertical);													//追加
				nNowNomber = myList.Count - 1;												//セット
				EnemyVertical.GetComponent<EnemyBase> ().SetNomber = nNowNomber;				//番号セット
                EnemyVertical.transform.SetParent(this.gameObject.transform);
				break;

			case (int)EnemyMove.SideAdvance:				//横移動
                    nSpawPoint = Random.Range(0, SpawPos.Length);
                    vecSpaw = SpawPos[nSpawPoint];
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
                    nSpawPoint = Random.Range(0, SpawPos_Sq.Length);
                    vecSpaw = SpawPos_Sq[nSpawPoint];
                    GameObject EnemyRotation = Instantiate(_EnemyRotObj.CreateEnemyRot(), vecSpaw, _EnemyRotObj.RotRot(Random.Range(0, 2), nSpawPoint));
				myList.Add (EnemyRotation);
				nNowNomber = myList.Count - 1;												//セット
				EnemyRotation.GetComponent<EnemyBase> ().SetNomber = nNowNomber;
                EnemyRotation.transform.SetParent(this.gameObject.transform);
				break;
			}

			nNowEnemy++;
            nEyMax++;
			nCnt = 0;
		}
	}

	public static void DestroyEnemy(int nNom)
    {
        ////if (myList.Count > 0)
        ////{

        //    myList.ForEach(x =>
        //    {
        //        EnemyBase Script = x.GetComponent<EnemyBase>();

        //        if (Script.GetNomber > nNom)
        //            Script.SetNomber = Script.GetNomber - 1;
        //    });
        ////}

        //foreach (GameObject EnemyList in myList)
        //{
        //    EnemyBase Script = EnemyList.GetComponent<EnemyBase>();

        //    if (Script.GetNomber > nNom)
        //        Script.SetNomber = Script.GetNomber - 1;
        //}

        //myList.RemoveAt(nNom);
		nNowEnemy--;

	}
    public static void DestroyEnemy(GameObject DestryObj)
    {
        myList.Remove(DestryObj);
        //Debug.Log("chihaya");
        nNowEnemy--;
        
        if (nNowEnemy <= 0)
            nNowEnemy = 0;
        Debug.Log(nNowEnemy);
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
