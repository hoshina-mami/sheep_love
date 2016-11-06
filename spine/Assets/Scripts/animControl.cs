using UnityEngine;
using System.Collections;
using System;
using Spine;

public class animControl : MonoBehaviour {

	[SpineAnimation]
	SkeletonAnimation skelAnim;

	//SKIN DEFINE
	private const string SKIN_DEFAULT1 = "default1";//口閉じ
	private const string SKIN_DEFAULT2 = "default2";//口開き
	private const string SKIN_NORMAL1 = "normal1";//ほっぺたあり、口閉じ
	private const string SKIN_NORMAL2 = "normal2";//ほっぺたあり、口開き
	private const string SKIN_CLOSE1 = "close1";//ほっぺなし
	private const string SKIN_CLOSE2 = "close2";//ほっぺあり
	private const string SKIN_SMILE1 = "smile1";//口閉じ
	private const string SKIN_SMILE2 = "smile2";//口開き
	private const string SKIN_SAD = "sad";
	private const string SKIN_SLEEPY = "sleepy";

	//ANIM DEFINE
	private const string ANIM_DEFAULT = "animation";
	private const string ANIM_GREET = "greet";
	private const string ANIM_IDLE1 = "idle1";
	private const string ANIM_IDLE2 = "idle2";
	private const string ANIM_NOTICE = "notice";
	private const string ANIM_SLEEP = "sleep";


	// Use this for initialization
	void Start () {

		skelAnim = GetComponent<SkeletonAnimation>();

		//デフォルトアニメを流す
		PlayNormalAnim();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void changeAnim (string targetAnim) 
	{
		//change skin
		switch (targetAnim)
		{
			case "notice":
				skelAnim.skeleton.SetSkin(SKIN_DEFAULT2);
				break;

			case "greet":
				skelAnim.skeleton.SetSkin(SKIN_NORMAL2);
				break;
		
			default:
				skelAnim.skeleton.SetSkin(SKIN_DEFAULT1);
				break;
		}

		skelAnim.state.SetAnimation(0, targetAnim, false);
		StartCoroutine(normalMotion ());
	}

	private IEnumerator normalMotion () 
	{
		skelAnim.state.AddAnimation(0, "animation", true, 1f);
		yield return new WaitForSeconds (1f);
		skelAnim.skeleton.SetSkin(SKIN_DEFAULT1);
	}
		


	//アニメーション設定

	//デフォルト
	private void PlayNormalAnim ()
	{
		skelAnim.skeleton.SetSkin(SKIN_DEFAULT1);
		skelAnim.state.AddAnimation(0, "animation", true, 0);
	}
}
