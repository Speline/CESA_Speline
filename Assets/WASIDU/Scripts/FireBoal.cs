//========================================================
// 火球の動き
//========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBoal : MonoBehaviour
{
    //--- メンバ変数 -----------------------------------------------------------------------------------
    //--- メンバ定数
    protected const float MOVE_SIZE             = 5.0f;   // 移動量
    protected const float BULLET_CORE_SCALE     = 0.5f;   // エフェクトスケール
    protected const float BULLET_PARTICLE_SCALE = 1.0f;   // エフェクトスケール

    //--- メンバ変数
    private Vector3     m_MoveVec;      // 移動方向
    private bool        m_EnemyHit;    // 敵に当たっているか
    private bool m_HitFireBoal;

    // 出現時の処理用
    private bool        m_MoveStart;
    private float       m_StartTime;
    private GameObject  m_BulletCore;
    private GameObject  m_BulletParticle;

    //--- メンバ関数 -----------------------------------------------------------------------------------
    FireBoal()
    {
        m_MoveVec   = Vector3.zero;
        m_EnemyHit  = false;
        m_HitFireBoal = false;

        m_StartTime = 0.0f;
        m_MoveStart = false;
    }

    void Start()
    {
        m_BulletCore        = transform.FindChild("Bullet_Core").gameObject;
        m_BulletParticle    = m_BulletCore.transform.FindChild("Bullet_Particle").gameObject;

        m_BulletCore.GetComponent<ParticleSystem>().Play();

        m_BulletCore.transform.localScale       = Vector3.zero;
        m_BulletParticle.transform.localScale   = Vector3.zero;
    }

    void Update()
    {
        //--- ステートによる変更処理
        switch (GameManager.Instance.NowState)
        {
            case GameManager.GameState.SETTING:
            case GameManager.GameState.MAGIC_SQUARE_SETTING:
            case GameManager.GameState.PLAYER_SETTING:
            case GameManager.GameState.GAME_START:
                break;

            case GameManager.GameState.GAME_MAIN:
                GameMain();
                break;

            case GameManager.GameState.GAME_CLEAR:
                Destroy(this.gameObject);
                break;

            case GameManager.GameState.GAME_OVER:
                Destroy(this.gameObject);
                break;
        }

    }

    void LateUpdate()
    {
        //--- ステートによる変更処理
        switch (GameManager.Instance.NowState)
        {
            case GameManager.GameState.SETTING:
            case GameManager.GameState.MAGIC_SQUARE_SETTING:
            case GameManager.GameState.PLAYER_SETTING:
            case GameManager.GameState.GAME_START:
                break;

            case GameManager.GameState.GAME_MAIN:
                if (m_HitFireBoal)
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

                    SEManager.Instance.Play("ポップな爆発");
                    Destroy(this.gameObject);

                }
                break;

            case GameManager.GameState.GAME_CLEAR:
            case GameManager.GameState.GAME_OVER:
                break;
        }
    }

    void GameMain()
    {
        if (m_MoveStart)
        {
            MoveUpdate();
        }
        else
        {
            m_StartTime += Time.deltaTime;

            float BulletCoreScale = BULLET_CORE_SCALE * m_StartTime;
            float BulletParticleScale = BULLET_PARTICLE_SCALE * m_StartTime;

            m_BulletCore.transform.localScale = new Vector3(BulletCoreScale, BulletCoreScale, BulletCoreScale);
            m_BulletParticle.transform.localScale = new Vector3(BulletParticleScale, BulletParticleScale, BulletParticleScale);

            if (m_StartTime >= 1.0f)
                m_MoveStart = true;
        }
    }

    private void MoveUpdate()
    {
        Vector3 Movement;   // 移動量
        Movement = m_MoveVec * Time.deltaTime * MOVE_SIZE;

        //--- 位置移動
        gameObject.transform.position += Movement;
    }

    // 当たり判定
    void OnTriggerEnter(Collider col)
    {
        GameObject HitObject = col.gameObject;

        //障害物との判定
        if (HitObject.tag == "Target")
        {
            m_EnemyHit = true;
        }

        //自分以外の攻撃との判定
        if (HitObject.tag == "FireBoal")
        {
            // 方向を取得して消すかの判定を付けるか
            FireBoal HitBeastSqript = HitObject.GetComponent<FireBoal>();
            Vector3 InversionHitMoveVec = HitBeastSqript.MoveVec;
            Vector3 CheckVector = m_MoveVec + InversionHitMoveVec;

            if (CheckVector.x >= -0.1f && CheckVector.x <= 0.1f &&
                CheckVector.y >= -0.1f && CheckVector.y <= 0.1f &&
                CheckVector.z >= -0.1f && CheckVector.z <= 0.1f)
            {
                m_HitFireBoal = true;
            }
        }
    }

    // 設定
    public Vector3 SetMoveVec { set { m_MoveVec = value; } }

    // 取得
    public Vector3  MoveVec     { get { return m_MoveVec; } }
    public bool     EnemyHit    { get { return m_EnemyHit; } }
}
