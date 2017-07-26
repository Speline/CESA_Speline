//========================================================
// 魔方陣の制御
//========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSquare : GameMainObjectBase
{
    //--- メンバ変数 ------------------------------------------------------------------------------------------------------------
    //--- 静的メンバ変数
    private static GameObject   m_FireBoal = null;

    //--- メンバ変数
    private Vector3     m_SummonsVec;       // 召喚方向
    private bool        m_UseFinisher;      // 必殺技を使うかの判定
    private Material    m_ParticleMaterial; // パーティクルのマテリアル

    //--- メンバ関数 ------------------------------------------------------------------------------------------------------------
    MagicSquare()
    {
        m_UseFinisher = false;
    }

    // Use this for initialization
    void Start()
    {
        if (m_FireBoal == null)
        {
            m_FireBoal = Resources.Load("FireBoal") as GameObject;
        }

        m_ParticleMaterial = transform.FindChild("SkillEnchant").gameObject.GetComponent<Renderer>().material;
    }

    protected override void MagicSquareSetting()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.PLAYER_SETTING);
    }

    protected override void GameStart()
    {
        Destroy(this.gameObject);
    }

    protected override void GameMain()
    {
        if (m_UseFinisher)
        {
            float r = Mathf.Cos(2 * Mathf.PI * 3.0f * Time.fixedTime / 3 + 0.0f);
            float g = Mathf.Cos(2 * Mathf.PI * 3.0f * Time.fixedTime / 3 + 2.0f);
            float b = Mathf.Cos(2 * Mathf.PI * 3.0f * Time.fixedTime / 3 + 4.0f);

            m_ParticleMaterial.SetColor("_TintColor", new Color(r, g, b, 0.3f));
        }
    }

    // 召喚
    //戻り値：召喚した攻撃オブジェクト
    public GameObject Summon()
    {
        Quaternion SetRot = Quaternion.LookRotation(m_SummonsVec);  // 角度
        Vector3 SetPos = transform.position;                        // 出現位置
        SetPos += m_SummonsVec;

        GameObject FireBoalData = Instantiate(m_FireBoal, SetPos, SetRot, this.transform.parent);

        FireBoalData.GetComponent<FireBoal>().SetMoveVec = m_SummonsVec;

        Destroy(this.gameObject);

        return FireBoalData;
    }

    public void UseFinisher() { m_UseFinisher = true; }

    public Vector3 SetSummonsVec { set { m_SummonsVec = value; } }
}
