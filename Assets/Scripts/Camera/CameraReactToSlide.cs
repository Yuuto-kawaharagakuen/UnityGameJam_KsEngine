using UnityEngine;

// プレイヤーが近くでスライドした瞬間、そちらの方向を向く監視カメラ
public class CameraReactToSlide : MonoBehaviour
{
    [Tooltip("この距離以内でのスライドにだけ反応する")]
    public float followRange = 8f;

    [Tooltip("1秒あたり何度回転するか")]
    public float rotateSpeed = 40f;

    [Tooltip("回転時、プレイヤーの高さのどのあたりを狙うか(足元基準からのオフセット)")]
    public float aimHeightOffset = 1f;

    private Transform player;
    private PlayerController playerController;
    private Quaternion initialRotation;
    private float fixedZ;

    void Start()
    {
        initialRotation = transform.rotation;
        fixedZ = transform.eulerAngles.z;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerController = playerObj.GetComponent<PlayerController>();
        }
        else
        {
            Debug.LogWarning("Playerタグが付いたオブジェクトが見つかりません");
        }
    }

    void Update()
    {
        if (player == null || playerController == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        bool inRange = distance <= followRange;

        Quaternion targetRotation;

        if (playerController.IsSliding && inRange)
        {
            Vector3 targetPoint = player.position + Vector3.up * aimHeightOffset;
            Vector3 dir = targetPoint - transform.position;

            if (dir.sqrMagnitude < 0.0001f) return;

            targetRotation = Quaternion.LookRotation(dir);
        }
        else
        {
            // スライドしていない、または範囲外の時は初期角度へ戻す
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