using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float detectedTime = 0f;

    [Tooltip("累計何秒検知されたらゲームオーバーか")]
    public float gameOverTime = 2f;

    private bool isGameOver = false;

    public void AddDetectionTime(float amount)
    {
        if (isGameOver) return;

        detectedTime += amount;

        Debug.Log("累計検知時間：" + detectedTime);

        if (detectedTime >= gameOverTime)
        {
            isGameOver = true;

            FindObjectOfType<GameOverScreen>()
                .StartGameOver();

            Debug.Log("GAME OVER");
        }
    }
}