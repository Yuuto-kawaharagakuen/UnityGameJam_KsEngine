using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
public class GameOverScreen : MonoBehaviour
{
    public CanvasGroup blackPanel;
    public CanvasGroup gameOverText;

    //フェードイン用のオブジェクト
    public CanvasGroup retryButton;
    public CanvasGroup titleButton;

    //選択用のオブジェクト
    public GameObject retryButtonObject;
    public GameObject titleButtonObject;
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

    public void StartGameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        // 初期状態
        gameOverText.alpha = 0f;
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
        yield return StartCoroutine(
            Fade(blackPanel, 0f, 1f, 2f));

        // GAME OVER表示
        yield return StartCoroutine(
            Fade(gameOverText, 0f, 1f, 2f));

        yield return new WaitForSeconds(1f);

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

        EventSystem.current.SetSelectedGameObject(titleButtonObject);
    }
}