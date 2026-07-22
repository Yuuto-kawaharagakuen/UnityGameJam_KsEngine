using UnityEngine;

// カメラの「首」部分(回転する見た目のパーツ)にアタッチする
// 左右(Yaw)と上下(Pitch)、同時に首振りするパトロール
public class SurveillanceCamera : MonoBehaviour
{
    [Header("左右の首振り(Yaw)")]
    [Tooltip("初期の向きから左右に何度まで振るか")]
    public float horizontalAngle = 45f;
    [Tooltip("左右の首振りの速さ")]
    public float horizontalSpeed = 20f;

    [Header("上下の首振り(Pitch)")]
    [Tooltip("初期の向きから上下に何度まで振るか(0にすれば上下には動かない)")]
    public float verticalAngle = 15f;
    [Tooltip("上下の首振りの速さ")]
    public float verticalSpeed = 10f;

    private Quaternion baseRotation;
    private float yaw;
    private float pitch;
    private int yawDirection = 1;
    private int pitchDirection = 1;

    void Start()
    {
        baseRotation = transform.localRotation;
    }

    void Update()
    {
        // 左右(Yaw)
        yaw += yawDirection * horizontalSpeed * Time.deltaTime;
        if (yaw > horizontalAngle) { yaw = horizontalAngle; yawDirection = -1; }
        else if (yaw < -horizontalAngle) { yaw = -horizontalAngle; yawDirection = 1; }

        // 上下(Pitch)
        pitch += pitchDirection * verticalSpeed * Time.deltaTime;
        if (pitch > verticalAngle) { pitch = verticalAngle; pitchDirection = -1; }
        else if (pitch < -verticalAngle) { pitch = -verticalAngle; pitchDirection = 1; }

        transform.localRotation = baseRotation * Quaternion.Euler(pitch, yaw, 0f);
    }
}
