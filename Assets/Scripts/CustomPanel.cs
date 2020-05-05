using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CustomPanel : MonoBehaviour {

	private Button goButton;
	private Text gameText; //ゲーム再開用のText
	[SerializeField] Slider customSlider; // 画面上のカスタムスライダー

	// Use this for initialization
	void Start () {
		goButton =  transform.Find("GoGameButtom").GetComponent<Button>();
		gameText = GameObject.Find("GameSetText").GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {
		
	}

    // カスタムパネルをクローズ
    public void closeCustomPanel() {
        //ゲーム再開
		customSlider.value = 0f;
		Time.timeScale = 1.0f;
		this.gameObject.SetActive(false);


	}
}
