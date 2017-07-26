//========================================================
// 必殺技オブジェクト
//========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FinisherAtackObj : MonoBehaviour
{
    //--- 判定サイズ
    class CheckSize
    {
        public float SizeX;
        public float SizeZ;

        public CheckSize(float SetSizeX, float SetSizeZ)
        {
            SizeX = SetSizeX;
            SizeZ = SetSizeZ;
        }
    };

    //--- メンバ変数 ------------------------------------------------------------------------------------------------------------
    //--- メンバ定数
    private const float CHECK_SIZE = 0.6f;

    //--- 静的メンバ変数
    private static GameObject   m_EnemyParent;
    private static Material     m_FinisherAtackMaterial;    // マテリアル

    //--- メンバ変数
    GameObject[]    m_TriangleVertexPosObject;  // 三角形の頂点位置のオブジェクト情報
    Vector3[]       m_TriangleVertex;           // 三角形の頂点情報
    float           m_LiveTime;                 // 生存時間

    //--- メンバ関数 ------------------------------------------------------------------------------------------------------------
    // コンストラクタ
    FinisherAtackObj()
    {
        m_LiveTime = 0.0f;
    }

    void Start()
    {
        ParticleManager.Instance.Deathblow.Play();
        SEManager.Instance.Play("se_maoudamashii_magical21");

        float PosX = (m_TriangleVertex[0].x + m_TriangleVertex[1].x + m_TriangleVertex[2].x) / 3.0f;
        float PosZ = (m_TriangleVertex[0].z + m_TriangleVertex[1].z + m_TriangleVertex[2].z) / 3.0f;

        ParticleManager.Instance.DeathblowObj.transform.position = new Vector3(PosX,0.0f,PosZ);
    }

    // Update is called once per frame
    void Update()
    {
        m_LiveTime += Time.deltaTime;

        if (m_LiveTime > 3.0f)
        {
            Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        float PosX = (m_TriangleVertex[0].x + m_TriangleVertex[1].x + m_TriangleVertex[2].x) / 3.0f;
        float PosZ = (m_TriangleVertex[0].z + m_TriangleVertex[1].z + m_TriangleVertex[2].z) / 3.0f;

        ParticleManager.Instance.MainExplosion.Play();
        ParticleManager.Instance.MainExplosionObj.transform.position = new Vector3(PosX, 0.0f, PosZ);
        SEManager.Instance.Play("魔法系エフェクト");
    }

    public void CheckHit()
    {
        //--- 子オブジェクト
        Transform children = m_EnemyParent.transform;

        //--- 子オブジェクトすべてチェック
        foreach (Transform ob in children)
        {
            bool Hit = TriangleToQuadCheckHit(ob.position);

            ob.gameObject.GetComponent<EnemyBase>().MoveStop();

            if (Hit)
            {
                ob.gameObject.GetComponent<EnemyBase>().DestryEnemy();
            }
        }
    }

    //--- 三角形の頂点設定
    public void SetVertex(GameObject ObjA, GameObject ObjB, GameObject ObjC)
    {
        Vector3 A = ObjA.transform.position;
        Vector3 B = ObjB.transform.position;
        Vector3 C = ObjC.transform.position;

        //--- 裏表判定
        Vector3 AB = A - B;
        Vector3 BC = B - C;
        double check = BC.x * AB.z - BC.z * AB.x;
        // 法線が下向きの場合
        if (check < 0.0f)
        {// 反転
            Vector3 change = A;
            A = B;
            B = change;
        }

        //--- 頂点設定
        m_TriangleVertex = new Vector3[]
        {
            A,
            B,
            C,
        };

        //--- 頂点位置のオブジェクト設定
        m_TriangleVertexPosObject = new GameObject[]
        {
            ObjA,
            ObjB,
            ObjC,
        };
    }

    //--- 三角形の頂点設定
    public void SetVertex(GameObject[] Obj)
    {
        Vector3 A = Obj[0].transform.position;
        Vector3 B = Obj[1].transform.position;
        Vector3 C = Obj[2].transform.position;

        //--- 裏表判定
        Vector3 AB = A - B;
        Vector3 BC = B - C;
        double check = BC.x * AB.z - BC.z * AB.x;
        // 法線が下向きの場合
        if (check < 0.0f)
        {// 反転
            Vector3 change = A;
            A = B;
            B = change;
        }

        //--- 頂点設定
        m_TriangleVertex = new Vector3[]
        {
            A,
            B,
            C,
        };

        //--- 頂点位置のオブジェクト設定
        m_TriangleVertexPosObject = Obj;

        SetMesh();
    }

    //--- メッシュ生成
    void SetMesh()
    {
        Mesh FinisherMesh = new Mesh();

        // 頂点設定
        FinisherMesh.vertices = m_TriangleVertex;

        // 頂点番号設定
        FinisherMesh.triangles = new int[]
        {
            0,
            1,
            2
        };

        MeshFilter filter = GetComponent<MeshFilter>();
        filter.sharedMesh = FinisherMesh;

        if (m_FinisherAtackMaterial == null)
        {
            m_FinisherAtackMaterial = Resources.Load<Material>("FinisherAtackMaterial");
        }

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.material = m_FinisherAtackMaterial;

        //--- 魔方陣削除
        m_TriangleVertexPosObject.ToList().ForEach(Obj =>
        {
            Obj.GetComponent<Player>().AtackCancel();
        });
    }

    //--- 三角形と四角形の当たり判定
    private bool TriangleToQuadCheckHit(Vector3 CheckObjPos)
    {
        Vector3 CheckPos = CheckObjPos;

        CheckSize[] CheckSizeData = new CheckSize[]
        {
            new CheckSize( CHECK_SIZE, CHECK_SIZE),
            new CheckSize( CHECK_SIZE,-CHECK_SIZE),
            new CheckSize(-CHECK_SIZE,-CHECK_SIZE),
            new CheckSize(-CHECK_SIZE, CHECK_SIZE),
        };

        //--- 四頂点比べる
        for (int nCnt = 0; nCnt < 4; nCnt++)
        {
            CheckPos.x += CheckSizeData[nCnt].SizeX;
            CheckPos.z += CheckSizeData[nCnt].SizeZ;

            bool HitFlg = TriangleToDotCheckHit(CheckPos);

            //--- 一カ所でも当たったらtrue
            if (HitFlg)
                return true;

            CheckPos = CheckObjPos;
        }

        return false;
    }

    //--- 三角形と点の当たり判定
    private bool TriangleToDotCheckHit(Vector3 CheckObjPos)
    {
        //--- 三角形の頂点の位置
        Vector3 A = m_TriangleVertex[0];
        Vector3 B = m_TriangleVertex[1];
        Vector3 C = m_TriangleVertex[2];

        //--- 三角形のそれぞれの辺のベクトル
        Vector3 AB = A - B;
        Vector3 BC = B - C;
        Vector3 CA = C - A;

        //--- 三角形と判定点へのベクトル
        Vector3 AP = A - CheckObjPos;
        Vector3 BP = B - CheckObjPos;
        Vector3 CP = C - CheckObjPos;

        //--- 辺のベクトルと点へのベクトルの外積(Y軸だけ求める)
        double c1 = AB.x * BP.z - AB.z * BP.x;
        double c2 = BC.x * CP.z - BC.z * CP.x;
        double c3 = CA.x * AP.z - CA.z * AP.x;

        if ((c1 > 0 && c2 > 0 && c3 > 0) ||
            (c1 < 0 && c2 < 0 && c3 < 0))
        {
            //三角形の内側に点がある
            return true;
        }

        return false;
    }

    public static GameObject EnemyParent { set { m_EnemyParent = value; } }
}
