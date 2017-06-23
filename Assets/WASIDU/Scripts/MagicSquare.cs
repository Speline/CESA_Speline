//========================================================
// 魔方陣の制御
//========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MagicSquare : MonoBehaviour
{
    //--- メンバ変数
    // メンバ定数

    // メンバ変数
    private static GameObject m_SummonsBeast;
    private static bool m_ObjectLoad = false;

    private Vector3 m_SummonsVec;   // 召喚方向

    private bool m_bUseFinisher;
    private Material m_Material;

    //--- メンバ関数
    MagicSquare()
    {
        m_bUseFinisher = false;
    }

    // Use this for initialization
    void Start()
    {
        if (!m_ObjectLoad)
        {
            m_SummonsBeast = Resources.Load("FireBoal") as GameObject;
            m_ObjectLoad = true;
        }

        if (GameManager.Instance.NowState == GameManager.GameState.MAGIC_SQUARE_SETTING)
        {
            transform.localScale = new Vector3(0.0f,0.0f,0.0f);
        }

        m_Material = transform.FindChild("Mahoujinn").GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 1.0f, 0), 1.0f);

        if (m_bUseFinisher)
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
                transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);

                if (transform.localScale.x >= 1.0f)
                    GameManager.Instance.ChangeState(GameManager.GameState.PLAYER_SETTING);
                break;

            case GameManager.GameState.PLAYER_SETTING:
                break;

            case GameManager.GameState.START:
                Destroy(this.gameObject);
                break;
        }
    }

    // 召喚
    //戻り値：召喚した攻撃オブジェクト
    public GameObject Summon()
    {
        Quaternion SetRot = Quaternion.LookRotation(m_SummonsVec);
        GameObject SummonsBeastData = Instantiate(m_SummonsBeast, transform.position, SetRot);

        SummonsBeastData.GetComponent<SummonsBeast>().SetMoveVec = m_SummonsVec;

        SummonsBeastData.transform.parent = this.transform.parent;

        Destroy(this.gameObject);

        return SummonsBeastData;
    }

    public void UseFinisher() { m_bUseFinisher = true; }

    public Vector3 SetSummonsVec { set { m_SummonsVec = value; } }
}
