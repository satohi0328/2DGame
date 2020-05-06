using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackScript : MonoBehaviour {

    //プライベート変数
    private Animator anim = null;
    private CustomPanel customPanel; // カスタムパネルクラス 選択されたSpellを利用

    public GameObject fireBall; // ファイアボールオブジェクト
    public GameObject ice; // アイスオブジェクト
    public GameObject blackHall; // ブラックホールオブジェクト

    public enum ATTACK_KIND {
        FIREBALL,
        ICE,
        SLIME,
        DEFENCER,
        AWAKE,
        BLACKHALL
    }

    public string specialAttack; //特別攻撃の番号
	// Use this for initialization
	void Start () {

        //コンポーネントのインスタンスを取得
        anim = GetComponent<Animator>();
        customPanel = GameObject.Find("CustomPanel").GetComponent<CustomPanel>();
        Debug.Log(GameObject.Find("CustomPanel").name);

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void SpecialAttack() {
        // TODO カスタムパネルで選ばれたやつで攻撃するようにする
        switch (customPanel.SelectedSpell.name) {
            case "spell1":
                Attack_FireBall();
                break;
            case "spell6":
                Attack_BlackHall();
                break;
        }

    }

    private void Attack_FireBall() {

        // キャラクターの向き取得
        float myDirection = this.gameObject.transform.localScale.x;

        //攻撃アニメーション
        anim.SetTrigger("attack_Trigger");
        // 攻撃オブジェクトを生成
        GameObject g1 = Instantiate(fireBall);
        GameObject g2 = Instantiate(fireBall);
        GameObject g3 = Instantiate(fireBall);
        //攻撃オブジェクトの配置
        g1.transform.localScale = new Vector2(g1.transform.localScale.x * myDirection, g1.transform.localScale.y);
        g2.transform.localScale = new Vector2(g2.transform.localScale.x * myDirection, g2.transform.localScale.y);
        g3.transform.localScale = new Vector2(g3.transform.localScale.x * myDirection, g3.transform.localScale.y);
        g1.transform.position = new Vector2(this.transform.position.x + 0.2f * myDirection, transform.position.y - 0.2f);
        g2.transform.position = new Vector2(this.transform.position.x + 1f * myDirection, transform.position.y - 0.5f);
        g3.transform.position = new Vector2(this.transform.position.x + 0.2f * myDirection, transform.position.y - 0.7f);

    }
    private void Attack_BlackHall() {

        // キャラクターの向き取得
        float myDirection = this.gameObject.transform.localScale.x;

        //攻撃アニメーション
        anim.SetTrigger("attack_Trigger");
        // 攻撃オブジェクトを生成
        GameObject g1 = Instantiate(blackHall);
        GameObject g2 = Instantiate(blackHall);
        GameObject g3 = Instantiate(blackHall);
        //攻撃オブジェクトの配置
        g1.transform.localScale = new Vector2(g1.transform.localScale.x * myDirection, g1.transform.localScale.y);
        g2.transform.localScale = new Vector2(g2.transform.localScale.x * myDirection+0.3f, g2.transform.localScale.y+0.3f);
        g3.transform.localScale = new Vector2(g3.transform.localScale.x * myDirection+0.7f, g3.transform.localScale.y+0.7f);
        g1.transform.position = new Vector2(this.transform.position.x + 0.2f * myDirection, transform.position.y - 0.2f);
        g2.transform.position = new Vector2(this.transform.position.x + 0.2f * myDirection, transform.position.y - 0.2f);
        g3.transform.position = new Vector2(this.transform.position.x + 0.2f * myDirection, transform.position.y - 0.2f);
        g1.GetComponent<Rigidbody2D>().AddForce(new Vector2(500, 0));
        g2.GetComponent<Rigidbody2D>().AddForce(new Vector2(500, 0));
        g3.GetComponent<Rigidbody2D>().AddForce(new Vector2(500, 0));

    }
}
