using UnityEngine;

// 金庫(ゴール)のオブジェクトにアタッチする
// Collider(Is Trigger = true)が必要
// プレイヤーが範囲内に入った瞬間、自動でクリア扱いにする
[RequireComponent(typeof(Collider))]
public class GoalZone : MonoBehaviour
{
    private bool cleared = false;

    void Reset()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (cleared) return;
        if (!other.CompareTag("Player")) return;

        cleared = true;

        // タイマーを止める(TimerUIがシーン内にあれば)
        var timer = FindFirstObjectByType<TimerUI>();
        float elapsed = 0f;
        if (timer != null)
        {
            elapsed = timer.GetElapsedTime();
            timer.StopTimer();
        }

        ClearManager.TriggerClear(elapsed);
    }
}

