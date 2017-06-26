﻿//========================================================
// 必殺技処理
//========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class FinisherAtack : MonoBehaviour
{
    //--- ゲームステート
    public enum FinisherState
    {
        START,
        CUTIN,
        ATACK_ANIM,
        SET_OBJECT,
        CHECK_HIT,
        EFECT,
        END,
    };

    // カットインオブジェクトの位置情報クラス
    class CutinObjPosData
    {
        private Vector3 m_InitPos;     // 初期位置
        private Vector3 m_MovePos;     // 移動位置

        public CutinObjPosData(Vector3 InitPos, Vector3 MovePos)
        {
            m_InitPos = InitPos;
            m_MovePos = MovePos;
        }

        public Vector3 InitPos { get { return m_InitPos; } }
        public Vector3 MovePos { get { return m_MovePos; } }

    }

    //--- メンバ変数 ------------------------------------------------------------------------------------------------------------
    //--- メンバ定数
    private const float CHECK_SIZE = 0.6f;

    #region カットインオブジェクトの位置情報
    private CutinObjPosData[] m_CutinObjPosData = new CutinObjPosData[3]
    {
       new CutinObjPosData(new Vector3(-1020.0f,    34.0f, 0.0f),new Vector3(-503.0f,  34.0f, 0.0f)),
       new CutinObjPosData(new Vector3( 1020.0f,    34.0f, 0.0f),new Vector3( 503.0f,  34.0f, 0.0f)),
       new CutinObjPosData(new Vector3(    0.0f, -1320.0f, 0.0f),new Vector3(   0.0f, 100.0f, 0.0f)),
    };
    #endregion

    //--- メンバ変数
    [SerializeField] private TargetManager m_TargetManagerScript;
    [SerializeField] private GameObject m_FinisherAtackObjPrefub;   // 必殺技オブジェクトプレハブ
    [SerializeField] private GameObject m_FinisherStockImagePrefub; // 必殺技ストック画像プレハブ
    [SerializeField] private GameObject m_FinisherStockImageParent; // 必殺技ストック画像親オブジェクト

    [SerializeField] private RenderTexture m_CutInImageA; // カットインイメージA
    [SerializeField] private RenderTexture m_CutInImageB; // カットインイメージB
    [SerializeField] private RenderTexture m_CutInImageC; // カットインイメージC

    private FinisherState m_FinisherState;
    private FinisherState m_NextChangeState;

    private GameObject m_CutInObjA; // カットインオブジェクトA
    private GameObject m_CutInObjB; // カットインオブジェクトB
    private GameObject m_CutInObjC; // カットインオブジェクトC

    private List<GameObject>    m_FinisherStockImageList;
    private GameObject[]        m_TriangleVertexPosObject;  // 三角形の頂点位置のオブジェクト情報
    private float               m_FinisherAtackTime;        // 時間
    private bool                m_UseFinisher;              // 必殺技を使うかのフラグ
    private FinisherAtackObj    m_FinishSqript;

    //--- メンバ関数 ------------------------------------------------------------------------------------------------------------
    // コンストラクタ
    FinisherAtack()
    {
        m_FinisherAtackTime = 0.0f;
        m_UseFinisher = false;

        m_FinisherState = FinisherState.START;
        m_NextChangeState = m_FinisherState;

        m_FinisherStockImageList = new List<GameObject>();
    }

    void Start()
    {
        m_CutInObjA = transform.FindChild("CutinObjA").gameObject;
        m_CutInObjB = transform.FindChild("CutinObjB").gameObject;
        m_CutInObjC = transform.FindChild("CutinObjC").gameObject;

        CutinObjAllSetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_UseFinisher)
            return;

        m_FinisherAtackTime += Time.deltaTime;

        switch (m_FinisherState)
        {
            case FinisherState.START:
                StartUpdate();
                break;

            case FinisherState.CUTIN:
                CutinUpdate();
                break;

            case FinisherState.ATACK_ANIM:
                AtackAnimation();
                break;

            case FinisherState.SET_OBJECT:
                SetFinisherAtackObject();
                break;

            case FinisherState.CHECK_HIT:
                m_FinishSqript.CheckHit();
                ChangeState(FinisherState.EFECT);
                break;

            case FinisherState.EFECT:
                EfectUpdate();
                break;

            case FinisherState.END:
                m_UseFinisher = false;
                m_TargetManagerScript.UpdateTime = true;
                EnemyManager.AllMoveStart();
                GameMainCamera.Instance.ChangeRot();
                ChangeState(FinisherState.START);
                m_TriangleVertexPosObject.ToList().ForEach(x => x.GetComponent<Player>().EndFinisherAtack());
                break;

        }
    }

    //--- 更新(ループ中最後)
    void LateUpdate()
    {
        //--- ステート変更
        if (m_FinisherState != m_NextChangeState)
            m_FinisherState = m_NextChangeState;
    }

    public void UseFinisher(GameObject ObjA, GameObject ObjB, GameObject ObjC)
    {
        //--- 頂点位置のオブジェクト設定
        m_TriangleVertexPosObject = new GameObject[]
        {
            ObjA,
            ObjB,
            ObjC,
        };

        m_TargetManagerScript.UpdateTime = false;
        m_UseFinisher = true;

        ChangeState(FinisherState.START);
    }

    public void UseFinisher(GameObject[] Obj)
    {
        UseFinisher(Obj[0], Obj[1], Obj[2]);
    }

    void StartUpdate()
    {
        GameMainCamera.Instance.ChangeRot();

        CutinObjAllSetActive(true);

        m_TriangleVertexPosObject[0].GetComponent<Player>().SetFinisherAtackImage(m_CutInImageA);
        m_TriangleVertexPosObject[1].GetComponent<Player>().SetFinisherAtackImage(m_CutInImageB);
        m_TriangleVertexPosObject[2].GetComponent<Player>().SetFinisherAtackImage(m_CutInImageC);

        EnemyManager.AllMoveStop();

        m_CutInObjA.GetComponent<Animator>().Play("CutinObjA",0,0.0f);
        m_CutInObjB.GetComponent<Animator>().Play("CutinObjB",0,0.0f);
        m_CutInObjC.GetComponent<Animator>().Play("CutinObjC",0,0.0f);

        ChangeState(FinisherState.CUTIN);

        m_TriangleVertexPosObject.ToList().ForEach(x => x.GetComponent<Player>().FinisherAtackAnim());

    }

    //--- カットイン中の更新
    void CutinUpdate()
    {
        Animator AnimatorA = m_CutInObjA.GetComponent<Animator>();
        Animator AnimatorB = m_CutInObjB.GetComponent<Animator>();
        Animator AnimatorC = m_CutInObjC.GetComponent<Animator>();

        if (!GameMainCamera.Instance.MoveCamera &&
            AnimatorA.GetCurrentAnimatorStateInfo(0).IsName("CutinObjA") &&
            AnimatorA.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 &&
            AnimatorB.GetCurrentAnimatorStateInfo(0).IsName("CutinObjB") &&
            AnimatorB.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 &&
            AnimatorC.GetCurrentAnimatorStateInfo(0).IsName("CutinObjC") &&
            AnimatorC.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            ChangeState(FinisherState.ATACK_ANIM);
        }
    }

    //--- 攻撃アニメーション
    void AtackAnimation()
    {
        if (m_FinisherAtackTime > 4.0f)
        {
            CutinObjAllSetActive(false);
            ChangeState(FinisherState.SET_OBJECT);
        }
    }

    //--- 必殺技オブジェクト生成
    void SetFinisherAtackObject()
    {
        //--- 必殺技オブジェクト生成
        GameObject FinishObj = Instantiate(m_FinisherAtackObjPrefub, Vector3.zero, Quaternion.identity);
        m_FinishSqript = FinishObj.GetComponent<FinisherAtackObj>();

        //--- 三角形の頂点の位置
        m_FinishSqript.SetVertex(m_TriangleVertexPosObject);

        ChangeState(FinisherState.CHECK_HIT);
    }

    void EfectUpdate()
    {
        if (m_FinisherAtackTime > 2.0f)
        {
            ChangeState(FinisherState.END);
        }
    }

    //--- カットインオブジェクト全てのSetActive
    void CutinObjAllSetActive(bool Active)
    {
        m_CutInObjA.SetActive(Active);
        m_CutInObjB.SetActive(Active);
        m_CutInObjC.SetActive(Active);
    }

    void ChangeState(FinisherState NextState)
    {
        m_NextChangeState = NextState;
        m_FinisherAtackTime = 0.0f;
    }

    //--- 必殺技ストック加算
    public void AddFinisherStock()
    {
        GameObject FinissherStockImage = Instantiate(m_FinisherStockImagePrefub, Vector3.zero, Quaternion.identity);    // Image生成
        FinissherStockImage.transform.SetParent(m_FinisherStockImageParent.transform);  // 親設定
        FinissherStockImage.transform.position = m_FinisherStockImageParent.transform.position;
        FinissherStockImage.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        m_FinisherStockImageList.Add(FinissherStockImage);

        int Count = m_FinisherStockImageList.Count;

        if (Count > 1)
        {
            Vector3 pos = m_FinisherStockImageList[Count - 2].transform.position;
            pos.x -= 10.0f;
            m_FinisherStockImageList[Count - 1].transform.position = pos;
        }
    }

    public bool GetUseFinisher { get { return m_UseFinisher; } }
    public int GetFinisherStock { get { return m_FinisherStockImageList.Count; } }
}