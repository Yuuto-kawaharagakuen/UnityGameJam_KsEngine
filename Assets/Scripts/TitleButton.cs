using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleButton : MonoBehaviour
{
    public void title()
    {
        Debug.Log("titleが押された。");
        SceneManager.LoadScene("titlescene");
    }
}
