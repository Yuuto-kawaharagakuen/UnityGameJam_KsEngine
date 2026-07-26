using UnityEngine;

public class StartButton : MonoBehaviour
{
    [Tooltip("「始める」ボタンの見た目オブジェクト(TitleSequenceのStart Buttonと同じもの)")]
    public GameObject startButtonVisual;

    [Tooltip("モード選択用のパネル")]
    public GameObject modeSelectPanel;

    public void StartGame()
    {
        Debug.Log("Startボタンが押された");

        if (startButtonVisual != null)
            startButtonVisual.SetActive(false);

        modeSelectPanel.SetActive(true);
    }
}
