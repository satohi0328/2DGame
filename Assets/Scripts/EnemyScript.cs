using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour {

    //プライベート変数
    private Animator anim = null;
    private Rigidbody2D rb = null;
    private bool isGround = false;
    private bool isJump = false;
    private bool isHead = false;
    private bool isWalk = false;
    private float jumpPos = 0.0f;
    private float beforeKey;
    private float dashTime, jumpTime;
    private float lapseTime = 0.0f; //攻撃後の経過時間
    private bool isAttack = false; // 攻撃したかどうか
    private float myDirection = -1f; //キャラクターの向き
    private bool isReachTargetPosition;    // 目的地に着いたかどうか
    private bool isPatternEnd = true; //行動が終了しているか判定
    private PATTERN pattern;
    private bool gameSetFlg = false; //ゲーム終了フラグ
    private Slider _slider;

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
    public float coolTime = 0.5f; //攻撃のクールタイム
    public int life; //体力
    public float motionTime; //モーション時間
    public Text gameSetText; //ゲーム終了Text

    //行動パターン
    public enum PATTERN {
        AHEAD,
        BACK,
        JUMP,
        ATTACK,
        ATTACK_2
    };

    // 体力ゲージの初期化
    private void ResetLife() {
        _slider.maxValue = life;
        _slider.value = life;
    }

    // Use this for initialization
    void Start() {
        //コンポーネントのインスタンスを捕まえる
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // スライダーを取得する
        _slider = GameObject.Find("EmenyLife").GetComponent<Slider>();

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
            anim.SetBool("die", true);
            gameSetText.text = "YOU WIN";
            return;
        }

        //接地判定
        isGround = ground.IsGround();

        //行動が終わっている場合
        if (isPatternEnd) {
            isPatternEnd = false;
            // 行動パターンをランダムで選択
            int intRandom;
            intRandom = Random.Range(1, 100);
            if (1 <= intRandom && intRandom <= 20) {
                pattern = PATTERN.AHEAD;
            } else if (21 <= intRandom && intRandom <= 40) {
                pattern = PATTERN.ATTACK_2;
            } else if (41 <= intRandom && intRandom <= 60) {
                pattern = PATTERN.BACK;
            } else if (61 <= intRandom && intRandom <= 90) {
                pattern = PATTERN.ATTACK;
            } else {
                pattern = PATTERN.JUMP;
            }

            Debug.Log(pattern);
        }

        // 
        switch (pattern) {
            case PATTERN.AHEAD:
                Pattern_ahead();
                break;
            case PATTERN.BACK:
                Pattern_back();
                break;
            case PATTERN.JUMP:
                Pattern_jump();
                break;
            case PATTERN.ATTACK:
                Pattern_attack();
                break;
            case PATTERN.ATTACK_2:
                Pattern_attack_2();
                break;

        }

        //// 攻撃できるか判定
        //if (JudgeAttack()) {
        //    if (Input.GetKey(KeyCode.Space)) {
        //        Ibarst();
        //    }

        //}

        ////移動速度を設定
        //rb.velocity = new Vector2(xSpeed, ySpeed);

        //アニメーションを適用
        SetAnimation();
    }

    /**
     *モーション:前移動
     */
    private void Pattern_ahead() {
        //画面中央まで移動可能 or モーション時間以内
        if (this.transform.position.x > 0f && lapseTime < motionTime) {
            lapseTime += Time.deltaTime;
            rb.velocity = new Vector2(-speed, -gravity);
            isWalk = true;
            //画面中央を越えようとした場合、前移動モーション終了
        } else {
            ResetMotion();
        }
    }

    /**
     *モーション: 後ろ移動
     */
    private void Pattern_back() {
        //x座標7まで移動可能
        if (this.transform.position.x < 7f && lapseTime < motionTime) {
            lapseTime += Time.deltaTime;
            rb.velocity = new Vector2(speed, -gravity);
            isWalk = true;
            //画面中央を越えようとした場合、前移動モーション終了
        } else {
            ResetMotion();
        }
    }

    /**
    *モーション:ジャンプ
    */
    private void Pattern_jump() {
        //ジャンプ
        if (this.transform.position.y < 2f && lapseTime < motionTime) {
            rb.velocity = new Vector2(0f, jumpSpeed);
            isJump = true;

        } else {
            ResetMotion();
        }
    }

    /**
    *モーション:攻撃
    */
    private void Pattern_attack() {
        Ibarst();
        isPatternEnd = true;
    }

    /**
    *モーション:攻撃(強)
    */
    private void Pattern_attack_2() {
        Ibarst_2();
        isPatternEnd = true;
    }

    //モーション終了後の処理
    private void ResetMotion() {
        // 諸々初期化
        rb.velocity = new Vector2(0f, gravity);
        lapseTime = 0f;
        isPatternEnd = true;
        isJump = false;
        isWalk = false;
        SetAnimation();
    }


    /// <summary>
    /// アニメーションを設定する
    /// </summary>
    private void SetAnimation() {
        anim.SetBool("jump", isJump);
        anim.SetBool("is_walk", isWalk);

    }

    private void Ibarst() {

        //攻撃アニメーション
        anim.SetTrigger("attack_Trigger");

        // 攻撃オブジェクトを生成
        GameObject g = Instantiate(ibarst);
        //右向きの場合
        g.transform.localScale = new Vector2(g.transform.localScale.x * myDirection, g.transform.localScale.y);
        g.transform.position = new Vector2(this.transform.position.x + 1f * myDirection, transform.position.y - 0.5f);
        isAttack = true;
    }

    private void Ibarst_2() {
        //攻撃アニメーション
        anim.SetTrigger("attack_Trigger");

        // 攻撃オブジェクトを生成
        GameObject g1 = Instantiate(ibarst);
        GameObject g2 = Instantiate(ibarst);
        GameObject g3 = Instantiate(ibarst);
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
        if (collision.CompareTag("Ibarst")) {
            life -= 1;
            _slider.value = life;

            if (life == 0) {
                ResetMotion();
                gameSetFlg = true;
            }
        }
    }

    //public void SetState(MyState tempState) {
    //    if (tempState == MyState.Normal) {
    //        state = MyState.Normal;
    //    } else if (tempState == MyState.Attack) {
    //        velocity = Vector3.zero;
    //        state = MyState.Attack;
    //        animator.SetTrigger("Attack");
    //    }
    //}


}
