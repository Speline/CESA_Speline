//========================================================
// BGM管理
//========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    //--- メンバ変数
    // プライベート
    private static BGMManager instance;

    private AudioSource m_AudioSource;
    private Dictionary<string, AudioClip> m_AudioClipData;

    public static BGMManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (BGMManager)FindObjectOfType(typeof(BGMManager));

                if (instance == null)
                {
                    GameObject go = new GameObject("BGMManager");
                    instance = go.AddComponent<BGMManager>();
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

        object[] bgmList = Resources.LoadAll("BGM");

        // コレクションのすべての要素を1回ずつ読み出すための構文(コピペ)
        foreach (AudioClip bgm in bgmList)
        {
            m_AudioClipData[bgm.name] = bgm;
        }

    }

    //--- 再生
    // 引数: 再生したいBGMの名前
    public void Play(string seName)
    {
        if (!m_AudioClipData.ContainsKey(seName))
        {
            Debug.Log(seName + "が入ってない");
            return;
        }

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
