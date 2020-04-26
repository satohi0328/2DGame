using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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



    //Playerの状態
    public enum MyState {
        Idle,
        Attack,
        Jum
    };

    // Use this for initialization
    void Start() {
        //コンポーネントのインスタンスを捕まえる
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        //lapseTimeを初期化
        lapseTime = 0.0f;
    }

    // Update is called once per frame
    void FixedUpdate() {
        //接地判定
        isGround = ground.IsGround();

        //各種座標軸の速度を求める
        float xSpeed = GetXSpeed();
        float ySpeed = GetYSpeed();

        // 攻撃できるか判定
        if (JudgeAttack()) {
            if (Input.GetKey(KeyCode.Space)) {
                Ibarst();
                anim.SetTrigger("attack_Trigger");
            }

        }

        //移動速度を設定
        rb.velocity = new Vector2(xSpeed, ySpeed);

        //アニメーションを適用
        SetAnimation();

    }


    /// <summary>
    /// Y成分で必要な計算をし、速度を返す。
    /// </summary>
    /// <returns>Y軸の速さ</returns>
    private float GetYSpeed() {
        float verticalKey = Input.GetAxis("Vertical");
        float ySpeed = -gravity;

        if (isGround) {
            if (verticalKey > 0 && jumpTime < jumpLimitTime) {
                ySpeed = jumpSpeed;
                jumpPos = transform.position.y; //ジャンプした位置を記録する
                isJump = true;
                jumpTime = 0.0f;
            } else {
                isJump = false;
            }
        } else if (isJump) {
            //上ボタンを押されている。かつ、現在の高さがジャンプした位置から自分の決めた位置より下ならジャンプを継続する
            if (verticalKey > 0 && jumpPos + jumpHeight > transform.position.y && jumpTime < jumpLimitTime && !isHead) {
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

    /// <summary>
    /// X成分で必要な計算をし、速度を返す。
    /// </summary>
    /// <returns>X軸の速さ</returns>
    private float GetXSpeed() {
        float horizontalKey = Input.GetAxis("Horizontal");
        float xSpeed = 0.0f;

        if (horizontalKey > 0) {
            transform.localScale = new Vector3(1, 1, 1);
            dashTime += Time.deltaTime;
            anim.SetBool("is_walk", true);
            xSpeed = speed;
        } else if (horizontalKey < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
            dashTime += Time.deltaTime;
            anim.SetBool("is_walk", true);
            xSpeed = -speed;
        } else {
            anim.SetBool("is_walk", false);
            xSpeed = 0.0f;
            dashTime = 0.0f;
        }

        //前回の入力からダッシュの反転を判断して速度を変える
        if (horizontalKey > 0 && beforeKey < 0) {
            dashTime = 0.0f;
        } else if (horizontalKey < 0 && beforeKey > 0) {
            dashTime = 0.0f;
        }

        beforeKey = horizontalKey;
        xSpeed *= dashCurve.Evaluate(dashTime);
        beforeKey = horizontalKey;
        return xSpeed;
    }

    /// <summary>
    /// アニメーションを設定する
    /// </summary>
    private void SetAnimation() {
        anim.SetBool("jump", isJump);
    }



    private void Ibarst() {
        // 攻撃オブジェクトを生成
        GameObject g = Instantiate(ibarst);
        g.transform.position = new Vector2(this.transform.position.x + 1f, transform.position.y - 0.5f);
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
