using UnityEngine;

// カメラの「首」部分(回転する見た目のパーツ)にアタッチする
// 左右に一定角度でゆっくり首振りする、シンプルな往復パトロール
public class SurveillanceCamera : MonoBehaviour
{
    [Header("首振り設定")]
    [Tooltip("初期の向きから左右に何度まで振るか")]
    public float patrolAngle = 25f;
    [Tooltip("首振りの速さ(値が大きいほど速い)")]
    public float patrolSpeed = 10f;

    private Quaternion baseRotation;
    private float currentAngle;
    private int direction = 1; // 1: 右へ, -1: 左へ

    void Start()
    {
        baseRotation = transform.localRotation;
    }

    void Update()
    {
        // 角度を往復させる(-patrolAngle 〜 +patrolAngle)
        currentAngle += direction * patrolSpeed * Time.deltaTime;

        if (currentAngle > patrolAngle)
        {
            currentAngle = patrolAngle;
            direction = -1;
        }
        else if (currentAngle < -patrolAngle)
        {
            currentAngle = -patrolAngle;
            direction = 1;
        }

        transform.localRotation = baseRotation * Quaternion.Euler(0f, currentAngle, 0f);
    }
}