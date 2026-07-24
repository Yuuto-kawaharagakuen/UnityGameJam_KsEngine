using UnityEngine;
using UnityEngine.SceneManagement;

public class retryButton : MonoBehaviour
{
    public void Retry()
    {
        Debug.Log("Retry");
        SceneManager.LoadScene("Test_Map");
    }
}