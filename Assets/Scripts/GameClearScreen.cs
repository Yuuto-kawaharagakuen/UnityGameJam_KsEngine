using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class GameClearScreen : MonoBehaviour
{
    public CanvasGroup blackPanel;
    public CanvasGroup gameClearText;

    //フェードイン用のオブジェクト
    public CanvasGroup ClearTimeText;
    public CanvasGroup ClearTime;
    public CanvasGroup retryButton;
    public CanvasGroup titleButton;
    public CanvasGroup GameClearPanel;

    //選択用のオブジェクト
    public GameObject retryButtonObject;
    public GameObject titleButtonObject;

    //クリアタイム
    public TMP_Text clearTimeText;
    void Start()
    {
        float time = ClearManager.LastClearTime;

        int minutes = Mathf.FloorToInt(time / 60.0f);
        float seconds = time % 60f;

        clearTimeText.text =
                $"{minutes:00}:{seconds:00.00}";
        StartGameClear();
    }
    IEnumerator Fade(CanvasGroup cg, float start, float end, float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            cg.alpha = Mathf.Lerp(
                start,
                end,
                time / duration);

            yield return null;
        }

        cg.alpha = end;
    }

    public void StartGameClear()
    {
        Debug.Log("StartGameClear");

        StartCoroutine(GameClearRoutine());
    }

    IEnumerator GameClearRoutine()
    {
        // 初期状態

        blackPanel.alpha = 1f;

        ClearTimeText.alpha = 0f;
        ClearTime.alpha = 0f;
        gameClearText.alpha = 0f;
        GameClearPanel.alpha = 0f;
        retryButton.alpha = 0f;
        titleButton.alpha = 0f;

        retryButton.interactable = false;
        retryButton.blocksRaycasts = false;

        titleButton.interactable = false;
        titleButton.blocksRaycasts = false;

        // 0.5秒停止
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;

        // 暗転
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(
            Fade(blackPanel, 1f, 0f, 0.5f));

        
        // GAME CLEAR表示
        yield return StartCoroutine(
            Fade(gameClearText, 0f, 1f, 2f));

        yield return StartCoroutine(
            Fade(GameClearPanel, 0f, 1f, 2f));
        yield return new WaitForSeconds(1f);


        //クリアタイムテキスト表示
        yield return StartCoroutine(
            Fade(ClearTimeText, 0f, 1f, 1f));
        
        //クリアタイム表示
        yield return StartCoroutine(
            Fade(ClearTime, 0f, 1f, 1f));

        yield return new WaitForSeconds(0.3f);

        // Retry表示
        yield return StartCoroutine(
            Fade(retryButton, 0f, 1f, 1f));

        retryButton.interactable = true;
        retryButton.blocksRaycasts = true;

        EventSystem.current.SetSelectedGameObject(retryButtonObject);


        yield return new WaitForSeconds(0.3f);

        // Title表示
        yield return StartCoroutine(
            Fade(titleButton, 0f, 1f, 1f));

        titleButton.interactable = true;
        titleButton.blocksRaycasts = true;

        //EventSystem.current.SetSelectedGameObject(titleButtonObject);
    }
}