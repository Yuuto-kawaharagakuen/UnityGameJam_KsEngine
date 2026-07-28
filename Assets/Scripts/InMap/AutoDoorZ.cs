using UnityEngine;

public class AutoDoorZ : MonoBehaviour
{
    [Header("検知設定")]
    [Tooltip("プレイヤーがこの距離以内に近づいたらドアが開く")]
    public float detectRange = 3f;
    [Tooltip("検知の基準点(ドアの中心に置いた空のGameObject。左右のドアで同じものを指定する)")]
    public Transform detectionPoint;

    [Header("ドア設定")]
    [Tooltip("開いた時のPosition.z")]
    public float openZ = 9.8f;
    [Tooltip("1秒あたりどれだけZ座標を動かすか")]
    public float moveSpeed = 1f;

    private Transform player;
    private float closedZ; // このドア自身の元のZ座標(戻る位置として使う)

    void Start()
    {
        closedZ = transform.position.z;

        if (detectionPoint == null)
        {
            Debug.LogWarning("detectionPointが設定されていません。インスペクターでドアの中心オブジェクトを割り当ててください");
        }

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
        if (player == null || detectionPoint == null) return;

        // 判定は共通の基準点(detectionPoint)から。ドア自身は動いても判定はブレない
        float distance = Vector3.Distance(detectionPoint.position, player.position);
        bool isNear = distance <= detectRange;

        float targetZ = isNear ? openZ : closedZ;

        Vector3 pos = transform.position;
        pos.z = Mathf.MoveTowards(pos.z, targetZ, moveSpeed * Time.deltaTime);
        transform.position = pos;
    }
}