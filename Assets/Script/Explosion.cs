using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // 手順7で作成
    public void DeleteExplosion()
    {
        Destroy(gameObject);
    }
}
