﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainCamera : SingletonMonoBehaviour<GameMainCamera>
{
    //--- メンバ変数 ------------------------------------------------------------------------------------------------------------
    //--- メンバ定数
    private const float FINISSHER_CAMERA_ROT_X = -45.0f;

    //--- メンバ変数
    private bool m_CameraRotChange;         // 角度が変わっているか
    private bool m_FinisherAnimationStart;  // 必殺技アニメーション開始か
    private float m_MoveTime;

    private bool m_UseTopViewCamera;    // トップビューにするかの情報

    //--- メンバ関数 ------------------------------------------------------------------------------------------------------------
    GameMainCamera()
    {
        m_CameraRotChange = false;
        m_FinisherAnimationStart = false;
        m_MoveTime  = 0.0f;

        m_UseTopViewCamera = true;
    }

	// Update is called once per frame
	void Update ()
    {
        if (m_UseTopViewCamera)
        {
            RotChangeUpdate();
        }
        else
        {
            transform.rotation = Quaternion.Euler(0.0f, -45.0f, 0.0f);
        }
	}

    //--- 角度更新
    void RotChangeUpdate()
    {
        if (!m_CameraRotChange)
            return;

        Quaternion From, To;

        m_MoveTime += Time.deltaTime;

        if (m_FinisherAnimationStart)
        {
            From = Quaternion.Euler(0.0f,0.0f,0.0f);
            To = Quaternion.Euler(FINISSHER_CAMERA_ROT_X, 0.0f, 0.0f); ;
        }
        else
        {
            From = Quaternion.Euler(FINISSHER_CAMERA_ROT_X, 0.0f, 0.0f); ;
            To = Quaternion.Euler(0.0f, 0.0f, 0.0f); ;
        }

        transform.rotation = Quaternion.Slerp(From, To, m_MoveTime);

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