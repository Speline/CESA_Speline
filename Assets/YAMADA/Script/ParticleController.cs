using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Z))
		{
			ParticleManager.Instance.NormalDeath.Play();
		}
		if (Input.GetKeyDown(KeyCode.X))
		{
			ParticleManager.Instance.Deathblow.Play();
		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			ParticleManager.Instance.Deathblow_Death.Play();
		}
		if (Input.GetKeyDown(KeyCode.V))
		{
			ParticleManager.Instance.FireBoll.Play();
		}
		if (Input.GetKeyDown(KeyCode.B))
		{
			ParticleManager.Instance.Summon_Circle.Play();
		}
		if (Input.GetKeyDown(KeyCode.N))
		{
			ParticleManager.Instance.MagicCollision.Play();
		}
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			ParticleManager.Instance.FireBoll.Stop();
		}
	}
}
