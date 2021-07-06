using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallStage1 : MonoBehaviour
{
    // 手順13で作成
    void Update()
    {
        if (Input.GetMouseButtonDown (0))
        {
            Application.LoadLevel("Stage1");
        }
    }
}
