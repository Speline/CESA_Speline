using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Char : MonoBehaviour
{
	[SerializeField]	float fStartPosX;
	[SerializeField]	float fEndPosX;
	[SerializeField]	float fSlideInTime;				// スライドインにかける時間
	[SerializeField]	float fAttackAnimeInterval;		// 攻撃アニメーションをする間隔(秒)
	[SerializeField]	Material[] mat = new Material[2];	// このモデルでは２つのマテリアルを使っている（フェードアウト用）
	[SerializeField]	float fFadeOutTime;

	Animator animator;
	bool bInitializ = true;
	float fParameter;
	float fAlpha;

	// Use this for initialization
	void Start ()
	{
		animator = GetComponent<Animator>();

		transform.localPosition = new Vector3(fStartPosX, transform.localPosition.y, transform.localPosition.z);
		transform.rotation = Quaternion.Euler(0.0f, 90.0f, Camera.main.transform.eulerAngles.x);		// 向き合わせる
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	// 攻撃アニメ開始
	public void StartAttackAnime()
	{
		StartCoroutine("AttackAnime");
	}

	// 攻撃アニメ停止
	public void StopAttackAnime()
	{
		StopCoroutine("AttackAnime");
	}

	// 一定時間ごとに攻撃アニメーションを行う
	IEnumerator AttackAnime()
	{
		float fTime = fAttackAnimeInterval;

		while(true)
		{
			fTime += Time.deltaTime;
			if(fTime >= fAttackAnimeInterval)
			{
				fTime = 0.0f;
				animator.Play("chara_moveMKougeki2", 0, 0.0f);		// 攻撃アニメ
			}

			yield return null;
		}
	}

	public bool SlideIn()
	{
		float fPos;

		if (bInitializ)
		{
			fParameter = 0.0f;
			bInitializ = false;
		}

		fParameter += Time.deltaTime / fSlideInTime;
		if (fParameter >= 1.0f)
		{
			bInitializ = true;

			transform.localPosition = new Vector3(fEndPosX, transform.localPosition.y, transform.localPosition.z);

			return true;
		}

		fPos = Mathf.Lerp(fStartPosX, fEndPosX, fParameter);
		transform.localPosition = new Vector3(fPos, transform.localPosition.y, transform.localPosition.z);

		return false;
	}

	// 
	public bool FadeOut()
	{
		float fPos;

		if (bInitializ)
		{
			fParameter = 0.0f;
			bInitializ = false;
		}

		fParameter += Time.deltaTime / fFadeOutTime;
		if (fParameter >= 1.0f)
		{
			bInitializ = true;

			transform.localPosition = new Vector3(fStartPosX, transform.localPosition.y, transform.localPosition.z);

			return true;
		}

		fPos = Mathf.Lerp(fEndPosX, fStartPosX, fParameter);
		transform.localPosition = new Vector3(fPos, transform.localPosition.y, transform.localPosition.z);

		return false;
	}

}
