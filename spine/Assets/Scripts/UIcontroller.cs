using UnityEngine;
using System.Collections;

public class UIcontroller : MonoBehaviour {

	[SerializeField]
	private GameObject LoadingDisplay;
	[SerializeField]
	private GameObject GameDisplay;
	[SerializeField]
	private GameObject BackPanel;

	[SerializeField]
	private GameObject Btn_Setting;
	[SerializeField]
	private GameObject Btn_Hint;


	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;

		StartCoroutine(StartApp ());
	}
	
	// Update is called once per frame
	void Update () {
		// プラットフォームがアンドロイドかチェック
		if (Application.platform == RuntimePlatform.Android)
		{
			// エスケープキー取得
			if (Input.GetKey(KeyCode.Escape))
			{
				// アプリケーション終了
				Application.Quit();
				return;
			}
		}
	}

	//アプリ起動時の処理
	private IEnumerator StartApp ()
	{
		//Loadingと見せかけてロゴを主張し、ゲーム画面へ
		yield return new WaitForSeconds (1.5f);
		HideObjects (LoadingDisplay);
		HideObjects (BackPanel);
	}


	//画面遷移時にオブジェクトを消す
	void HideObjects (GameObject Obj) {
		//makes object's alpha 0
		if(Obj.GetComponent<uTools.uTweenAlpha> () != null) {
			Obj.GetComponent<uTools.uTweenAlpha> ().enabled = true;
		}
	}
}
