using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // 手順10で追加
    public int healPoint = 20;

    // 手順11で変更
    // Prefab化するとInspectorから指定できない為private化
    private Life llifeScript;

    private void Start()
    {
        // HPタグの付いているオブジェクトのLifeScriptを取得
        llifeScript = GameObject.FindGameObjectWithTag("HP").GetComponent<Life>();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        // ユニティちゃんと衝突した時
        if (collision.gameObject.tag == "UnityChan")
        {
            // LifeUpメソッドを呼び出す　引数はhealPoint
            llifeScript.LifeUp(healPoint);

            // アイテムを削除する
            Destroy(gameObject);
        }
    }
}
