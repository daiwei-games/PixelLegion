using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ztest123 : MonoBehaviour
{
    public Transform target;        // 目標物體的Transform組件
    private float gravity;          // 重力加速度
    private Vector3 initialPosition; // 初始位置

    private void Start()
    {

        gravity = Mathf.Abs(Physics2D.gravity.y);
        initialPosition = transform.position;
    }
    void Update()
    {
        // 計算目標點和初始位置的水平距離和垂直高度差
        float deltaX = target.position.x - initialPosition.x;
        float deltaY = target.position.y - initialPosition.y;

        // 計算初始速度
        float initialVelocity = Mathf.Sqrt((deltaY + 0.5f * gravity * Mathf.Pow(deltaX, 2)) / deltaX);

        // 計算發射角度
        float launchAngle = Mathf.Atan2(deltaY, deltaX);
        launchAngle = launchAngle * Mathf.Rad2Deg;

        // 設定初始速度向量的x和y分量
        float initialVelocityX = initialVelocity * Mathf.Cos(launchAngle * Mathf.Deg2Rad);
        float initialVelocityY = initialVelocity * Mathf.Sin(launchAngle * Mathf.Deg2Rad);

        // 設定初始速度向量
        Vector3 initialVelocityVector = new Vector3(initialVelocityX, initialVelocityY, 0f);

        // 在拋物線運動的每一幀更新物體位置
        float time = Time.time - Time.fixedTime; // 使用這種方式獲取每一幀的時間
        float x = initialPosition.x + initialVelocityVector.x * time;
        float y = initialPosition.y + initialVelocityVector.y * time - 0.5f * gravity * Mathf.Pow(time, 2);
        transform.position = new Vector3(x, y, 0f);
    }

}
