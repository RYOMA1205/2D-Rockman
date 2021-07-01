using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    // 手順7で作成
    private Rigidbody2D rigidbody2D;

    public int speed = -3;

    public GameObject explosion;

    // 8で追加
    public int attackPoint = 10;

    public Life lifeScript;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            Destroy(gameObject);

            Instantiate(explosion, transform.position, transform.rotation);
        }
    }

    // 8で追加
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // UnityChanとぶつかった時
        if (collision.gameObject.tag == "UnityChan")
        {
            // LifeScriptのLifeDownメソッドを実行
            lifeScript.LifeDown(attackPoint);
        }
    }
}
