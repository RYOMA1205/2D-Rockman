using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallMain : MonoBehaviour
{
    // 13で作成
    // Use this for initialization
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2);

        Application.LoadLevel("Main");
    }
}
