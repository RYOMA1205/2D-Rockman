using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // 手順2で作成
    // 歩くスピード
    public float speed = 4f;

    // 手順4で追加
    // ジャンプ力
    public float jumpPower = 700;

    // 手順4で追加
    // Linecastで判定するLayer
    public LayerMask groundLayer;
    
    // 手順3で追加
    // カメラを使ってUnityChanが一定の位置を超えたら背景も動くように設定するための宣言
    public GameObject mainCamera;

    // 手順5で追加
    // 弾生成用
    public GameObject bullet;

    // 手順13で追加
    public Life lifeScript;

    private Rigidbody2D rigidbody2D;

    // アニメーション設定用
    private Animator anim;

    // 手順4で追加
    // 着地判定
    private bool isGrounded;

    // 9で追加
    private Renderer renderer;

    // 手順13で追加
    // ゲームクリアしたら操作を無効にする
    private bool gameClear = false;

    // ゲームクリア時に表示するテキスト
    public Text clearText;

    void Start()
    {
        // 各コンポーネントをキャッシュしておく
        anim = GetComponent<Animator>();

        rigidbody2D = GetComponent<Rigidbody2D>();

        // 9で追加
        renderer = GetComponent<Renderer>();
    }

    // 手順4で追加
    private void Update()
    {
        // Linecastでユニティちゃんの足元に地面があるか判定
        isGrounded = Physics2D.Linecast(transform.position + transform.up * 1, transform.position - transform.up * 0.05f, groundLayer);

        // UnityChanがいる位置に青い線を出してる
        // どの位置に判定などがあるかを視覚化する為
        Debug.DrawLine(transform.position + transform.up * 1, transform.position - transform.up * 0.05f, Color.blue, 1.0f);

        // 手順13で追加
        // ジャンプさせない
        if (!gameClear)
        {
            // スペースキーを押し
            if (Input.GetKeyDown("space"))
            {
                // 着地していた時
                if (isGrounded == true)
                {
                    // Dashアニメーションを止めて
                    anim.SetBool("Dash", false);

                    // Jumpアニメーションを実行
                    anim.SetTrigger("Jump");

                    // 着地判定をfalse
                    isGrounded = false;

                    // AddForceにて上方向へ力を加える
                    rigidbody2D.AddForce(Vector2.up * jumpPower);
                }
            }
        }

        // 上下への移動速度を取得
        float velY = rigidbody2D.velocity.y;

        // 移動速度が0.1より大きければ上昇
        bool isJumping = velY > 0.1f ? true : false;

        // 移動速度が-0.1より小さければ下降
        bool isFalling = velY < -0.1f ? true : false;

        // 結果をアニメータービューの変数へ反映する
        anim.SetBool("isJumping", isJumping);

        anim.SetBool("isFalling", isFalling);

        // 手順13で追加
        // 弾を打たせない、ゲームオーバーにさせない
        if (!gameClear)
        {
            // 手順5で追加
            // どこのボタンを押したら弾が出るかを決定
            if (Input.GetKeyDown("left shift"))
            {
                // ボタンを押して弾が出る度にログが出るようにした
                Debug.Log("弾生成");

                // ボタンが押されたタイミングでセットしたアニメーションが反映される
                anim.SetTrigger("Shot");

                // Prefab化したものを特定の位置から出し続けられるように設定
                Instantiate(bullet, transform.position + new Vector3(0f, 1.2f, 0f), transform.rotation);
            }

            // 手順13で追加
            // 現在のカメラの位置から8低くした位置を下回った時
            if (gameObject.transform.position.y < Camera.main.transform.position.y - 8)
            {
                // LifeScriptのGameOverメソッドを実行する
                lifeScript.GameOver();
            }
        }

    }

    // 物理演算に適している
    private void FixedUpdate()
    {
        // 手順13で追加
        // 左右に移動させない MainCameraを動かさない
        if (!gameClear)
        {
            // 左キー: -1、右キー: 1
            float x = Input.GetAxisRaw("Horizontal");

            // 左か右を入力したら
            if (x != 0)
            {
                // 入力方向へ移動
                rigidbody2D.velocity = new Vector2(x * speed, rigidbody2D.velocity.y);

                // localScale.xを-1にすると画像が反転する
                Vector2 temp = transform.localScale;

                temp.x = x;

                transform.localScale = temp;

                // Wait → Dash
                anim.SetBool("Dash", true);

                // 手順3で追加
                // 画面中央から左に4移動した位置をユニティちゃんが超えたら
                if (transform.position.x > mainCamera.transform.position.x - 4)
                {
                    // カメラの位置を取得
                    Vector3 cameraPos = mainCamera.transform.position;

                    // ユニティちゃんの位置から右に4移動した位置を画面中央にする
                    cameraPos.x = transform.position.x + 4;

                    mainCamera.transform.position = cameraPos;
                }

                // カメラ表示領域の左下をワールド座標に変換
                Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

                // カメラ表示領域の右上をワールド座標に変換
                Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

                // ユニティちゃんのポジションを取得
                Vector2 pos = transform.position;

                // ユニティちゃんのx座標の移動範囲をClampメソッドで制限
                pos.x = Mathf.Clamp(pos.x, min.x + 0.5f, max.x);

                transform.position = pos;
            }

            // 左も右も入力していなかったら
            else
            {
                // 横移動の速度を0にしてピタッと止まるようにする
                rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);

                // Dash → Wait
                anim.SetBool("Dash", false);
            }
        }
        // 手順13で追加
        else
        {
            // クリアテキストを表示
            clearText.enabled = true;

            // アニメーションは走り
            anim.SetBool("Dash", true);

            // 右に進み続ける
            rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);

            // 5秒後にタイトル画面へ戻るCallTitleメソッドを呼び出す
            Invoke("CallTitle", 5);
        }
    }

    // 9で追加
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Enemyとぶつかった時にコルーチンを実行
        if (collision.gameObject.tag == "Enemy")
        {
            StartCoroutine("Damage");
        }
    }

    IEnumerator Damage ()
    {
        // レイヤーをPlayerDamageに変更
        gameObject.layer = LayerMask.NameToLayer("PlayerDamage");

        // whileを10回ループ
        int count = 10;

        while (count > 0)
        {
            // 透明にする
            renderer.material.color = new Color(1, 1, 1, 1);

            // 0.05秒待つ
            yield return new WaitForSeconds(0.05f);

            count--;
        }

        // レイヤーをPlayerに戻す
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    // 手順13で追加
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // タグがClearZoneであるTriggerにぶつかったら
        if (collision.tag == "ClearZone")
        {
            // ゲームクリア
            gameClear = true;
        }
    }

    // 手順13で追加
    void CallTitle ()
    {
        // タイトル画面へ
        Application.LoadLevel("Title");
    }
}
