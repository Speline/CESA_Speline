using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagramObj : MonoBehaviour
{
    //--- メンバ変数 -----------------------------------------------------------------------------------
    //--- 静的メンバ変数
    private const float BREAK_ADD_FORCE_Y   = 50.0f;    // 破壊時の加える力（Y軸）
    private const float BREAK_ADD_FORCE_XZ  = 30.0f;    // 破壊時の加える力（X・Z軸）

    //--- メンバ変数
    private GameObject m_FieldHexagramObject;
    private GameObject m_FieldHexagramBaraObject;

    private bool m_GameOverStart;   // ゲームオーバー開始判定

    [SerializeField]
    private ParticleSystem m_CrearEfect;     // クリア時のエフェクト

    //--- メンバ関数 -----------------------------------------------------------------------------------
	// Use this for initialization
	void Start ()
    {
        m_GameOverStart = false;

        m_FieldHexagramObject       = transform.FindChild("FieldHexagram").gameObject;
        m_FieldHexagramBaraObject   = transform.FindChild("FieldHexagramBara").gameObject;

        m_FieldHexagramBaraObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
        switch (GameManager.Instance.NowState)
        {
            case GameManager.GameState.SETTING:
            case GameManager.GameState.MAGIC_SQUARE_SETTING:
            case GameManager.GameState.PLAYER_SETTING:
            case GameManager.GameState.GAME_START:
            case GameManager.GameState.GAME_MAIN:
                break;

            case GameManager.GameState.GAME_CLEAR:
                float HexagramScale = 1f - GameManager.Instance.GetNowStateElapsedTime;
                if (HexagramScale > 0.0f)
                {
                    transform.localScale = new Vector3(HexagramScale, HexagramScale, HexagramScale);
                }
                else if (m_CrearEfect.isPlaying == false)
                {
                    m_CrearEfect.Play();
                    ParticleManager.Instance.MainExplosion.Play();
                    ParticleManager.Instance.MainExplosionObj.transform.position = transform.position;
                }
                break;

            case GameManager.GameState.GAME_OVER:
                GameOver();
                break;
        }
	}

    private void GameOver()
    {
        if (!m_GameOverStart)
        {
            m_FieldHexagramBaraObject.SetActive(true);
            m_FieldHexagramObject.SetActive(false);

            Transform BaraObjChildlen = m_FieldHexagramBaraObject.transform;

            Vector3 AddForceData = Vector3.zero;

            AddForceData.y = BREAK_ADD_FORCE_Y;

            foreach(Transform OneObj in BaraObjChildlen)
            {
                AddForceData.x = Random.Range(-BREAK_ADD_FORCE_XZ, BREAK_ADD_FORCE_XZ);
                AddForceData.z = Random.Range(-BREAK_ADD_FORCE_XZ, BREAK_ADD_FORCE_XZ);
                OneObj.GetComponent<Rigidbody>().AddForce(AddForceData);
            }

            m_GameOverStart = true;
        }

    }
}
