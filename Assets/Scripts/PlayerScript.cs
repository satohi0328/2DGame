using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

    //プライベート変数
    private Animator anim = null;
    private Rigidbody2D rb = null;
    private bool isGround = false;
    private bool isJump = false;
    private bool isHead = false;
    private float jumpPos = 0.0f;
    private float beforeKey;
    private float dashTime, jumpTime;
    private float lapseTime = 0.0f; //攻撃後の経過時間
    private bool isAttack = false; // 攻撃したかどうか
    private float myDirection; //プレイヤーが向いている方向
    private Slider _slider;
    private bool gameSetFlg = false; //ゲーム終了フラグ

    
    private bool isLButtonDown = false;//左ボタン押下の判定
    private bool isRButtonDown = false;//右ボタン押下の判定
    private bool isUpButtonDown = false;//上ボタン押下の判定
    private bool isAckButtonDown = false;//攻撃ボタン押下の判定


    //パブリック変数
    public float speed; //速度
    public float gravity; //重力
    public float jumpSpeed;//ジャンプする速度
    public float jumpHeight;//高さ制限
    public float jumpLimitTime;//ジャンプ制限時間
    public GroundCheck ground; //接地判定
    public GroundCheck head;//頭ぶつけた判定
    public AnimationCurve dashCurve; //
    public AnimationCurve jumpCurve; //
    public GameObject ibarst; // 攻撃オブジェクト
    public GameObject fire; // 攻撃オブジェクト(強)
    public float coolTime = 0.5f; //攻撃のクールタイム
    public int life; //体力
    public Text gameSetText; //ゲーム終了Text


    // Use this for initialization
    void Start() {
        //コンポーネントのインスタンスを捕まえる
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // スライダーを取得する
        _slider = GameObject.Find("PlayerLife").GetComponent<Slider>();

        //体力初期化
        ResetLife();

        //lapseTimeを初期化
        lapseTime = 0.0f;
    }

    // Update is called once per frame
    void FixedUpdate() {

        //ゲーム終了時は動きを止める
        if (gameSetFlg) {
            rb.velocity = new Vector2(0f, 0f);
            anim.Play("die");
            Destroy(this.gameObject, 3f);
            gameSetText.text = "YOU LOSE";

            return;
        }
        //接地判定
        isGround = ground.IsGround();

        //各種座標軸の速度を求める
        float xSpeed = GetXSpeed();
        float ySpeed = GetYSpeed();

        // 攻撃できるか判定
        if (JudgeAttack()) {
            if (Input.GetKey(KeyCode.Space) || isAckButtonDown == true) {
                //Ibarst();
                Ibarst_2();
                anim.SetTrigger("attack_Trigger");
            }
        }

        //移動速度を設定
        rb.velocity = new Vector2(xSpeed, ySpeed);

        //アニメーションを適用
        SetAnimation();
    }


    // 体力ゲージの初期化
    private void ResetLife() {
        _slider.maxValue = life;
        _slider.value = life;
    }

    /// Y軸の動きを取得
    private float GetYSpeed() {
        float verticalKey = Input.GetAxis("Vertical");
        float ySpeed = -gravity;

        if (isGround) {
            if ((verticalKey > 0 || isUpButtonDown == true) && jumpTime < jumpLimitTime) {
                ySpeed = jumpSpeed;
                jumpPos = transform.position.y; //ジャンプした位置を記録する
                isJump = true;
                jumpTime = 0.0f;
            } else {
                isJump = false;
            }
        } else if (isJump) {
            //上ボタンを押されている。かつ、現在の高さがジャンプした位置から自分の決めた位置より下ならジャンプを継続する
            if ((verticalKey > 0 || isUpButtonDown == true) && jumpPos + jumpHeight > transform.position.y && jumpTime < jumpLimitTime && !isHead) {
                ySpeed = jumpSpeed;
                jumpTime += Time.deltaTime;
            } else {
                isJump = false;
                jumpTime = 0.0f;
            }
        }

        if (isJump) {
            ySpeed *= jumpCurve.Evaluate(jumpTime);
        }

        return ySpeed;
    }

    /// X軸の動きを取得
    private float GetXSpeed() {
        float horizontalKey = Input.GetAxis("Horizontal");
        float xSpeed = 0.0f;

        // (→)右方向の場合
        if ((horizontalKey > 0 || isRButtonDown == true )&& this.transform.position.x <= -0.5f) {
            transform.localScale = new Vector3(1, 1, 1);
            anim.SetBool("is_walk", true);
            myDirection = 1;

            xSpeed = speed;
            // (←)左方向の場合
        } else if ((horizontalKey < 0 || isLButtonDown == true )&& this.transform.position.x >= -8f) {
            transform.localScale = new Vector3(-1, 1, 1);
            anim.SetBool("is_walk", true);
            myDirection = -1;

            xSpeed = -speed;
        } else {
            anim.SetBool("is_walk", false);
            xSpeed = 0.0f;
        }

        return xSpeed;
    }


    // アニメーションを設定
    private void SetAnimation() {
        anim.SetBool("jump", isJump);
    }



    private void Ibarst() {
        //攻撃アニメーション
        anim.SetTrigger("attack_Trigger");

        // 攻撃オブジェクトを生成
        GameObject g = Instantiate(ibarst);
        //攻撃オブジェクトの配置
        g.transform.localScale = new Vector2(g.transform.localScale.x * myDirection, g.transform.localScale.y);
        g.transform.position = new Vector2(this.transform.position.x + 1f * myDirection, transform.position.y - 0.5f);
        isAttack = true;
    }

    private void Ibarst_2() {
        //攻撃アニメーション
        anim.SetTrigger("attack_Trigger");

        // 攻撃オブジェクトを生成
        GameObject g1 = Instantiate(fire);
        GameObject g2 = Instantiate(fire);
        GameObject g3 = Instantiate(fire);
        //攻撃オブジェクトの配置
        g1.transform.localScale = new Vector2(g1.transform.localScale.x * myDirection, g1.transform.localScale.y);
        g2.transform.localScale = new Vector2(g2.transform.localScale.x * myDirection, g2.transform.localScale.y);
        g3.transform.localScale = new Vector2(g3.transform.localScale.x * myDirection, g3.transform.localScale.y);
        g1.transform.position = new Vector2(this.transform.position.x + 0.2f * myDirection, transform.position.y - 0.2f);
        g2.transform.position = new Vector2(this.transform.position.x + 1f * myDirection, transform.position.y - 0.5f);
        g3.transform.position = new Vector2(this.transform.position.x + 0.2f * myDirection, transform.position.y - 0.7f);

        isAttack = true;
    }

    private bool JudgeAttack() {

        // 攻撃モーション中の場合
        if (isAttack) {
            // クールタイム中の場合
            if (coolTime > lapseTime) {
                // 経過時間を加算して終了
                lapseTime += Time.deltaTime;
                return false;
            } else {
                // 変数初期化
                isAttack = false;
                lapseTime = 0;
                return true;
            }
        }

        return true;

    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Ibarst_Black")) {
            life -= 1;
            _slider.value = life;

            if (life == 0) {                
                gameSetFlg = true;
            }
        }
    }

    //左ボタンを押し続けた場合の処理
    public void GetMyLeftButtonDown() {
        this.isLButtonDown = true;
    }
    //左ボタンを離した場合の処理
    public void GetMyLeftButtonUp() {
        this.isLButtonDown = false;
    }

    //右ボタンを押し続けた場合の処理
    public void GetMyRightButtonDown() {
        this.isRButtonDown = true;
    }
    //右ボタンを離した場合の処理
    public void GetMyRightButtonUp() {
        this.isRButtonDown = false;
    }
    //上ボタンを押し続けた場合の処理
    public void GetMyUpButtonDown() {
        this.isUpButtonDown = true;
    }
    //上ボタンを離した場合の処理
    public void GetMyUpButtonUp() {
        this.isUpButtonDown = false;
    }

    //攻撃ボタンを押し続けた場合の処理
    public void GetMyAttackButtonDown() {
        this.isAckButtonDown = true;
    }
    //攻撃ボタンを離した場合の処理
    public void GetMyAttackButtonUp() {
        this.isAckButtonDown = false;
    }
}
