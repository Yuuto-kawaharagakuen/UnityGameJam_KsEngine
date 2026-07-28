using UnityEngine;

// 監視カメラのセンサー(検知用の光)を一定間隔でON/OFFする
[RequireComponent(typeof(CameraDetectionZone))]
public class SensorToggle : MonoBehaviour
{
    [Tooltip("センサーがONになっている秒数")]
    public float onDuration = 3f;

    [Tooltip("センサーがOFFになっている秒数")]
    public float offDuration = 2f;

    private CameraDetectionZone detectionZone;
    private MeshRenderer coneRenderer; // Conecameraが描画に使っているMeshRenderer
    private float timer;
    private bool isOn = true;

    void Awake()
    {
        detectionZone = GetComponent<CameraDetectionZone>();

        // Conecameraと同じGameObject(またはその子)にあるMeshRendererを探す
        coneRenderer = GetComponent<MeshRenderer>();
        if (coneRenderer == null)
            coneRenderer = GetComponentInChildren<MeshRenderer>();
    }

    void Start()
    {
        ApplyState();
    }

    void Update()
    {
        timer += Time.deltaTime;

        float currentDuration = isOn ? onDuration : offDuration;

        if (timer >= currentDuration)
        {
            timer -= currentDuration; // 誤差を次の周期に持ち越す
            isOn = !isOn;
            ApplyState();
        }
    }

    void ApplyState()
    {
        detectionZone.enabled = isOn;

        // Conecameraのコード自体は変更せず、描画(MeshRenderer)だけ外側からON/OFFする
        if (coneRenderer != null)
            coneRenderer.enabled = isOn;
    }
}