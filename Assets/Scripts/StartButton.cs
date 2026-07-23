using UnityEngine;
using UnityEngine.SceneManagement;
public class StartButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        Debug.Log("Startボタンが押された");
        SceneManager.LoadScene("Test_Map");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
