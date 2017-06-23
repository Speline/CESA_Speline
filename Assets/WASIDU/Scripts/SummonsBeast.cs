//========================================================
// 召喚獣の動き(魔法に変更予定)
//========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonsBeast : MonoBehaviour
{
    //--- メンバ変数
    // メンバ定数
    protected const float MOVE_SIZE = 5.0f;   // 移動量

    // 静的メンバ変数

    // メンバ変数
    protected Vector3 m_StartPos    = Vector3.zero; // 初期位置
    protected Vector3 m_MoveVec     = Vector3.zero; // 移動方向

    private bool m_bEnemyHit;

    //--- メンバ関数
    SummonsBeast()
    {
        m_StartPos  = Vector3.zero;
        m_MoveVec   = Vector3.zero;
        m_bEnemyHit = false;
    }

    protected void Start()
    {
        ParticleManager.Instance.FireBoll.Play();
        m_StartPos = gameObject.transform.position;
    }

    void OnDestroy()
    {
        ParticleManager.Instance.FireBoll.Stop();

        ParticleManager.Instance.MagicCollision.Play();

        ParticleManager.Instance.MagicCollisionObj.transform.position = transform.position;
    }

    void Update()
    {
        MoveUpdate();

        ParticleManager.Instance.FireBollObj.transform.position = transform.position;


    }

    private void MoveUpdate()
    {
        Vector3 Movement;   // 移動量
        Movement = m_MoveVec * Time.deltaTime * MOVE_SIZE;

        //--- 位置移動
        gameObject.transform.position += Movement;
    }

    // 設定
    public Vector3 SetMoveVec { set { m_MoveVec = value; } }

    // 取得
    public Vector3 MoveVec { get { return m_MoveVec; } }

    // 当たり判定
    void OnTriggerEnter(Collider col)
    {
        GameObject HitObject = col.gameObject;

        //障害物との判定
        if (HitObject.tag == "Target")
        {
            m_bEnemyHit = true;
        }

        //自分以外の攻撃との判定
        if (HitObject.tag == "SummonsBeast")
        {
            // 方向を取得して消すかの判定を付けるか
            SummonsBeast HitBeastSqript = HitObject.GetComponent<SummonsBeast>();
            Vector3 InversionHitMoveVec = HitBeastSqript.MoveVec;// *-1f; // 移動方向反転
            Vector3 CheckVector = m_MoveVec + InversionHitMoveVec;

            if (CheckVector.x >= -0.1f && CheckVector.x <= 0.1f &&
                CheckVector.y >= -0.1f && CheckVector.y <= 0.1f &&
                CheckVector.z >= -0.1f && CheckVector.z <= 0.1f)
            {
                //--- 子オブジェクト
                Transform children = gameObject.transform;

                //--- 子オブジェクトすべてチェック
                foreach (Transform ob in children)
                {
                    // 敵が残っていた場合消す
                    if (ob.gameObject.tag == "Target")
                    {
                        ob.gameObject.GetComponent<EnemyBase>().DestryEnemy();
                    }
                }

                Destroy(this.gameObject);
            }
        }
    }

    public bool EnemyHit { get { return m_bEnemyHit; } }
}
