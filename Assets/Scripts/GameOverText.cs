using System.Collections;
using UnityEngine;

public class GameOverSequence : MonoBehaviour
{
    public CanvasGroup blackPanel;
    public CanvasGroup gameOverText;

    public GameObject retryButton;
    public GameObject titleButton;

    IEnumerator Fade(CanvasGroup cg,
                     float start,
                     float end,
                     float duration)
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
        retryButton.SetActive(false);
        titleButton.SetActive(false);

        // 黒画面
        yield return StartCoroutine(
            Fade(blackPanel, 0f, 1f, 2f));

        // GAME OVER表示
        yield return StartCoroutine(
            Fade(gameOverText, 0f, 1f, 2f));

        retryButton.SetActive(true);
        titleButton.SetActive(true);
    }
}