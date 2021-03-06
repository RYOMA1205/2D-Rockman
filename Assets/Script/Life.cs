using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    // 8で作成
    RectTransform rt;

    // 手順13で追加
    // ユニティちゃん
    public GameObject unityChan;

    // 爆発アニメーション
    public GameObject explosion;

    // ゲームオーバーの文字
    public Text gameOverText;

    // ゲームオーバー判定
    private bool gameOver = false;

    public int playerCount;

    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    // 手順13で追加
    private void Update()
    {
        // ライフが0以下になった時
        if (rt.sizeDelta.y <= 0)
        {
            // ゲームオーバー判定がfalseなら爆発アニメーションを生成
            // GameOverメソッドでtrueになるので、1回のみ実行
            if (gameOver == false)
            {
                Instantiate(explosion, unityChan.transform.position + new Vector3(0, 1, 0), unityChan.transform.rotation);
            }

            // ゲームオーバー判定をtrueにし、ユニティちゃんを消去
            GameOver();
        }

        // ゲームオーバー判定がtrueの時
        if (gameOver)
        {
            // ゲームオーバーの文字を表示
            gameOverText.enabled = true;

            // 画面をクリックすると
            if (Input.GetMouseButtonDown (0))
            {
                // タイトルへ戻る
                Application.LoadLevel("Title");
            }
        }
    }

    public void LifeDown (int ap)
    {
        // RectTransformのサイズを取得し、マイナスする
        rt.sizeDelta -= new Vector2(0, ap);
    }

    // 手順10で追加
    public void LifeUp (int hp)
    {
        // RectTransformのサイズを取得し、プラスする
        rt.sizeDelta += new Vector2(0, hp);

        // 最大値を超えたら、最大値まで上書きする
        if (rt.sizeDelta.y > 240f)
        {
            rt.sizeDelta = new Vector2(51f, 240f);
        }
    }

    // 手順13で追加
    public void GameOver ()
    {
        // プレイヤーの残機数の確認
        if (playerCount > 0)
        {
            // 残機数がまだ残っている場合は残機数を減らす
            playerCount --;

            // プレイヤーを初期位置にてリスポーンさせる
            unityChan.transform.position = unityChan.GetComponent<Player>().respawnPos;

            // カメラの位置を取得
            Vector3 cameraPos = Camera.main.transform.position;

            // ユニティちゃんの位置から右に4移動した位置を画面中央にする
            cameraPos.x = unityChan.transform.position.x + 4;

            Camera.main.transform.position = cameraPos;
        }
        // 残機数が0になったらgameoverになるようにする
        else
        {
            gameOver = true;

            Destroy(unityChan);
        }
    }
}
