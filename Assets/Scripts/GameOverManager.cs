using UnityEngine;
using UnityEngine.SceneManagement;

// 発覚時にゲームオーバー処理(タイトルへ戻る)をまとめる、シーン内に1つだけ置く想定
public class GameOverManager : MonoBehaviour
{
    [Tooltip("ゲームオーバー時に戻るシーン名。Build Settingsに追加してあるシーン名を正確に入力すること")]
    public static string titleSceneName = "titlescene";

    private static bool isGameOver;

    void OnEnable()
    {
        isGameOver = false;
    }

    // どこからでも呼べるように static にしてある
    public static void TriggerGameOver()
    {
        if (isGameOver) return; // 二重発火防止
        isGameOver = true;

        Debug.Log("プレイヤーが発覚しました。ゲームオーバー。");

        // タイトルシーンがBuild Settingsに登録されていればロード
        // (まだシーンが無い/名前が違う場合はここでエラーが出るので、その時は名前を実際のシーン名に合わせて直すこと)
        SceneManager.LoadScene(titleSceneName);
    }
}
