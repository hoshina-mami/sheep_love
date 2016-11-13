using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class AnimController : MonoBehaviour {

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
	private const string ANIM_GLAD = "glad";
	private const string ANIM_GREET = "greet";
	private const string ANIM_IDLE = "idle";
	private const string ANIM_IDLE2 = "idle2";
	private const string ANIM_NOTICE = "notice";
	private const string ANIM_SLEEP = "sleep";
	private const string ANIM_OFF_HAND = "offHand";
	private const string ANIM_RAISE_HAND = "raiseHand";
	private const string ANIM_ROLLING_IN = "rollingIn";
	private const string ANIM_ROLLING_OUT = "rollingOut";
	private const string ANIM_VANISH = "vanish";

	private const float MOVE_DISTANCE = 0.5f;

	private Vector3 aTapPoint;
	private Vector3 EndPoint;
	private Collider2D aCollider2d;
	private bool isTouched = false;
	private bool isMoving = false;

	enum STATE {
		DEFAULT,
		SLEEP,
		NOTICE,
		GREET,
		GLAD
	}

	private STATE state = STATE.DEFAULT;


	// Use this for initialization
	void Start () {

		skelAnim = GetComponent<SkeletonAnimation>();

		//アニメを流す
		SetAnim();

	}

	// Update is called once per frame
	void Update () {
		
		if (Input.GetMouseButtonDown(0)) {

			if(isTouched || isMoving) { return; }

			aTapPoint   = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			aCollider2d = Physics2D.OverlapPoint(aTapPoint);

			if (aCollider2d) {
				GameObject obj = aCollider2d.transform.gameObject;
				isTouched = true;
			}
		}
			
		if (Input.GetMouseButtonUp (0)) {

			if (isMoving) {
				
				//ドラッグからのリリース
				Debug.Log ("ドラッグした");
				isTouched = false;
				isMoving = false;
				PlayNormalAnim ();
				
			} else if (isTouched) {
				
				//タップからのリリース
				Debug.Log ("タップした");
				ChangeAnim ();
				isTouched = false;
			
			}

		} else if (isTouched && !isMoving) {
			
			Debug.Log (Mathf.Abs(aTapPoint.x - EndPoint.x));
			EndPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);

			if (Mathf.Abs(aTapPoint.x - EndPoint.x) > MOVE_DISTANCE || Mathf.Abs(aTapPoint.y - EndPoint.y) > MOVE_DISTANCE) {
				isMoving = true;
				//撫で中のアニメセット
				ChangeDragAnim();
			}

		} else if (isMoving) {
			
			//撫で中
			EndPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			aCollider2d = Physics2D.OverlapPoint(EndPoint);

			if (!aCollider2d) {
				Debug.Log ("ドラッグした");
				isTouched = false;
				isMoving = false;
				PlayNormalAnim ();
			}
			
		}
	}
		
	//初期の動きを設定
	public void SetAnim () 
	{
		//条件によって動きを変える
		int randomNum = Random.Range( 0, 2 );
		Debug.Log (randomNum);
		if (randomNum == 0) {
			PlayNormalAnim ();
		} else {
			PlaySleepAnim ();
		}

	}


	//タップにより動きを変化させる
	public void ChangeAnim () 
	{
		switch (state) 
		{

		case STATE.DEFAULT:
			StartCoroutine(PlayGreetAnim ());
			break;

		case STATE.SLEEP:
			StartCoroutine(PlayNoticeAnim ());
			break;
			
		}
		
	}

	public void ChangeDragAnim () 
	{
		switch (state) 
		{

		case STATE.DEFAULT:
			PlayGladAnim ();
			break;

//		case STATE.SLEEP:
//			StartCoroutine(PlayNoticeAnim ());
//			break;

		}

	}




	//アニメーション設定

	//デフォルト
	private void PlayNormalAnim ()
	{
		skelAnim.skeleton.SetSkin(SKIN_DEFAULT1);
		skelAnim.state.AddAnimation(0, ANIM_DEFAULT, true, 0);
		state = STATE.DEFAULT;
	}

	//眠る
	private void PlaySleepAnim ()
	{
		skelAnim.skeleton.SetSkin (SKIN_CLOSE1);
		skelAnim.state.AddAnimation (0, ANIM_SLEEP, true, 0);
		state = STATE.SLEEP;
	}

	//気づく
	private IEnumerator PlayNoticeAnim()
	{
		skelAnim.skeleton.SetSkin (SKIN_DEFAULT2);
		skelAnim.state.ClearTracks ();
		skelAnim.state.AddAnimation (0, ANIM_NOTICE, false, 0);
		state = STATE.NOTICE;

		yield return new WaitForSeconds (1f);
		PlayNormalAnim ();
		StartCoroutine(SetSkin (SKIN_SAD, true));
	}

	//わいわい
	private IEnumerator PlayGreetAnim ()
	{
		skelAnim.skeleton.SetSkin (SKIN_NORMAL2);
		skelAnim.state.ClearTracks ();
		skelAnim.state.AddAnimation (0, ANIM_GREET, true, 0);
		state = STATE.GREET;

		yield return new WaitForSeconds (0.9f);
		PlayNormalAnim();
	}

	//撫でられて喜ぶ
	private void PlayGladAnim ()
	{
		skelAnim.skeleton.SetSkin (SKIN_SMILE1);
		skelAnim.state.ClearTracks ();
		skelAnim.state.AddAnimation (0, ANIM_GLAD, true, 0);
		state = STATE.GLAD;
	}




	private IEnumerator SetSkin (string SkinName, bool SetDefault)
	{
		skelAnim.skeleton.SetSkin (SkinName);

		if (SetDefault) {
			yield return new WaitForSeconds (4f);
			skelAnim.skeleton.SetSkin (SKIN_DEFAULT1);
		}
	}
}
