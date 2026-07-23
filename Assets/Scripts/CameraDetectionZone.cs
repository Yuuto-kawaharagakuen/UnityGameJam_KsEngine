using UnityEngine;

// 検知範囲を表すオブジェクトにアタッチする(ConeDetectionMeshと同じオブジェクト)
// Collider/Triggerを使わず、円錐の床の円(ConeDetectionMeshが計算してる)との
// 距離計算でプレイヤーが範囲内にいるか判定する
[RequireComponent(typeof(Conecamera))]
public class CameraDetectionZone : MonoBehaviour
{
    [Header("検知設定")]
    [Tooltip("この秒数だけ連続で光に当たり続けるとアウト")]
    public float detectionThreshold = 2f;

    [Tooltip("プレイヤーの狙う高さのオフセット(足元基準からどれだけ上を狙うか)")]
    public float aimHeightOffset = 1f;

    [Tooltip("壁など、光を遮る対象のレイヤー(プレイヤー自身は含めないこと)")]
    public LayerMask obstructionMask;

    private Conecamera cone;
    private Transform player;
    private float exposureTimer;

    void Awake()
    {
        cone = GetComponent<Conecamera>();
    }

    void Start()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Playerタグが付いたオブジェクトが見つかりません。CameraDetectionZoneが機能しません。");
        }
    }

    void Update()
    {
        if (player == null) return;

        // 円の中心(床)とプレイヤーの水平距離を比較(高さは無視)
        Vector3 groundPoint = cone.GroundPoint;
        Vector2 flatDiff = new Vector2(player.position.x - groundPoint.x, player.position.z - groundPoint.z);

        bool inRange = flatDiff.magnitude <= cone.Radius;

        if (inRange && IsPlayerVisible())
        {
            exposureTimer += Time.deltaTime;

            if (exposureTimer >= detectionThreshold)
            {
                OnPlayerCaught();
            }
        }
        else
        {
            // 範囲外、または遮蔽物に隠れている間はタイマーをリセット
            exposureTimer = 0f;
        }
    }

    private bool IsPlayerVisible()
    {
        Vector3 origin = transform.position;
        Vector3 targetPoint = player.position + Vector3.up * aimHeightOffset;
        Vector3 dir = targetPoint - origin;
        float distance = dir.magnitude;

        if (Physics.Raycast(origin, dir.normalized, out RaycastHit hit, distance, obstructionMask))
        {
            return false; // 遮蔽物に遮られている
        }

        return true;
    }

    private void OnPlayerCaught()
    {
        exposureTimer = 0f;
        //GameOverManager.TriggerGameOver();
    }
}

