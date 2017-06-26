//========================================================
// プレイヤーの制御(魔方陣制御に変更か)
//========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;  // 現在のシーン取得用

public class PlayerManager : MonoBehaviour
{
    // 攻撃ガイド線用クラス
    class AtackLineObjData
    {
        public GameObject AtackLine;    // 攻撃ガイド線オブジェクト
        public GameObject AtackObjA;    // 攻撃オブジェクトA
        public GameObject AtackObjB;    // 攻撃オブジェクトB
        public bool AddComboFlg;

        public AtackLineObjData(GameObject SetAtackLine, GameObject SetObjA, GameObject SetObjB)
        {
            AtackLine = SetAtackLine;
            AtackObjA = SetObjA;
            AtackObjB = SetObjB;

            LineRenderer AtackLineRenderer = SetAtackLine.GetComponent<LineRenderer>();
            AtackLineRenderer.SetPosition(0, AtackObjA.transform.position);
            AtackLineRenderer.SetPosition(1, AtackObjB.transform.position);

            AddComboFlg = false;
        }

        public AtackLineObjData(GameObject SetAtackLine, GameObject[] SetObjArrey):this(SetAtackLine, SetObjArrey[0], SetObjArrey[1]) {}
    }

    // プレイヤーキャラの初期情報クラス
    class SetPlayerDAta
    {
        public string   ObjName;    // オブジェクト名
        public Vector3 SetPos;     // 位置
        public Vector3 SetRot;     // 位置

        public SetPlayerDAta(string Name, Vector3 Pos, Vector3 Rot)
        {
            ObjName = Name;
            SetPos = Pos;
            SetRot = Rot;
        }

    }

    //--- メンバ変数 ------------------------------------------------------------------------------------------------------------
    //--- メンバ定数
    private const float MOVE_TIME = 1.0f;  // 移動するまでの時間

    //--- 静的メンバ変数
    private static ScoreManager m_ScoreManagerScript;  // スコア管理スクリプト

    //--- メンバ変数
    [SerializeField] private GameObject m_AtackLinePrefub;  // 攻撃ガイド線プレハブ

    // 攻撃設定用
    private GameObject          m_AttackSettingHitObj;  // 攻撃設定用オブジェクト(タップした時のオブジェクト)
    private List<GameObject>    m_AttackSettingObjList; // 攻撃設定用オブジェクトList
    private bool                m_NormalAtackFlg;

    // プレイヤーキャラ
    #region プレイヤーキャラの初期位置情報
    private SetPlayerDAta[] m_SetPlayerDataArrey = new SetPlayerDAta[6]
    {
        new SetPlayerDAta( "A", new Vector3( 0.0f, 0.0f, 6.0f), new Vector3(0.0f, 180.0f, 0.0f)),
        new SetPlayerDAta( "B", new Vector3(-4.5f, 0.0f, 3.2f), new Vector3(0.0f,  90.0f, 0.0f)),
        new SetPlayerDAta( "C", new Vector3(-4.5f, 0.0f,-3.2f), new Vector3(0.0f,  90.0f, 0.0f)),
        new SetPlayerDAta( "D", new Vector3( 0.0f, 0.0f,-6.0f), new Vector3(0.0f,   0.0f, 0.0f)),
        new SetPlayerDAta( "E", new Vector3( 4.5f, 0.0f,-3.2f), new Vector3(0.0f, -90.0f, 0.0f)),
        new SetPlayerDAta( "F", new Vector3( 4.5f, 0.0f, 3.2f), new Vector3(0.0f, -90.0f, 0.0f)),
    };
    #endregion
    [SerializeField] private GameObject m_PlayerObjPrefub;  // プレイヤーオブジェクトのプレハブ
    [SerializeField] private GameObject m_FireBoalParent;   // 攻撃の親オブジェクト

    // 必殺技用
    [SerializeField] private FinisherAtack m_FinisherAtackScript;
    private bool    m_UseFinisher;      // 必殺技を使うかのフラグ

    private List<string> m_LinkPairDataList;    // 隣同士の情報
    private List<AtackLineObjData> m_PairObjDataList;

    //--- メンバ関数 ------------------------------------------------------------------------------------------------------------
    PlayerManager()
    {
        #region 隣同士の情報
        m_LinkPairDataList = new List<string>
        {
            {"A,B"},
            {"B,C"},
            {"C,D"},
            {"D,E"},
            {"E,F"},
            {"F,A"},
            {"B,A"},
            {"C,B"},
            {"D,C"},
            {"E,D"},
            {"F,E"},
            {"A,F"},
        };
        #endregion

        m_PairObjDataList = new List<AtackLineObjData>();
        // 攻撃設定用初期化
        m_AttackSettingObjList = new List<GameObject>();

        m_NormalAtackFlg = false;

        m_UseFinisher   = false;
    }

    void Awake()
    {
        GameObject AddObjData;
        m_SetPlayerDataArrey.ToList().ForEach(Data => {
            AddObjData = Instantiate(m_PlayerObjPrefub,Data.SetPos,Quaternion.Euler(Data.SetRot));
            AddObjData.name = Data.ObjName;
            AddObjData.transform.SetParent(transform);
            AddObjData.GetComponent<Player>().FireBoalParent = m_FireBoalParent;
        });
    }

	void Update ()
    {
        switch (GameManager.Instance.NowState)
        {
            case GameManager.GameState.SETTING:
            case GameManager.GameState.MAGIC_SQUARE_SETTING:
            case GameManager.GameState.PLAYER_SETTING:
            case GameManager.GameState.START:
                break;

            case GameManager.GameState.GAME_MAIN:
                GameMainUpdate();
                break;

            case GameManager.GameState.GAME_CLEAR:
                break;

            case GameManager.GameState.GAME_OVER:
                break;
        }
    }

    private void GameMainUpdate()
    {
        if (m_FinisherAtackScript.GetUseFinisher)
            return;

        AtackSetting();

        //--- 攻撃ガイド線消滅判定
        m_PairObjDataList.ForEach(x =>
        {
            // 攻撃ガイド線に対応しているオブジェクトが存在するか判定
            if (x.AtackObjA != null &&
                x.AtackObjB != null)
            {
                SummonsBeast AtackScriptA = x.AtackObjA.GetComponent<SummonsBeast>();
                SummonsBeast AtackScriptB = x.AtackObjB.GetComponent<SummonsBeast>();

                //--- どちらかでも敵に当たっているか
                x.AddComboFlg = AtackScriptA.EnemyHit | AtackScriptB.EnemyHit;
            }
            else
            {
                Destroy(x.AtackLine);                           // ガイド線削除
                m_ScoreManagerScript.SetCombo(x.AddComboFlg);   // コンボ情報設定
                m_PairObjDataList.Remove(x);                    // Listから削除
                SEManager.Instance.Play("SE_Explosion");
            }
        });
    }

    //--- 攻撃設定
    private void AtackSetting()
    {
        switch(InputManager.Instance.GetClick())
        {
            case InputManager.CLICK_STATE.NONE:
                break;

            case InputManager.CLICK_STATE.ONECLICK:
                ClickRayCheck();
                //--- エフェクト出す(予定)
                ParticleManager.Instance.TapEffect.Play();
                ParticleManager.Instance.TapEffectObj.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0.0f, 0.0f, 10.0f)) + new Vector3(0.0f, -1.0f, 0.0f);
                break;

            case InputManager.CLICK_STATE.CLICK:

                //--- オブジェクトチェック
                if (m_AttackSettingHitObj == null)
                {
                    SEManager.Instance.Play("beep_fast");
                    AtackCansel();
                    return;
                }

                if (!m_UseFinisher)
                    SetNormalAtackObject();
                else
                    SetFinisherAtackObject();

                break;

            case InputManager.CLICK_STATE.DOUBLECLICK:

                // ストックがあるか
                if (m_FinisherAtackScript.GetFinisherStock < 1)
                    return;

                //--- オブジェクトチェック
                if (m_AttackSettingHitObj == null)
                {
                    SEManager.Instance.Play("beep_fast");
                    AtackCansel();
                    return;
                }

                if (m_NormalAtackFlg)
                    return;

                m_UseFinisher = true;

                SetFinisherAtackObject();
                break;

        }

    }

    //--- クリック先オブジェクト取得
    // 戻り値:当たったオブジェクト(無かった場合null)
    GameObject ClickRayCheck()
    {
        RaycastHit hit;
        Ray ray;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // タップ（クリック）した先にオブジェクトが無かった場合
        if (!Physics.Raycast(ray, out hit))
        {
            m_AttackSettingHitObj = null;
            return null;
        }

        m_AttackSettingHitObj = hit.transform.gameObject;

        // タップ（クリック）したオブジェクトがPlayerか判定
        if (m_AttackSettingHitObj.tag != "Player")
        {
            m_NormalAtackFlg = false;
            m_AttackSettingHitObj = null;
            return null;
        }


        return hit.transform.gameObject;
    }

    //--- 通常攻撃オブジェクト設定
    void SetNormalAtackObject()
    {
        //--- 攻撃オブジェクト設定
        SetAtackObj();

        m_NormalAtackFlg = true;

        //--- 攻撃オブジェクトが二つ設定されているか
        if (m_AttackSettingObjList.Count >= 2)
        {
            // オブジェクトA,Bが隣同士か判定
            bool Pair = m_LinkPairDataList.Contains(m_AttackSettingObjList[0].name + "," + m_AttackSettingObjList[1].name);

            if (Pair)
            {// 隣同士の場合、第一オブジェクト変更
                AtackObjectChange();
                return;
            }

            Player PlayerScriptA = m_AttackSettingObjList[0].GetComponent<Player>();
            Player PlayerScriptB = m_AttackSettingObjList[1].GetComponent<Player>();
            PlayerScriptA.ChangeRot(m_AttackSettingObjList[1]);
            PlayerScriptB.ChangeRot(m_AttackSettingObjList[0]);

            // 召喚
            GameObject ObjA = PlayerScriptA.Summon();
            GameObject ObjB = PlayerScriptB.Summon();

            AtackLineObjData SetData = new AtackLineObjData(Instantiate(m_AtackLinePrefub), ObjA, ObjB);
            m_PairObjDataList.Add(SetData);

            AtackEnd();
        }

    }

    //--- 必殺技オブジェクト設定
    void SetFinisherAtackObject()
    {
        //--- 攻撃オブジェクト設定
        SetAtackObj();

        //--- 攻撃オブジェクトが三つ設定されているか
        if (m_AttackSettingObjList.Count >= 3)
        {
            //--- 必殺技発動

            m_FinisherAtackScript.UseFinisher(m_AttackSettingObjList.ToArray());

            AtackEnd();
        }
    }

    //--- 攻撃オブジェクト設定
    void SetAtackObj()
    {
        //--- 同じ物が選択されたか判定
        if (m_AttackSettingObjList.Any(x => x.name == m_AttackSettingHitObj.name))
            return;

        m_AttackSettingObjList.Add(m_AttackSettingHitObj);


        Player HitObjSqript = m_AttackSettingHitObj.GetComponent<Player>();

        //--- 魔方陣セット
        HitObjSqript.SetMagicSquare();

        //--- 必殺技時
        if (m_UseFinisher)
            HitObjSqript.UseFinisherAtack();

        m_AttackSettingHitObj = null;
    }

    void AtackEnd()
    {

        m_UseFinisher = false;
        m_NormalAtackFlg = false;

        m_AttackSettingObjList.Clear();
    }


    //--- 攻撃キャンセル
    void AtackCansel()
    {
        m_AttackSettingObjList.ForEach(x => x.GetComponent<Player>().AtackCancel());
        AtackEnd();
    }

    //--- 第一攻撃オブジェクト変更
    void AtackObjectChange()
    {
        m_AttackSettingObjList[0].GetComponent<Player>().AtackCancel();
        m_AttackSettingObjList.RemoveAt(0);
    }

    public static ScoreManager ScoreManagerScript { set { m_ScoreManagerScript = value; } }
}
