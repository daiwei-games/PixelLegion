using UnityEngine;
public class ParabolaScript : LeadToSurviveGameBaseClass
{
    /// <summary>
    /// 物件起始點
    /// </summary>
    public Transform startPoint;
    /// <summary>
    /// 移動物件
    /// </summary>
    public Transform movePoint;
    /// <summary>
    /// 控制點
    /// </summary>
    public Transform controlPoint;
    /// <summary>
    /// 物件結束點
    /// </summary>
    public Transform endPoint;
    /// <summary>
    /// 速度
    /// </summary>
    private float speed;

    private float t;
    private void OnEnable()
    {
        _Tf = transform;
        _Go = gameObject;

        if(speed < 1) speed = 1;
        t = 0f;
    }
    /// <summary>
    /// 前往目標
    /// </summary>
    public void Goto()
    {

        // 在0到1之間變化的參數t
        t += speed * Time.deltaTime;

        // 限制t的範圍在0到1之間
        t = Mathf.Clamp01(t);

        // 計算二次貝茲曲線上的點
        Vector3 targetPosition = CalculateQuadraticBezierPoint(t, startPoint.position, controlPoint.position, endPoint.position);


        Vector3 direction = targetPosition - _Tf.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        // 移動物件到計算得到的點
        _Tf.position = targetPosition;
        _Tf.rotation = rotation;

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
    }

}
