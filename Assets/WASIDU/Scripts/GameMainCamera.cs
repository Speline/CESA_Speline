using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainCamera : SingletonMonoBehaviour<GameMainCamera>
{
    //--- メンバ変数 ------------------------------------------------------------------------------------------------------------
    //--- メンバ定数
    private const float CAMERA_ROT_X = -45.0f;
    private const float FINISSHER_CAMERA_ROT_Y = 180.0f;

    //--- メンバ変数
    private bool m_CameraRotChange;         // 角度が変わっているか
    private bool m_FinisherAnimationStart;  // 必殺技アニメーション開始か
    private float m_MoveTime;

    private Vector3 m_Rotate;

    private bool m_UseTopViewCamera;    // トップビューにするかの情報

    //--- メンバ関数 ------------------------------------------------------------------------------------------------------------
    GameMainCamera()
    {
        m_CameraRotChange = false;
        m_FinisherAnimationStart = false;
        m_MoveTime  = 0.0f;
        m_Rotate = new Vector3(CAMERA_ROT_X, 0.0f, 0.0f);
    }

	// Update is called once per frame
	void Update ()
    {
        RotChangeUpdate();
	}

    //--- 角度更新
    void RotChangeUpdate()
    {
        if (!m_CameraRotChange)
            return;

        //Quaternion From, To;

        m_MoveTime += Time.deltaTime / 2.0f;

        //if (m_FinisherAnimationStart)
        //{
        //    From = Quaternion.Euler(CAMERA_ROT_X, 0.0f, 0.0f);
        //    To = Quaternion.Euler(CAMERA_ROT_X, FINISSHER_CAMERA_ROT_Y, 0.0f); ;
        //}
        //else
        //{
        //    From = Quaternion.Euler(CAMERA_ROT_X, FINISSHER_CAMERA_ROT_Y, 0.0f); ;
        //    To = Quaternion.Euler(CAMERA_ROT_X, 0.0f, 0.0f); ;
        //}

        //transform.rotation = Quaternion.Slerp(From, To, m_MoveTime);

        m_Rotate.y += FINISSHER_CAMERA_ROT_Y * Time.deltaTime / 2.0f;

        transform.rotation = Quaternion.Euler(m_Rotate);

        if (m_MoveTime >= 1.0f)
        {
            m_MoveTime = 0.0f;
            m_CameraRotChange = false;
        }
    }

    //--- 角度変更
    public void ChangeRot()
    {
        m_CameraRotChange = true;
        m_FinisherAnimationStart ^= true;
    }

    //--- 情報取得
    public bool MoveCamera { get { return m_CameraRotChange; } }
}
