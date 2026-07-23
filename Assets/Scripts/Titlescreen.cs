using System.Collections;
using UnityEngine;

public class TitleSequence : MonoBehaviour
{
    public CanvasGroup blackPanel;
    public CanvasGroup unityLogo;
    public CanvasGroup titleText;

    public GameObject startButton;

    IEnumerator Fade(CanvasGroup cg, float start, float end, float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            cg.alpha = Mathf.Lerp(start, end, time / duration);

            yield return null;
        }

        cg.alpha = end;
    }

    IEnumerator Start()
    {
        blackPanel.alpha = 1f;

        unityLogo.alpha = 0f;
        titleText.alpha = 0f;

        startButton.SetActive(false);

        yield return new WaitForSeconds(1f);

        // 黒画面を消す
        yield return StartCoroutine(
            Fade(blackPanel, 1f, 0f, 2f));

        // Unity表示
        yield return StartCoroutine(
            Fade(unityLogo, 0f, 1f, 1.5f));

        yield return new WaitForSeconds(1f);

        // Unity消す
        yield return StartCoroutine(
            Fade(unityLogo, 1f, 0f, 1.5f));

        // タイトル表示
        yield return StartCoroutine(
            Fade(titleText, 0f, 1f, 2f));

        yield return new WaitForSeconds(0.5f);

        startButton.SetActive(true);
        Debug.Log("シーケンス終了");
    }
}