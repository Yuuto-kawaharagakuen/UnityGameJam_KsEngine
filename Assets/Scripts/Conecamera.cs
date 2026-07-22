using UnityEngine;

// 「カメラ→床に落ちる光」のイメージで、毎フレームカメラの向いてる方向(forward)へ
// Raycastを飛ばし、床に当たった位置に円を作って、そこと頂点(カメラ側)をつなぐ
// 見た目(Mesh)専用。当たり判定はCameraDetectionZone側で距離計算により行う
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Conecamera : MonoBehaviour
{
    [Header("床への当たり判定")]
    [Tooltip("床とみなすレイヤー。これに当たった位置に円ができる")]
    public LayerMask groundMask;
    [Tooltip("床が見つからなかった場合に使う、頂点からの距離")]
    public float fallbackDistance = 10f;

    [Header("円錐の形状")]
    [Tooltip("床にできる円の半径")]
    public float radius = 3f;
    [Tooltip("円周の分割数(多いほど滑らかだが重くなる、16程度で十分)")]
    public int segments = 16;

    private Mesh mesh;

    // CameraDetectionZone側が参照する、現在の床の円の情報
    public Vector3 GroundPoint { get; private set; }
    public float Radius => radius;

    void Awake()
    {
        mesh = new Mesh();
        mesh.name = "DetectionCone";
        mesh.MarkDynamic();
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    void LateUpdate()
    {
        Vector3 groundPoint;

        // カメラが向いてる方向(forward)にRaycastを飛ばして床の位置を探す
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, fallbackDistance * 2f, groundMask))
        {
            groundPoint = hit.point;
        }
        else
        {
            groundPoint = transform.position + transform.forward * fallbackDistance;
        }

        GroundPoint = groundPoint;
        BuildCone(groundPoint);
    }

    private void BuildCone(Vector3 groundPointWorld)
    {
        Vector3 localCenter = transform.InverseTransformPoint(groundPointWorld);
        Vector3 worldRight = Vector3.right;
        Vector3 worldForward = Vector3.forward;

        Vector3[] vertices = new Vector3[segments + 2];
        vertices[0] = Vector3.zero; // 頂点(カメラ側の位置、ローカル原点)

        for (int i = 0; i < segments; i++)
        {
            float angle = (float)i / segments * Mathf.PI * 2f;
            Vector3 offset = (worldRight * Mathf.Cos(angle) + worldForward * Mathf.Sin(angle)) * radius;
            vertices[i + 1] = transform.InverseTransformPoint(groundPointWorld + offset);
        }
        vertices[segments + 1] = localCenter;

        int[] triangles = new int[segments * 3 * 2];
        int t = 0;

        for (int i = 0; i < segments; i++)
        {
            int current = i + 1;
            int next = (i + 1) % segments + 1;
            triangles[t++] = 0;
            triangles[t++] = current;
            triangles[t++] = next;
        }

        int capCenter = segments + 1;
        for (int i = 0; i < segments; i++)
        {
            int current = i + 1;
            int next = (i + 1) % segments + 1;
            triangles[t++] = capCenter;
            triangles[t++] = next;
            triangles[t++] = current;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}