using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float detectedTime = 0f;

    [Tooltip("累計何秒検知されたらゲームオーバーか")]
    public float gameOverTime = 2f;

    private bool isGameOver = false;

    void Start()
    {
        if (GameSettings.Instance != null)
        {
            gameOverTime = GameSettings.Instance.GameOverTime;
            Debug.Log($"[PlayerManager] GameSettings.Instance ID: {GameSettings.Instance.GetInstanceID()}, 難易度: {GameSettings.Instance.currentDifficulty}, gameOverTime: {gameOverTime}");
        }
    }

    public void AddDetectionTime(float amount)
    {
        if (isGameOver) return;

        detectedTime += amount;

        Debug.Log("累計検知時間：" + detectedTime);

        if (detectedTime >= gameOverTime)
        {
            isGameOver = true;

            GetComponent<PlayerController>()?.SetMovable(false);
            FindObjectOfType<GameOverScreen>()
                .StartGameOver();

            Debug.Log("GAME OVER");
        }
    }
}