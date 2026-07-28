using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ModeSelectManager : MonoBehaviour
{
    public GameObject firstSelected;

    private bool selected = false; // 一度選択されたら以降は無視する

    void OnEnable()
    {
        selected = false;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    public void SelectNormal()
    {
        if (selected) return;
        selected = true;

        GameSettings.Instance.SetDifficulty(GameSettings.Difficulty.Normal);
        SceneManager.LoadScene("InGame");
    }

    public void SelectHard()
    {
        if (selected) return;
        selected = true;

        GameSettings.Instance.SetDifficulty(GameSettings.Difficulty.Hard);
        SceneManager.LoadScene("InGame");
    }
}