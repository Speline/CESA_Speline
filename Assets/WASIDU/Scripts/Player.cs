﻿//========================================================
// プレイヤーキャラ一体の動き
//========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //--- メンバ変数 -----------------------------------------------------------------------------------
    //--- メンバ定数
    private const float SET_ROT_X = -35.0f;

    //--- 静的メンバ変数
    private static GameObject m_MahoujinPrefub = null;

    //--- メンバ変数
    private GameObject m_MagicSquareObject; // 魔方陣オブジェクト
    private GameObject m_FireBoalParent;

    private GameObject m_ChildObj;

    // スタート前の処理用
    private float m_SettingScale = 0.0f;
    private Vector3 m_SettingRot;
    private bool m_TurnEnd;

    // 必殺技用
    private ParticleSystem m_UseFinissherParticle;  // 必殺技使用設定時のパーティクル
    private GameObject m_FinisherCameraObj;

    //--- メンバ関数 -----------------------------------------------------------------------------------
	// Use this for initialization
    void Start()
    {
        if (m_MahoujinPrefub == null)
        {
            m_MahoujinPrefub = Resources.Load("MahoujinObject") as GameObject;
        }

        // スタート前の処理用
        m_ChildObj = transform.FindChild("chara_model").gameObject;
        m_ChildObj.transform.localScale = new Vector3(m_SettingScale, m_SettingScale, m_SettingScale);
        m_SettingRot = transform.rotation.eulerAngles;
        m_TurnEnd = false;


        // 必殺技用
        m_UseFinissherParticle = GetComponent<ParticleSystem>();
        m_UseFinissherParticle.Pause();

        m_FinisherCameraObj = transform.FindChild("FinissherCamera").gameObject;
        m_FinisherCameraObj.SetActive(false);

        SetMagicSquare();

    }

    void Update()
    {
        if (GameManager.Instance.NowState == GameManager.GameState.PLAYER_SETTING)
        {
            PreStartUpdate();
        }

        //--- 攻撃アニメーション終了後に魔方陣削除

    }

    //--- スタート前の更新
    void PreStartUpdate()
    {
        if (!m_TurnEnd)
        {
            m_SettingScale += 20.0f * Time.deltaTime;

            m_SettingRot.y += 360f * Time.deltaTime;

            m_ChildObj.transform.localScale = new Vector3(m_SettingScale, m_SettingScale, m_SettingScale);
            m_ChildObj.transform.rotation = Quaternion.Euler(m_SettingRot);

            if (m_ChildObj.transform.localScale.x >= 20.0f)
            {
                m_TurnEnd = true;
            }
        }
        else
        {
            m_SettingRot.x += SET_ROT_X * Time.deltaTime;

            m_ChildObj.transform.rotation = Quaternion.Euler(m_SettingRot);

            if (m_SettingRot.x <= SET_ROT_X)
                GameManager.Instance.ChangeState(GameManager.GameState.START);
        }
    }

    //--- 必殺技使用設定
    public void UseFinisherAtack()
    {
        m_UseFinissherParticle.Play();

        m_MagicSquareObject.GetComponent<MagicSquare>().UseFinisher();

    }

    //--- カットイン用設定
    public void SetFinisherAtackImage(RenderTexture CutInTex)
    {
        m_FinisherCameraObj.SetActive(true);
        m_FinisherCameraObj.GetComponent<Camera>().targetTexture = CutInTex;
    }

    //--- 魔方陣設置
    public void SetMagicSquare()
    {
        Vector3 Pos = transform.position;
        Pos.y += 0.1f;
        m_MagicSquareObject = Instantiate(m_MahoujinPrefub, Pos, Quaternion.identity) as GameObject;

        m_MagicSquareObject.transform.parent = m_FireBoalParent.transform;

    }

    //--- 召喚
    // 戻り値：出したオブジェクト
    public GameObject Summon()
    {
        GameObject SummonObj = m_MagicSquareObject.GetComponent<MagicSquare>().Summon();

        //--- 攻撃アニメーション

        return SummonObj;
    }

    //--- 攻撃キャンセル
    public void AtackCancel()
    {
        m_UseFinissherParticle.Stop();
        Destroy(m_MagicSquareObject);
        m_FinisherCameraObj.SetActive(false);
    }

    //--- 方向転換
    // 引数：向かせたいオブジェクト
    public void ChangeRot(GameObject ObjData)
    {
        transform.LookAt(ObjData.transform);
        m_MagicSquareObject.GetComponent<MagicSquare>().SetSummonsVec = transform.forward;

    }

    //--- 必殺技アニメーション再生
    public void FinisherAtackAnim()
    {
        m_ChildObj.GetComponent<Animator>().SetBool("FinisherAtack", true);
        m_ChildObj.transform.rotation = Quaternion.Euler(new Vector3(0.0f, m_SettingRot.y, m_SettingRot.z));
    }

    //--- 必殺技終了時処理
    public void EndFinisherAtack()
    {
        m_ChildObj.GetComponent<Animator>().SetBool("FinisherAtack", false);
        m_ChildObj.GetComponent<Animator>().Play("chara_moveMTaiki2");
        m_ChildObj.transform.rotation = Quaternion.Euler(m_SettingRot);
    }

    //--- 情報設定
    public GameObject FireBoalParent { set { m_FireBoalParent = value; } }
}
