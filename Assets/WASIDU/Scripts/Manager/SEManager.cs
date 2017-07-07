//====================================================
// SEManager.cs
//----------------------------------------------------
// 製作者  : 鷲津浩司
//====================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SEManager : MonoBehaviour
{
    //--- メンバ変数
    // プライベート
    private static SEManager instance;

    private AudioSource m_AudioSource;
    private Dictionary<string, AudioClip> m_AudioClipData;

    public static SEManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (SEManager)FindObjectOfType(typeof(SEManager));

                if (instance == null)
                {
                    GameObject go = new GameObject("SEManager");
                    instance = go.AddComponent<SEManager>();
                }
            }

            return instance;
        }
    }

    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        m_AudioSource = GetComponent<AudioSource>();

        if (m_AudioSource == null)
        {
            m_AudioSource = gameObject.AddComponent<AudioSource>();
        }

        m_AudioClipData = new Dictionary<string, AudioClip>();

        object[] seList = Resources.LoadAll("SE");

        // コレクションのすべての要素を1回ずつ読み出すための構文(コピペ)
        foreach (AudioClip se in seList)
        {
            m_AudioClipData[se.name] = se;
        }

    }

    //--- 再生
    // 引数: 再生したい効果音の名前
    public void Play(string seName)
    {
        if (!m_AudioClipData.ContainsKey(seName))
        {
            Debug.Log(seName + "が入ってない");
            return;
        }

        if (m_AudioSource.isPlaying && m_AudioSource.name == seName)
            return;

        m_AudioSource.clip = m_AudioClipData[seName];
        m_AudioSource.Play();
    }

    //--- 停止
    public void Pause()
    {
        m_AudioSource.Pause();
    }

    public void SetVolume(float SetVolume)
    {
        m_AudioSource.volume = SetVolume;
    }
}

