using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [Tooltip("この距離以内にいる時だけプレイヤーの方を向く")]
    public float followRange = 10f;

    [Tooltip("1秒あたり何度回転するか")]
    public float rotateSpeed = 10f;

    [Tooltip("回転時、プレイヤーの高さのどのあたりを狙うか(足元基準からのオフセット)")]
    public float aimHeightOffset = 1f;

    private Transform player;
    private float fixedZ;
    private Quaternion initialRotation; // Start時点の初期の向き(戻り先)

    void Start()
    {
        initialRotation = transform.rotation;
        fixedZ = transform.eulerAngles.z;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Playerタグが付いたオブジェクトが見つかりません");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        bool inRange = distance <= followRange;

        Quaternion targetRotation;

        if (inRange)
        {
            Vector3 targetPoint = player.position + Vector3.up * aimHeightOffset;
            Vector3 dir = targetPoint - transform.position;

            if (dir.sqrMagnitude < 0.0001f) return;

            targetRotation = Quaternion.LookRotation(dir);
        }
        else
        {
            // 範囲外なら初期角度へ戻す
            targetRotation = initialRotation;
        }

        Quaternion rotated = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotateSpeed * Time.deltaTime);

        Vector3 euler = rotated.eulerAngles;
        transform.rotation = Quaternion.Euler(euler.x, euler.y, fixedZ);
    }
}