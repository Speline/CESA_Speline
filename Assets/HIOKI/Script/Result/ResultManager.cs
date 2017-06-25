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

	#endregion


	private Touch touch;
	private int nSelect = 0;
	private static float fSetTime;
	private static int nSetScore;
	private static int nSetStage;
	private int nCnt = 0;

	private int nScore_0 = 0;
	private int nScore_1 = 0;
	private int nScore_2 = 0;
	private int nScore_3 = 0;
	private int nScore_4 = 0;
	private int nTime_0 = 0;
	private int nTime_1 = 0;
	private int nTime_2 = 0;
	private int nTime_3 = 0;
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
		#endregion

		#region スコアの設定
		//nSetScore = 12345;
		nScore_0 = nSetScore / 10000;
		nScore_1 = nSetScore / 1000 - nScore_0 * 10;
		nScore_2 = nSetScore / 100 - nScore_0 * 100 - nScore_1 * 10;
		nScore_3 = nSetScore / 10 - nScore_0 * 1000 - nScore_1 * 100 - nScore_2 * 10;
		nScore_4 = nSetScore - nScore_0 * 10000 - nScore_1 * 1000 - nScore_2 * 100 - nScore_3 * 10;

		#endregion

		#region タイムの設定
		//fSetTime = 12.34f;
		nTime_0 = (int)fSetTime / 10;
		nTime_1 = (int)fSetTime - nTime_0 * 10;
		nTime_2 = (int)(fSetTime * 10) - nTime_0 * 100 - nTime_1 * 10;
		nTime_3 = (int)(fSetTime * 100) - nTime_0 * 1000 - nTime_1 * 100 - nTime_2 * 10;

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
		case 2:					//羽ペン+スコア+タイム移動表示
			TimeScore();
			DisplayTouchCheck ();
			break;
		case 3:					//羽ペン画面外移動
			HaneDisplayOut();
			DisplayTouchCheck ();
			break;
		case 4:					//ハンコ押す
			HankoMove();
			DisplayTouchCheck ();
			break;
		case 5:					//最終画面
			LastDisplayResult();
			break;
		}
	}


	private void RequestFormMove()
	{
		//変更用
		if (_RequestObj.RequestFlg ()) {
			nSelect++;
			_HaneObj.HaneMove ();
			_ElTimeObj.SetElapsedTime (nTime_0, nTime_1, nTime_2, nTime_3);
			_FinScObj.SetFinalScore (nScore_0, nScore_1, nScore_2, nScore_3, nScore_4);
		}
	}

	private void HanePen()
	{
		if (_HaneObj.HaneFlg ()) {
			nSelect++;
			_ElTimeObj.MoveTime ();
			_FinScObj.ChageFlg ();
		}
	}

	private void TimeScore()
	{

		if (!_HaneObj.HaneFlg ()) {
			nSelect++;
		}
	}

	private void HaneDisplayOut()
	{
		if (_HaneObj.HaneFlg ()) {
			nSelect++;
			_QuestObj.MoveFlg ();
		}
	}

	private void HankoMove()
	{
		if (!_QuestObj.CheckFlg ()) {
			_LogoObj.ChangeFlgLogo();
			_NestBObj.DisplayActive ();
			_ReqBObj.DisplayActive();
			nSelect++;
		}
	}

	private void LastDisplayResult()
	{
		//NestStage
		if (_NestBObj.LookCheckFlg()) {
			if (nCnt == 0) {
				//次へ
				Scenemanager.Instance.LoadLevel("GameMain", 0.5f, 0.5f, 0.5f);
				nCnt++;
			}
		}
			
		//StageSelect
		if (_ReqBObj.LookCheckFlg ()) {
			if (nCnt == 0) {
				GameManager.SetStage = nSetStage;
				Scenemanager.Instance.LoadLevel("StageSelect", 0.5f, 0.5f, 0.5f);
				nCnt++;
			}
		}
	}
	private void DisplayTouchCheck()
	{
		if (Input.GetKeyDown (KeyCode.Return)) {
			//ここにタッチした時の効果音をいれる
			TouchDisplay(nSelect);
		}

		if (Input.touchCount > 0) {
			if (touch.phase == TouchPhase.Began) {
				//ここにタッチした時の効果音をいれる
				TouchDisplay(nSelect);
			}
		}
	}

	private void TouchDisplay(int nTo)
	{
		switch (nTo) {
		case 0:
			_RequestObj.SetStopPos();							//依頼書
			_ElTimeObj.SetElapsedTime(nTime_0, nTime_1, nTime_2, nTime_3);					//タイムの数字
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
		}

		_ElTimeObj.LastDisplay();							//タイムの最終画面
		_FinScObj.LastDisplayScore();						//スコアの最終画面
		_LogoObj.ChangeFlgLogo();							//リザルトのロゴが動く
		_NestBObj.DisplayActive();							//ボタン表示
		_ReqBObj.DisplayActive();							//ボタン表示
		_QuestObj.LastQuest();								//ハンコ
		nSelect = 5;										//最終画面
	}


	static public void TimeandScore(float fTime, int nScore)
	{
		fSetTime = fTime;
		nSetScore = nScore;
		//nSetStage = nScore;
	}




}
