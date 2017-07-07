//========================================================
// プレイヤーキャラ一体の動き
//========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : GameMainObjectBase
{
    //--- メンバ変数 -----------------------------------------------------------------------------------
    //--- 静的メンバ変数
    private static GameObject m_MahoujinPrefub = null; // 魔方陣オブジェクトプレハブ

    //--- メンバ変数
    private GameObject m_MagicSquareObject; // 魔方陣オブジェクト
    private GameObject m_FireBoalParent;    // 攻撃親オブジェクト
    private GameObject m_ChildPlayerModel;  // 子オブジェクトのプレイヤーモデル

    private FinisherAtack m_FinisherAtackScript;    // 必殺技管理スクリプト

    // スタート前の処理用
    private float       m_SettingScale = 0.0f;
    private Vector3     m_SettingRot;
    private GameObject  m_WarpObject;

    // 必殺技用
    private ParticleSystem  m_CanUseFinissherParticle;  // 必殺技使用可能時のパーティクル
    private ParticleSystem  m_UseFinissherParticle;     // 必殺技使用設定時のパーティクル
    private GameObject      m_FinisherCameraObj;        // 必殺技カットイン用カメラ

    //--- メンバ関数 -----------------------------------------------------------------------------------
	// Use this for initialization
    void Start()
    {
        if (m_MahoujinPrefub == null)
        {
            m_MahoujinPrefub = Resources.Load("MahoujinObject") as GameObject;
        }

        // スタート前の処理用
        m_ChildPlayerModel = transform.FindChild("chara_model").gameObject;
        m_ChildPlayerModel.transform.localScale = new Vector3(m_SettingScale, m_SettingScale, m_SettingScale);
        m_SettingRot = transform.rotation.eulerAngles;


        // 必殺技用
        m_UseFinissherParticle = transform.FindChild("UseFinisher").GetComponent<ParticleSystem>();
        m_UseFinissherParticle.Pause();

        m_FinisherCameraObj = transform.FindChild("FinissherCamera").gameObject;
        m_FinisherCameraObj.SetActive(false);

        m_CanUseFinissherParticle = transform.FindChild("Shock").GetComponent<ParticleSystem>();
        m_CanUseFinissherParticle.Pause();

    }

    protected override void PlayerSetting()
    {
        m_SettingScale += 20.0f * Time.deltaTime;

        m_SettingRot.y += 360f * Time.deltaTime;

        m_ChildPlayerModel.transform.localScale = new Vector3(m_SettingScale, m_SettingScale, m_SettingScale);
        m_ChildPlayerModel.transform.rotation = Quaternion.Euler(m_SettingRot);

        if (m_ChildPlayerModel.transform.localScale.x >= 20.0f)
        {
            GameManager.Instance.ChangeState(GameManager.GameState.GAME_START);
        }
    }

    protected override void GameStart()
    {
        Destroy(m_WarpObject);
    }

    protected override void GameMain()
    {
        if (m_FinisherAtackScript.GetFinisherStock > 0 && !m_CanUseFinissherParticle.isPlaying)
        {
            m_CanUseFinissherParticle.Play();
        }

        if (m_FinisherAtackScript.GetFinisherStock == 0 && m_CanUseFinissherParticle.isPlaying)
        {
            m_CanUseFinissherParticle.Stop();
        }

        if (GameManager.Instance.GetChangedState)
        {
            Animator Animator = m_ChildPlayerModel.GetComponent<Animator>();

            Animator.speed = 1.0f;
        }
    }

    protected override void Pause()
    {
        if (GameManager.Instance.GetChangedState)
        {
            Animator Animator = m_ChildPlayerModel.GetComponent<Animator>();

            Animator.speed = 0.0f;
        }
    }

    protected override void GameClear()
    {
        AtackCancel();
    }

    protected override void GameOver()
    {
        AtackCancel();
        m_ChildPlayerModel.GetComponent<Animator>().SetBool("GameOver", true);
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
        m_MagicSquareObject = Instantiate(m_MahoujinPrefub, Pos, Quaternion.identity, m_FireBoalParent.transform) as GameObject;

    }

    //--- 召喚
    // 戻り値：出したオブジェクト
    public GameObject Summon()
    {
        GameObject SummonObj = m_MagicSquareObject.GetComponent<MagicSquare>().Summon();

        //--- 攻撃アニメーション
        m_ChildPlayerModel.GetComponent<Animator>().Play("chara_moveMKougeki2",0,0.0f);


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
        m_ChildPlayerModel.GetComponent<Animator>().SetBool("FinisherAtack", true);
    }

    //--- 必殺技終了時処理
    public void EndFinisherAtack()
    {
        m_ChildPlayerModel.GetComponent<Animator>().SetBool("FinisherAtack", false);
        m_ChildPlayerModel.GetComponent<Animator>().Play("chara_moveMTaiki2");
        m_ChildPlayerModel.transform.rotation = Quaternion.Euler(m_SettingRot);
    }

    //--- 情報設定
    public GameObject FireBoalParent            { set { m_FireBoalParent        = value; } }
    public GameObject WarpObject                { set { m_WarpObject            = value; } }
    public FinisherAtack FinisherAtackScript    { set { m_FinisherAtackScript   = value; } }
}
