using UnityEngine;
using UnityEngine.SceneManagement;

// クリア時の処理(タイトルへ戻る、クリアタイムのログなど)をまとめる
// GameOverManagerと対になる存在。シーン内に置く必要はなく、staticメソッドのみで動く
public class ClearManager : MonoBehaviour
{
    [Tooltip("クリア時に戻るシーン名。Build Settingsに追加してあるシーン名を正確に入力すること")]
    public static string titleSceneName = "titlescene";

    private static bool isCleared;

    void OnEnable()
    {
        isCleared = false;
    }

    // GoalZoneから呼ばれる
    public static void TriggerClear(float elapsedTime)
    {
        if (isCleared) return; // 二重発火防止
        isCleared = true;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        Debug.Log($"クリア! タイム: {minutes:00}:{seconds:00}");

        // 今はシンプルにタイトルへ戻す(後でクリア専用UIに差し替え可能)
        SceneManager.LoadScene(titleSceneName);
    }
}
