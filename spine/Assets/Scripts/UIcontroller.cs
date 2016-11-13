using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIcontroller : MonoBehaviour {

	[SerializeField]
	private GameObject LoadingDisplay;
	[SerializeField]
	private GameObject GameDisplay;
	[SerializeField]
	private GameObject HintDisplay;
	[SerializeField]
	private GameObject SettingDisplay;
	[SerializeField]
	private GameObject BackPanel;

	[SerializeField]
	private GameObject DebugBtn;

	private readonly Color ACTIVE_COLOR = new Color (255f, 255f, 255f, 255f);


	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;

		HintDisplay.SetActive (false);
		SettingDisplay.SetActive (false);

//		#if !UNITY_EDITOR
//		DebugBtn.SetActive (false);
//		#endif

		StartCoroutine(_StartApp ());
	}
	
	// Update is called once per frame
	void Update () {
		// プラットフォームがアンドロイドかチェック
		if (Application.platform == RuntimePlatform.Android)
		{
			// エスケープキー取得
			if (Input.GetKey(KeyCode.Escape))
			{
				if (HintDisplay.activeSelf || SettingDisplay.activeSelf) {
					CloseSubDisplay ();
					return;
				} else {
					// アプリケーション終了
					Application.Quit();
					return;
				}
			}
		}
	}


	//ヒント・設定画面の表示
	public void ShowSubDisplay (string displayName)
	{
		switch (displayName) 
		{
		case "HINT":
			HintDisplay.SetActive (true);
			break;

		case "SETTING":
			SettingDisplay.SetActive (true);
			break;
		}

		BackPanel.GetComponent<Image> ().color = ACTIVE_COLOR;
		BackPanel.SetActive (true);
	}

	public void CloseSubDisplay ()
	{
		HintDisplay.SetActive (false);
		SettingDisplay.SetActive (false);
		BackPanel.SetActive (false);
	}


	//アプリ起動時の処理
	private IEnumerator _StartApp ()
	{
		//Loadingと見せかけてロゴを主張し、ゲーム画面へ
		yield return new WaitForSeconds (1f);
		_HideObjects (LoadingDisplay);
		_HideObjects (BackPanel);
	}


	//画面遷移時にオブジェクトを消す
	void _HideObjects (GameObject Obj) {
		//makes object's alpha 0
		if(Obj.GetComponent<uTools.uTweenAlpha> () != null) {
			Obj.GetComponent<uTools.uTweenAlpha> ().enabled = true;
		}
	}
}
