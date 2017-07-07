using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeDraw : MonoBehaviour
{
    //--- メンバ変数 ------------------------------------------------------------------------------------------------------------
    //--- メンバ変数
    private Image       m_TimeImage;        // 残り時間情報画像
    private GameObject  m_ClockHandObject;  // 時計の針オブジェクト

    // ゲーム開始時用
    private bool    m_MoveSetPos;
    private bool    m_SetEnd;
    private float   m_MoveTime;     // 移動時間
    private Vector3 m_StartPos;     // 初期位置
    private Vector3 m_MovePos;      // 移動先位置

    //--- メンバ関数 ------------------------------------------------------------------------------------------------------------
	// Use this for initialization
	void Start ()
    {
        m_TimeImage = transform.FindChild("MagicSquareImage").gameObject.GetComponent<Image>();
        m_TimeImage.fillAmount = 0.0f;

        m_ClockHandObject = transform.FindChild("ClockHand").gameObject;

        transform.localPosition = m_StartPos = Vector3.zero;
        m_MovePos = new Vector3(0f, 647f, 0.0f);

        m_MoveSetPos    = false;
        m_SetEnd        = false;
        m_MoveTime      = 0.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //--- 時間表示色変更
        if (m_TimeImage.fillAmount >= 0.6f)
        {
            m_TimeImage.color = Color.green;
        }
        else if (m_TimeImage.fillAmount >= 0.3f)
        {
            m_TimeImage.color = Color.yellow;
        }
        else if (m_TimeImage.fillAmount >= 0.0f)
        {
            m_TimeImage.color = Color.red;
        }

        //--- ステートによる変更処理
        switch (GameManager.Instance.NowState)
        {
            case GameManager.GameState.SETTING:
            case GameManager.GameState.MAGIC_SQUARE_SETTING:
            case GameManager.GameState.PLAYER_SETTING:
            case GameManager.GameState.GAME_START:
                Setting();
                break;

            case GameManager.GameState.GAME_MAIN:
            case GameManager.GameState.GAME_CLEAR:
            case GameManager.GameState.GAME_OVER:
                break;
        }
    }

    void Setting()
    {
        if (!m_MoveSetPos)
        {
            TimeData += 0.5f * Time.deltaTime;
            if (m_TimeImage.fillAmount >= 1.0f)
            {
                m_MoveSetPos = true;
            }
        }
        else if (!m_SetEnd)
        {

            m_MoveTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(m_StartPos, m_MovePos, m_MoveTime);

            if (m_MoveTime > 1.0f)
            {
                m_SetEnd = true;
                GameManager.Instance.SetTimeMoveEnd = true;
            }
        }
    }

    public float TimeData
    {
        get
        {
            return m_TimeImage.fillAmount;
        }
        set
        {
            m_TimeImage.fillAmount = value;
            float RotZ = m_TimeImage.fillAmount * 360;
            m_ClockHandObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -RotZ);
        }
    }
}
