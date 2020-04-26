using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IbarstBlackScript : MonoBehaviour {

	[Header("スピード")] public float speed = 5.0f;
	[Header("最大移動距離")] public float maxDistance = 100.0f;

	public GameObject explosion;

	private Rigidbody2D rb;
	private Vector3 defaultPos;

	private float direction;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		if (rb == null) {
			Debug.Log("設定が足りません");
			Destroy(this.gameObject);
		}

		direction = transform.localScale.x;
	}
	
	// Update is called once per frame
	void FixedUpdate() {
        //      if(this.transform.position.x > Screen.width) {
        //	Destroy(this.gameObject);
        //      } else {
        //	rb.MovePosition(transform.position += Vector3.left * Time.deltaTime * speed);
        //}
        rb.MovePosition(transform.position += Vector3.right * Time.deltaTime * speed * direction);

    }

	private void OnTriggerEnter2D(Collider2D collision) {

        // 攻撃した本人と、その攻撃オブジェクトは衝突判定しない
        if (!(collision.CompareTag("Ibarst_Black") || collision.CompareTag("Enemy"))) {
            Instantiate(explosion,transform.position,transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
