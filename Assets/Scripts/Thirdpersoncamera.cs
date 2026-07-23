using UnityEngine;
using UnityEngine.InputSystem;

// 正式操作: 右スティック=カメラ操作
// テスト用: マウス移動でもカメラ操作できる(コントローラーが無くても動作確認できるように)
public class ThirdPersonCamera : MonoBehaviour
{
    [Header("追従対象")]
    public Transform target; // プレイヤーのTransform

    [Header("カメラ設定")]
    public Vector3 offset = new Vector3(0f, 1.6f, 0f); // 注視点の高さオフセット
    public float distance = 4f;
    public float sensitivity = 120f;
    public float mouseSensitivity = 0.15f;
    public float minPitch = -20f;
    public float maxPitch = 60f;
    [Tooltip("開始時のカメラの水平方向の向き(度数)。180にすると反対向きから始まる")]
    public float initialYaw = 0f;

    [Header("壁との衝突")]
    [Tooltip("壁とみなすレイヤー(Environment/Groundなど)")]
    public LayerMask collisionMask;
    [Tooltip("壁に当たった時、めり込まないようにする余白")]
    public float collisionBuffer = 0.3f;

    private float yaw;
    private float pitch = 15f;

    void Start()
    {
        yaw = initialYaw;
    }

    void LateUpdate()
    {
        if (target == null) return;

        var gamepad = Gamepad.current;
        var mouse = Mouse.current;

        if (gamepad != null && gamepad.rightStick.ReadValue().sqrMagnitude > 0.01f)
        {
            Vector2 look = gamepad.rightStick.ReadValue();
            yaw += look.x * sensitivity * Time.deltaTime;
            pitch -= look.y * sensitivity * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }
        else if (mouse != null)
        {
            Vector2 delta = mouse.delta.ReadValue();
            yaw += delta.x * mouseSensitivity;
            pitch -= delta.y * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 focusPoint = target.position + offset;
        Vector3 desiredPos = focusPoint - rot * Vector3.forward * distance;

        // 注視点とカメラ位置の間に壁がないかチェックし、あれば手前に寄せる
        Vector3 dir = desiredPos - focusPoint;
        float desiredDistance = dir.magnitude;

        if (Physics.SphereCast(focusPoint, 0.2f, dir.normalized, out RaycastHit hit, desiredDistance, collisionMask))
        {
            float safeDistance = Mathf.Max(hit.distance - collisionBuffer, 0.1f);
            desiredPos = focusPoint + dir.normalized * safeDistance;
        }

        transform.position = desiredPos;
        transform.LookAt(focusPoint);
    }
}