using UnityEngine;

// タイトルで選んだ難易度を保持し、シーンをまたいでも消えない設定オブジェクト
public class GameSettings : MonoBehaviour
{
    public enum Difficulty { Normal, Hard }

    public static GameSettings Instance { get; private set; }

    [Header("現在選択中の難易度")]
    public Difficulty currentDifficulty = Difficulty.Normal;

    [Header("制限時間(モード別)")]
    public float normalTimeLimit = 80f;
    public float hardTimeLimit = 40f;

    [Header("カメラに見つかるまでの時間(モード別)")]
    public float normalGameOverTime = 3f;
    public float hardGameOverTime = 1.5f;

    public float TimeLimit => currentDifficulty == Difficulty.Hard ? hardTimeLimit : normalTimeLimit;
    public float GameOverTime => currentDifficulty == Difficulty.Hard ? hardGameOverTime : normalGameOverTime;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetDifficulty(Difficulty difficulty)
    {
        currentDifficulty = difficulty;
        Debug.Log($"[GameSettings] 難易度を{currentDifficulty}に設定 (Instance ID: {GetInstanceID()})");
    }
}
