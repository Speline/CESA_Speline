//========================================================
// 魔方陣の制御
//========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MagicSquare : MonoBehaviour
{
    //--- メンバ変数 ------------------------------------------------------------------------------------------------------------
    //--- メンバ定数

    //--- 静的メンバ変数
    private static GameObject   m_FireBoal = null;

    //--- メンバ変数
    private Vector3 m_SummonsVec;   // 召喚方向

    private bool        m_UseFinisher;
    private Material    m_Material;

    private GameObject m_Effect;

    //--- メンバ関数 ------------------------------------------------------------------------------------------------------------
    MagicSquare()
    {
        m_UseFinisher = false;
    }

    // Use this for initialization
    void Start()
    {
        m_Effect = transform.FindChild("SkillEnchant").gameObject;

        if (m_FireBoal == null)
        {
            m_FireBoal = Resources.Load("FireBoal") as GameObject;
        }

        if (GameManager.Instance.NowState == GameManager.GameState.SETTING)
        {
            m_Effect.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        }

        m_Material = transform.FindChild("Mahoujinn").GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(new Vector3(0, 1.0f, 0), 1.0f);

        if (m_UseFinisher)
        {
            float r = Mathf.Cos(2 * Mathf.PI * 3.0f * Time.fixedTime / 3 + 0.0f);
            float g = Mathf.Cos(2 * Mathf.PI * 3.0f * Time.fixedTime / 3 + 2.0f);
            float b = Mathf.Cos(2 * Mathf.PI * 3.0f * Time.fixedTime / 3 + 4.0f);

            m_Material.color = new Color(r,g,b);
        }

        switch (GameManager.Instance.NowState)
        {
            case GameManager.GameState.SETTING:
                break;

            case GameManager.GameState.MAGIC_SQUARE_SETTING:
                SquareSettingUpdate();
                break;

            case GameManager.GameState.PLAYER_SETTING:
                break;

            case GameManager.GameState.START:
                Destroy(this.gameObject);
                break;

            case GameManager.GameState.GAME_MAIN:
                break;

            case GameManager.GameState.GAME_CLEAR:
                break;

            case GameManager.GameState.GAME_OVER:
                break;
        }
    }

    void SquareSettingUpdate()
    {
        m_Effect.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);

        if (m_Effect.transform.localScale.x >= 0.7f)
            GameManager.Instance.ChangeState(GameManager.GameState.PLAYER_SETTING);
    }

    // 召喚
    //戻り値：召喚した攻撃オブジェクト
    public GameObject Summon()
    {
        Quaternion SetRot = Quaternion.LookRotation(m_SummonsVec);
        GameObject SummonsBeastData = Instantiate(m_FireBoal, transform.position, SetRot);

        SummonsBeastData.GetComponent<SummonsBeast>().SetMoveVec = m_SummonsVec;

        SummonsBeastData.transform.parent = this.transform.parent;

        Destroy(this.gameObject);

        return SummonsBeastData;
    }

    public void UseFinisher() { m_UseFinisher = true; }

    public Vector3 SetSummonsVec { set { m_SummonsVec = value; } }
}
