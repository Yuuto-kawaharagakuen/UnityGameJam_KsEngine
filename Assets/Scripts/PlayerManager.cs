using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public float detectedTime = 0f;

    [Tooltip("累計何秒検知されたらゲームオーバーか")]
    public float gameOverTime = 2f;

    [Header("UI表示")]
    [Tooltip("検知度合いを表示するゲージ(Slider)")]
    public Slider detectionGauge;

    private bool isGameOver = false;

    void Start()
    {
        if (GameSettings.Instance != null)
        {
            gameOverTime = GameSettings.Instance.GameOverTime;
        }

        UpdateGauge();
    }

    public void AddDetectionTime(float amount)
    {
        if (isGameOver) return;

        detectedTime += amount;

        UpdateGauge();

        if (detectedTime >= gameOverTime)
        {
            isGameOver = true;

            GetComponent<PlayerController>()?.SetMovable(false);
            FindObjectOfType<GameOverScreen>()
                .StartGameOver();

            Debug.Log("GAME OVER");
        }
    }

    private void UpdateGauge()
    {
        if (detectionGauge == null) return;

        // 0〜1の割合でゲージに反映
        detectionGauge.value = Mathf.Clamp01(detectedTime / gameOverTime);
    }
}