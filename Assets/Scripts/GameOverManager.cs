using UnityEngine;

// 発覚時・時間切れ時にゲームオーバー処理をまとめる、シーン内に1つだけ置く想定
public class GameOverManager : MonoBehaviour
{
    private static bool isGameOver;

    void OnEnable()
    {
        isGameOver = false;
    }

    // どこからでも呼べるように static にしてある
    public static void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("ゲームオーバー。");

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerObj.GetComponent<PlayerController>()?.SetMovable(false);
        }

        FindFirstObjectByType<GameOverScreen>().StartGameOver();
    }
}