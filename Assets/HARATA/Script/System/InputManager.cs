using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	private static InputManager instance;

	public enum CLICK_STATE
	{
		NONE,		// クリックしていない
		ONECLICK,	// 1クリック目された瞬間
		CLICK,		// クリックが確定した時
		DOUBLECLICK	// ダブルクリックが確定した時
	};

	Vector2 slideStartPosition;
	Vector2 prevPosition;
	Vector2 delta = Vector2.zero;
	public bool bMove = false;

	// ダブルクリック用変数
	CLICK_STATE Click = CLICK_STATE.NONE;
	float fTime = 0.5f;				// クリックタイマー
	Vector2 ClickPos;				// 1クリック目に押された座標
	bool bClick = false;			// 1クリックされたかどうか
	float fDoubleClickTime = 0.2f;	// ダブルクリックの判定に使う時間


	public static InputManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = (InputManager)FindObjectOfType(typeof(InputManager));

				if (instance == null)
				{
					GameObject go = new GameObject("InputManager");
					instance = go.AddComponent<InputManager>();
				}
			}

			return instance;
		}
	}

	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		// タッチ開始地点
		if (Input.GetButtonDown("Fire1"))
			slideStartPosition = GetCursolPosition();

		// 一定以上スライドさせたらスライド開始
		if (Input.GetButton("Fire1"))
		{
			if (Vector2.Distance(slideStartPosition, GetCursolPosition()) >= 0.1f)
				bMove = true;
			else
				bMove = false;
		}

		if(Input.GetButtonUp("Fire1"))
			bMove = false;

		// 移動量を求める
		if (bMove)
			delta = GetCursolPosition() - prevPosition;
		else
			delta = Vector2.zero;

		// カーソル位置を更新
		prevPosition = GetCursolPosition();

		// クリック
		DoubleClick();


		// シーン遷移だだがき
		if (Input.GetKeyDown(KeyCode.Alpha1))
			Scenemanager.Instance.LoadLevel("Title", 0.5f, 0.5f, 0.5f);
		if (Input.GetKeyDown(KeyCode.Alpha2))
			Scenemanager.Instance.LoadLevel("StageSelect", 0.5f, 0.5f, 0.5f);
		if (Input.GetKeyDown(KeyCode.Alpha3))
			Scenemanager.Instance.LoadLevel("Config", 0.5f, 0.5f, 0.5f);
		if (Input.GetKeyDown(KeyCode.Alpha4))
			Scenemanager.Instance.LoadLevel("GameMain", 0.5f, 0.5f, 0.5f);
		if (Input.GetKeyDown(KeyCode.Alpha5))
			Scenemanager.Instance.LoadLevel("Clear", 0.5f, 0.5f, 0.5f);
		if (Input.GetKeyDown(KeyCode.Alpha6))
			Scenemanager.Instance.LoadLevel("GameOver", 0.5f, 0.5f, 0.5f);
	}


	// クリック判定
	private void DoubleClick()
	{
		fTime += Time.deltaTime;

		if (Input.GetButtonDown("Fire1") && !bClick &&fTime > fDoubleClickTime)
		{
			fTime = 0.0f;
			ClickPos = GetCursolPosition();		// クリック場所記憶
			Click = CLICK_STATE.ONECLICK;
			bClick = true;						// クリック開始
		}
		else if (!Input.GetButtonDown("Fire1") && bClick && fTime > fDoubleClickTime)
		{
			Click = CLICK_STATE.CLICK;			// クリック確定
			fTime = 10.0f;						// 適当に大きな数字
			bClick = false;
		}
		else if (Input.GetButtonDown("Fire1") && bClick && fTime <= fDoubleClickTime)
		{
			float fDistance = Vector2.Distance(ClickPos, GetCursolPosition());
			if (fDistance <= 10.0f)
			{
				Click = CLICK_STATE.DOUBLECLICK;// ダブルクリック確定
				fTime = 10.0f;					// 適当に大きな数字
				bClick = false;
			}
		}
		else
		{
			Click = CLICK_STATE.NONE;			// クリックしていない
		}
	}

	// クリックの状態を返す
	public CLICK_STATE GetClick()
	{
		return Click;
	}



	public Vector2 GetCursolPosition()
	{
		Vector3 MousePoint_Screen = Camera.main.WorldToScreenPoint(transform.position);
		Vector3 newVector = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, MousePoint_Screen.z));

		return new Vector2(newVector.x, newVector.y);
	}

	// スライド時のカーソル移動
	public Vector2 GetDeltaPosition()
	{
		return delta;
	}

	// スライド中かどうか
	public bool GetMove()
	{
		return bMove;
	}
}
