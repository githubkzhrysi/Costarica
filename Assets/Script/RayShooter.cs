using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayShooter : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //メインカメラからマウスクリックポイントまでのrayを作成
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit構造体宣言
            RaycastHit hit;
            //rayを距離100で照射して、colliderがあればtrueを返しその衝突したものの情報をhitにつめる。
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
