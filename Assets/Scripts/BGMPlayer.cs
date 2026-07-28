using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMPlayer : MonoBehaviour
{
    [Tooltip("再生するBGM(全体2分の曲)")]
    public AudioClip bgmClip;

    [Tooltip("最初の再生を始める秒数(今回は60秒地点から)")]
    public float startTime = 60f;

    [Range(0f, 1f)]
    public float volume = 0.5f;

    private AudioSource audioSource;
    private bool isStoppedIntentionally = false; // 意図的に止められたかどうか

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = bgmClip;
        audioSource.volume = volume;
        audioSource.loop = false;
    }

    void Start()
    {
        audioSource.time = startTime;
        audioSource.Play();
    }

    void Update()
    {
        if (isStoppedIntentionally) return; // 意図的に止めた後は何もしない

        if (!audioSource.isPlaying)
        {
            audioSource.time = 0f;
            audioSource.Play();
        }
    }

    // ゲームオーバーなどで、外部から明示的に止めたい時に呼ぶ
    public void StopBGM()
    {
        isStoppedIntentionally = true;
        audioSource.Stop();
    }
}