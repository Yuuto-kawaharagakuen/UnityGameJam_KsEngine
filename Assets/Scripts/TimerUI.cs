using UnityEngine;
using TMPro;

// 制限時間からカウントダウンし、0になったらゲームオーバーにする
// Canvas上のテキスト(TextMeshPro)にmm:ss形式で表示する
public class TimerUI : MonoBehaviour
{
    [Header("タイマー設定")]
    [Tooltip("制限時間(秒)。例: 60なら1分")]
    public float timeLimit = 60f;

    [Header("参照")]
    [Tooltip("時間を表示するテキスト(TextMeshProUGUI)")]
    public TMP_Text timerText;

    [Tooltip("残り時間がこの秒数以下になったら文字を赤くする(0以下で無効)")]
    public float warningThreshold = 10f;
    public Color normalColor = Color.white;
    public Color warningColor = Color.red;

    private float remaining;
    private bool isRunning = true;

    void Start()
    {
        remaining = timeLimit;
        UpdateDisplay();
    }

    void Update()
    {
        if (!isRunning) return;

        remaining -= Time.deltaTime;

        if (remaining <= 0f)
        {
            remaining = 0f;
            isRunning = false;
            UpdateDisplay();
            GameOverManager.TriggerGameOver();
            return;
        }

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(remaining / 60f);
        int seconds = Mathf.FloorToInt(remaining % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";

        if (warningThreshold > 0f)
        {
            timerText.color = remaining <= warningThreshold ? warningColor : normalColor;
        }
    }

    // ゴール到達時など、外部からタイマーを止めたい時に呼ぶ
    public void StopTimer()
    {
        isRunning = false;
    }

    // 経過時間(クリアタイム表示などに使いたい場合)
    public float GetElapsedTime()
    {
        return timeLimit - remaining;
    }
}
