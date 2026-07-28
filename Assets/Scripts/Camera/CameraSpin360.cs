using UnityEngine;

public class CameraSpin360 : MonoBehaviour
{
    [Tooltip("1秒あたり何度回転するか")]
    public float rotateSpeed = 30f;

    [Tooltip("回転方向が切り替わるまでの秒数を順番に指定(最後まで行ったら最初に戻る)")]
    public float[] switchIntervals = new float[] { 3f, 3f, 5f };

    private float timer;
    private int intervalIndex = 0;
    private int direction = 1; // 1 = 正回転、-1 = 逆回転

    void Update()
    {
        if (switchIntervals.Length == 0) return;

        timer += Time.deltaTime;

        float currentInterval = switchIntervals[intervalIndex];

        if (timer >= currentInterval)
        {
            timer -= currentInterval; // 誤差を次の周期に持ち越す
            direction *= -1;

            intervalIndex++;
            if (intervalIndex >= switchIntervals.Length)
                intervalIndex = 0; // 配列の最後まで行ったら最初に戻る
        }

        transform.Rotate(Vector3.up * rotateSpeed * direction * Time.deltaTime, Space.Self);
    }
}