using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CustomPanel : MonoBehaviour {

	private Button goButton;
	private Text gameText; //ゲーム再開用のText
	public GameObject specialAttackButton; // 画面上のスペシャルアタックボタン
	[SerializeField] Slider customSlider; // 画面上のカスタムスライダー
	[SerializeField] Button customButton; // カスタムライダー右横のボタン

	public GameObject spell1;
	public GameObject spell2;
	public GameObject spell3;
	public GameObject spell4;
	public GameObject spell5;
	public GameObject spell6;

    // 選択されたSpell
	private GameObject selectedSpell;
	public GameObject SelectedSpell {
		get { return this.selectedSpell; }
		set { this.selectedSpell = value; }
	}

	// Use this for initialization
	void Start () {
		this.gameObject.SetActive(false);
		specialAttackButton.SetActive(false);
		customButton.interactable = false; //ボタンを非活性
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
		specialAttackButton.SetActive(true);
		customButton.interactable = false; //ボタンを非活性
        //スペシャル攻撃ボタンのImageを差し替え
		specialAttackButton.GetComponent<Image>().sprite = selectedSpell.GetComponent<Image>().sprite;
	}

	//カスタムパネル上でスキルを押された場合の処理
	public void ClickSpell1() {
		TransformSpellButton(spell1);
	}
	public void ClickSpell2() {
		TransformSpellButton(spell2);
	}
	public void ClickSpell3() {
		TransformSpellButton(spell3);
	}
	public void ClickSpell4() {
		TransformSpellButton(spell4);
	}
	public void ClickSpell5() {
		TransformSpellButton(spell5);
	}
	public void ClickSpell6() {
		TransformSpellButton(spell6);
	}
    public void TransformSpellButton(GameObject spell) {

		//押されたImageのスケールを大きくする
		spell.transform.localScale *= 1.25f;

		// 初回はnullなので、選択されたSpellを設定して処理終了
		if (selectedSpell == null) {
			selectedSpell = spell;
			return;
        } else {
			//前回押下されたSpellのサイズを戻す
			selectedSpell.transform.localScale = new Vector3(1, 1, 1);
			selectedSpell = spell;
		}

	}

}
