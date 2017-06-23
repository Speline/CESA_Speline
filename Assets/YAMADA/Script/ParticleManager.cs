
﻿//////////////////////////////////////////////////
//ソース名　　　：ParticleManager.cs			//
//**********************************************//
//プログラム概要：パーティクルマネージャー		//
//**********************************************//
//作成者　　　　：山田雄太						//
//**********************************************//
//更新履歴	     :2017/06/19					//
//////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParticleManager : MonoBehaviour {

	#region 自身のインスタンス
	static ParticleManager mInstance = null;
	public static ParticleManager Instance
	{
		get
		{
			return mInstance;
		}
	}
	#endregion

	//新しいパーティクルを追加した場合は、ここも必ず追加すること
	#region　パーティクルリスト
	public enum ParticleNum
	{
		NormalDeath = 0,	//死亡時（エネミー）
		Deathblow,			//必殺技時
		Deathblow_Death,	//必殺技時の死亡（エネミー）
		FireBoll,			//ファイアーボール（炎）
		TapEffect,			//タップエフェクト
		Summon_Circle,		//エネミー出現時用魔法陣
		MagicCollision,		//魔法衝突時
		NoneAndMax
	}
	#endregion
	
	private List<string> ParticleName = new List<string>();
	private GameObject[] pObject;
	private ParticleSystem[] pParticle;

	#region 各パーティクルの取得
	public ParticleSystem GetParticleSystem(ParticleNum pn)
	{
		return pParticle[(int)pn];
	}
	public GameObject GetParticleSystemObj(ParticleNum pn)
	{
		return pObject[(int)pn];
	}

	public ParticleSystem NormalDeath
	{
		get
		{
			return GetParticleSystem(ParticleNum.NormalDeath);
		}
	}
	public GameObject NormalDeathObj
	{
		get
		{
			return GetParticleSystemObj(ParticleNum.NormalDeath);
		}
	}
	public ParticleSystem Deathblow
	{
		get
		{
			return GetParticleSystem(ParticleNum.Deathblow);
		}
	}
	public GameObject DeathblowObj
	{
		get
		{
			return GetParticleSystemObj(ParticleNum.Deathblow);
		}
	}
	public ParticleSystem Deathblow_Death
	{
		get
		{
			return GetParticleSystem(ParticleNum.Deathblow_Death);
		}
	}
	public GameObject Deathblow_DeathObj
	{
		get
		{
			return GetParticleSystemObj(ParticleNum.Deathblow_Death);
		}
	}
	public ParticleSystem FireBoll
	{
		get
		{
			return GetParticleSystem(ParticleNum.FireBoll);
		}
	}
	public GameObject FireBollObj
	{
		get
		{
			return GetParticleSystemObj(ParticleNum.FireBoll);
		}
	}
	public ParticleSystem TapEffect
	{
		get
		{
			return GetParticleSystem(ParticleNum.TapEffect);
		}
	}
	public GameObject TapEffectObj
	{
		get
		{
			return GetParticleSystemObj(ParticleNum.TapEffect);
		}
	}
	public ParticleSystem Summon_Circle
	{
		get
		{
			return GetParticleSystem(ParticleNum.Summon_Circle);
		}
	}
	public GameObject Summon_CircleObj
	{
		get
		{
			return GetParticleSystemObj(ParticleNum.Summon_Circle);
		}
	}
	public ParticleSystem MagicCollision
	{
		get
		{
			return GetParticleSystem(ParticleNum.MagicCollision);
		}
	}
	public GameObject MagicCollisionObj
	{
		get
		{
			return GetParticleSystemObj(ParticleNum.MagicCollision);
		}
	}
	#endregion

	// Use this for initialization
	void Start () {
		#region インスタンスの作成
		if (mInstance == null)
			mInstance = this;
		else
		{
			Destroy(gameObject);
			mInstance = null;
		}
		#endregion

		#region パーティクル名登録
		ParticleName.Add("NormalDeath");		//0
		ParticleName.Add("DeathBlow");			//1
		ParticleName.Add("Deathblow_Death");    //2
		ParticleName.Add("FireBoll_Fire");      //3
		ParticleName.Add("TapEffect");          //4
		ParticleName.Add("Summon_Circle");      //5
		ParticleName.Add("MagicCollision");		//6
		#endregion

		pObject = new GameObject[ParticleName.Count];
		pParticle = new ParticleSystem[ParticleName.Count];

		for (int i = 0 ; i < ParticleName.Count; i++)
		{
			pObject[i] = GameObject.Find(ParticleName[i]);
			pParticle[i] = pObject[i].GetComponent<ParticleSystem>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
