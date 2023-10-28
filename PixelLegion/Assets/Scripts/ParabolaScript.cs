using System.Collections;
using System.Net;
using UnityEngine;
public class ParabolaScript : MonoBehaviour
{
    public Transform movePoint;
    public Transform startPoint;
    public Transform controlPoint;
    public Transform endPoint;
    public float speed;

    private float t;
    private void Start()
    {
        speed = 1f;
        t = 0f;
    }
    void Update()
    {

        // 在0到1之間變化的參數t
        t += speed * Time.deltaTime;

        // 限制t的範圍在0到1之間
        t = Mathf.Clamp01(t);

        // 計算二次貝茲曲線上的點
        Vector3 targetPosition = CalculateQuadraticBezierPoint(t, startPoint.position, controlPoint.position, endPoint.position);


        Vector3 direction = targetPosition - movePoint.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        // 移動物件到計算得到的點
        movePoint.position = targetPosition;
        movePoint.rotation = rotation;

        // 如果t等於1，表示物件已經移動到了控制點P2，這裡你可以觸發相應的事件或做其他處理
        if (t >= 1f)
        {
            // 在控制點P2處做一些事情，例如觸發事件、切換狀態等等
            // ...

            // 重置t值，重新開始移動
            t = 0f;
        }
    }

    Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        return u * u * p0 + 2 * u * t * p1 + t * t * p2;


        //// 计算参数 t 的补数 u
        //float u = 1 - t;

        //// 计算三个部分
        //// 第一个部分：u^2 * p0，对应于公式中的 (1 - t)^2 * p0
        //Vector3 part1 = u * u * p0;

        //// 第二个部分：2 * u * t * p1，对应于公式中的 2 * (1 - t) * t * p1
        //Vector3 part2 = 2 * u * t * p1;

        //// 第三个部分：t^2 * p2，对应于公式中的 t^2 * p2
        //Vector3 part3 = t * t * p2;

        //// 将三个部分相加，得到曲线上的点
        //Vector3 bezierPoint = part1 + part2 + part3;

        //// 返回曲线上的点
        //return bezierPoint;
    }

}
