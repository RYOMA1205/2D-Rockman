using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 手順5で作成
    private GameObject player;

    // 弾の速度の設定
    private int speed = 20;

    void Start()
    {
        // ユニティちゃんオブジェクトを取得
        player = GameObject.FindWithTag("UnityChan");

        // rigidbody2Dコンポーネントを取得
        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();

        // ユニティちゃんの向いている向きに弾を飛ばす
        rigidbody2D.velocity = new Vector2(speed * player.transform.localScale.x, rigidbody2D.velocity.y);

        // 画像の向きをユニティちゃんに合わせる
        // Vector2型のtempという変数をStart内でだけ取得
        // スクリプトがアタッチされているゲームオブジェクトの
        // Transform コンポーネントの Scale の情報を temp に代入している
        Vector2 temp = transform.localScale;

        // player 変数に代入されているゲームオブジェクト(Unitychan)の
        // Transform コンポーネントの Scale の情報の x の情報を temp.x に代入している
        temp.x = player.transform.localScale.x;

        // temp 変数の情報を、スクリプトがアタッチされているゲームオブジェクトの
        // Transform コンポーネントの Scale の情報に代入している
        // これにより、xの情報のみが書き換わる
        transform.localScale = temp;

        // 5秒後に消滅
        Destroy(gameObject, 5);
    }

    // 手順7で追加
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
