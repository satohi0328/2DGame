using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	[SerializeField] Slider customSlider; // 画面上のカスタムスライダー
	[SerializeField] Button customButton; // カスタムライダー右横のボタン
	[SerializeField] GameObject customPanel; // カスタムパネル

    public float gageSpeed = 0.01f; //ケージの溜まる速さ

	private float lapseTime; // 経過経過

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //ゲージ溜め
		customSlider.value += gageSpeed *Time.deltaTime;

		// ゲージが溜まった場合
		if (customSlider.value == customSlider.maxValue) {
            //ボタンを使えるようにする
			customButton.enabled = true;

            // Qキーを押された場合(画面上のカスタムボタン押下と同じ)
            if (Input.GetKeyDown(KeyCode.Q)){
				callCutsomPanel();
            }
        }
	}

    public void callCutsomPanel() {
		//ゲームストップ
		Time.timeScale = 0f;

		customPanel.SetActive(true);

    }
}
