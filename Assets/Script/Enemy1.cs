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

    // 手順12で追加
    // メインカメラのタグ名　constは定数(絶対に変わらない値)
    private const string Main_CAMERA_TAG_NAME = "MainCamera";

    // カメラに映っているかの判定
    private bool _isRendered = false;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        // 手順11で追加
        lifeScript = GameObject.FindGameObjectWithTag("HP").GetComponent<Life>();
    }

    void Update()
    {
        // 手順12で追加
        if (_isRendered)
        {
            rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
        }

        // 手順13で追加
        if (gameObject.transform.position.y < Camera.main.transform.position.y - 8 ||
            gameObject.transform.position.x < Camera.main.transform.position.x - 10)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 手順12で追加
        if (_isRendered)
        {
            if (collision.tag == "Bullet")
            {
                Destroy(gameObject);

                Instantiate(explosion, transform.position, transform.rotation);

                // 手順11で追加
                // 四分の一の確率で回復アイテムを落とす
                if (Random.Range(0, 4) == 0)
                {
                    Instantiate(item, transform.position, transform.rotation);
                }

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

    // 手順12で追加
    // Rendererがカメラに映っている間に呼ばれ続ける
    private void OnWillRenderObject()
    {
        // メインカメラに映った時だけ_isRenderdをtrue
        if (Camera.current.tag == Main_CAMERA_TAG_NAME)
        {
            _isRendered = true;
        }
    }

}
