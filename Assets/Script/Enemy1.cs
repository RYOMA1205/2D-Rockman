using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    // 手順7で作成
    private Rigidbody2D rigidbody2D;

    public int speed = -3;

    public GameObject explosion;

    // 手順11で追加
    public GameObject item;

    // 8で追加
    public int attackPoint = 10;

    // 手順11で変更
    private Life lifeScript;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        // 手順11で追加
        lifeScript = GameObject.FindGameObjectWithTag("HP").GetComponent<Life>();
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

            // 手順11で追加
            // 四分の一の確率で回復アイテムを落とす
            if (Random.Range (0, 4) == 0)
            {
                Instantiate (item, transform.position, transform.rotation);
            }

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
