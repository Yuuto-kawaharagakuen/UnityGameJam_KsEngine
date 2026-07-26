using UnityEngine;

[RequireComponent(typeof(Conecamera))]
public class CameraDetectionZone : MonoBehaviour
{
    [Header("検知設定")]
    public float aimHeightOffset = 1f;

    [Tooltip("壁など、光を遮る対象のレイヤー")]
    public LayerMask obstructionMask;

    private Conecamera cone;
    private Transform player;
    private PlayerManager playerManager;

    void Awake()
    {
        cone = GetComponent<Conecamera>();
    }

    void Start()
    {
        GameObject playerObj =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
            playerManager =
                playerObj.GetComponent<PlayerManager>();
        }
        else
        {
            Debug.LogWarning(
                "Playerタグが付いたオブジェクトが見つかりません");
        }
    }

    void Update()
    {
        if (player == null) return;

        Vector3 groundPoint = cone.GroundPoint;

        Vector2 flatDiff = new Vector2(
            player.position.x - groundPoint.x,
            player.position.z - groundPoint.z);

        bool inRange =
            flatDiff.magnitude <= cone.Radius;

        if (inRange && IsPlayerVisible())
        {
            if (playerManager != null)
            {
                playerManager.AddDetectionTime(
                    Time.deltaTime);
            }
        }
    }

    private bool IsPlayerVisible()
    {
        Vector3 origin = transform.position;

        Vector3 targetPoint =
            player.position +
            Vector3.up * aimHeightOffset;

        Vector3 dir = targetPoint - origin;

        float distance = dir.magnitude;

        if (Physics.Raycast(
            origin,
            dir.normalized,
            out RaycastHit hit,
            distance,
            obstructionMask))
        {
            return false;
        }

        return true;
    }
}