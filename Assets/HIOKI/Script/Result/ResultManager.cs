using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour {

	#region 他のスクリプト呼ぶよう
	[SerializeField]
	private GameObject _Request;
	private RequestForm _RequestObj;

	[SerializeField]
	private GameObject _Hane;
	private Hane _HaneObj;

	[SerializeField]
	private GameObject _ElTime;
	private ElapsedTime _ElTimeObj;

	[SerializeField]
	private GameObject _FinSc;
	private FinalScore _FinScObj;

	[SerializeField]
	private GameObject _NestB;
	private NestQuestButton _NestBObj;

	[SerializeField]
	private GameObject _ReqB;
	private RequestButton _ReqBObj;

	[SerializeField]
	private GameObject _Logo;
	private ResultLogo _LogoObj;

	[SerializeField]
	private GameObject _Quest;
	private Quest _QuestObj;

	[SerializeField]
	private GameObject _Mag;
	private ResultMagic _MagObj;

	#endregion


	private Touch touch;
	private int nSelect = 0;
	private static float fSetTime;
	private static int nSetScore;
	private int nCnt = 0;

	private int nScore_0 = 0;
	private int nScore_1 = 0;
	private int nScore_2 = 0;
	private int nScore_3 = 0;
	private int nScore_4 = 0;
	private int nTime_0 = 0;
	private int nTime_1 = 0;
	private int nTime_2 = 0;
	// Use this for initialization
	void Start () {
		#region 呼ぶよう設定
		_RequestObj = _Request.GetComponent<RequestForm> ();
		_HaneObj = _Hane.GetComponent<Hane>();
		_ElTimeObj = _ElTime.GetComponent<ElapsedTime>();
		_FinScObj = _FinSc.GetComponent<FinalScore>();
		_LogoObj = _Logo.GetComponent<ResultLogo>();
		_NestBObj = _NestB.GetComponent<NestQuestButton>();
		_ReqBObj = _ReqB.GetComponent<RequestButton>();
		_QuestObj = _Quest.GetComponent<Quest>();
		_MagObj = _Mag.GetComponent<ResultMagic>();
		#endregion

		Debug.Log("GameManager.GetStage : " + (GameManager.GetStage+1));


		if (PlayerPrefs.HasKey("TimeStage" + (GameManager.GetStage + 1)))
		{
			float fTime = PlayerPrefs.GetFloat("TimeStage" + (GameManager.GetStage + 1));
			if (fTime > fSetTime)
			{
				PlayerPrefs.SetFloat("TimeStage" + (GameManager.GetStage + 1), fSetTime);
				Debug.Log("タイム新記録のステージ番号 : " + (GameManager.GetStage + 1));
			}
		}
		else
		{
			PlayerPrefs.SetFloat("TimeStage" + (GameManager.GetStage + 1), fSetTime);
			Debug.Log("タイムを保存 : " + fSetTime);
		}


		if (PlayerPrefs.HasKey("ScoreStage" + (GameManager.GetStage + 1)))
		{
			int nScore = PlayerPrefs.GetInt("ScoreStage" + (GameManager.GetStage + 1));
			if (nScore < nSetScore)
			{
				PlayerPrefs.SetInt("ScoreStage" + (GameManager.GetStage + 1), nSetScore);
				Debug.Log("スコア新記録のステージ番号 : " + nSetScore);
			}
		}
		else
		{
			PlayerPrefs.SetInt("ScoreStage" + (GameManager.GetStage + 1), nSetScore);
			Debug.Log("スコアを保存 : " + nSetScore);
		}

		PlayerPrefs.Save ();

		#region スコアの設定
		//nSetScore = 12345;
		nScore_0 = nSetScore / 10000;
		nScore_1 = nSetScore / 1000 - nScore_0 * 10;
		nScore_2 = nSetScore / 100 - nScore_0 * 100 - nScore_1 * 10;
		nScore_3 = nSetScore / 10 - nScore_0 * 1000 - nScore_1 * 100 - nScore_2 * 10;
		nScore_4 = nSetScore - nScore_0 * 10000 - nScore_1 * 1000 - nScore_2 * 100 - nScore_3 * 10;

		#endregion

		#region タイムの設定
		//fSetTime = 72.34f;
		nTime_0 = (int)fSetTime / 60;
		nTime_1 = ((int)fSetTime % 60) / 10;
		nTime_2 = ((int)fSetTime % 60) - nTime_1 * 10;

        #endregion

        BGMManager.Instance.Play("Game_Result");
	}
	
	// Update is called once per frame
	void Update () {
		
		switch (nSelect) {
		case 0:					//依頼書移動
			RequestFormMove();
			DisplayTouchCheck ();
			break;
		case 1:					//羽ペン移動
			HanePen();
			DisplayTouchCheck ();
			break;
		case 2:					//羽ペン+タイム移動表示
			TimeHane();
			DisplayTouchCheck ();
			break;
		case 3:					//羽ペン移動
			HanePen();
			DisplayTouchCheck ();
			break;
		case 4:					//羽ペン+スコア移動表示
			TimeHane();
			DisplayTouchCheck ();
			break;
		case 5:					//羽ペン画面外移動
			HaneDisplayOut();
			DisplayTouchCheck ();
			break;
		case 6:					//ハンコ押す
			HankoMove();
			DisplayTouchCheck ();
			break;
		case 7:					//最終画面
			LastDisplayResult();
			break;
		}
	}

	//０
	private void RequestFormMove()
	{
		//変更用
		if (_RequestObj.RequestFlg ()) {
			nSelect++;
			_HaneObj.HaneMove ();
			_ElTimeObj.SetElapsedTime (nTime_0, nTime_1, nTime_2);
			_FinScObj.SetFinalScore (nScore_0, nScore_1, nScore_2, nScore_3, nScore_4);
		}
	}

	//１+３
	private void HanePen()
	{
		if (_HaneObj.HaneFlg ()) {
			if(nSelect == 1)
				_ElTimeObj.MoveTime ();
			else
				_FinScObj.ChageFlg ();
            nSelect++;

            SEManager.Instance.Play("sei_ge_bpen_kaku01");
		}
	}

	//２+4
	private void TimeHane()
	{

		if (!_HaneObj.HaneFlg ()) {
			nSelect++;
		}
	}

	//5
	private void HaneDisplayOut()
	{
		if (_HaneObj.HaneFlg ()) {
			nSelect++;
			_QuestObj.MoveFlg ();
		}
	}

	//6
	private void HankoMove()
	{
		if (!_QuestObj.CheckFlg ()) {
			_LogoObj.ChangeFlgLogo();
			_NestBObj.DisplayActive (GameManager.GetStage);
			_ReqBObj.DisplayActive();
			_MagObj.FlgChenge();
			nSelect++;

            SEManager.Instance.Play("机を叩く音");
		}
	}

	//7
	private void LastDisplayResult()
	{
		//NestStage
		if (_NestBObj.LookCheckFlg()) {
			if (nCnt == 0) {
				//次へ
				int nSet = GameManager.GetStage;
				nSet++;
				GameManager.SetStage = nSet;
				Scenemanager.Instance.LoadLevel("GameMain", 0.5f, 0.5f, 0.5f);
				nCnt++;
			}
		}
			
		//StageSelect
		if (_ReqBObj.LookCheckFlg ()) {
			if (nCnt == 0) {
				Scenemanager.Instance.LoadLevel("StageSelect", 0.5f, 0.5f, 0.5f);
				nCnt++;
			}
		}
	}
	private void DisplayTouchCheck()
	{
		if (Input.GetKeyDown (KeyCode.Return)) {
            //ここにタッチした時の効果音をいれる
            SEManager.Instance.Play("アイテム発見");
			TouchDisplay(nSelect);
		}

		if (Input.touchCount > 0) {
			if (touch.phase == TouchPhase.Began) {
                //ここにタッチした時の効果音をいれる
                SEManager.Instance.Play("アイテム発見");
				TouchDisplay(nSelect);
			}
		}
	}

	private void TouchDisplay(int nTo)
	{
		switch (nTo) {
		case 0:
			_RequestObj.SetStopPos();							//依頼書
			_ElTimeObj.SetElapsedTime(nTime_0, nTime_1, nTime_2);					//タイムの数字
			_FinScObj.SetFinalScore (nScore_0, nScore_1, nScore_2, nScore_3, nScore_4);			//スコアの数字
			break;
		case 1:
			_HaneObj.LastHane();
			break;
		case 2:
			_HaneObj.LastHane();
			break;
		case 3:
			_HaneObj.LastHane();
			break;
		case 4:
			_HaneObj.LastHane();
			break;
		case 5:
			_HaneObj.LastHane();
			break;
		}

		_ElTimeObj.LastDisplay();							//タイムの最終画面
		_FinScObj.LastDisplayScore();						//スコアの最終画面
		_LogoObj.ChangeFlgLogo();							//リザルトのロゴが動く
		_NestBObj.DisplayActive(GameManager.GetStage);							//ボタン表示
		_ReqBObj.DisplayActive();							//ボタン表示
		_QuestObj.LastQuest();								//ハンコ
		_MagObj.FlgChenge();
		nSelect = 7;										//最終画面
	}


	static public void TimeandScore(float fTime, int nScore)
	{
		fSetTime = fTime;
		nSetScore = nScore;
	}




}
